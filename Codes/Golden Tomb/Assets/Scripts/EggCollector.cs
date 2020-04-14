using UnityEngine.UI;
using UnityEngine;

public class EggCollector : MonoBehaviour
{
    public static int EggCount = 0;
    public static int EggMax = 100;
    [SerializeField] private GameObject GoldenEggParticles;
    [SerializeField] private Transform ParticlesParent;
    [SerializeField] private Text GoldenEggText;
    [SerializeField] private GameObject ElisaText;
    [SerializeField] private Vector3 EggOffset = new Vector3(0f,0.5f,0f);

    private void Start()
    {
        EggCount = 0;
        GoldenEggText.text = EggCount + " / " + EggMax;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg")) {
            EggCount++;
            GoldenEggText.text = EggCount + " / " + EggMax;
            Instantiate(GoldenEggParticles,other.transform.position + EggOffset,Quaternion.identity,ParticlesParent);
            Destroy(other.gameObject,0f);
            SoundManager.PlaySound(SoundManager.EggCollectedSound);
            if (EggCount == EggMax) {
                ElisaText.SetActive(true);
                Debug.Log("text");
            }
        }
    }
}
