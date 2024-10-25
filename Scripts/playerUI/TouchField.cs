using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 pointerOld;
    private int pointerId;
    private bool pressed;

    public Vector2 SwipeDelta { get; private set; }
    public float touchSensitivity = 24f; // Added touch sensitivity

    private void Update()
    {
        if (pressed)
        {
            if (pointerId >= 0 && pointerId < Input.touches.Length)
            {
                SwipeDelta = (Input.touches[pointerId].position - pointerOld) * touchSensitivity;
                pointerOld = Input.touches[pointerId].position;
            }
            else
            {
                SwipeDelta = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld) * touchSensitivity;
                pointerOld = Input.mousePosition;
            }
        }
        else
        {
            SwipeDelta = Vector2.zero;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        pointerId = eventData.pointerId;
        pointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }
}
