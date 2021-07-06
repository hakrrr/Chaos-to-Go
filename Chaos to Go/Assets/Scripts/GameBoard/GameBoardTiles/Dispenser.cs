using System;
using System.Collections;
using UnityEngine;


public class Dispenser : BaseTile
{
    private InputStates btnSelect = new InputStates(KeyCode.Mouse0);
    private InputStates btnBlock = new InputStates(KeyCode.Mouse1);

    [SerializeField]
    private ScrollTexture upScrollTex;
    [SerializeField]
    private ScrollTexture downScrollTex;
    [SerializeField]
    private ScrollTexture leftScrollTex;
    [SerializeField]
    private ScrollTexture rightScrollTex;

    private bool opened = false;

    public new void Start()
    {
        base.Start();
        start = eDirection.up;
        end = eDirection.down;
    }


    public void Update()
    {
        if (movePattern != null)
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
            Debug.Log("Swapping Direction: " + direction);
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
        if (!GameBoard.GAME_BOARD.Contains(this))
        {
            return;
        }

        if(btnBlock.Check() == InputStates.InputState.JustPressed)
        {
            if(!(movePattern as DispenserMovePattern).IsDispensing())
            {
                Block();
            }
        }
        else if(btnSelect.Check() == InputStates.InputState.JustPressed)
        {
            Vector3 mouseScrPos = Input.mousePosition;
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            Vector2 v = new Vector2(mouseScrPos.x - screenPos.x, mouseScrPos.y - screenPos.y);
            if(Math.Abs(v.x) > Math.Abs(v.y))
            {
                if(v.x > 0.0f)
                {
                    OpenDirection(eDirection.right);
                }
                else
                {
                    OpenDirection(eDirection.left);
                }
            }
            else
            {
                OpenDirection(eDirection.down);
            }
        }

        UpdateConveyorBelts();
    }


    private void UpdateConveyorBelts()
    {
        if (!opened)
        {
            upScrollTex.scrollY = 0.0f;
            downScrollTex.scrollY = 0.0f;
            leftScrollTex.scrollY = 0.0f;
            rightScrollTex.scrollY = 0.0f;
        }
        else
        {
            upScrollTex.scrollY = 0.2f;
            switch (end)
            {
                case eDirection.left:
                    {
                        
                        downScrollTex.scrollY = 0.0f;
                        leftScrollTex.scrollY = 0.2f;
                        rightScrollTex.scrollY = 0.0f;
                    } break;
                case eDirection.down:
                    {
                        downScrollTex.scrollY = 0.2f;
                        leftScrollTex.scrollY = 0.0f;
                        rightScrollTex.scrollY = 0.0f;
                    }
                    break;
                case eDirection.right:
                    {
                        downScrollTex.scrollY = 0.0f;
                        leftScrollTex.scrollY = 0.0f;
                        rightScrollTex.scrollY = 0.2f;
                    }
                    break;
            }
        }
    }
}