using System;
using Unity.VisualScripting;
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

    private static int GetInt(string key, int defaultValue)
    {
        if (PlayerPrefs.HasKey(key))
            return PlayerPrefs.GetInt(key);
        else
            return defaultValue;
    }

    private static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    private static Vector3 GetVector3(string key, Vector3 defaultValue)
    {
        if (!PlayerPrefs.HasKey(key))
            return defaultValue;

        return new Vector3(PlayerPrefs.GetFloat(key),
            PlayerPrefs.GetFloat(key + "_1"),
            PlayerPrefs.GetFloat(key + "_2"));
    }

    private static void SetVector3(string key, Vector3 value)
    {
        PlayerPrefs.SetFloat(key, value.x);
        PlayerPrefs.SetFloat(key + "_1", value.y);
        PlayerPrefs.SetFloat(key + "_2", value.z);
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

    public static int GetCheckpointEventDepth()
    {
        return GetInt("checkpoint_event_depth", 0);
    }

    public static void SetCheckpointEventDepth(int value)
    {
        SetInt("checkpoint_event_depth", value);
    }

    public static Vector3 GetCheckpointPlayerPosition()
    {
        return GetVector3("checkpoint_player_position", Vector3.up);
    }

    public static void SetCheckpointPlayerPosition(Vector3 value)
    {
        SetVector3("checkpoint_player_position", value);
    }

    public static bool GetEnemyDefeatedStatus(int id)
    {
        string key = "enemy_defeated_" + id;
        if (PlayerPrefs.HasKey(key))
        {
            return Convert.ToBoolean(PlayerPrefs.GetInt(key));
        }
        else
        {
            return false;
        }
    }

    public static void SetEnemyDefeatedStatus(int id, bool value)
    {
        string key = "enemy_defeated_" + id;
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }
}