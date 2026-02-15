using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 pointerOld;
    private int pointerId = -1;
    private ETouch.Finger trackedFinger;
    private bool pressed = false;

    [Header("Pinch")]
    public float PinchDelta;

    public Vector2 SwipeDelta { get; private set; }
    public float touchSensitivity = 24f;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {

        if (ETouch.Touch.activeTouches.Count == 2)
        {
            var t0 = ETouch.Touch.activeTouches[0];
            var t1 = ETouch.Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(t0.screenPosition, t1.screenPosition);
            float previousDistance = Vector2.Distance(
                t0.screenPosition - t0.delta,
                t1.screenPosition - t1.delta
            );

            PinchDelta = currentDistance - previousDistance; // positive = zoom out, negative = zoom in
        }
        else
        {
            PinchDelta = 0f;

        }
        if (!pressed || trackedFinger == null)
        {
            SwipeDelta = Vector2.zero;
            return;
        }

        foreach (var touch in ETouch.Touch.activeTouches)
        {
            if (touch.finger == trackedFinger)
            {
                Vector2 currentPos = touch.screenPosition;
                SwipeDelta = (currentPos - pointerOld) * touchSensitivity;
                pointerOld = currentPos;
                return;
            }
        }

        // Finger lifted or lost
        SwipeDelta = Vector2.zero;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
        pointerOld = eventData.position;
        pointerId = eventData.pointerId;

        // Try to find the correct Finger by screen position (since Input System doesn't expose pointerId directly)
        foreach (var touch in ETouch.Touch.activeTouches)
        {
            if (Vector2.Distance(touch.screenPosition, eventData.position) < 50f)
            {
                trackedFinger = touch.finger;
                break;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == pointerId)
        {
            pressed = false;
            pointerId = -1;
            trackedFinger = null;
            SwipeDelta = Vector2.zero;
        }
    }
}