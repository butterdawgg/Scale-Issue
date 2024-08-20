using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticker : MonoBehaviour
{
    [SerializeField] private Transform sticker;
    [SerializeField] private float lifetime;

    private void Awake()
    {
        Destroy(sticker.gameObject, lifetime);
    }

    private void Update()
    {
        if (sticker != null)
            transform.position = sticker.position;
        else
            Destroy(gameObject);
    }

    public void SetSticker(Transform parent, Vector3 position)
    {
        if (sticker == null)
            return;

        sticker.parent = parent;
        sticker.position = position;
    }
}
