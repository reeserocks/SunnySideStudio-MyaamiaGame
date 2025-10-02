using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    public GameObject player;
    public GameObject itemSpawner;
    public GameObject gameCanvas;

    private bool playerActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSpawner.SetActive(false);
        gameCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchController();
        }
    }

    void SwitchController()
    {
        if (playerActive)
        {
            player.SetActive(false);
            itemSpawner.SetActive(true);
            gameCanvas.SetActive(true);
            Debug.Log("Switching to typing mode");
        }
        else
        {
            player.SetActive(true);
            itemSpawner.SetActive(false);
            gameCanvas.SetActive(false);
            Debug.Log("Switching to player mode");
        }
        playerActive = !playerActive;
    }
}
