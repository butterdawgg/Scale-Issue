using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GunPlayer : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float spread;
    [SerializeField] private float projectileVelocity;
    [SerializeField] private float projectileLifetime;
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

    private bool isShooting;

    private void Update()
    {
        if (Time.timeScale <= 0f)
            return;

        Vector3 checkPoint = Player.Instance.LookPivot.position;
        Vector3 checkDirection = Player.Instance.LookPivot.forward;
        Vector3 lookPoint = Player.Instance.LookPoint;

        if (Physics.SphereCast(checkPoint, checkRadius, checkDirection,
            out RaycastHit hit, checkDistance, checkLayerMask))
        {
            gunPivot.localPosition = Vector3.Lerp(gunPivot.localPosition,
                idlePoint.localPosition, lerpK * Time.deltaTime);

            gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation,
                idlePoint.localRotation, lerpK * Time.deltaTime);

            return;
        }

        activePoint.rotation = Quaternion.LookRotation(lookPoint +
            (activePoint.position - muzzlePoint.position) - activePoint.position, transform.up);

        if (isShooting)
            return;

        gunPivot.localPosition = Vector3.Lerp(gunPivot.localPosition,
            activePoint.localPosition, lerpK * Time.deltaTime);

        gunPivot.localRotation = Quaternion.Lerp(gunPivot.localRotation,
            activePoint.localRotation, lerpK * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Mouse0) &&
                Vector3.Angle(gunPivot.forward, activePoint.forward) < 5f)
            Shoot();
    }

    protected void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        isShooting = true;

        Projectile.Launch(projectile, muzzlePoint.position, muzzlePoint.forward, projectileVelocity,
            spread, projectileLifetime, projectileLayerMask, damage, true);

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