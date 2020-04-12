using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{    
    //Movement
    private bool MovementPermission = true;
    [SerializeField] private float MoveSpeedMax = 10f;
    private float MoveSpeed;
    [HideInInspector] public int FaceDir;
    private bool isWalking = false;
    [HideInInspector] public bool isShielding = false;
    [HideInInspector] public bool isAttacking = false; 
    [SerializeField] private float ParryTimeMax = 0.5f;
    [HideInInspector] public float ParryTime = 0f;

    //Attack
    [SerializeField] private float AttackCooldownMax = 1f;
    /*[HideInInspector]*/ public float [] AttackCooldowns = new float [3];
    [SerializeField] private float AttackSpeedMax = 20f;
    [SerializeField] private float AttackSpeedMin = 2f;
    [SerializeField] private float AttackFriction = 30f;
    private float AttackSpeed = 20f;

    //Inputs
    private float horInput = 0f;
    private float verInput = 0f;
    private float horSpeed = 0f;
    private float verSpeed = 0f;

    //Knockback
    [SerializeField] private float KnockbackSpeedMax = 8f;
    [SerializeField] private float KnockbackFriction = 30f;
    private float KnockbackSpeed = 0f;

    //Others
    public static SpriteRenderer sprRenderer;
    private LayerMask BlocksMask;
    private Animator animator;
    [SerializeField] private Slider [] Sliders = new Slider [3];
    [SerializeField] private Transform ParticleParent;
    [SerializeField] private GameObject CharacterEffect;


    void Awake()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        FaceDir = 3;
        BlocksMask = LayerMask.GetMask("Blocks");
        MoveSpeed = MoveSpeedMax;
    }

    void Update()
    {
        Cooldown();
        HandleInput();

        if (Input.GetButtonDown("Enter")) {
            ChickenSpawner.CreateChicken();
        }
    }

    void HandleInput()
    {
        if (!MovementPermission) return;

        //Girdileri topla
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
        bool attackInput = Input.GetButtonDown("Attack");
        bool shieldInput = Input.GetButton("Shield");

        //Kalkan
        if (shieldInput && !attackInput && !isAttacking && !isShielding) {
            isShielding = true;
            isWalking = false;
            animator.SetTrigger("trigShield");
            ParryTime = ParryTimeMax;
        }

        //Atak
        else if (!shieldInput && attackInput && !isAttacking) {
            bool CanAttack = true;
            int selection = AttackCooldowns.Length - 1;
            while (AttackCooldowns [selection] > 0f) {
                selection--;
                if (selection == -1) {
                    CanAttack = false;
                    break;
                }
            }
            if (CanAttack) {
                SoundManager.PlaySound(SoundManager.DashSound);
                for(int i = selection; i < AttackCooldowns.Length; i++) {
                    Sliders [i].value = 0f;
                    AttackCooldowns[i] = AttackCooldownMax;
                }
                isAttacking = true;
                isShielding = false;
                isWalking = false;
                animator.SetTrigger("trigAttack");
                AttackCooldowns[selection] = AttackCooldownMax;
                if (horInput == 0f && verInput == 0f) {
                    Vector2 Vect = Vector2.right;
                    for (var i = 0; i < FaceDir; i++) {
                        Vect = Vector2.Perpendicular(Vect);
                    }
                    horSpeed = Vect.x;
                    verSpeed = Vect.y;
                }
            }
        }

        //Kalkan veya atak sonlandığı zaman için ufak ayarlamalar
        if (!shieldInput) {
            isShielding = false;
        }

        if (!isAttacking) {
            horSpeed = horInput;
            verSpeed = verInput;
        }

        //Gitmeye çalıştığımız yere gidebiliyor muyuz? Ona göre ayarlamalar yapılsın
        if (horInput != 0 || verInput != 0) {
            if (CanWalk(new Vector2(horInput,verInput))) {
            }
            else if (CanWalk(new Vector2(horInput,0f))) {
                verSpeed = 0f;
            }
            else if (CanWalk(new Vector2(0f,verInput))) {
                horSpeed = 0f;
            }
            else {
                horSpeed = 0f;
                verSpeed = 0f;
            }
        }

        //Kalkan ve atak durumu yoksa yürüme ile ilgili ayarlamaları yap
        int tempDir = FaceDir;
        bool tempWalking = isWalking;
        if (horSpeed != 0) {
            tempDir = 1 - Mathf.FloorToInt(Mathf.Sign(horSpeed));
            if (!isShielding && !isAttacking) {
                isWalking = true;
            }
        }
        else if (verSpeed != 0) {
            tempDir = 2 - Mathf.FloorToInt(Mathf.Sign(verSpeed));
            if (!isShielding && !isAttacking) {
                isWalking = true;
            }
        }
        else {
            isWalking = false;
        }
        
        //Animasyon işlerini hallet
        if (isWalking != tempWalking) {
            animator.SetTrigger("trigWalk");
        }

        if (FaceDir != tempDir) {
            FaceDir = tempDir;
            if (isWalking) {
                animator.SetTrigger("trigWalk");
            }
            else if (isShielding) {
                animator.SetTrigger("trigShield");
            }
        }
        animator.SetBool("isWalking",isWalking);
        animator.SetBool("isShielding",isShielding);
        animator.SetBool("isAttacking",isAttacking);
        animator.SetInteger("FaceDir",FaceDir);

        if (horInput == 0f && verInput == 0f) {
            animator.SetBool("Idle",true);
        }
        else {
            animator.SetBool("Idle",false);
        }

        //Pozisyon güncellemesini gerçekleştir
        if (!isAttacking) {
            Vector2 direction = new Vector2(horSpeed,verSpeed).normalized * MoveSpeed * Time.deltaTime;
            transform.Translate(direction);
        }
        else {
            Vector2 direction = new Vector2(horSpeed,verSpeed).normalized * AttackSpeed * Time.deltaTime;
            bool boolCanWalk = CanWalk(direction);
            if (boolCanWalk) {
                transform.Translate(direction);
                AttackSpeed -= AttackFriction * Time.deltaTime;
            }
            if (!boolCanWalk || AttackSpeed <= AttackSpeedMin || (horSpeed == 0 && verSpeed == 0)) {
                AttackSpeed = AttackSpeedMax;
                isAttacking = false;
                animator.SetBool("isAttacking",isAttacking);
            }
        }
    }

    bool CanWalk(Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,dir,1f,BlocksMask);
        if (hit.collider != null) {
            return false;
        }
        return true;
    }

    void Cooldown()
    {
        for (int i = 0; i < AttackCooldowns.Length; i++) {
            if (AttackCooldowns[i] > 0f) {
                AttackCooldowns[i] = Mathf.Max(AttackCooldowns[i] - Time.deltaTime, 0f);
                Sliders [i].value = 1f - (AttackCooldowns [i] / AttackCooldownMax);
                if (AttackCooldowns[i] == 0f) {
                    SpriteRenderer characterEffect = Instantiate(CharacterEffect,transform.position,Quaternion.identity,transform).GetComponent<SpriteRenderer>();
                    characterEffect.sprite = sprRenderer.sprite;
                }
                break;
            }
        }

        if (ParryTime > 0) {
            ParryTime = Mathf.Max(ParryTime - Time.deltaTime, 0f);
        }
    }

    public IEnumerator Knockback(int direction)
    {
        MovementPermission = false;
        direction = direction * 90;
        Vector3 vect = new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction),Mathf.Sin(Mathf.Deg2Rad * direction));
        KnockbackSpeed = KnockbackSpeedMax;
        while (KnockbackSpeed > 0f) {
            Vector3 parryVector = vect * KnockbackSpeed * Time.deltaTime;
            if (CanWalk(parryVector)) {
                transform.Translate(parryVector);
                KnockbackSpeed -= KnockbackFriction * Time.deltaTime;
                yield return null;
            }
            else {
                break;
            }
        }
        MovementPermission = true;
    }

    public void StopAttacking()
    {
        if (isAttacking) {
            isAttacking = false;
            animator.SetBool("isAttacking",isAttacking);
        }
    }
}
