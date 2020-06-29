using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CombatSystem : ComponentSystem
{
    private EntityManager entityManager;
    private float distance;
    private CombatStatsComponent attackerStatsTemp;
    private Translation attackerTranslationTemp;
    private float timestamp;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        timestamp = 1f;
    }

    protected override void OnUpdate()
    {
        Entities.WithAll<CombatStatsComponent>().ForEach((ref CombatStatsComponent combatStats) =>
        {
            combatStats.attackCooldown -= Time.DeltaTime;
        });

        if(timestamp <= 0)
        {
            Entities.WithAll<CombatStatsComponent, Translation>().ForEach((Entity attackerEntity, ref CombatStatsComponent attackerStats, ref Translation attackerTranslation) =>
            {
                attackerStatsTemp = attackerStats;
                attackerTranslationTemp = attackerTranslation;

                if (!attackerStatsTemp.inCombat)
                {
                    Entities.WithAll<CombatStatsComponent>().ForEach((Entity defenderEntity, ref CombatStatsComponent defenderStats) =>
                    {
                        if ((defenderStats.laneIndex == attackerStatsTemp.laneIndex) && (defenderStats.team != attackerStatsTemp.team))
                        {
                            if(!attackerStatsTemp.inCombat)
                            {
                                distance = math.distance(attackerTranslationTemp.Value, entityManager.GetComponentData<Translation>(defenderEntity).Value);

                                if (distance <= attackerStatsTemp.engageRange)
                                {
                                    attackerStatsTemp.targetIndex = defenderEntity.Index;
                                    attackerStatsTemp.targetVersion = defenderEntity.Version;
                                    attackerStatsTemp.inCombat = true;
                                }
                            }
                        }
                    });
                }
                else if (attackerStatsTemp.attackCooldown <= 0)
                {
                    attackerStatsTemp.attackCooldown = UnitStats.attackCooldown;

                    Entities.WithAll<CombatStatsComponent>().ForEach((Entity defenderEntity, ref CombatStatsComponent defenderStats) =>
                    {
                        if (defenderEntity.Index == attackerStatsTemp.targetIndex && defenderEntity.Version == attackerStatsTemp.targetVersion)
                        {
                            defenderStats.health -= attackerStatsTemp.damage;

                            if (defenderStats.health <= 0)
                            {
                                attackerStatsTemp.inCombat = false;
                                defenderStats.toDestroy = true;
                            }
                        }
                    });
                }

                attackerStats = attackerStatsTemp;

                if (attackerStats.toDestroy)
                {
                    //Debug.Log("Destroyed");
                    entityManager.DestroyEntity(attackerEntity);
                }
            });

            timestamp = 1f;
        }
        else
        {
            timestamp -= Time.DeltaTime;
        }
    }
}
