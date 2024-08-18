using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Event
{
    public EventDialogue[] dialogues;

    public EventCondition[] conditions;

    public EventEffect[] effects;
}
