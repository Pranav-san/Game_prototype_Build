using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPop : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler
{
    public float popScale = 1.2f;
    public float scaleSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHeld = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        // Smoothly scale to target
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isHeld = true;
        targetScale = originalScale * popScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHeld = false;
        targetScale = originalScale;
    }

    void OnDisable()
    {
        // Reset scale in case button is disabled while held
        transform.localScale = originalScale;
        targetScale = originalScale;
        isHeld = false;
    }
}
