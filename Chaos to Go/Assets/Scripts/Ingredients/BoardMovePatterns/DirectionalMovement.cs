using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class StraightMovement : IBoardMovePattern
{
    private BaseTile.eDirection direction;
    private float radius, speed;

    private Vector3 startPos, endPos;


    public StraightMovement(BaseTile.eDirection direction, Vector3 centerPos, float radius, float speed)
    {
        this.direction = direction;
        this.radius = radius;
        this.speed = speed;

        switch (direction)
        {
            case BaseTile.eDirection.up:
                startPos = centerPos - radius * new Vector3(0, 0, 1);
                endPos = centerPos + radius * new Vector3(0, 0, 1);
                break;
            case BaseTile.eDirection.down:
                startPos = centerPos - radius * new Vector3(0, 0, -1);
                endPos = centerPos + radius * new Vector3(0, 0, -1);
                break;
            case BaseTile.eDirection.left:
                startPos = centerPos - radius * new Vector3(-1, 0, 0);
                endPos = centerPos + radius * new Vector3(-1, 0, 0);
                break;
            case BaseTile.eDirection.right:
                startPos = centerPos - radius * new Vector3(1, 0, 0);
                endPos = centerPos + radius * new Vector3(1, 0, 0);
                break;
        }
    }


    public Vector2 NextTile()
    {
        switch (direction)
        {
            case BaseTile.eDirection.up:
                return new Vector2(0, 1);
            case BaseTile.eDirection.down:
                return new Vector2(0, -1);
            case BaseTile.eDirection.left:
                return new Vector2(-1, 0);
            case BaseTile.eDirection.right:
                return new Vector2(1, 0);
        }
        return Vector3.zero;
    }


    public bool ReachedDestination(Vector3 position)
    {   
        return (endPos - position).magnitude < 0.01f;
    }


    public Vector3 Step(Vector3 position)
    {
        return position + speed * (endPos - startPos).normalized;
    }


    public Vector3 GetStart()
    {
        return startPos;
    }
}


public class TurningMovement : IBoardMovePattern
{
    private Vector3 tileCenter;
    private float tileRadius;
    private BaseTile.eDirection start, end;
    private float speed;

    private Vector3 startPos;
    private Vector3 endPos;


    public TurningMovement(Vector3 tileCenter, float tileRadius, BaseTile.eDirection start, BaseTile.eDirection end, float speed)
    {
        this.tileCenter = tileCenter;
        this.tileRadius = tileRadius;
        this.start = start;
        this.end = end;
        this.speed = speed;

        // Not very elegant but I'm too lazy to come up with a computation...
        switch (start)
        {
            case BaseTile.eDirection.left:
                startPos = tileCenter + tileRadius * new Vector3(-1, 0, 0);
                break;
            case BaseTile.eDirection.right:
                startPos = tileCenter + tileRadius * new Vector3(1, 0, 0);
                break;
            case BaseTile.eDirection.up:
                startPos = tileCenter + tileRadius * new Vector3(0, 0, 1);
                break;
            case BaseTile.eDirection.down:
                startPos = tileCenter + tileRadius * new Vector3(0, 0, -1);
                break;
        }
        switch (end)
        {
            case BaseTile.eDirection.left:
                endPos = tileCenter + tileRadius * new Vector3(-1, 0, 0);
                break;
            case BaseTile.eDirection.right:
                endPos = tileCenter + tileRadius * new Vector3(1, 0, 0);
                break;
            case BaseTile.eDirection.up:
                endPos = tileCenter + tileRadius * new Vector3(0, 0, 1);
                break;
            case BaseTile.eDirection.down:
                endPos = tileCenter + tileRadius * new Vector3(0, 0, -1);
                break;
        }
    }


    public Vector3 GetStart()
    {
        return startPos;
    }


    public Vector2 NextTile()
    {
        switch (end)
        {
            case BaseTile.eDirection.up:
                return new Vector2(0, 1);
            case BaseTile.eDirection.down:
                return new Vector2(0, -1);
            case BaseTile.eDirection.left:
                return new Vector2(-1, 0);
            case BaseTile.eDirection.right:
                return new Vector2(1, 0);
        }
        return Vector3.zero;
    }


    public bool ReachedDestination(Vector3 position)
    {
        return (endPos - position).magnitude < 0.01f;
    }


    public Vector3 Step(Vector3 position)
    {
        return position;
        Vector3 v1 = startPos - tileCenter;
        v1.y = 0.0f;
        Vector3 v2 = endPos - tileCenter;
        v2.y = 0.0f;
        Vector3 m = tileCenter + v1 + v2;

        Vector3 p = position - m;
        Vector3 axis = startPos - m;
        float alpha = Vector3.Angle(axis, p);
        alpha = (alpha / 360.0f) * 2.0f * Mathf.PI;
        return position + speed * new Vector3(Mathf.Cos(alpha), 0.0f, -Mathf.Sin(alpha)).normalized;
    }
}