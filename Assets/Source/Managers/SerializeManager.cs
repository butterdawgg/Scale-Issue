using System;
using UnityEngine;

public static class SerializeManager
{
    private static float GetFloat(string key, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetFloat(key);
        else
            return defaultValue;
    }

    private static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    public static float GetMasterVolume()
    {
        return GetFloat("master_volume", 1f);
    }

    public static void SetMasterVolume(float value)
    {
        SetFloat("master_volume", value);
    }

    public static float GetSFXVolume()
    {
        return GetFloat("sfx_volume", 1f);
    }

    public static void SetSFXVolume(float value)
    {
        SetFloat("sfx_volume", value);
    }

    public static float GetMusicVolume()
    {
        return GetFloat("music_volume", 1f);
    }

    public static void SetMusicVolume(float value)
    {
        SetFloat("music_volume", value);
    }

    public static float GetMouseSensitivity()
    {
        return GetFloat("mouse_sensitivity", 1f);
    }

    public static void SetMouseSensitivity(float value)
    {
        SetFloat("mouse_sensitivity", value);
    }
}