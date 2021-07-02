using UnityEngine;
using System;


public interface IBoardMovePattern
{
    Vector3 Step(Ingredient ingr, Vector3 position);
    Vector3 RotStep(Ingredient ingr);
    Vector3 GetStart(Ingredient ingr);
    bool ReachedDestination(Ingredient ingr, Vector3 position);
    Vector2 NextTile(Ingredient ingr);
}