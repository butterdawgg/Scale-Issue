using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxVelocity;
    [SerializeField] private float velocityLerpK;
    [Header("Movement")]
    [SerializeField] private float mouseSensitivity;
    [Header("Ground check")]
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayerMask;
    [Header("Composition")]
    [SerializeField] private Transform lookPoint;
    [SerializeField] private Gun gun;

    private CharacterController cc;

    private Vector3 velocity;
    private float lookPitch;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        ControlMovement();
        ControlOrientation();
    }

    private void ControlMovement()
    {
        Vector3 input =
            transform.forward *
            (Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S))) +
            transform.right *
            (Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A)));

        Vector3 targetVelocity = input.normalized * maxVelocity;

        velocity = Vector3.Lerp(velocity, targetVelocity, velocityLerpK * Time.deltaTime);
        velocity.y = 0f;

        cc.Move(velocity * Time.deltaTime);
    }

    private void ControlOrientation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        lookPitch -= mouseY * (mouseSensitivity * SerializeManager.GetMouseSensitivity());
        lookPitch = Mathf.Clamp(lookPitch, -80f, 80f);

        lookPoint.localRotation = Quaternion.Euler(lookPitch, 0f, 0f);
        transform.Rotate(transform.up * mouseX *
            (mouseSensitivity * SerializeManager.GetMouseSensitivity()));
    }
}
