using UnityEngine;
using UnityEngine.VFX;

public abstract class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private bool isFriendly;
    [Header("Composition")]
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private VisualEffect hitVFX;
    [SerializeField] private VisualEffect muzzleFlashVFX;

    private bool isDead;

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }
}
