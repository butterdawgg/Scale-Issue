using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Door connectedDoor;
    [SerializeField] private float interactionRadius;
    [SerializeField] private KeyCode interactionKey;
    [SerializeField] private bool isInteractable;

    void Update()
    {
        if (!isInteractable)
            return;

        float distance = (Player.Instance.Position - transform.position).magnitude;
        if (distance <= interactionRadius && Input.GetKeyUp(interactionKey))
        {
            //open \ close the door
            connectedDoor.ToggleActive();
        }
    }

    public void SetInteractions(bool value)
    {
        isInteractable = value;
    }
}
