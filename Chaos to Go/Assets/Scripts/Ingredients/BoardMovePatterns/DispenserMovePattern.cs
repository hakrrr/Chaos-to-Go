using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DispenserMovePattern : IBoardMovePattern
{
    private BaseTile.eDirection direction = BaseTile.eDirection.down;
    private Queue<Ingredient> queue = new Queue<Ingredient>();
    private bool blocked = true;
    private bool nextEntered = false;
    private Dispenser tile;

    private float radius, speed;
    protected Vector3 startPos, endPos, centerPos;

    private StraightMovement left;
    private StraightMovement down;
    private StraightMovement right;


    public DispenserMovePattern(Vector3 centerPos, float radius, float speed, Dispenser tile)
    {
        this.radius = radius;
        this.speed = speed;
        this.centerPos = centerPos;
        startPos = centerPos + radius * new Vector3(0, 0, 1);

        left = new StraightMovement(BaseTile.eDirection.left, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f);
        down = new StraightMovement(BaseTile.eDirection.down, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f);
        right = new StraightMovement(BaseTile.eDirection.right, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f);

        this.tile = tile;
    }


    public Vector3 Step(Ingredient ingr, Vector3 position)
    {
        if (!queue.Contains(ingr))
        {
            queue.Enqueue(ingr);
        }

        if(!queue.Peek() == ingr)
        {
            if((position - centerPos).z > 0.8f)
            {
                return down.Step(ingr, position);
            }
            return Vector3.zero;
        }
        else
        {
            if ((position - centerPos).z > 0.8f)
            {
                return down.Step(ingr, position);
            }
            else if((position - centerPos).z > 0.05f && !blocked)
            {
                return down.Step(ingr, position);
            }
            else if((position - centerPos).z > 0.05f && blocked)
            {
                return Vector3.zero;
            }
            else if (nextEntered)
            {
                switch (direction)
                {
                    case BaseTile.eDirection.left: return left.Step(ingr, position);
                    case BaseTile.eDirection.down: return down.Step(ingr, position);
                    case BaseTile.eDirection.right: return right.Step(ingr, position);
                }
            }
            else if(!blocked)
            {
                nextEntered = true;
            }
            return Vector3.zero;
        }
    }


    public Vector3 RotStep(Ingredient ingr)
    {
        return Vector3.zero;
    }


    public Vector3 GetStart(Ingredient ingr)
    {
        return startPos;
    }


    public bool ReachedDestination(Ingredient ingr, Vector3 position)
    {
        bool r = false;
        switch (direction)
        {
            case BaseTile.eDirection.left: r = left.ReachedDestination(ingr, position); break;
            case BaseTile.eDirection.down: r = down.ReachedDestination(ingr, position); break;
            case BaseTile.eDirection.right: r = right.ReachedDestination(ingr, position); break;
        }
        return r;
    }


    public Vector2 NextTile(Ingredient ingr)
    {
        switch (direction)
        {
            case BaseTile.eDirection.left: return left.NextTile(ingr);
            case BaseTile.eDirection.down: return down.NextTile(ingr);
            case BaseTile.eDirection.right: return right.NextTile(ingr);
        }
        return Vector3.zero;
    }


    public void UpdateQueue()
    {
        if (queue.Count == 0)
        {
            return;
        }
        if (!queue.Peek().IsBlockingTile(tile))
        {
            queue.Dequeue();
            nextEntered = false;
        }
    }


    public void SetBlocked(bool blocked)
    {
        this.blocked = blocked;
    }


    public bool SetDirection(BaseTile.eDirection direction)
    {
        if(queue.Count != 0)
        {
            if((queue.Peek().transform.position - centerPos).z <= 0.6f)
            {
                return false;
            }
        }

        this.direction = direction;
        return true;
    }
}