using UnityEngine;
using UnityEngine.AI;

public class UnitAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private bool playerOwned;
    private Animator animator;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if(navMeshAgent == null)
        {
            Debug.LogError("UnitAI - NavMeshAgent not found on " + gameObject.name);
            Destroy(gameObject);
        }
        if(animator == null)
        {
            Debug.LogError("UnitAI - Animator not found on " + gameObject.name);
        }

        WalkToTarget();
    }

    private void Update()
    {
        animator.SetFloat("MovementSpeed", navMeshAgent.speed);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetPlayerOwned(bool isPlayerOwned)
    {
        playerOwned = isPlayerOwned;
    }

    private void WalkToTarget()
    {
        navMeshAgent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("PlayerBase") && !playerOwned)
        {
            ResourceTracker.PlayerHealthCurrent--;
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("AIBase") && playerOwned)
        {
            ResourceTracker.AIHealthCurrent--;
            Destroy(gameObject);
        }
    }
}
