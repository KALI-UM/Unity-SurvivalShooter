using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;

    public float detectionDistance = 10f;
    public float pathFindInterval = 5f;
    public float attackDistance = 2f;
    public float attackInterval = 2f;
    public int damage = 10;

    public LayerMask targetLayer;
    public LivingEntity targetEntity = null;

    private Coroutine coFindPath;
    private Coroutine coAttack;
    public bool HasTarget
    {
        get => targetEntity != null;
    }

    public readonly int hashIsDead = Animator.StringToHash("IsDead");
    public readonly int hashMove = Animator.StringToHash("Move");

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        agent.enabled = true;
        coFindPath = StartCoroutine(CoFindPath());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        agent.enabled= false;
    }
    private void Update()
    {
        if (coAttack == null && targetEntity != null && !targetEntity.IsDead && Vector3.Distance(transform.position, targetEntity.transform.position) < attackDistance)
        {
            StopCoroutine(coFindPath);
            coAttack = StartCoroutine(CoAttack());
        }

        animator.SetBool(hashMove, agent.velocity.magnitude!=0);
    }



    public LivingEntity FindTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, detectionDistance, targetLayer.value);
        foreach (var col in cols)
        {
            var livingEntity = col.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.IsDead)
            {
                Debug.Log("find!");
                return livingEntity;
            }
        }
        return null;
    }

    private IEnumerator CoFindPath()
    {
        while (true)
        {
            if (!HasTarget)
            {
                agent.isStopped = true;
                targetEntity = FindTarget();
            }

            if (HasTarget)
            {
                rb.Sleep();
                agent.isStopped = false;
                agent.SetDestination(targetEntity.transform.position);
            }

            yield return new WaitForSeconds(pathFindInterval);
        }

    }

    private IEnumerator CoAttack()
    {
        while (!targetEntity.IsDead && Vector3.Distance(transform.position, targetEntity.transform.position) < attackDistance)
        {
            agent.isStopped = true;
            targetEntity.OnDamage(damage, Vector3.zero, Vector3.zero);
            Debug.Log("Attack");
            yield return new WaitForSeconds(attackInterval);
        }

        coAttack = null;
        coFindPath = StartCoroutine(CoFindPath());
    }
}
