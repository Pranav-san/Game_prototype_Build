using UnityEngine;
using System.Collections;

public class MenuFadeInOut : MonoBehaviour
{
    [Header("Fade Settings")]
    public float fadeDuration = 0.3f;
    public AnimationCurve fadeEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0;          // start hidden
        gameObject.SetActive(false);    // fully hidden at start
    }

    public void FadeIn()
    {

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(0f, 1f));
    }

    public void FadeOut(bool disableAfter = true)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(1f, 0f, disableAfter));
    }

    private IEnumerator Fade(float from, float to, bool disableAfter = false)
    {
        gameObject.SetActive(true);

        float t = 0;

        while (t < fadeDuration)
        {
            float normalized = t / fadeDuration;
            float eased = fadeEase.Evaluate(normalized);

            canvasGroup.alpha = Mathf.Lerp(from, to, eased);

            t += Time.unscaledDeltaTime;
            yield return null;
        }

        canvasGroup.alpha = to;

        if (disableAfter && to == 0)
            gameObject.SetActive(false);
    }
}
