using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0.0f , 2.25f, -0.6f);

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;
        }
    }
}
