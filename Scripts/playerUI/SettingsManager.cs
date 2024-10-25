using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    
    public Slider touchSensitivitySlider;
    public Slider fovSlider;
    private const string sensitivityKey = "TouchSensitivity";
    private const string fovKey = "FOV";
    [SerializeField] TextMeshProUGUI fovText;


    //[Header("Settings_Limitations")]
    // Minimum and maximum FOV values




    private void Start()
    {
        StartCoroutine(UpdateFOVTextCoroutine(0.05f));


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
    private IEnumerator UpdateFOVTextCoroutine(float delay)
    {
        while (true)
        {
            float currentFOV = fovSlider.value;
            fovText.text = $"FOV: {currentFOV:F0}";
            yield return new WaitForSeconds(delay);
        }
    }

}
