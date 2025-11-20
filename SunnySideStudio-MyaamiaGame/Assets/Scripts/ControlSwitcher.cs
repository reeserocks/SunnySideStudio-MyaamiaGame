using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    private static Player player;
    private static ItemSpawner itemSpawner;
    private static GameObject gameCanvas;
    private static GameObject inCamera;
    private static GameObject outCamera;

    private bool playerActive = true;
    private bool levelStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        SetLevelStart();
    }

    public void SetLevelStart()
    {
        player = Player.FindFirstObjectByType<Player>();
        itemSpawner = ItemSpawner.FindAnyObjectByType<ItemSpawner>();
        gameCanvas = GameObject.Find("ItemTextBox");
        inCamera = GameObject.Find("InCamera");
        outCamera = GameObject.Find("OutCamera");
        gameCanvas.SetActive(false);
        inCamera.SetActive(false);
        outCamera.SetActive(true);
        player.canMove = false;
        levelStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelStarted && Input.anyKeyDown) { 
            player.canMove = true;
            levelStarted = false;
            SwitchCamera();
        }
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.isPlacing == false && player.isGrounded)
        {
            SwitchController();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCamera();
        }
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
            player.SetBook(true);
            player.canMove = false;
            GameManager.canType = true;
            gameCanvas.SetActive(true);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("page_turn"), Camera.main.transform.position);
            Debug.Log("Switching to typing mode");
        }
        else
        {
            player.SetBook(false);
            player.canMove = true;
            GameManager.canType = false;
            gameCanvas.SetActive(false);
            AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("page_turn"), Camera.main.transform.position);
            Debug.Log("Switching to player mode");
        }
        playerActive = !playerActive;
    }
}
