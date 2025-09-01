// GameSettingsData.cs
using UnityEngine;

public static class GameSettingsData
{
    public static float TouchSensitivity = 1f;
    public static float FOV = 68f;
    public static float RenderScale = 1f;

    public static void LoadFromPrefs()
    {
        TouchSensitivity = PlayerPrefs.GetFloat("TouchSensitivity", 1f);
        FOV = PlayerPrefs.GetFloat("FOV", 68f);
        RenderScale = PlayerPrefs.GetFloat("RenderScale", 1f);
    }

    public static void SaveToPrefs()
    {
        PlayerPrefs.SetFloat("TouchSensitivity", TouchSensitivity);
        PlayerPrefs.SetFloat("FOV", FOV);
        PlayerPrefs.SetFloat("RenderScale", RenderScale);
        PlayerPrefs.Save(); // Ensure it writes on mobile
    }
}
