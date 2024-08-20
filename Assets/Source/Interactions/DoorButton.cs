using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : Interaction
{
    [SerializeField] private Door[] connectedDoors;
    [SerializeField] private GameObject lockedModel;
    [SerializeField] private GameObject unlockedModel;

    private void Update()
    {
        lockedModel.SetActive(IsLocked);
        unlockedModel.SetActive(!IsLocked);

        foreach (Door door in connectedDoors)
        {
            door.SetLockedState(IsLocked);
        }
    }

    public override void Interact()
    {
        foreach (Door door in connectedDoors)
        {
            door.Toggle();
        }
    }
}
