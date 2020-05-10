using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField]
    private Mesh mesh_unit01;
    [SerializeField]
    private Mesh mesh_unit02;
    [SerializeField]
    private Material material_unit01;
    [SerializeField]
    private Material material_unit02;
    [SerializeField]
    private float spawnCooldown = 5f;
    [SerializeField]
    private int wavesBeforeIncrement = 3;
    [SerializeField]
    private int[] spawnsInWave = new int[] { 1, 9, 25, 100 };
    [SerializeField]
    private Transform[] zombieSpawnPoints;
    [SerializeField]
    private Transform[] satyrSpawnPoints;

    private EntityManager entityManager;
    private EntityArchetype archetype_unit01;
    private EntityArchetype archetype_unit02;
    private float spawnTimer;
    private int currentWave = 0;
    private int maxWave;
    private int currentSpawnInWave;
    private bool activeSpawning = true;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        CheckForUnassignedComponents();
        CreateArchtypes();
        spawnTimer = spawnCooldown;
        maxWave = spawnsInWave.Length - 1;
        currentSpawnInWave = 1;
    }

    private void Update()
    {
        if(spawnTimer <= 0 && activeSpawning)
        {
            SpawnUnits(spawnsInWave[currentWave]);

            spawnTimer += spawnCooldown;
            if(currentSpawnInWave == wavesBeforeIncrement)
            {
                currentSpawnInWave = 1;
                currentWave++;
            }
            else
            {
                currentSpawnInWave++;
            }

            if(currentWave > maxWave)
            {
                activeSpawning = false;
                Debug.Log("Stopped spawning");
            }
        }

        spawnTimer -= Time.deltaTime;
    }

    private void SpawnUnits(int amount)
    {
        if(amount == 1)
        {
            SpawnSingleUnit();
        }
        else
        {
            SpawnMultipleUnits(amount);
        }
    }

    private void SpawnSingleUnit()
    {
        NativeArray<Entity> zombieArray = new NativeArray<Entity>(zombieSpawnPoints.Length, Allocator.Temp);
        NativeArray<Entity> satyrArray = new NativeArray<Entity>(satyrSpawnPoints.Length, Allocator.Temp);

        entityManager.CreateEntity(archetype_unit01, zombieArray);
        entityManager.CreateEntity(archetype_unit02, satyrArray);

        for (int i = 0; i < zombieSpawnPoints.Length; i++)
        {
            Entity entity = zombieArray[i];

            entityManager.SetComponentData(entity, new Translation
            {
                Value =
                new float3(zombieSpawnPoints[i].position.x,
                zombieSpawnPoints[i].position.y,
                zombieSpawnPoints[i].position.z)
            });
            entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit01, material = material_unit01 });
        }

        for (int i = 0; i < satyrSpawnPoints.Length; i++)
        {
            Entity entity = satyrArray[i];

            entityManager.SetComponentData(entity, new Translation
            {
                Value =
                new float3(satyrSpawnPoints[i].position.x,
                satyrSpawnPoints[i].position.y,
                satyrSpawnPoints[i].position.z)
            });
            entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit02, material = material_unit02 });
        }

        zombieArray.Dispose();
        satyrArray.Dispose();
    }

    private void SpawnMultipleUnits(int amount)
    {
        double root = Mathf.Sqrt(amount);
        if(root % 1 != 0)
        {
            Debug.Log("UnitSpawner.cs: Amount is not a perfect square!");
            return;
        }

        NativeArray<Entity> zombieArray = new NativeArray<Entity>(zombieSpawnPoints.Length * amount, Allocator.Temp);
        NativeArray<Entity> satyrArray = new NativeArray<Entity>(satyrSpawnPoints.Length * amount, Allocator.Temp);

        entityManager.CreateEntity(archetype_unit01, zombieArray);
        entityManager.CreateEntity(archetype_unit02, satyrArray);

        int currentIndex = 0;

        for(int k = 0; k < zombieSpawnPoints.Length; k++)
        {
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    Entity entity = zombieArray[currentIndex];

                    entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit01, material = material_unit01 });
                    entityManager.SetComponentData(entity, new Translation
                    {
                        Value =
                        new float3(zombieSpawnPoints[k].position.x + (i * 2), zombieSpawnPoints[k].position.y, zombieSpawnPoints[k].position.z + (j * 2))
                    });

                    currentIndex++;
                }
            }
        }

        currentIndex = 0;

        for (int k = 0; k < satyrSpawnPoints.Length; k++)
        {
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    Entity entity = satyrArray[currentIndex];

                    entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit02, material = material_unit02 });
                    entityManager.SetComponentData(entity, new Translation
                    {
                        Value =
                        new float3(satyrSpawnPoints[k].position.x + (i * 2), satyrSpawnPoints[k].position.y, satyrSpawnPoints[k].position.z + (j * 2))
                    });

                    currentIndex++;
                }
            }
        }

        zombieArray.Dispose();
        satyrArray.Dispose();
    }

    private void CheckForUnassignedComponents()
    {
        if(mesh_unit01 == null || mesh_unit02 == null)
        {
            Debug.LogError("UnitSpawner.cs: One or more mesh is not assigned!");
        }
        if(material_unit01 == null || material_unit02 == null)
        {
            Debug.LogError("UnitSpawner.cs: One or more materials is not assigned!");
        }
    }

    private void CreateArchtypes()
    {
        archetype_unit01 = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds));

        archetype_unit02 = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds));
    }
}
