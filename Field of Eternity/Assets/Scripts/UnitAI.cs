using UnityEngine;
using UnityEngine.AI;

public class UnitAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private bool playerOwned;
    private Animator animator;
    private bool groupMember = false;
    private bool dead = false;
    private UnitAI targetEnemy;

    [SerializeField]
    private int health = 10;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float engageRange = 5f;
    [SerializeField]
    private float attackSpeed = 1f;

    public bool inCombat = false;
    public IUnitManager unitManager;

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
        if(dead)
        {
            HandleGroupMemberDestroy();
            unitManager.StopTrackingUnit(this);
            Destroy(gameObject);
        }

        if(inCombat)
        {
            navMeshAgent.SetDestination(targetEnemy.transform.position);
        }

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

    public bool GetPlayerOwned()
    {
        return playerOwned;
    }

    public void SetGroupMember(bool isGroupMember)
    {
        groupMember = isGroupMember;
    }

    public float GetEngageRange()
    {
        return engageRange;
    }

    public void EngageTarget(UnitAI enemyUnit)
    {
        navMeshAgent.SetDestination(enemyUnit.transform.position);
        inCombat = true;
        targetEnemy = enemyUnit;
        InvokeRepeating("AttackEnemy", 0.1f, 1 / attackSpeed);
    }

    private void AttackEnemy()
    {
        animator.SetTrigger("AttackTrigger");
        if(targetEnemy.TakeDamage(damage))
        {
            inCombat = false;
            WalkToTarget();
            CancelInvoke("AttackEnemy");
        }
    }

    public bool TakeDamage(int damageToTake)
    {
        health -= damageToTake;
        if(health <= 0)
        {
            dead = true;
        }

        return dead;
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
            unitManager.UnitReachedBase();
            unitManager.StopTrackingUnit(this);
            HandleGroupMemberDestroy();
            Destroy(gameObject);
        }
        else if (other.gameObject.tag.Equals("AIBase") && playerOwned)
        {
            ResourceTracker.ComputerHealthCurrent--;
            unitManager.UnitReachedBase();
            unitManager.StopTrackingUnit(this);
            HandleGroupMemberDestroy();
            Destroy(gameObject);
        }
    }

    private void HandleGroupMemberDestroy()
    {
        if (groupMember)
        {
            GetComponentInParent<UnitGroupDestroyer>().UnitDestroyed();
        }
    }
}
