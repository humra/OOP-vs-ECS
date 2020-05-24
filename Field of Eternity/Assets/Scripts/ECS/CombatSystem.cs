using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

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
        if(timestamp <= 0)
        {
            Entities.WithAll<CombatStatsComponent, Translation>().ForEach((Entity attackerEntity, ref CombatStatsComponent attackerStats, ref Translation attackerTranslation) =>
            {
                attackerStats.attackCooldown -= Time.DeltaTime;

                attackerStatsTemp = attackerStats;
                attackerTranslationTemp = attackerTranslation;

                if (!attackerStatsTemp.inCombat)
                {
                    Entities.WithAll<CombatStatsComponent>().ForEach((Entity defenderEntity, ref CombatStatsComponent defenderStats) =>
                    {
                        if ((defenderStats.laneIndex == attackerStatsTemp.laneIndex) && (defenderStats.team != attackerStatsTemp.team))
                        {
                            distance = math.distance(attackerTranslationTemp.Value, entityManager.GetComponentData<Translation>(defenderEntity).Value);

                            if (distance <= attackerStatsTemp.engageRange)
                            {
                                attackerStatsTemp.targetIndex = defenderEntity.Index;
                                attackerStatsTemp.targetVersion = defenderEntity.Version;
                                attackerStatsTemp.inCombat = true;
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
