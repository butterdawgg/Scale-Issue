using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float maxHealth;
    [Header("Movement")]
    [SerializeField] private float maxVelocity;
    [SerializeField] private float velocityLerpK;
    [Header("Movement")]
    [SerializeField] private float mouseSensitivity;
    [Header("Composition")]
    [SerializeField] private Transform lookPivot;
    [Header("Layer masks")]
    [SerializeField] private LayerMask lookLayerMask;

    public static Player Instance { get; private set; }
    public float Health { get { return _health; } set { if (value > 0) _health = value; else _health = 0f; } }
    private float _health;
    public float MaxHealth { get { return maxHealth; } }
    public Camera Camera { get; private set; }
    public Transform LookPivot { get; private set; }
    public Vector3 LookPoint { get; private set; }
    public Vector3 Position { get; private set; }
    public bool IsDefeated { get; private set; }
    public bool IsActionLocked { get; set; }

    //private bool isDead;

    private Rigidbody rb;
    private Camera cam;
    private GunPlayer gun;

    private float lookPitch;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Health = maxHealth;

        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

        gun = GetComponentInChildren<GunPlayer>();
        
        ControlProperties();
    }

    private void Update()
    {
        ControlProperties();
        ControlMovement();
        ControlOrientation();
    }

    private void ControlProperties()
    {
        LookPivot = lookPivot;
        Position = transform.position;
        Camera = cam;

        Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, lookLayerMask))
            LookPoint = hit.point;
        else
            LookPoint = ray.GetPoint(100f);
    }

    private void ControlMovement()
    {
        Vector3 input =
            transform.forward *
            (Convert.ToInt32(Input.GetKey(KeyCode.W)) - Convert.ToInt32(Input.GetKey(KeyCode.S))) +
            transform.right *
            (Convert.ToInt32(Input.GetKey(KeyCode.D)) - Convert.ToInt32(Input.GetKey(KeyCode.A)));

        Vector3 targetVelocity = input.normalized * maxVelocity;

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, velocityLerpK * Time.deltaTime);
    }

    private void ControlOrientation()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        lookPitch -= mouseY * (mouseSensitivity * SerializeManager.GetMouseSensitivity());
        lookPitch = Mathf.Clamp(lookPitch, -80f, 80f);

        lookPivot.localRotation = Quaternion.Euler(lookPitch, 0f, 0f);
        transform.Rotate(transform.up * mouseX *
            (mouseSensitivity * SerializeManager.GetMouseSensitivity()));
    }
}
