using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float maxHealth;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float detectDistance;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackDistance;
    [SerializeField] private float timeToForget;
    [SerializeField] private float rotaionLerpK;

    private bool isPlayerDetected;
    private float lastPlayerDetectedTime;

    public int ID { get; set; }


    public float Health { get { return _health; }
        set { if (value > 0) _health = value; else _health = 0f; } }

    private float _health;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead;
    private bool isAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        Health = maxHealth;
    }

    private void Update()
    {
        if (isDead)
            return;

        if (Health <= 0f)
        {
            //death
            isDead = true;
            agent.isStopped = true;
            animator.SetTrigger("dead");
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                Destroy(collider);
            }

            SerializeManager.SetEnemyDefeatedStatus(ID, true);
        }

        float distance = Vector3.ProjectOnPlane(Player.Instance.Position - transform.position,
            Vector3.up).magnitude;

        Ray ray = new Ray(transform.position, Player.Instance.transform.position - transform.position);
        if (distance <= detectDistance)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, targetLayerMask))
            {
                if (hit.transform == Player.Instance.transform)
                {
                    isPlayerDetected = true;
                    lastPlayerDetectedTime = Time.time;
                }
            }
        }

        if (isPlayerDetected && lastPlayerDetectedTime < Time.time - timeToForget)
            isPlayerDetected = false;

        if (!isPlayerDetected)
            return;

        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(Player.Instance.transform.position - transform.position), 
            rotaionLerpK * Time.deltaTime);

        if (distance <= attackDistance && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
        }

        agent.SetDestination(Player.Instance.transform.position);
    }

    public void Aggro()
    {
        isPlayerDetected = true;
        lastPlayerDetectedTime = Time.time;
    }

    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;

        animator.SetTrigger("attack");
        AudioManager.Instance.PlaySound("EnemyAttack");

        yield return new WaitForSeconds(attackDelay);

        Vector3 direction = Vector3.ProjectOnPlane(Player.Instance.Position - transform.position,
            Vector3.up);

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        float distance = direction.magnitude;

        if (distance <= attackDistance && Vector3.Angle(direction, forward) < 5f)
        {
            Player.Instance.Health -= attackDamage;

            Player.Instance.ShakeCamera(0.1f, 0.2f);
        }

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }
}
