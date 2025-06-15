using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    
    public Slider touchSensitivitySlider;
    public Slider fovSlider;
    public Slider renderScaleSlider;


    private const string sensitivityKey = "TouchSensitivity";
    private const string fovKey = "FOV";
    private const string renderScaleKey = "RenderScale";


    [SerializeField] TextMeshProUGUI fovText;
    [SerializeField] TextMeshProUGUI renderScaleText;


    //[Header("Settings_Limitations")]
    // Minimum and maximum FOV values




    private void Start()
    {
        StartCoroutine(UpdateFOVTextCoroutine(0.05f));
        StartCoroutine(UpdateRenderScaleTextCoroutine(0.05f));



        // Load the saved sensitivity value or set a default value
        if (PlayerPrefs.HasKey(sensitivityKey))
        {
            touchSensitivitySlider.value = PlayerPrefs.GetFloat(sensitivityKey);
        }
        else
        {
            touchSensitivitySlider.value = 1f;  // Default sensitivity value
        }

        
        if (PlayerPrefs.HasKey(fovKey))
        {
            fovSlider.value = PlayerPrefs.GetFloat(fovKey);
        }
        else
        {
            fovSlider.value = 68f;
        }
        renderScaleSlider.value = PlayerPrefs.GetFloat(renderScaleKey, 1f);

        renderScaleSlider.onValueChanged.AddListener(OnRenderScaleChanged);

        // Add listener to handle slider value change
        touchSensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        fovSlider.onValueChanged.AddListener(OnFOVChanged);
    }

    private void OnSensitivityChanged(float value)
    {
        // Save the updated sensitivity value
        PlayerPrefs.SetFloat(sensitivityKey, value);
    }
    private void OnFOVChanged(float value)
    {
        PlayerPrefs.SetFloat(fovKey, value);
       
    }

    private void OnRenderScaleChanged(float value)
    {
        PlayerPrefs.SetFloat(renderScaleKey, value);

        // Update render scale in URP settings
        UniversalRenderPipelineAsset urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;
        urpAsset.renderScale = value;
    }
    private IEnumerator UpdateFOVTextCoroutine(float delay)
    {
        while (true)
        {
            float currentFOV = fovSlider.value;
            fovText.text = $"FOV: {currentFOV:F0}";
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator UpdateRenderScaleTextCoroutine(float delay)
    {
        while (true)
        {
            float currentRenderScale = renderScaleSlider.value;
            renderScaleText.text = $"Render Scale: {currentRenderScale:F2}";
            yield return new WaitForSeconds(delay);
        }
    }
}
