using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private List<Door> connectedDoors;
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
            //open \ close the doors
            foreach (Door connectedDoor in connectedDoors)
            {
                connectedDoor.ToggleActive();
            }
        }
    }

    public void SetInteractions(bool value)
    {
        isInteractable = value;
    }
}
