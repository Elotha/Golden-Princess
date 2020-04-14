using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Sprite [] Sprites = new Sprite [6];
    [SerializeField] private Image [] Buttons;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject HowToPlaySection;
    private bool HowToPlay = false;
    private int Selection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (!HowToPlay) { //Main menu
            if (Input.GetButtonDown("Down")) {
                Selection = (Selection + 1) % 3;
                ChangeSelection();

            }
            else if (Input.GetButtonDown("Up")) {
                Selection = (Selection + 2) % 3;
                ChangeSelection();
            }
            else if (Input.GetButtonDown("Enter")) {
                ExecuteSelection();
            }
        }
        else { //How to play section
            if (Input.GetButtonDown("Enter") || Input.GetKeyDown(KeyCode.Escape)) {
                SoundManager.PlaySound(SoundManager.ButtonBackSound);
                MainMenu.SetActive(true);
                HowToPlaySection.SetActive(false);
                HowToPlay = false;
            }
        }
    }

    void ChangeSelection ()
    {
        for (int i = 0; i < Buttons.Length; i++) {
            if (i == Selection) {
                Buttons [i].sprite = Sprites [i * 2 + 1];
            }
            else {
                Buttons [i].sprite = Sprites [i * 2];
            }
        }
        SoundManager.PlaySound(SoundManager.ButtonChangeSound);
    }

    void ExecuteSelection()
    {
        switch(Selection) {
            case 0: //Play
                SoundManager.PlaySound(SoundManager.ButtonSelectedSound);
                SceneManager.LoadScene("MainLevel");
                break;

            case 1: //How to Play
                SoundManager.PlaySound(SoundManager.ButtonSelectedSound);
                MainMenu.SetActive(false);
                HowToPlaySection.SetActive(true);
                HowToPlay = true;
                break;

            case 2: //Exit
                SoundManager.PlaySound(SoundManager.ButtonBackSound);
                Application.Quit();
                break;
        }
    }
}
