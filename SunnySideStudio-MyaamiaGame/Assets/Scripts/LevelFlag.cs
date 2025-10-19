using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class LevelFlag : MonoBehaviour
{
    public int currentLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        SaveSystem.Save();

        if (other.gameObject.name == "Player")
        {
           
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                GameManager.UnlockLevel(currentLevel);
            
        }
    }
}
