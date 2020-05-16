using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CombatSystem : ComponentSystem
{
    public static List<Entity> player1Entities = new List<Entity>();
    public static List<Entity> player2Entities = new List<Entity>();

    private EntityManager entityManager;
    private List<Entity> attackers = new List<Entity>();
    private List<Entity> defenders = new List<Entity>();
    private List<Entity> toBeDestroyed = new List<Entity>();

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref CombatStatsComponent combatStatsComponent, ref Translation translation) =>
        {
            combatStatsComponent.attackCooldown -= Time.DeltaTime;

            if (combatStatsComponent.team == 1)
            {
                for (int i = 0; i < player2Entities.Count; i++)
                {
                    if (combatStatsComponent.team == entityManager.GetComponentData<CombatStatsComponent>(player2Entities[i]).team)
                    {
                        continue;
                    }

                    float distance = math.distance(translation.Value, entityManager.GetComponentData<Translation>(player2Entities[i]).Value);

                    if (distance <= combatStatsComponent.engageRange)
                    {
                        combatStatsComponent.inCombat = true;
                        attackers.Add(entity);
                        defenders.Add(player2Entities[i]);
                        break;
                    }
                }

            }
            else
            {
                for (int i = 0; i < player1Entities.Count; i++)
                {
                    if (combatStatsComponent.team == entityManager.GetComponentData<CombatStatsComponent>(player1Entities[i]).team)
                    {
                        continue;
                    }

                    float distance = math.distance(translation.Value, entityManager.GetComponentData<Translation>(player1Entities[i]).Value);

                    if (distance <= combatStatsComponent.engageRange)
                    {
                        combatStatsComponent.inCombat = true;
                        attackers.Add(entity);
                        defenders.Add(player1Entities[i]);
                        break;
                    }
                }

            }
        });

        for (int i = 0; i < attackers.Count; i++)
        {
            if (entityManager.GetComponentData<CombatStatsComponent>(attackers[i]).attackCooldown > 0)
            {
                continue;
            }

            CombatStatsComponent defenderStats = entityManager.GetComponentData<CombatStatsComponent>(defenders[i]);
            defenderStats.health -= entityManager.GetComponentData<CombatStatsComponent>(attackers[i]).damage;
            entityManager.SetComponentData(defenders[i], defenderStats);

            CombatStatsComponent attackerStats = entityManager.GetComponentData<CombatStatsComponent>(attackers[i]);

            if (entityManager.GetComponentData<CombatStatsComponent>(defenders[i]).health <= 0)
            {
                toBeDestroyed.Add(defenders[i]);
                attackerStats.inCombat = false;
            }

            attackerStats.attackCooldown = UnitStats.attackCooldown;
            entityManager.SetComponentData(attackers[i], attackerStats);
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
        defenders.Clear();
        attackers.Clear();
    }
}
