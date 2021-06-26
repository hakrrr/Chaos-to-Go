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

    protected Vector3 startPos, endPos, centerPos;


    public StraightMovement(BaseTile.eDirection direction, Vector3 centerPos, float radius, float speed)
    {
        this.direction = direction;
        this.radius = radius;
        this.speed = speed;
        this.centerPos = centerPos;

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


    public virtual bool ReachedDestination(Vector3 position)
    {
        // Failsafe if lagging (meaning basically if ingredient gets too far away)
        Vector2 p = new Vector2(position.x, position.z);
        Vector2 c = new Vector2(centerPos.x, centerPos.z);
        if((c - p).magnitude > 1.05f * radius)
        {
            return true;
        }

        return (endPos - position).magnitude < 0.05f;
    }


    public Vector3 Step(Vector3 position)
    {
        return speed * Time.deltaTime * (endPos - startPos).normalized;
    }


    public Vector3 RotStep()
    {
        return Vector3.zero;
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


    public virtual bool ReachedDestination(Vector3 position)
    {
        // Failsafe if lagging (meaning basically if ingredient gets too far away)
        Vector2 p = new Vector2(position.x, position.z);
        Vector2 c = new Vector2(tileCenter.x, tileCenter.z);
        if ((c - p).magnitude > 1.05f * tileRadius)
        {
            return true;
        }

        return (endPos - position).magnitude < 0.03f;
    }


    public Vector3 Step(Vector3 position)
    {
        Vector3 tileCenter = new Vector3(this.tileCenter.x, startPos.y, this.tileCenter.z);
        Vector3 v1 = startPos - tileCenter;
        v1.y = 0.0f;
        Vector3 v2 = endPos - tileCenter;
        v2.y = 0.0f;
        Vector3 m = tileCenter + v1 + v2;

        Vector3 dir = Vector3.Cross(m - position, new Vector3(0, 1, 0));

        if(((int) start + 1) % 4 == (int)end)
        {
            dir = -dir;
        }

        return speed * Time.deltaTime * dir.normalized;
    }


    // This does not work in Build Mode ... why ...  have I ever?
    /*public Vector3 RotStep()
    {
        Vector3 tileCenter = new Vector3(this.tileCenter.x, startPos.y, this.tileCenter.z);
        Vector3 v1 = startPos - tileCenter;
        v1.y = 0.0f;
        Vector3 v2 = endPos - tileCenter;
        v2.y = 0.0f;
        Vector3 m = tileCenter + v1 + v2;

        Vector3 p1 = startPos - m;
        Vector3 p2 = (startPos + Step(startPos)) - m;


        float angle = Vector3.Angle(p1, p2);
        if (((int)start + 1) % 4 == (int)end)
        {
            return new Vector3(0, -angle, 0);
        }
        return new Vector3(0, angle, 0);
    }*/


    public Vector3 RotStep()
    {
        if (((int)start + 1) % 4 == (int)end)
        {
            return new Vector3(0, -23.0f * Time.deltaTime, 0);
        }
        return new Vector3(0, 23.0f * Time.deltaTime, 0);
    }
}