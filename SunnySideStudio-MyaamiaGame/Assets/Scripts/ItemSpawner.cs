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
    public List<GameObject> englishValidObjects = new List<GameObject>();
    public List<string> myaamiaValidObjects = new List<string>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) { 
            Debug.Log("Submitting word: " + currentText);
            int objectPos = containsObject(currentText);
            if (objectPos >= 0)
            {
                wordBank.SetActive(false);
                Vector3 offset = new Vector3(3.5f, 3f, 0);
                Instantiate(englishValidObjects[objectPos], player.transform.position + offset, Quaternion.identity);
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
            if (wordBank.activeSelf)
            {
                wordBank.SetActive(false);
            }
            else
            {
                wordBank.SetActive(true);
            }
        }
        else if (Input.inputString != "") {
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
}
