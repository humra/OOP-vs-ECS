using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class Testing : MonoBehaviour
{
    [SerializeField]
    private Mesh mesh;
    [SerializeField]
    private Material material;
    [SerializeField]
    private int spawnNumber = 5;

    private void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent), 
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds),
            typeof(MoveSpeedComponent));
        NativeArray<Entity> entityArray = new NativeArray<Entity>(spawnNumber, Allocator.Temp);

        entityManager.CreateEntity(entityArchetype, entityArray);
        
        for(int i = 0; i < entityArray.Length; i++)
        {
            Entity entity = entityArray[i];
            entityManager.SetComponentData(entity, new LevelComponent { level = UnityEngine.Random.Range(1, 20) });
            entityManager.SetComponentData(entity, new MoveSpeedComponent { moveSpeed = UnityEngine.Random.Range(1f, 3f) });
            entityManager.SetComponentData(entity, new Translation { Value = new float3(UnityEngine.Random.Range(1, 20), UnityEngine.Random.Range(1, 20), 0) });

            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = this.mesh,
                material = this.material
            });
        }

        entityArray.Dispose();
    }
}
