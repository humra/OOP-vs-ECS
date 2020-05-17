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
    private float unitMovementSpeed = 3f;
    [SerializeField]
    private int wavesBeforeIncrement = 3;
    [SerializeField]
    private int[] spawnsInWave = new int[] { 1, 9, 25, 100 };
    [SerializeField]
    private Transform[] player1SpawnPoints;
    [SerializeField]
    private Transform[] player2SpawnPoints;
    [SerializeField]
    private BoxCollider player1BaseCollider;
    [SerializeField]
    private BoxCollider player2BaseCollider;

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
        NativeArray<Entity> units01Array = new NativeArray<Entity>(player1SpawnPoints.Length, Allocator.Temp);
        NativeArray<Entity> units02Array = new NativeArray<Entity>(player2SpawnPoints.Length, Allocator.Temp);

        entityManager.CreateEntity(archetype_unit01, units01Array);
        entityManager.CreateEntity(archetype_unit02, units02Array);

        Entity entity;

        for (int i = 0; i < player1SpawnPoints.Length; i++)
        {
            entity = units01Array[i];

            entityManager.SetComponentData(entity, new Translation
            {
                Value =
                new float3(player1SpawnPoints[i].position.x,
                player1SpawnPoints[i].position.y + 0.5f,
                player1SpawnPoints[i].position.z)
            });
            entityManager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(-90, 90, 0) });
            entityManager.SetComponentData(entity, new MovementComponent { movementDirection = 1, movementSpeed = unitMovementSpeed });
            entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit01, material = material_unit01 });
            entityManager.SetComponentData(entity, new CollisionComponent
            {
                maxX = player2BaseCollider.bounds.max.x,
                maxY = player2BaseCollider.bounds.max.y,
                maxZ = player2BaseCollider.bounds.max.z,
                minX = player2BaseCollider.bounds.min.x,
                minY = player2BaseCollider.bounds.min.y,
                minZ = player2BaseCollider.bounds.min.z
            });
            entityManager.SetComponentData(entity, new CombatStatsComponent
            {
                health = UnitStats.health,
                damage = UnitStats.damage,
                engageRange = UnitStats.engageRange,
                attackCooldown = UnitStats.attackCooldown,
                inCombat = false,
                targetIndex = -1
            });
            
            CombatSystem.player1Entities.Add(entity);
        }

        for (int i = 0; i < player2SpawnPoints.Length; i++)
        {
            entity = units02Array[i];

            entityManager.SetComponentData(entity, new Translation
            {
                Value =
                new float3(player2SpawnPoints[i].position.x,
                player2SpawnPoints[i].position.y + 0.5f,
                player2SpawnPoints[i].position.z)
            });
            entityManager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(90, 90, 0) });
            entityManager.SetComponentData(entity, new MovementComponent { movementDirection = -1, movementSpeed = unitMovementSpeed });
            entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit02, material = material_unit02 });
            entityManager.SetComponentData(entity, new CollisionComponent
            {
                maxX = player1BaseCollider.bounds.max.x,
                maxY = player1BaseCollider.bounds.max.y,
                maxZ = player1BaseCollider.bounds.max.z,
                minX = player1BaseCollider.bounds.min.x,
                minY = player1BaseCollider.bounds.min.y,
                minZ = player1BaseCollider.bounds.min.z
            });
            entityManager.SetComponentData(entity, new CombatStatsComponent
            {
                health = UnitStats.health,
                damage = UnitStats.damage,
                engageRange = UnitStats.engageRange,
                attackCooldown = UnitStats.attackCooldown,
                inCombat = false,
                targetIndex = -1
            });

            CombatSystem.player2Entities.Add(entity);
        }

        units01Array.Dispose();
        units02Array.Dispose();
    }

    private void SpawnMultipleUnits(int amount)
    {
        double root = Mathf.Sqrt(amount);
        if(root % 1 != 0)
        {
            Debug.Log("UnitSpawner.cs: Amount is not a perfect square!");
            return;
        }

        NativeArray<Entity> units01Array = new NativeArray<Entity>(player1SpawnPoints.Length * amount, Allocator.Temp);
        NativeArray<Entity> units02Array = new NativeArray<Entity>(player2SpawnPoints.Length * amount, Allocator.Temp);

        entityManager.CreateEntity(archetype_unit01, units01Array);
        entityManager.CreateEntity(archetype_unit02, units02Array);

        int currentIndex = 0;
        Entity entity;

        for(int k = 0; k < player1SpawnPoints.Length; k++)
        {
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    entity = units01Array[currentIndex];

                    entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit01, material = material_unit01 });
                    entityManager.SetComponentData(entity, new MovementComponent { movementDirection = 1, movementSpeed = unitMovementSpeed });
                    entityManager.SetComponentData(entity, new Translation
                    {
                        Value =
                        new float3(player1SpawnPoints[k].position.x + (i * 4), player1SpawnPoints[k].position.y + 0.5f, player1SpawnPoints[k].position.z + (j * 4))
                    });
                    entityManager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(-90, 90, 0) });
                    entityManager.SetComponentData(entity, new CollisionComponent
                    {
                        maxX = player2BaseCollider.bounds.max.x,
                        maxY = player2BaseCollider.bounds.max.y,
                        maxZ = player2BaseCollider.bounds.max.z,
                        minX = player2BaseCollider.bounds.min.x,
                        minY = player2BaseCollider.bounds.min.y,
                        minZ = player2BaseCollider.bounds.min.z
                    });
                    entityManager.SetComponentData(entity, new CombatStatsComponent
                    {
                        health = UnitStats.health,
                        damage = UnitStats.damage,
                        engageRange = UnitStats.engageRange,
                        attackCooldown = UnitStats.attackCooldown,
                        inCombat = false,
                        targetIndex = -1
                    });

                    CombatSystem.player1Entities.Add(entity);
                    currentIndex++;
                }
            }
        }

        currentIndex = 0;

        for (int k = 0; k < player2SpawnPoints.Length; k++)
        {
            for (int i = 0; i < root; i++)
            {
                for (int j = 0; j < root; j++)
                {
                    entity = units02Array[currentIndex];

                    entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh_unit02, material = material_unit02 });
                    entityManager.SetComponentData(entity, new MovementComponent { movementDirection = -1, movementSpeed = unitMovementSpeed });
                    entityManager.SetComponentData(entity, new Translation
                    {
                        Value =
                        new float3(player2SpawnPoints[k].position.x + (i * 4), player2SpawnPoints[k].position.y + 0.5f, player2SpawnPoints[k].position.z + (j * 4))
                    });
                    entityManager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(90, 90, 0) } );
                    entityManager.SetComponentData(entity, new CollisionComponent
                    {
                        maxX = player1BaseCollider.bounds.max.x,
                        maxY = player1BaseCollider.bounds.max.y,
                        maxZ = player1BaseCollider.bounds.max.z,
                        minX = player1BaseCollider.bounds.min.x,
                        minY = player1BaseCollider.bounds.min.y,
                        minZ = player1BaseCollider.bounds.min.z
                    });
                    entityManager.SetComponentData(entity, new CombatStatsComponent
                    {
                        health = UnitStats.health,
                        damage = UnitStats.damage,
                        engageRange = UnitStats.engageRange,
                        attackCooldown = UnitStats.attackCooldown,
                        inCombat = false,
                        targetIndex = -1
                    });

                    CombatSystem.player2Entities.Add(entity);
                    currentIndex++;
                }
            }
        }

        units01Array.Dispose();
        units02Array.Dispose();
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
            typeof(RenderBounds),
            typeof(Rotation),
            typeof(MovementComponent),
            typeof(CollisionComponent),
            typeof(CombatStatsComponent));

        archetype_unit02 = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds),
            typeof(Rotation),
            typeof(MovementComponent),
            typeof(CollisionComponent),
            typeof(CombatStatsComponent));
    }
}
