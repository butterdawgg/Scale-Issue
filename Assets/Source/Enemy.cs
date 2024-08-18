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
    [SerializeField] private float attackDistance;
    [SerializeField] private float timeToForget;
    [SerializeField] private float rotaionLerpK;
    [Header("Debug")]
    [SerializeField] private bool isPlayerDetected;
    [SerializeField] private float lastPlayerDetectedTime;

    public float Health { get { return _health; }
        set { if (value > 0) _health = value; else _health = 0f; } }

    private float _health;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isDead;

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
        }

        float distance = (transform.position - Player.Instance.transform.position).magnitude;
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

        if (distance <= attackDistance)
        {
            //attack
            animator.SetTrigger("attack");
        }

        agent.SetDestination(Player.Instance.transform.position);
    }
}
