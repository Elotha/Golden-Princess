using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public static SpriteRenderer sprRenderer;
    public static GameObject Character;
    private float MoveSpeed = 10f;
    private Animator animator;
    [HideInInspector] public int FaceDir;
    private bool isWalking = false;
    [HideInInspector] public bool isShielding = false;
    [HideInInspector] public bool isAttacking = false; 
    private bool MovementPermission = false;
    [SerializeField] private float AttackCooldownMax = 60f;
    [SerializeField] private float AttackCooldown = 60f;
    private float AttackSpeed = 20f;
    [SerializeField] private float AttackSpeedMax = 20f;
    [SerializeField] private float AttackSpeedMin = 2f;
    [SerializeField] private float AttackFriction = 0.2f;
    private float horInput = 0f;
    private float verInput = 0f;
    private float horSpeed = 0f;
    private float verSpeed = 0f;
    [HideInInspector] public float ParryTime = 0f;
    private LayerMask BlocksMask;
    private bool boolKnockBack = false;
    [SerializeField] private float KnockbackSpeedMax = 8f;
    [SerializeField] private float KnockbackFriction = 0.2f;
    private float KnockbackSpeed = 0f;


    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        FaceDir = 3;
        BlocksMask = LayerMask.GetMask("Blocks");
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
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
        bool attackInput = Input.GetButtonDown("Attack");
        bool shieldInput = Input.GetButton("Shield");
        if (shieldInput && !attackInput && !isAttacking && !isShielding) {
            isShielding = true;
            isWalking = false;
            animator.SetTrigger("trigShield");
            ParryTime = 0.5f;
        }
        else if (!shieldInput && attackInput && !isAttacking && AttackCooldown == 0) {
            isAttacking = true;
            isShielding = false;
            isWalking = false;
            animator.SetTrigger("trigAttack");
            AttackCooldown = AttackCooldownMax;
            if (horInput == 0f && verInput == 0f) {
                Vector2 Vect = Vector2.right;
                for(var i = 0; i < FaceDir; i++) {
                    Vect = Vector2.Perpendicular(Vect);
                }
                horSpeed = Vect.x;
                verSpeed = Vect.y;
            }
        }
        if (!shieldInput) {
            isShielding = false;
        }

        if (!isAttacking) {
            horSpeed = horInput;
            verSpeed = verInput;
        }

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

        if (!isAttacking) {
            Vector2 direction = new Vector2(horSpeed,verSpeed).normalized * MoveSpeed * Time.deltaTime;
            transform.Translate(direction);
        }
        else {
            Vector2 direction = new Vector2(horSpeed,verSpeed).normalized * AttackSpeed * Time.deltaTime;
            bool boolCanWalk = CanWalk(direction);
            if (boolCanWalk) {
                transform.Translate(direction);
                AttackSpeed -= AttackFriction;
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
        if (AttackCooldown > 0) {
            AttackCooldown -= 0.5f;
        }
        if (ParryTime > 0) {
            ParryTime -= Time.deltaTime;
        }
    }

    public IEnumerator Knockback(int direction)
    {
        boolKnockBack = true;
        direction = direction * 90;
        Vector3 vect = new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction),Mathf.Sin(Mathf.Deg2Rad * direction));
        KnockbackSpeed = KnockbackSpeedMax;
        while (KnockbackSpeed > 0f) {
            Vector3 parryVector = vect * KnockbackSpeed * Time.deltaTime;
            if (CanWalk(parryVector)) {
                transform.Translate(parryVector);
                KnockbackSpeed -= KnockbackFriction;
                yield return null;
            }
            else {
                break;
            }
        }
        boolKnockBack = false;
    }
}
