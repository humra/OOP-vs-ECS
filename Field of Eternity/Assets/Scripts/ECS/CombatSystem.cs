using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class CombatSystem : ComponentSystem
{
    public static List<Entity> player1Entities = new List<Entity>();
    public static List<Entity> player2Entities = new List<Entity>();

    private EntityManager entityManager;
    private List<Entity> toBeDestroyed = new List<Entity>();

    private CombatStatsComponent attackerStats;
    private CombatStatsComponent defenderStats;
    private float3 attackerPosition;
    private float distance;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        for (int i = 0; i < player1Entities.Count; i++)
        {
            attackerStats = entityManager.GetComponentData<CombatStatsComponent>(player1Entities[i]);
            attackerStats.attackCooldown -= Time.DeltaTime;

            if (!attackerStats.inCombat)
            {
                attackerPosition = entityManager.GetComponentData<Translation>(player1Entities[i]).Value;

                for (int j = 0; j < player2Entities.Count; j++)
                {
                    if (attackerStats.laneIndex == entityManager.GetComponentData<CombatStatsComponent>(player2Entities[j]).laneIndex)
                    {
                        distance = math.distance(attackerPosition, entityManager.GetComponentData<Translation>(player2Entities[j]).Value);

                        if (distance <= attackerStats.engageRange)
                        {
                            attackerStats.inCombat = true;
                            attackerStats.targetIndex = j;
                            break;
                        }
                    }
                }
            }
            else if (attackerStats.attackCooldown <= 0)
            {
                defenderStats = entityManager.GetComponentData<CombatStatsComponent>(player2Entities[attackerStats.targetIndex]);
                defenderStats.health -= attackerStats.damage;
                attackerStats.attackCooldown = UnitStats.attackCooldown;

                if (defenderStats.health <= 0)
                {
                    attackerStats.inCombat = false;
                    toBeDestroyed.Add(player2Entities[attackerStats.targetIndex]);
                }
                else
                {
                    entityManager.SetComponentData(player2Entities[attackerStats.targetIndex], defenderStats);
                }

            }

            entityManager.SetComponentData(player1Entities[i], attackerStats);
        }

        for (int i = 0; i < player2Entities.Count; i++)
        {
            attackerStats = entityManager.GetComponentData<CombatStatsComponent>(player2Entities[i]);
            attackerStats.attackCooldown -= Time.DeltaTime;

            if (!attackerStats.inCombat)
            {
                attackerPosition = entityManager.GetComponentData<Translation>(player2Entities[i]).Value;

                for (int j = 0; j < player1Entities.Count; j++)
                {
                    if (attackerStats.laneIndex == entityManager.GetComponentData<CombatStatsComponent>(player1Entities[j]).laneIndex)
                    {
                        distance = math.distance(attackerPosition, entityManager.GetComponentData<Translation>(player1Entities[j]).Value);

                        if (distance <= attackerStats.engageRange)
                        {
                            attackerStats.inCombat = true;
                            attackerStats.targetIndex = j;
                            break;
                        }
                    }
                }
            }
            else if (attackerStats.attackCooldown <= 0)
            {
                defenderStats = entityManager.GetComponentData<CombatStatsComponent>(player1Entities[attackerStats.targetIndex]);
                defenderStats.health -= attackerStats.damage;
                attackerStats.attackCooldown = UnitStats.attackCooldown;

                if (defenderStats.health <= 0)
                {
                    attackerStats.inCombat = false;
                    toBeDestroyed.Add(player1Entities[attackerStats.targetIndex]);
                }
                else
                {
                    entityManager.SetComponentData(player1Entities[attackerStats.targetIndex], defenderStats);
                }

            }

            entityManager.SetComponentData(player2Entities[i], attackerStats);
        }

        for (int i = toBeDestroyed.Count - 1; i >= 0; i--)
        {
            if (player1Entities.Contains(toBeDestroyed[i]))
            {
                player1Entities.Remove(toBeDestroyed[i]);
            }
            else if (player2Entities.Contains(toBeDestroyed[i]))
            {
                player2Entities.Remove(toBeDestroyed[i]);
            }

            entityManager.DestroyEntity(toBeDestroyed[i]);
        }

        toBeDestroyed.Clear();
    }
}
