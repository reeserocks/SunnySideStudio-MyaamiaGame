using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    public GameObject player;
    public GameObject itemSpawner;

    private bool playerActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemSpawner.SetActive(false);   
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
            Debug.Log("Switching to typing mode");
        }
        else
        {
            player.SetActive(true);
            itemSpawner.SetActive(false);
            Debug.Log("Switching to player mode");
        }
        playerActive = !playerActive;
    }
}
