using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{
    public TMP_InputField textBar;
    public GameObject player;
    private string currentText = "";
    public List<GameObject> myaamiaValidObjects = new List<GameObject>();
    public List<string> englishValidObjects = new List<string>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space bar has been pressed");
        }

        if (Input.GetKeyDown(KeyCode.Return)) { 
            Debug.Log("Submitting word: " + currentText);
            int objectPos = containsObject(currentText);
            if (objectPos >= 0)
            {
                Vector3 offset = new Vector3(3.5f, 3f, 0);
                Instantiate(myaamiaValidObjects[objectPos], player.transform.position + offset, Quaternion.identity);
                //Play loud correct buzzer sound
            } else
            {
                //Play loud incorrect buzzer sound
            }
            currentText = string.Empty;
            textBar.text = string.Empty;
            new WaitForSeconds(.2f);
        }
        else if (Input.inputString != "") {
            currentText += Input.inputString;
            textBar.text = currentText;
        }
    }

    int containsObject(string search)
    {
        int index = 0;
        foreach (string name in englishValidObjects)
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
