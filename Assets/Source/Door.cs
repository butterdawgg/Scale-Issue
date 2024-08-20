using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openedOffset;
    [SerializeField] private float openTime;
    [SerializeField] private GameObject lockedModel;
    [SerializeField] private GameObject unlockedModel;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool isOpen = false;
    private bool canOpen = true;

    private bool isLocked = false;

    private void Awake()
    {
        closedPosition = transform.position;

        openPosition = closedPosition +
            (transform.forward * openedOffset.z) +
            (transform.right * openedOffset.x) +
            (transform.up * openedOffset.y);
    }

    private IEnumerator OpenCoroutine()
    {
        canOpen = false;

        float t = 0f;
        float dt = 0.01f;

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(closedPosition, openPosition, t);

            t += dt;

            yield return new WaitForSeconds(openTime * dt);
        }

        canOpen = true;
    }

    private IEnumerator CloseCoroutine()
    {
        canOpen = false;

        float t = 0f;
        float dt = 0.01f;

        while (t < 1f)
        {
            transform.position = Vector3.Lerp(openPosition, closedPosition, t);

            t += dt;

            yield return new WaitForSeconds(openTime * dt);
        }

        canOpen = true;
    }

    public void Toggle()
    {
        if (isLocked)
            return;

        if (!canOpen)
            return;

        if (isOpen)
            StartCoroutine(CloseCoroutine());
        else
            StartCoroutine(OpenCoroutine());

        isOpen = !isOpen;
    }

    public void SetLockedState(bool value)
    {
        isLocked = value;

        lockedModel.SetActive(value);
        unlockedModel.SetActive(!value);
    }
}