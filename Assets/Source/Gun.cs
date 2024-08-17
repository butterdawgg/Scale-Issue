using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [Header("Gun Movement")]
    [SerializeField] private Transform gunPivot;
    [SerializeField] private Transform activePoint;
    [SerializeField] private Transform idlePoint;
    [SerializeField] private Transform recoilPoint;
    [SerializeField] private float lerpK;
    [Header("Collision check")]
    [SerializeField] private float checkRadius;
    [SerializeField] private float checkDistance;
    [Header("Shooting")]
    [SerializeField] private Transform muzzlePoint;
    [SerializeField] private Projectile projectile;
    [Header("Layer Masks")]
    [SerializeField] private LayerMask checkLayerMask;
    [SerializeField] private LayerMask projectileLayerMask;

    public Transform LookPoint { get; set; }
    public bool CanShoot { get; set; }

    private bool isShooting;

    private void Update()
    {
        if (Time.timeScale <= 0f)
            return;

        Vector3 lookPoint = Vector3.zero;

        if (Physics.SphereCast(LookPoint.position, checkRadius, LookPoint.forward,
            out RaycastHit hit, checkDistance, checkLayerMask))
        {
            gunPivot.localPosition = Vector3.Lerp(gunPivot.localPosition,
                idlePoint.localPosition, lerpK * Time.deltaTime);

            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation,
                idlePoint.localRotation, lerpK * Time.deltaTime);

            return;
        }

        activePoint.rotation = Quaternion.LookRotation(LookPoint.position +
            (activePoint.position - muzzlePoint.position) - activePoint.position, transform.up);

        if (isShooting)
            return;

        gunPivot.localPosition = Vector3.Lerp(gunPivot.localPosition,
            activePoint.localPosition, lerpK * Time.deltaTime);

        gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation,
            activePoint.localRotation, lerpK * Time.deltaTime);

        if (CanShoot & Vector3.Angle(gunPivot.forward, activePoint.forward) < 5f)
            Shoot();
    }

    protected void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        isShooting = true;

        Projectile.Launch(projectile, muzzlePoint.position, muzzlePoint.forward, projectileLayerMask, maxDistance, damage);

        float halfCooldown = cooldown * 0.5f;

        gunPivot.localPosition = activePoint.localPosition;
        gunPivot.localRotation = activePoint.localRotation;

        float timeElapsed = 0f;
        while (timeElapsed < halfCooldown)
        {
            float t = Mathf.SmoothStep(0f, 1f, timeElapsed / halfCooldown);

            gunPivot.localPosition = Vector3.Lerp(activePoint.localPosition,
                recoilPoint.localPosition, t);

            gunPivot.localRotation = Quaternion.Lerp(activePoint.localRotation,
                recoilPoint.localRotation, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gunPivot.localPosition = recoilPoint.localPosition;
        gunPivot.localRotation = recoilPoint.localRotation;

        timeElapsed = 0f;
        while (timeElapsed < halfCooldown)
        {
            float t = Mathf.SmoothStep(0f, 1f, timeElapsed / halfCooldown);

            gunPivot.localPosition = Vector3.Lerp(recoilPoint.localPosition,
                activePoint.localPosition, t);

            gunPivot.localRotation = Quaternion.Lerp(recoilPoint.localRotation,
                activePoint.localRotation, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gunPivot.localPosition = activePoint.localPosition;
        gunPivot.localRotation = activePoint.localRotation;

        isShooting = false;
    }
}