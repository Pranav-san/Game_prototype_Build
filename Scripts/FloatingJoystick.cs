using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    [Header("Settings")]
    public float handleLimit = 75f;
    public bool isFloating = true;    // Enable floating mode
    public bool autoHide = true;
    public bool animateOnInput = true;

    private Vector3 defaultScale;
    private Coroutine scaleCoroutine;

    [HideInInspector] public Vector2 inputVector;
    [HideInInspector] public int joystickFingerId = -1;
    public RectTransform joyStickrect;

    private Vector2 joystickCenter;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        defaultScale = joystickBackground.localScale;
        joyStickrect = GetComponent<RectTransform>();
    }

    void Start()
    {
        joystickCenter = joystickBackground.position;
        inputVector = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystickFingerId = eventData.pointerId;

        if (isFloating)
        {
            joystickBackground.position = eventData.position;
            joystickCenter = eventData.position;

        }
        else
        {
            joystickCenter = joystickBackground.position;
        }

        UnHideJoystick();
        OnDrag(eventData);

        if (animateOnInput)
            StartScaleAnimation(1.1f);

       
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(" OnDrag: " + eventData.position);
        Vector2 direction = eventData.position - joystickCenter;
        float distance = Mathf.Min(direction.magnitude, handleLimit);
        joystickHandle.position = joystickCenter + direction.normalized * distance;

        inputVector = direction.normalized * (distance / handleLimit);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystickFingerId = -1;

        inputVector = Vector2.zero;
        joystickHandle.position = joystickCenter;

        if (autoHide)
            HideJoystick();

        if (animateOnInput)
            StartScaleAnimation(1f);


    }

    public Vector2 GetInput() => inputVector;


    private void HideJoystick()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        //canvasGroup.blocksRaycasts = false;
    }

    public void UnHideJoystick()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void HideJoystickCompletely()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    private void StartScaleAnimation(float targetScale)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);

        scaleCoroutine = StartCoroutine(ScaleJoystick(targetScale));
    }

    private System.Collections.IEnumerator ScaleJoystick(float target)
    {
        float duration = 0.1f;
        float time = 0f;

        Vector3 startScale = joystickBackground.localScale;
        Vector3 endScale = defaultScale * target;

        while (time < duration)
        {
            time += Time.deltaTime;
            joystickBackground.localScale = Vector3.Lerp(startScale, endScale, time / duration);
            yield return null;
        }

        joystickBackground.localScale = endScale;
    }


}
