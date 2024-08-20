using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionNote : Interaction
{
    [TextArea]
    [SerializeField] private string noteText;

    public override void Interact()
    {
        FindFirstObjectByType<HUDManager>().DisplayNote(noteText);
    }
}
