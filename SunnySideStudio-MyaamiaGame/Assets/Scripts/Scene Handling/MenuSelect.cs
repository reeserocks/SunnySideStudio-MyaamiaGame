using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelect : MonoBehaviour
{
    public int world;
    [SerializeField] AudioClip audioClip;

    [SerializeField] GameObject menus;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject controlsMenu;
    [SerializeField] GameObject optionsMenu;

    [SerializeField] TextMeshProUGUI buttonText;

    public void OpenScene()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        SceneManager.LoadScene("World" + world.ToString() +"LevelSelect");
    }

    public void QuitGame()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        Application.Quit();
    }

    public void ControlsMenu()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        controlsMenu.SetActive(true);

        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void OptionsMenu()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        optionsMenu.SetActive(true);

        if (pauseMenu != null)
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }
        }
    }

    public void CloseMenu()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        if (optionsMenu.activeSelf)
        {
            optionsMenu.SetActive(false);
        }
        else if (controlsMenu.activeSelf)
        {
            controlsMenu.SetActive(false);
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        menus.SetActive(false);

        Cursor.visible = false;
    }

    public void ToTitle()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
        if (menus != null)
        {
            menus.SetActive(false);
        }
        SceneManager.LoadScene("Title");
    }

    public void HardMode()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);

        if (buttonText.text == "OFF")
        {
            buttonText.text = "ON";
        }
        else
        {
            buttonText.text = "OFF";
        }
    }
}
