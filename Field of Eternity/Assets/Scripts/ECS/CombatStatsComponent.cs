using Unity.Entities;

public struct CombatStatsComponent : IComponentData
{
    public int health;
    public int damage;
    public float attackCooldown;
    public float engageRange;
    public bool inCombat;
    public int targetIndex;
    public int targetVersion;
    public int laneIndex;
    public int team;
    public bool toDestroy;
}
