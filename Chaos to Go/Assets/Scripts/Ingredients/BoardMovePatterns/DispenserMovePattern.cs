using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DispenserMovePattern : IBoardMovePattern
{
    private BaseTile.eDirection direction;
    private float radius, speed;

    protected Vector3 startPos, endPos, centerPos;


    public DispenserMovePattern(BaseTile.eDirection direction, Vector3 centerPos, float radius, float speed)
    {
        this.direction = direction;
        this.radius = radius;
        this.speed = speed;
        this.centerPos = centerPos;
    }


    public Vector3 Step(Ingredient ingr, Vector3 position)
    {
        throw new NotImplementedException();
    }


    public Vector3 RotStep(Ingredient ingr)
    {
        throw new NotImplementedException();
    }


    public Vector3 GetStart(Ingredient ingr)
    {
        throw new NotImplementedException();
    }


    public bool ReachedDestination(Ingredient ingr, Vector3 position)
    {
        throw new NotImplementedException();
    }


    public Vector2 NextTile(Ingredient ingr)
    {
        throw new NotImplementedException();
    }
}