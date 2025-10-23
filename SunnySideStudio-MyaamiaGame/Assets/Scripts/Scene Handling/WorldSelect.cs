using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSelect : MonoBehaviour
{
    public int world;
    [SerializeField] AudioClip audioClip;

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
}
