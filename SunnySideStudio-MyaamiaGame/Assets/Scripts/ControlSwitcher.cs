using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    private static Player player;
    private static ItemSpawner itemSpawner;
    private static GameObject gameCanvas;
    private static GameObject inCamera;
    private static GameObject outCamera;

    private bool playerActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = Player.FindFirstObjectByType<Player>();
        itemSpawner = ItemSpawner.FindAnyObjectByType<ItemSpawner>();
        gameCanvas = GameObject.Find("ItemTextBox");
        inCamera = GameObject.Find("InCamera");
        outCamera = GameObject.Find("OutCamera");
        gameCanvas.SetActive(false);
        inCamera.SetActive(true);
        outCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.isPlacing == false)
        {
            SwitchController();
        }
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    SwitchCamera();
        //}
    }

    void SwitchCamera()
    {
        inCamera.SetActive(!inCamera.activeSelf);
        outCamera.SetActive(!outCamera.activeSelf);
    }

    void SwitchController()
    {
        if (playerActive)
        {
            player.canMove = false;
            itemSpawner.canType = true;
            gameCanvas.SetActive(true);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("page_turn"), Camera.main.transform.position);
            Debug.Log("Switching to typing mode");
        }
        else
        {
            player.canMove = true;
            itemSpawner.canType = false;
            gameCanvas.SetActive(false);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("page_turn"), Camera.main.transform.position);
            Debug.Log("Switching to player mode");
        }
        playerActive = !playerActive;
    }
}
