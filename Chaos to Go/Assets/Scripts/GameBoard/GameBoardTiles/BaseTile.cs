using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : GameBoardTile
{
    public const float Y_VALUE = 1.25f;

    public enum eDirection
    {
        left, up, right, down
    }

    [SerializeField]
    protected eDirection start = eDirection.up;
    [SerializeField]
    protected eDirection end = eDirection.down;

    //[SerializeField]
    private GameObject arrowStart, arrowEnd;


    /**
     * This stuff is hard-coded for the arrows! Replace later with routibne for a real used model!
     */
    private void AlignArrows()
    {
        Vector3 axisX = new Vector3(1, 0, 0);

        Vector3 vStart = Quaternion.Euler(0, 90.0f * (int)start + 180.0f, 0) * axisX;
        arrowStart.transform.Translate(0.75f * vStart);
        arrowStart.transform.Rotate(new Vector3(0, 90.0f * (int)start + 180.0f, 0));

        Vector3 vEnd = Quaternion.Euler(0, 90.0f * (int)end + 180.0f, 0) * axisX;
        arrowEnd.transform.Translate(0.75f * vEnd);
        arrowEnd.transform.Rotate(new Vector3(0, 90.0f * (int)end, 0));
    }

    private void SetCurvatureAndDirection()
    {
        ////straight start end: 02, 20, 13, 31 -> even sum
        if (((int)start + (int)end) % 2 == 0)
        {
            //turn curve off
            Transform convc = gameObject.transform.Find("curve");
            if(convc != null)
            {
                convc.gameObject.SetActive(false);
            }

            Transform convcb = gameObject.transform.Find("curveBorder");
            if (convcb != null)
            {
                convcb.gameObject.SetActive(false);
            }

            //rotate
            gameObject.transform.Rotate(new Vector3(0, 90.0f * (int)start, 0));
        }
        ////curve start end: 01, 10, 03, 30, 12, 21, 23, 32 -> uneven sum
        else
        {
            //turn straight off 
            Transform convs = gameObject.transform.Find("straight");
            if (convs != null)
            {
                convs.gameObject.SetActive(false);
            }

            Transform convsb = gameObject.transform.Find("straightBorder");
            if (convsb != null)
            {
                convsb.gameObject.SetActive(false);
            }


            //set texture scrolling
            if (((int)start == 0 && (int)end == 3) || ((int)start == 1 && (int)end == 0) || ((int)start == 2 && (int)end == 1) || ((int)start == 3 && (int)end == 2))
            {
                //rotate prefab
                gameObject.transform.Rotate(new Vector3(0, 90.0f * (int)start, 0));
            }
            if (((int)start == 3 && (int)end == 0) || ((int)start == 0 && (int)end == 1) || ((int)start == 1 && (int)end == 2) || ((int)start == 2 && (int)end == 3))
            {
                //rotate prefab
                gameObject.transform.Rotate(new Vector3(0, 90.0f * (int)end, 0));

                //flip texture
                gameObject.transform.Find("curve").GetComponent<Renderer>().material.SetFloat("_TexScaleY", -1.0f);
                //rotate scrolling
                gameObject.transform.Find("curve").GetComponent<ScrollTexture>().scrollY *= -1;
            }
        }
    }


    public void Start()
    {
        //AlignArrows();
        SetCurvatureAndDirection();
        RegisterDragBehavior();
    }


    public void SetDirections(eDirection start, eDirection end)
    {
        this.start = start;
        this.end = end;
        if(start == end)
        {
            Debug.LogError("BaseTile does not allow same start and end direction! Set Values to up->down");
            start = eDirection.up;
            end = eDirection.down;
        }
    }


    public eDirection GetStart()
    {
        return start;
    }


    public eDirection GetEnd()
    {
        return end;
    }


    // Why do these exist? Do we really need them? I mean, look up... ^ ^ ^ 
    /*public string getStart()
    {
        switch(this.start)
        {
            case eDirection.down:
                return "down";
            case eDirection.up:
                return "top";
            case eDirection.left:
                return "left";
            case eDirection.right:
                return "right";
        }
        return "none";
    }

    public string getEnd()
    {
        switch (this.end)
        {
            case eDirection.down:
                return "down";
            case eDirection.up:
                return "top";
            case eDirection.left:
                return "left";
            case eDirection.right:
                return "right";
        }
        return "none";
    }*/


    public override void InitMovementPattern()
    {
        bool straight = (start == eDirection.up && end == eDirection.down) ||
            (start == eDirection.left && end == eDirection.right) ||
            (start == eDirection.right && end == eDirection.left) ||
            (start == eDirection.down && end == eDirection.up);

        Vector3 centerPos = new Vector3(transform.position.x, Y_VALUE, transform.position.z);
        if (straight)
        {
            movePattern = new StraightMovement(end, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.6f);
        }
        else
        {
            movePattern = new TurningMovement(centerPos, Game.BOARD.GetTileLengths().x / 2.0f, start, end, 0.6f);
        }
    }

}
