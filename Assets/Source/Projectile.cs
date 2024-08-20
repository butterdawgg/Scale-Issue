using UnityEngine;
using UnityEngine.VFX;

public class Projectile : MonoBehaviour
{
    [Header("Composition")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private VisualEffect hitVFX;
    [SerializeField] private VisualEffect muzzleFlashVFX;

    private Vector3 velocity;
    private float lifetime;
    private LayerMask hitLayerMask;
    private float damage;
    private bool isFriendly;

    private bool isDead = false;
    private float awakeTime = 0f;

    private void Awake()
    {
        awakeTime = Time.time;

        muzzleFlashVFX.transform.parent = default;
        muzzleFlashVFX.Play();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (awakeTime < Time.time - lifetime)
        {
            OnDeath();

            return;
        }

        if (Physics.Raycast(transform.position, velocity, out RaycastHit hit,
            (velocity * Time.deltaTime).magnitude, hitLayerMask))
        {
            transform.position = hit.point;

            hitVFX.transform.parent = default;
            hitVFX.GetComponent<Sticker>().SetSticker(hit.transform, hit.point);

            if (isFriendly)
            {
                Enemy enemy = hit.collider.gameObject.GetComponentInParent<Enemy>();

                if (enemy != null)
                {
                    enemy.Health -= damage;

                    enemy.Aggro();
                }
            }
            else
            {
                if (hit.transform == Player.Instance.transform)
                {
                    Player.Instance.Health -= damage;
                }
            }

            OnDeath();

            return;
        }

        transform.position += velocity * Time.deltaTime;
    }

    private void OnDeath()
    {
        Destroy(meshRenderer.gameObject);
        Destroy(trailRenderer.gameObject);
        Destroy(muzzleFlashVFX.gameObject, 1f);
        Destroy(hitVFX.gameObject, 1f);
        Destroy(gameObject, 1f);

        hitVFX.Play();

        isDead = true;
    }

    public static void Launch(Projectile prototype, Vector3 position, Vector3 direction, float speed,
        float spread, float lifetime, LayerMask hitLayerMask, float damage, bool isFriendly)
    {
        Projectile projectile = Instantiate(prototype.gameObject, position,
            Quaternion.LookRotation(direction), default).GetComponent<Projectile>();

        projectile.transform.localEulerAngles += new Vector3(Random.Range(-1f, 1f),
            Random.Range(-1f, 1f), 0f).normalized * Random.Range(-spread, spread);

        projectile.velocity = projectile.transform.forward * speed;

        projectile.lifetime = lifetime;

        projectile.hitLayerMask = hitLayerMask;

        projectile.damage = damage;

        projectile.isFriendly = isFriendly;
    }
}
