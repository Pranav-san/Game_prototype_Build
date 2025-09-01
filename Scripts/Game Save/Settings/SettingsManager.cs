// SettingsManager.cs
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider touchSensitivitySlider;
    public Slider fovSlider;
    public Slider renderScaleSlider;

    [SerializeField] private TextMeshProUGUI fovText;
    [SerializeField] private TextMeshProUGUI renderScaleText;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    private void Start()
    {
        // Load saved settings
        GameSettingsData.LoadFromPrefs();

        // Setup slider ranges
        touchSensitivitySlider.minValue = 1f;
        touchSensitivitySlider.maxValue = 70f;

        fovSlider.minValue = 50f;
        fovSlider.maxValue = 160f;

        // Apply saved values to sliders
        touchSensitivitySlider.value = GameSettingsData.TouchSensitivity;
        fovSlider.value = GameSettingsData.FOV;
        renderScaleSlider.value = GameSettingsData.RenderScale;

        // Set initial UI text
        sensitivityText.text = $"Sensitivity: {touchSensitivitySlider.value:F0}";
        fovText.text = $"FOV: {fovSlider.value:F0}";
        renderScaleText.text = $"Render Scale: {renderScaleSlider.value:F2}";

        // Add listeners
        touchSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        fovSlider.onValueChanged.AddListener(OnFOVChanged);
        renderScaleSlider.onValueChanged.AddListener(OnRenderScaleChanged);
    }

    private void OnSensitivityChanged(float value)
    {
        GameSettingsData.TouchSensitivity = value;
        GameSettingsData.SaveToPrefs();

        sensitivityText.text = $"Sensitivity: {value:F0}";
    }

    private void OnFOVChanged(float value)
    {
        GameSettingsData.FOV = value;
        GameSettingsData.SaveToPrefs();

        fovText.text = $"FOV: {value:F0}";
    }

    private void OnRenderScaleChanged(float value)
    {
        GameSettingsData.RenderScale = value;
        GameSettingsData.SaveToPrefs();

        UniversalRenderPipelineAsset urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;
        urpAsset.renderScale = value;

        renderScaleText.text = $"Render Scale: {value:F2}";
    }
}
