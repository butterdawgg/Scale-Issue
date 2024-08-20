using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Interaction[] interactions;
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactionLayerMask;

    private HUDManager hudManager;

    private bool isLocked = true;

    private void Awake()
    {
        hudManager = FindFirstObjectByType<HUDManager>();
    }

    private void Update()
    {
        foreach (var interaction in interactions)
        {
            interaction.IsLocked = isLocked;
        }

        if (isLocked)
            return;

        if (Player.Instance.IsActionLocked)
            return;

        float playerDistance = (Player.Instance.Position - interactionPoint.position).magnitude;

        if (playerDistance > interactionDistance)
        {
            hudManager.RemoveInteractable(this);

            return;
        }

        if (!Physics.Raycast(Player.Instance.LookPivot.position, Player.Instance.LookPivot.forward,
            out RaycastHit hit, 1000f, interactionLayerMask))
        {
            hudManager.RemoveInteractable(this);

            return;
        }

        Interactable hitInteractable = hit.transform.GetComponentInParent<Interactable>();

        if (hitInteractable == null)
        {
            hudManager.RemoveInteractable(this);

            return;
        }

        if (hitInteractable != this)
        {
            hudManager.RemoveInteractable(this);

            return;
        }

        hudManager.AddInteractable(this);

        if (!Input.GetKeyDown(KeyCode.E))
            return;

        foreach (var interaction in interactions)
        {
            interaction.Interact();
        }
    }

    public void SetLockedState(bool value)
    {
        isLocked = value;
    }
}
