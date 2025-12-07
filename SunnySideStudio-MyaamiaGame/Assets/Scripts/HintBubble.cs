using System.Collections;
using UnityEngine;

public class HintBubble : MonoBehaviour
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas.worldCamera = GameObject.Find("InCamera").GetComponent<Camera>();
        canvasGroup.alpha = 1.0f;
        StartCoroutine(fadeOut());
    }

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(.04f);
        canvasGroup.alpha -= .01f;
        StartCoroutine(fadeOut());
    }
}
