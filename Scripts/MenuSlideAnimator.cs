using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuSlideAnimator : MonoBehaviour
{
    public enum SlideDirection { Left, Right, Up, Down }

    [Header("Slide Settings")]
    public SlideDirection slideInFrom = SlideDirection.Left;
    [Range(0.1f, 2f)] public float slideDuration = 0.3f;
    public AnimationCurve easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("References")]
    public RectTransform menuTransform;

    private Vector2 _hiddenPosition;
    private Vector2 _visiblePosition;
    private Coroutine _animationRoutine;
    private bool _isVisible;

    private void Awake()
    {
        if (menuTransform == null)
            menuTransform = GetComponent<RectTransform>();

        _visiblePosition = menuTransform.anchoredPosition;
        _hiddenPosition = GetHiddenPosition();
        menuTransform.anchoredPosition = _hiddenPosition;
        gameObject.SetActive(false);
    }

    private Vector2 GetHiddenPosition()
    {
        Vector2 offset = Vector2.zero;
        Vector2 size = ((RectTransform)menuTransform.parent).rect.size;

        switch (slideInFrom)
        {
            case SlideDirection.Left: offset = new Vector2(-size.x, 0); break;
            case SlideDirection.Right: offset = new Vector2(size.x, 0); break;
            case SlideDirection.Up: offset = new Vector2(0, size.y); break;
            case SlideDirection.Down: offset = new Vector2(0, -size.y); break;
        }

        return _visiblePosition + offset;
    }

    public void ShowMenu()
    {
        if (_isVisible) return;
        _isVisible = true;
        gameObject.SetActive(true);

        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(AnimateMenu(_hiddenPosition, _visiblePosition));
    }

    public void HideMenu()
    {
        if (!_isVisible) return;
        _isVisible = false;

        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(AnimateMenu(_visiblePosition, _hiddenPosition, disableOnFinish: true));
    }

    private IEnumerator AnimateMenu(Vector2 from, Vector2 to, bool disableOnFinish = false)
    {
        float time = 0f;

        while (time < slideDuration)
        {
            float t = time / slideDuration;
            float easedT = easing.Evaluate(t);
            menuTransform.anchoredPosition = Vector2.Lerp(from, to, easedT);
            time += Time.unscaledDeltaTime; // unaffected by pause
            yield return null;
        }

        menuTransform.anchoredPosition = to;

        if (disableOnFinish)
            gameObject.SetActive(false);
    }
}
