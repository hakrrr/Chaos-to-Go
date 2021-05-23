using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : GameBoardTile
{
    public enum eDirection
    {
        left, up, right, down
    }

    [SerializeField]
    private eDirection start = eDirection.up;
    [SerializeField]
    private eDirection end = eDirection.down;

    [SerializeField]
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


    public void Start()
    {
        AlignArrows();
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

    public string getStart()
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
    }

}
