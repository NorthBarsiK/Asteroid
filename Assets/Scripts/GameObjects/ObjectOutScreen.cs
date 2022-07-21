using UnityEngine;

public class ObjectOutScreen : MonoBehaviour
{
    [SerializeField] private float offset = 1.0f;

    private Rect screenRect = new Rect();
    private Camera cam = null;

    private void Start()
    {
        cam = Camera.main;
        screenRect = new Rect(-offset, -offset, Screen.width + offset, Screen.height + offset);
    }

    private void Update()
    {
        CheckObjectInScreen();
    }

    private void CheckObjectInScreen()
    {
        Vector3 newPosition = Vector3.zero;
        Vector3 currentPosition = cam.WorldToScreenPoint(this.transform.position);

        if (currentPosition.x > screenRect.xMax + offset)
        {
            newPosition.x = cam.ScreenToWorldPoint(new Vector3(screenRect.xMin, 0, 0)).x;
        }
        else if (currentPosition.x < screenRect.xMin - offset)
        {
            newPosition.x = cam.ScreenToWorldPoint(new Vector3(screenRect.xMax, 0, 0)).x;
        }
        else
        {
            newPosition.x = transform.position.x;
        }

        if (currentPosition.y > screenRect.yMax + offset)
        {
            newPosition.y = cam.ScreenToWorldPoint(new Vector3(0, screenRect.yMin, 0)).y;
        }
        else if (currentPosition.y < screenRect.yMin - offset)
        {
            newPosition.y = cam.ScreenToWorldPoint(new Vector3(0, screenRect.yMax, 0)).y;
        }
        else
        {
            newPosition.y = transform.position.y;
        }

        if (newPosition != transform.position)
        {
            transform.position = newPosition;
        }
    }
}
