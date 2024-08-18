using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventDialogue
{
    public DialogueLine[] lines;

    public float initialDelay;

    public bool isPlayed;
}

[System.Serializable]
public struct DialogueLine
{
    public string speaker;

    [TextArea]
    public string text;

    public KeyCode fastForwardKey;

    public KeyCode skipKey;

    private string GetKeyString(KeyCode key)
    {
        if (key == KeyCode.Mouse0)
            return "LMB";
        else
            return key.ToString();
    }

    public string GetFastForwardKeyString()
    {
        return GetKeyString(fastForwardKey);
    }

    public string GetSkipKeyString()
    {
        return GetKeyString(skipKey);
    }
}