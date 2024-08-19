using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform childDoor;
    [Range(0, 1)] [SerializeField] private float lerpSpeed;

    private bool isActive = false;
    private bool finishedAnimation = true;

    private void Update()
    {
        if (finishedAnimation)
            return;

        if (isActive)
        {
            childDoor.localPosition = Vector3.Lerp(childDoor.localPosition, Vector3.forward * 1.6f, lerpSpeed);

            if(Mathf.Round(childDoor.localPosition.z * 10) / 10 == 1.5f)
            {
                finishedAnimation = true;
                childDoor.localPosition = new Vector3(0, 0, 1.6f);
                GetComponent<MeshCollider>().enabled = false;
            }
        }
        else
        {
            GetComponent<MeshCollider>().enabled = true;
            childDoor.localPosition = Vector3.Lerp(childDoor.localPosition, Vector3.back * 0.1f, lerpSpeed);

            if (Mathf.Round(childDoor.localPosition.z * 10) / 10 == 0)
            {
                finishedAnimation = true;
                childDoor.localPosition = Vector3.zero;
            }
        }
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        finishedAnimation = false;
    }
}