using UnityEngine;
using UnityEngine.SceneManagement;

public class Statue : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (EggCollector.EggCount >= EggCollector.EggMax) {
            if (other.CompareTag("Umbrella")) {
                SceneManager.LoadScene("ThanksForPlaying");
            }
        }
    }
}
