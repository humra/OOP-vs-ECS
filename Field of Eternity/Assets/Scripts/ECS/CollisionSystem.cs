using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;

public class CollisionSystem : ComponentSystem
{
    private List<Entity> entitiesToDelete = new List<Entity>();
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref CollisionComponent collisionComponent, ref Translation translation) =>
        {
            if(collisionComponent.minX <= translation.Value.x && translation.Value.x <= collisionComponent.maxX
            && collisionComponent.minY <= translation.Value.y && translation.Value.y <= collisionComponent.maxY
            && collisionComponent.minZ <= translation.Value.z && translation.Value.z <= collisionComponent.maxZ)
            {
                entitiesToDelete.Add(entity);
                if(CombatSystem.player1Entities.Contains(entity))
                {
                    CombatSystem.player1Entities.Remove(entity);
                }
                else if(CombatSystem.player2Entities.Contains(entity))
                {
                    CombatSystem.player2Entities.Remove(entity);
                }
            }
        });

        if(entitiesToDelete.Count == 0)
        {
            return;
        }

        for(int i = entitiesToDelete.Count - 1; i >= 0; i--)
        {
            entityManager.DestroyEntity(entitiesToDelete[i]);
        }

        entitiesToDelete.Clear();
    }
}
