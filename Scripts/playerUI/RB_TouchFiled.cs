using UnityEngine;
using UnityEngine.EventSystems;


public class RB_TouchFiled : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isPressed;
    private float holdTimer;

    [SerializeField] private float holdThreshold = 0.25f;

    private PlayerInputManager input;

    private void Awake()
    {
        input = PlayerInputManager.Instance;
    }

    private void Update()
    {
        if (!isPressed)
            return;

        holdTimer += Time.deltaTime;

        // HOLD
        if (holdTimer >= holdThreshold)
        {
            input.Hold_RB_Input = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        holdTimer = 0f;

        // Start press (do NOT fire yet)
        input.Hold_RB_Input = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // TAP
        if (holdTimer < holdThreshold)
        {
            input.RB_Input = true;
        }

        // RELEASE
        input.Hold_RB_Input = false;

        isPressed = false;
        holdTimer = 0f;
    }
}
