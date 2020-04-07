using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCollector : MonoBehaviour
{
    public static int EggCount = 0;
    [SerializeField] private GameObject GoldenEggParticles;
    [SerializeField] private Transform ParticlesParent;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Egg") {
            EggCount++;
            Instantiate(GoldenEggParticles,other.transform.position,Quaternion.identity,ParticlesParent);
            Destroy(other.gameObject,0f);
        }
    }
}
