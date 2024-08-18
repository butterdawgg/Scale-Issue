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

    public string text;

    public KeyCode fastForwardKey;

    public KeyCode skipKey;
}