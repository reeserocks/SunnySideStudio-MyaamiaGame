using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject leftPath;
    public GameObject rightPath;
    private int currDir;

    public void Start()
    {
        if (leftPath.activeSelf)
        {
            currDir = -1;
        }
        else
        {
            currDir = 1;
        }
    }

    public void SwitchDirection(double scale)
    {
        if (scale < 0 && currDir == -1)
        {
            if (leftPath.activeSelf)
            {
                leftPath.SetActive(false);
                rightPath.SetActive(true);
            }
            else
            {
                leftPath.SetActive(true);
                rightPath.SetActive(false);
            }
        }
        else if (scale > 0 && currDir == 1)
        {
            if (rightPath.activeSelf)
            {
                rightPath.SetActive(false);
                leftPath.SetActive(true);
            }
            else
            {
                rightPath.SetActive(true);
                leftPath.SetActive(false);
            }
        }
    }
}
