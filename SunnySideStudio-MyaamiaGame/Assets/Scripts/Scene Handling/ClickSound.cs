using UnityEngine;

public class ClickSound : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    public void PlaySound()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position);
    }
}
