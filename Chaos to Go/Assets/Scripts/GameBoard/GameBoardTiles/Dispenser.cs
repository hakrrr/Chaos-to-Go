using System;
using System.Collections;
using UnityEngine;


public class Dispenser : BaseTile
{ 
    public new void Start()
    {
        base.Start();
        start = eDirection.up;
        end = eDirection.up;
    }


    public override void InitMovementPattern()
    {
        Vector3 centerPos = new Vector3(transform.position.x, Y_VALUE, transform.position.z);
        movePattern = new DispenserMovePattern(end, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f);
    }


    public void SetEndDirection(eDirection end)
    {
        this.end = end;
    }
}