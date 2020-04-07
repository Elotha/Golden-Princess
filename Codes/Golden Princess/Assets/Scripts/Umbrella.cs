using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour {
    private GameObject Character;
    private CharacterController ParentScript;
    private ChickenController ChickenScript;
    [SerializeField] private GameObject ChickenParticles;
    [SerializeField] private Transform ParticlesParent;
    [SerializeField] private GameObject GoldenEgg;
    [SerializeField] private Transform GoldenEggParent;
    private PlayerLives playerLives;

    // Start is called before the first frame update
    void Start()
    {
        ParentScript = transform.GetComponentInParent<CharacterController>();
        playerLives = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<PlayerLives>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Chicken") { 
            ChickenScript = other.gameObject.GetComponent<ChickenController>();
            Color chickenColor = ChickenScript.sprRenderer.color;
            int _faceDir = ChickenScript.FaceDir;
            int _direction = ParentScript.FaceDir;
            if (chickenColor.b <= 0 ) { //Tavuğun rengi tamamen sarı ise
                if (ParentScript.isShielding) { //Karakter kalkan açmış mı?
                    if (_faceDir == (_direction + 2) % 4) { //Tavuğun yönü ile karakterin yönü eşleşiyorsa
                        if (ParentScript.ParryTime > 0f) { //Parry
                            Debug.Log("parry");
                            ChickenScript.sprRenderer.material = PlayerLives.matWhite;
                            ChickenScript.Invoke("ColorChange",0.2f);
                            ChickenScript.StopAttacking();
                            StartCoroutine(ChickenScript.Parry());
                            return;
                        }
                        else { //Kalkan
                            ChickenScript.StopAttacking();
                            StartCoroutine(ParentScript.Knockback(_faceDir));
                            Debug.Log("shield");
                            return;
                        }
                    }
                }
                //Kalkan açık değil veya yanlış yöne bakıyor, oyuncu hasar alır
                ChickenScript.StopAttacking();
                StartCoroutine(ParentScript.Knockback(_faceDir));
                playerLives.TakeDamage();
                StartCoroutine(CameraEffect.ShakingEffect(_faceDir));

            }
            else {
                if (ParentScript.isAttacking) { //Karakter saldırıyorsa
                    Instantiate(ChickenParticles,other.transform.position,Quaternion.identity,ParticlesParent);
                    Vector3 pos = new Vector3(other.transform.position.x,transform.position.y + 0.5f,0f);
                    StartCoroutine(CreateEgg(pos));
                    Destroy(other.gameObject);
                }
            }
        }
    }

    IEnumerator CreateEgg(Vector2 pos)
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(GoldenEgg,pos,Quaternion.identity,GoldenEggParent);
    }
}
