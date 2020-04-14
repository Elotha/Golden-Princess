using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadMenu",2f);
    }

    private void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
