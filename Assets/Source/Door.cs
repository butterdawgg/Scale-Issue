using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float interactionRadius;
    [SerializeField] private KeyCode interactionKey;
    [SerializeField] private bool isInteractable;

    private Animator animator;

    private bool isActive = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!isInteractable)
            return;

        float distance = (Player.Instance.Position - transform.position).magnitude;
        if (distance <= interactionRadius && Input.GetKeyUp(interactionKey))
        {
            //open \ close the door
            animator.SetTrigger("activate");
            isActive = !isActive;
        }
    }

    public void SetInteractions(bool value)
    {
        isInteractable = value;
    }
}
