using System;
using System.Collections;
using UnityEngine;


public class Dispenser : BaseTile
{
    private InputStates btnSelect = new InputStates(KeyCode.Mouse0);
    private InputStates btnLeft = new InputStates(KeyCode.LeftArrow);
    private InputStates btnRight = new InputStates(KeyCode.RightArrow);
    private InputStates btnDown = new InputStates(KeyCode.DownArrow);

    private bool opened = false;

    public new void Start()
    {
        base.Start();
        start = eDirection.up;
        end = eDirection.down;
    }


    public void Update()
    {
        if(movePattern != null)
        {
            (movePattern as DispenserMovePattern).UpdateQueue();
        }
    }


    public override void InitMovementPattern()
    {
        if(movePattern != null)
        {
            return;
        }
        Vector3 centerPos = new Vector3(transform.position.x, Y_VALUE, transform.position.z);
        movePattern = new DispenserMovePattern(centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f, this);

        OpenDirection(eDirection.down);
        Block();
    }


    public void OpenDirection(eDirection direction)
    {
        if((movePattern as DispenserMovePattern).SetDirection(direction))
        {
            (movePattern as DispenserMovePattern).SetBlocked(false);
            end = direction;
            opened = true;
        }
        else
        {
            Debug.Log(":(");
        }
    }


    public void Block()
    {
        (movePattern as DispenserMovePattern).SetBlocked(true);
        opened = false;
    }


    public void Open()
    {
        (movePattern as DispenserMovePattern).SetBlocked(false);
        opened = true;
    }


    void OnMouseOver()
    {
        if (PauseMenu.PAUSED)
        {
            return;
        }

        if(btnLeft.Check() == InputStates.InputState.JustPressed)
        {
            Debug.Log("Switched direction: " + eDirection.left);
            OpenDirection(eDirection.left);
        }
        else if (btnRight.Check() == InputStates.InputState.JustPressed)
        {
            Debug.Log("Switched direction: " + eDirection.right);
            OpenDirection(eDirection.right);
        }
        else if (btnDown.Check() == InputStates.InputState.JustPressed)
        {
            Debug.Log("Switched direction: " + eDirection.down);
            OpenDirection(eDirection.down);
        }

        if (GameBoard.GAME_BOARD.Contains(this) && btnSelect.Check() == InputStates.InputState.JustPressed)
        {
            if (opened)
            {
                Block();
            }
            else
            {
                Open();
            }
        }
    }
}