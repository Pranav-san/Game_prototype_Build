
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsManager : PlayerUIMenu
{

    [Header("Camera Settings")]
    public Slider touchSensitivitySlider;
    public Slider fovSlider;
    public Slider renderScaleSlider;


    [SerializeField] private TextMeshProUGUI fovText;
    [SerializeField] private TextMeshProUGUI renderScaleText;
    [SerializeField] private TextMeshProUGUI sensitivityText;

    [Header("Game Settings")]
    [SerializeField] Image autolockOnEnabledIcon;
    [SerializeField] Image autoSwitchToNearestTargetEnabledIcon;
    [SerializeField] Image fpsEnabledIcon;


    [Header("UI")]
    [SerializeField] Color defaultSettingMenuTextColor;
    [SerializeField] Color selectedSettingMenuTextColor;
    [SerializeField] TextMeshProUGUI gameSettingsButtonText;
    [SerializeField] TextMeshProUGUI cameraSettingsButtonText;
    [SerializeField] TextMeshProUGUI soundSettingsButtonText;
    [SerializeField] TextMeshProUGUI graphicsSettingsButtonText;

    [Header("Settings Menu Objects")]
    [SerializeField] GameObject gameSettings;
    [SerializeField] GameObject cameraSettings;
    [SerializeField] GameObject soundSettings;
    [SerializeField] GameObject graphicsSettings;

    private void Start()
    {
        // Load saved settings
        GameSettingsData.LoadFromPrefs();

        // Setup slider ranges
        touchSensitivitySlider.minValue = 500f;
        touchSensitivitySlider.maxValue = 2000f;

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

        PlayerUIManager.instance.playerUICharacterMenuManager.ToggleSettingsButton(false);
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

    public void ToggleAutoLockOn()
    {
        if (!PlayerCamera.instance.autoLockOn)
        {
            PlayerCamera.instance.autoLockOn = true;
            autolockOnEnabledIcon.enabled = true;
            WorldSoundFXManager.instance.PlaySettingToggleSFX();
        }
        else
        {
            PlayerCamera.instance.autoLockOn = false;
            autolockOnEnabledIcon.enabled = false;
            WorldSoundFXManager.instance.PlaySettingToggleSFX();

        }

    }

    public void ToggleAutoSwitchToTheNearestTargetWhenLockedOn()
    {
        if (!PlayerCamera.instance.autoSwitchToNearestTarget)
        {
            PlayerCamera.instance.autoSwitchToNearestTarget = true;
            autoSwitchToNearestTargetEnabledIcon.enabled = true;
            WorldSoundFXManager.instance.PlaySettingToggleSFX();
        }
        else
        {
            PlayerCamera.instance.autoSwitchToNearestTarget = false;
            autoSwitchToNearestTargetEnabledIcon.enabled = false;
            WorldSoundFXManager.instance.PlaySettingToggleSFX();

        }

    }

    public void ToggleFPS()
    {
        if (!PlayerUIManager.instance.showFPS)
        {
            PlayerUIManager.instance.showFPS = true;
            PlayerUIManager.instance.fpsDisplay.EnableFPS();
            WorldSoundFXManager.instance.PlaySettingToggleSFX();

            fpsEnabledIcon.enabled = true;
        }
        else
        {
            PlayerUIManager.instance.showFPS = false;
            PlayerUIManager.instance.fpsDisplay.DisableFPS();
            WorldSoundFXManager.instance.PlaySettingToggleSFX();
            fpsEnabledIcon.enabled = false;
        }
    }



    public override void OpenMenu()
    {
        base.OpenMenu();

        PlayerUIManager.instance.playerUICharacterMenuManager.ToggleSettingsButton(false);
        PlayerUIManager.instance.mobileControls.ToggleInventoryButton(false);
        menu.SetActive(true);


        gameSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = selectedSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;




    }

    public override void CloseMenu()
    {
        base.CloseMenu();

        PlayerUIManager.instance.mobileControls.ToggleInventoryButton(true);







    }









    // Game Settings

    public void OpenGameSettingsMenu()
    {
        gameSettings.SetActive(true);

        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = selectedSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseGameSettingsMenu()
    {
        gameSettings.SetActive(false);
    }

    public void OpenCameraSettingsMenu()
    {
        cameraSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = selectedSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseCameraSettingsMenu()
    {
        cameraSettings.SetActive(false);
    }

    public void OpenSoundSettingsMenu()
    {
        soundSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = selectedSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseSoundSettingsMenu()
    {
        soundSettings.SetActive(false);
    }

    public void OpenGraphicsSettingsMenu()
    {
        graphicsSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = selectedSettingMenuTextColor;
    }

    public void CloseGraphicsSettingsMenu()
    {
        graphicsSettings.SetActive(false);
    }

    public void CloseAllSettingsMenu()
    {
        CloseGameSettingsMenu();
        CloseCameraSettingsMenu();
        CloseSoundSettingsMenu();
        CloseGraphicsSettingsMenu();

    }
}
