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
        if (Input.GetKeyDown(KeyCode.Return)) { 
            Debug.Log("Submitting word: " + currentText);
            int objectPos = containsObject(currentText);
            if (objectPos >= 0)
            {
                bankCanvas.alpha = 0f;
                Vector3 offset = new Vector3(3.5f, 3f, 0);
                Debug.Log("Creating object: " + englishValidObjects[objectPos].name);
                Instantiate(englishValidObjects[objectPos], new Vector3(0,0, -0.1f), Quaternion.identity);
                //Play loud correct buzzer sound
            } else
            {
                //Play loud incorrect buzzer sound
            }
            currentText = string.Empty;
            textBar.text = string.Empty;
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            currentText = currentText.Remove(currentText.Length - 1);
            textBar.text = currentText;
        }
        else if (Input.GetKeyDown(KeyCode.LeftControl))
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
        else if (Input.GetKeyDown(KeyCode.Q)) {
            currentText += "\u0161";
            textBar.text = currentText;
        }
        else if (Input.inputString != "")
        {
            currentText += Input.inputString;
            textBar.text = currentText;
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

    // SAVE DATA
    public static List<GameObject> spawnedObjects = new List<GameObject>();

    void SpawnObject(int index)
    {
        Vector3 offset = new Vector3(3.5f, 3f, 0);
        GameObject obj = Instantiate(englishValidObjects[index], player.transform.position + offset, Quaternion.identity);
        spawnedObjects.Add(obj);
    }
}
