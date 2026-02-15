using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPop : MonoBehaviour, IPointerDownHandler,  IPointerUpHandler
{
    public float popScale = 1.2f;
    public float scaleSpeed = 10f;
    public Button ignoreClickBTN;

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
        if (ignoreClickBTN != null && eventData.pointerEnter == ignoreClickBTN.gameObject)
        {
            return;
        }
            
        isHeld = true;
        targetScale = originalScale * popScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (ignoreClickBTN != null && eventData.pointerEnter == ignoreClickBTN.gameObject)
        {

            return;
        }

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
