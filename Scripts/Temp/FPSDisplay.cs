using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fpsText;

    private float updateTimer;
    private float frameCount;
    private float accumulatedTime;
    public Color plus50FPScolor;

    private float spikeFlashTimer;

    private const float updateInterval = 0.25f;
    private const float spikeThresholdMs = 40f;   // 40ms = noticeable spike
    private const float spikeFlashDuration = 0.2f;

    void Update()
    {
        float delta = Time.unscaledDeltaTime;
        float currentFrameMs = delta * 1000f;

        // Spike detection (instant, not averaged)
        if (currentFrameMs > spikeThresholdMs)
        {
            spikeFlashTimer = spikeFlashDuration;
        }

        frameCount++;
        accumulatedTime += delta;
        updateTimer += delta;

        if (updateTimer >= updateInterval)
        {
            float fps = frameCount / accumulatedTime;
            float avgFrameTime = (accumulatedTime / frameCount) * 1000f;

            int roundedFPS = Mathf.RoundToInt(fps);

            // Default color based on FPS
            Color baseColor;

            if (roundedFPS >= 50)
                baseColor = plus50FPScolor;
            else if (roundedFPS >= 30)
                baseColor = Color.yellow;
            else if (roundedFPS >= 20)
                baseColor = new Color(1f, 0.4f, 0f); // orange
            else
                baseColor = Color.red;

            // If spike recently happened  override color
            if (spikeFlashTimer > 0f)
            {
                fpsText.color = Color.red;
            }
            else
            {
                fpsText.color = baseColor;
            }

            fpsText.text =
                "FPS: " + roundedFPS +
                "\nFrame: " + avgFrameTime.ToString("0.0") + " ms";

            frameCount = 0f;
            accumulatedTime = 0f;
            updateTimer = 0f;
        }

        // Reduce spike flash timer
        if (spikeFlashTimer > 0f)
        {
            spikeFlashTimer -= delta;
        }
    }


    public void DisableFPS()
    {
        gameObject.SetActive(false);
    }
    public void EnableFPS()
    {
        gameObject.SetActive(true);
    }
}
