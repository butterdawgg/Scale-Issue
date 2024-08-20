using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    [Header("Sound Effects")]
    [SerializeField] private float stepDistance;
    [SerializeField] private float lowHealthThreshhold;

    public static Player Instance { get; private set; }
    public float Health { get { return _health; } set { if (value > 0) { 
                if (value < _health) AudioManager.Instance.PlaySound("Grunt" + UnityEngine.Random.Range(1, 5).ToString());
                if (value <= lowHealthThreshhold) AudioManager.Instance.PlaySound("LowHealth");
                _health = value;
            } 
            else _health = 0f; } }
    private float _health;
    public float MaxHealth { get { return maxHealth; } }
    public Camera Camera { get; private set; }
    public Transform LookPivot { get; private set; }
    public Vector3 LookPoint { get; private set; }
    public Vector3 Position { get; private set; }
    public bool IsActionLocked { get; set; }

    private bool isDead;

    private Rigidbody rb;
    private Camera cam;
    private GunPlayer gun;
    private HUDManager hudManager;

    private float lookPitch;
    private float lookYaw;

    private float currentStepDistance;
    private bool currentSteppingLeg = false;

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

        hudManager = GetComponentInChildren<HUDManager>();
        
        ControlProperties();

        IsActionLocked = false;
    }

    private void Update()
    {
        if (Health <= 0f)
        {
            isDead = true;

            hudManager.OnDefeat();
        }

        if (isDead)
            return;

        ControlProperties();

        if (Time.timeScale <= 0f)
            return;

        currentStepDistance += rb.velocity.magnitude * Time.deltaTime;
        if(currentStepDistance >= stepDistance)
        {
            currentStepDistance = 0;
            if (!currentSteppingLeg)
                AudioManager.Instance.PlaySound("Footstep1");
            else
                AudioManager.Instance.PlaySound("Footstep2");

            currentSteppingLeg = !currentSteppingLeg;
        }

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

        lookYaw += mouseX * (mouseSensitivity * SerializeManager.GetMouseSensitivity());

        lookPivot.localRotation = Quaternion.Euler(lookPitch, 0f, 0f);
        rb.rotation = Quaternion.Euler(0f, lookYaw, 0f);
    }

    public void Warp(Vector3 position)
    {
        transform.position = position;
    }
}
