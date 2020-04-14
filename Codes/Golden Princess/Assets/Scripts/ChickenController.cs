using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    private bool [] Paths = new bool [4];
    private Vector3 [] Directions = new Vector3 [4];
    private LayerMask BlocksMask;
    private LayerMask CharacterMask;
    private Vector3 Destination;
    private Vector3 Origin;
    [SerializeField] private int MoveDistance = 1;
    [SerializeField] private float MoveSpeed = 3f;
    [SerializeField] private float AttackSpeedMax = 8f;
    [SerializeField] private float AttackCooldownTime = 2f;
    private float AttackSpeed;
    private bool AttackCooldown;
    [SerializeField] private float AttackFriction = 7.5f;
    private int Repetition = 0;
    private List<int> emptyPaths = new List<int>();
    private Animator anim;
    public bool AttackMode = false;
    private Transform Character;
    [SerializeField] private float [] CheckDistances = new float [2];
     public int FaceDir = 0;
    [HideInInspector] public SpriteRenderer sprRenderer;
    [SerializeField] private float CastingTime = 0.5f;
    [SerializeField] private float ParrySpeedMax = 4f;
    [SerializeField] private float ParryFriction = 30f;
    private float ParrySpeed = 0f;
    [SerializeField] private bool boolParry = false;

    // Start is called before the first frame update
    void Start()
    {
        Character = GameObject.FindGameObjectWithTag("Character").transform;
        CheckDistances [0] = 2f;
        CheckDistances [1] = 5f;
        anim = GetComponent<Animator>();
        sprRenderer = GetComponent<SpriteRenderer>();
        BlocksMask = LayerMask.GetMask("Blocks");
        CharacterMask = LayerMask.GetMask("Character");
        Directions [0] = Vector3.right * MoveDistance;
        Directions [1] = Vector3.up * MoveDistance;
        Directions [2] = Vector3.left * MoveDistance;
        Directions [3] = Vector3.down * MoveDistance;
        Destination = PathChoosing();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform != null) {
            Move();
        }
    }
    
    void Move()
    {
        if (!AttackMode && !boolParry) {
            if (!AttackCheck()) {
                if (Destination != Vector3.zero) {
                    var Vect = Vector2.MoveTowards(transform.position,Origin + Destination,MoveSpeed * Time.deltaTime);
                    bool boolCanWalk = CanWalk(Vect - new Vector2(transform.position.x,transform.position.y), true);
                    if (boolCanWalk) {
                        transform.position = new Vector3(Vect.x,Vect.y,-1f);
                    }
                    if (!boolCanWalk || transform.position == Origin + Destination) {
                        Destination = PathChoosing();
                    }
                }
                else Destination = PathChoosing();
            }
        }
    }
    Vector3 PathChoosing()
    {
        emptyPaths.Clear();
        Origin = transform.position;
        Paths [0] = CanWalk(Vector3.right * MoveDistance, true);
        Paths [1] = CanWalk(Vector3.up * MoveDistance, true);
        Paths [2] = CanWalk(Vector3.left * MoveDistance, true);
        Paths [3] = CanWalk(Vector3.down * MoveDistance, true);
        for(var i = 0; i < 4; i++) {
            if (Paths[i]) {
                emptyPaths.Add(i);
            }
        }
        if (emptyPaths.Count > 0) {
            int rnd = Random.Range(0,emptyPaths.Count);
            Repetition = Random.Range(1,4);
            int selection = emptyPaths [rnd];
            ChangeAnimation(selection);
            FaceDir = selection;
            return Directions[selection] * Repetition;
        }
        return Vector3.zero;
    }
    bool CanWalk(Vector3 dir, bool boolChar)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir,1f,BlocksMask);
        if (hit.collider != null) {
            return false;
        }
        if (boolChar) {
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position,dir,1f,CharacterMask);
            if (hit2.collider != null) {
                return false;
            }
        }
        return true;
    }
    bool AttackCheck()
    {
        if (AttackCooldown) return false;
        int direction = FaceDir * 90;
        Vector3 vect = new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction),Mathf.Sin(Mathf.Deg2Rad * direction));
        vect = (vect * (CheckDistances [1] / 2)) + transform.position;
        float charX = Character.position.x;
        float charY = Character.position.y;
        //Debug.DrawLine(transform.position,vect,Color.red,0.1f);
        
        if (Mathf.Abs(vect.x - charX) < (CheckDistances[(FaceDir+1) % 2] / 2) && Mathf.Abs(vect.y - charY) < (CheckDistances[FaceDir % 2] / 2)) {
            AttackMode = true;
            anim.SetBool("Idle",true);
            StartCoroutine(AttackPreparation());
            return true;
        }
        return false;
    }


    void ChangeAnimation(int dir)
    {
        anim.SetInteger("FaceDir",dir);
        anim.SetTrigger("trigDirection");
    }


    IEnumerator AttackPreparation()
    {
        Color sprColor = sprRenderer.color;
        while (sprColor.b > 0) {
            sprColor.b -= (Time.deltaTime / CastingTime);
            sprRenderer.color = sprColor;
            yield return null;
        }
        anim.SetBool("Idle",false);
        StartCoroutine(Attack());

    }

    IEnumerator Attack() {
        int direction = FaceDir * 90;
        AttackSpeed = AttackSpeedMax;
        Vector3 vect = new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction),Mathf.Sin(Mathf.Deg2Rad * direction));
        while (AttackMode && AttackSpeed > 0) {
            Vector3 attackVector = vect * AttackSpeed * Time.deltaTime;
            if (CanWalk(attackVector, false)) {
                transform.Translate(attackVector);
                AttackSpeed -= AttackFriction * Time.deltaTime;
                yield return null;
            }
            else {
                break;
            }
        }
        StopAttacking();
    }

    IEnumerator Cooldown()
    {
        var time = AttackCooldownTime;
        while (time > 0f) {
            time -= Time.deltaTime;
            yield return null;
        }
        AttackCooldown = false;
    }

    public IEnumerator Parry()
    {
        int direction = ((FaceDir + 2) % 4) * 90;
        Vector3 vect = new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction),Mathf.Sin(Mathf.Deg2Rad * direction));
        boolParry = true;
        ParrySpeed = ParrySpeedMax;
        yield return new WaitForSeconds(0.1f);
        while (ParrySpeed > 0f) {
            if (this == null) {
                yield break;
            }
            Vector3 parryVector = vect * ParrySpeed * Time.deltaTime;
            if (CanWalk(parryVector, true)) {
                transform.Translate(parryVector);
                ParrySpeed -= ParryFriction * Time.deltaTime;
                yield return null;
            }
            else {
                break;
            }
        }
        boolParry = false;
    }

    public void StopAttacking()
    {
        AttackMode = false;
        AttackSpeed = 0f;
        AttackCooldown = true;
        sprRenderer.color = Color.white;
        Destination = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(Cooldown());
    }

    public void ColorChange()
    {
        sprRenderer.material = PlayerLives.matDefault;
    }

}
