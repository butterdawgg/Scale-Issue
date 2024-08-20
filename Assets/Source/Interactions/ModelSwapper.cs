using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSwapper : Interaction
{
    [SerializeField] private GameObject normalModel;
    [SerializeField] private GameObject alternativeModel;

    private void Awake()
    {
        normalModel.SetActive(true);
        alternativeModel.SetActive(false);
    }

    public override void Interact()
    {
        normalModel.SetActive(!normalModel.activeSelf);
        alternativeModel.SetActive(!alternativeModel.activeSelf);
    }
}
