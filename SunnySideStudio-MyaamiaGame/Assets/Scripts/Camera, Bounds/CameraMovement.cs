using System.Net.NetworkInformation;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 offset = new Vector3(0.0f , 2.25f, -0.6f);

    private Camera inCamera;
    private Bounds cameraBounds;
    private Vector3 targetPosition;

    private void Awake()
    {
        inCamera = GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        var height = inCamera.orthographicSize;
        var width = height * inCamera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.extents.x - width;

        var minY = Globals.WorldBounds.min.y + height;
        var maxY = Globals.WorldBounds.extents.y - height;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0));
    }

    void LateUpdate()
    {

        targetPosition = player.position + offset;
        targetPosition = GetCameraBounds();

        if (player != null)
        {
            transform.position = targetPosition;
        }
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x),
            Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y),
            transform.position.z
        );
    }
}
