﻿using Unity.Entities;
using Unity.Transforms;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementComponent movement, ref Translation translation) =>
        {
            translation.Value.x += movement.movementSpeed * movement.movementDirection * Time.DeltaTime;
        });
    }
}