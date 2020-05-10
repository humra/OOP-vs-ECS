using Unity.Entities;

public struct CombatStatsComponent : IComponentData
{
    public int health;
    public int damage;
    public float attackSpeed;
    public float engageRange;
}
