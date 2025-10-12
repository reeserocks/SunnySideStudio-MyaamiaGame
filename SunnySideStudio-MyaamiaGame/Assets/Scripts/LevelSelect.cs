using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public int level;

    public void OpenScene()
    {
        Debug.Log(level);
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
