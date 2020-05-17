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

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        for(int i = 0; i < player1Entities.Count; i++)
        {
            attackerStats = entityManager.GetComponentData<CombatStatsComponent>(player1Entities[i]);
            attackerStats.attackCooldown -= Time.DeltaTime;

            for(int j = 0; j < player2Entities.Count; j++)
            {
                float distance = math.distance(entityManager.GetComponentData<Translation>(player1Entities[i]).Value,
                    entityManager.GetComponentData<Translation>(player2Entities[j]).Value);
                if(distance <= attackerStats.engageRange)
                {
                    attackerStats.inCombat = true;
                    if(attackerStats.attackCooldown <= 0)
                    {
                        defenderStats = entityManager.GetComponentData<CombatStatsComponent>(player2Entities[j]);
                        defenderStats.health -= attackerStats.damage;
                        attackerStats.attackCooldown = UnitStats.attackCooldown;

                        if(defenderStats.health <= 0)
                        {
                            toBeDestroyed.Add(player2Entities[j]);
                            attackerStats.inCombat = false;
                        }
                        else
                        {
                            entityManager.SetComponentData(player2Entities[j], defenderStats);
                        }
                    }
                    break;
                }
            }

            entityManager.SetComponentData(player1Entities[i], attackerStats);
        }

        for (int i = 0; i < player2Entities.Count; i++)
        {
            attackerStats = entityManager.GetComponentData<CombatStatsComponent>(player2Entities[i]);
            attackerStats.attackCooldown -= Time.DeltaTime;

            for (int j = 0; j < player1Entities.Count; j++)
            {
                float distance = math.distance(entityManager.GetComponentData<Translation>(player2Entities[i]).Value,
                    entityManager.GetComponentData<Translation>(player1Entities[j]).Value);
                if (distance <= attackerStats.engageRange)
                {
                    attackerStats.inCombat = true;
                    if (attackerStats.attackCooldown <= 0)
                    {
                        defenderStats = entityManager.GetComponentData<CombatStatsComponent>(player1Entities[j]);
                        defenderStats.health -= attackerStats.damage;
                        attackerStats.attackCooldown = UnitStats.attackCooldown;

                        if (defenderStats.health <= 0)
                        {
                            toBeDestroyed.Add(player1Entities[j]);
                            attackerStats.inCombat = false;
                        }
                        else
                        {
                            entityManager.SetComponentData(player1Entities[j], defenderStats);
                        }
                    }
                    break;
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
