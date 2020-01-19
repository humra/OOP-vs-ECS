using UnityEngine;
using UnityEngine.AI;

public class UnitAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if(navMeshAgent == null)
        {
            Debug.LogError("UnitAI - NavMeshAgent not found on " + gameObject.name);
            Destroy(gameObject);
        }

        WalkToTarget();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void WalkToTarget()
    {
        navMeshAgent.SetDestination(target.position);
    }
}
