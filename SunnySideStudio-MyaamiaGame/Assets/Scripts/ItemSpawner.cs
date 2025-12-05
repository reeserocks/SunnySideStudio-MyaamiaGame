using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{
    public TMP_InputField textBar;
    public GameObject wordBank;
    public GameObject player;
    private string currentText = "";
    public CanvasGroup bankCanvas;
    public List<GameObject> englishValidObjects = new List<GameObject>();
    public List<string> myaamiaValidObjects = new List<string>();

    private Stack<GameObject> objectsStack = new Stack<GameObject>();
    private int itemCount = 0;

    [SerializeField] AudioClip audioClipSuccess;
    [SerializeField] AudioClip audioClipFail;

    public GameObject limitText;

    private void Start()
    {
        wordBank = GameObject.Find("WordBank");
    }

    private void Awake()
    {
        player = GameObject.Find("Player");
        bankCanvas = wordBank.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject item in objectsStack)
        {
            if (item.transform.position.y < -10)
            {
                Destroy(objectsStack.Pop());
                itemCount--;
            }
        }
        if (GameManager.canType)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Submitting word: " + currentText);
                if (itemCount < 5)
                {
                    int objectPos = containsObject(currentText);
                    if (objectPos >= 0)
                    {
                        itemCount++;
                        bankCanvas.alpha = 0f;
                        Vector3 offset = new Vector3(2f, 1f, 0);
                        Debug.Log("Creating object: " + englishValidObjects[objectPos].name);
                        AudioSource.PlayClipAtPoint(audioClipSuccess, Camera.main.transform.position);
                        GameObject objectSpawned = Instantiate(englishValidObjects[objectPos], player.transform.position + offset, Quaternion.identity);
                        objectsStack.Push(objectSpawned);
                        GameManager.isPlacing = true;
                        GameManager.canType = false;
                    }
                    else if (!GameManager.isPlacing)
                    {
                        
                        AudioSource.PlayClipAtPoint(audioClipFail, Camera.main.transform.position);
                    }
                    currentText = string.Empty;
                    textBar.text = string.Empty;
                }
                else
                {
                    currentText = string.Empty;
                    textBar.text = string.Empty;

                    setText();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                currentText = currentText.Remove(currentText.Length - 1);
                textBar.text = currentText;
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) && !GameManager.hardModeEnabled)
            {
                Debug.Log("Switching word bank mode");
                if (bankCanvas.alpha == 0f)
                {
                    bankCanvas.alpha = 1f;
                }
                else
                {
                    bankCanvas.alpha = 0f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                currentText += "\u0161";
                textBar.text = currentText;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                currentText = "";
                bankCanvas.alpha = 0f;
                textBar.text = currentText;
            }
            else if (Input.inputString != "" && !Input.GetKeyDown(KeyCode.R))
            {
                currentText += Input.inputString;
                textBar.text = currentText;
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !GameManager.isPlacing)
        {
            Destroy(objectsStack.Pop());
            itemCount--;
        }
    }

    int containsObject(string search)
    {
        int index = 0;
        foreach (string name in myaamiaValidObjects)
        {
            if (name.ToLower() == search.ToLower())
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    private void setText()   
    {
        limitText.SetActive(true);
        StartCoroutine(HideText(2f));
    }

    IEnumerator HideText(float delay)
    {
        yield return new WaitForSeconds(delay);
        limitText.SetActive(false);
    }


    // SAVE DATA
    public static List<GameObject> spawnedObjects = new List<GameObject>();
}
