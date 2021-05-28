using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameBoardTile : MonoBehaviour
{
    public uint x = 0, y = 0;

    protected IBoardMovePattern movePattern;

    // Start is called before the first frame update
    void Start()
    {
        RegisterDragBehavior();
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    protected void RegisterDragBehavior()
    {
        GameBoard board = FindObjectOfType<GameBoard>();
        GameBoardTileBehavior dragBehavior = new GameBoardTileBehavior(board, this);
        DraggableObject draggableObject = GetComponent<DraggableObject>();
        if(draggableObject != null)
        {
            draggableObject.behavior = dragBehavior;
        }
    }


    public IBoardMovePattern GetMovePattern()
    {
        return movePattern;
    }


    public virtual void InitMovementPattern() { }
}
