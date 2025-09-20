using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{
    public TMP_InputField textBar;
    private string currentText = "";

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space bar has been pressed");
        }

        if (Input.GetKeyDown(KeyCode.Return)) { 
            Debug.Log("Submitting word: " + currentText);
            currentText = string.Empty;
            textBar.text = string.Empty;
        }

        if (Input.inputString != "") {
            currentText += Input.inputString;
            textBar.text = currentText;
        }
    }
}
