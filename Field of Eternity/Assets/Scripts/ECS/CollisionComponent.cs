﻿using Unity.Entities;

public struct CollisionComponent : IComponentData
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
}
