using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSelect : MonoBehaviour
{
    public int world;

    public void OpenScene()
    {
        SceneManager.LoadScene("World" + world.ToString() +"LevelSelect");
    }
}
