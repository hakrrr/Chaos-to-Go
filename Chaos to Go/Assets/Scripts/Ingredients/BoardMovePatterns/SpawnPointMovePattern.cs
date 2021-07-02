using System;
using UnityEngine;

public class SpawnPointMovePattern : StraightMovement
{
    public SpawnPointMovePattern(BaseTile.eDirection direction, Vector3 centerPos, float radius, float speed, Vector3 startHeightOffset, Vector3 endheightOffset) : base(direction, centerPos, radius, speed)
    {
        startPos += startHeightOffset;
        endPos += endheightOffset;
    }


    public override bool ReachedDestination(Ingredient ingr, Vector3 position)
    {
        return (endPos - position).magnitude < 0.03f;
    }
}
