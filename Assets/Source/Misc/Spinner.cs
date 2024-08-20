using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.localEulerAngles += Vector3.up * rotationSpeed * Time.deltaTime;
    }
}
