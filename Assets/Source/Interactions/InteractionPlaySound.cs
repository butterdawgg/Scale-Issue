using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPlaySound : Interaction
{
    [SerializeField] private string soundName;

    public override void Interact()
    {
        AudioManager.Instance.PlaySound(soundName);
    }
}
