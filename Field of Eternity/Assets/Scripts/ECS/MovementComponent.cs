using Unity.Entities;

public struct MovementComponent : IComponentData
{
    public float movementSpeed;
    public int movementDirection;
}
