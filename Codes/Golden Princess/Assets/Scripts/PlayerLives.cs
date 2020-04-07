using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLives : MonoBehaviour {
    private static int LivesMax = 5;
    public static int Lives = LivesMax;
    public static Image [] Hearts = new Image [5];
    private GameObject HeartParent = null;
    [HideInInspector] public static Material matDefault;
    [HideInInspector] public static Material matWhite;

    void Start () {
        HeartParent = GameObject.FindGameObjectWithTag("HeartParent");
        for (int i = 0; i < HeartParent.transform.childCount; i++) {
            Hearts[i] = HeartParent.transform.GetChild(i).GetComponent<Image>();
        }
        matDefault = CharacterController.sprRenderer.material;
        matWhite = Resources.Load<Material>("WhiteFlash");
        if (matWhite == null) {
            Debug.LogError("null");
        }
        
    }
    public void TakeDamage()
    {
        if (Lives > 1) {
            Lives--;
            if (Lives < Hearts.Length) {
                Hearts [Lives].enabled = false;
            }
            CharacterController.sprRenderer.material = matWhite;
            Invoke("ColorChange",0.1f);
            
        }
        else {
            Debug.Log("You died!");
            Lives = LivesMax;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ColorChange()
    {
        CharacterController.sprRenderer.material = matDefault;
    }
}
