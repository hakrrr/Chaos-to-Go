using UnityEngine;
using System;


public interface IBoardMovePattern
{
    Vector3 Step(Vector3 position);
    Vector3 RotStep();
    Vector3 GetStart();
    bool ReachedDestination(Vector3 position);
    Vector2 NextTile();
}