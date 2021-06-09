using System;
using UnityEngine;

public class SpawnPointMovePattern : StraightMovement
{
    public SpawnPointMovePattern(BaseTile.eDirection direction, Vector3 centerPos, float radius, float speed, Vector3 heightOffset) : base(direction, centerPos, radius, speed)
    {
        startPos += heightOffset;        
    }
}
