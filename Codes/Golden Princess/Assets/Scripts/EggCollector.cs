using UnityEngine.UI;
using UnityEngine;

public class EggCollector : MonoBehaviour
{
    public static int EggCount = 0;
    [SerializeField] private GameObject GoldenEggParticles;
    [SerializeField] private Transform ParticlesParent;
    [SerializeField] private Text GoldenEggText;

    private void Start()
    {
        EggCount = 0;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Egg") {
            EggCount++;
            GoldenEggText.text = "X " + EggCount;
            Instantiate(GoldenEggParticles,other.transform.position,Quaternion.identity,ParticlesParent);
            Destroy(other.gameObject,0f);
            SoundManager.PlaySound(SoundManager.EggCollectedSound);
        }
    }
}
