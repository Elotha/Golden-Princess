using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour {
    GameObject Character;
    CharacterController ParentScript;
    ChickenController ChickenScript;

    // Start is called before the first frame update
    void Start()
    {
        ParentScript = transform.GetComponentInParent<CharacterController>();
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
                        if (ParentScript.ParryTime > 0f) { //Parry süresi bitmemiş
                            Debug.Log("parry");
                            ChickenScript.StopAttacking();
                            StartCoroutine(ChickenScript.Parry());
                            return;
                        }
                        else { //Parry süresi bitmiş
                            ChickenScript.StopAttacking();
                            StartCoroutine(ParentScript.Knockback(_faceDir));
                            Debug.Log("shield");
                            return;
                        }
                    }
                }
                //Kalkan açık değil veya yanlış yöne bakıyor
                Debug.Log("chicken attacks");
                ChickenScript.StopAttacking();
                StartCoroutine(ParentScript.Knockback(_faceDir));
                PlayerHealth.TakeDamage();
            }
            else {
                if (ParentScript.isAttacking) { //Karakter saldırıyorsa
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
