using UnityEngine;
using UnityEngine.SceneManagement;

public class EditorMode : MonoBehaviour
{
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown("f4")) { //Bölümü yeniden başlat
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        if (Input.GetKeyDown("f5") || Input.GetKeyDown(KeyCode.Escape) ) { //Oyunu yeniden başlat
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown("f12")) { //Oyunu kapat
            Application.Quit();
        }
    }
}
