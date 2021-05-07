using System;
using UnityEngine;


public class GameBoardTileBehavior : DraggableObject.IDragBehavior
{
    private GameBoard board;
    private GameBoardTile tile;


    public GameBoardTileBehavior(GameBoard board, GameBoardTile tile)
    {
        this.board = board;
        this.tile = tile;
    }


    public void OnDrag()
    {
        
    }

    public void OnPickup()
    {
        
    }

    public void OnAbort()
    {
        
    }

    public bool OnRelease()
    {
        if (board.FindAndReplace(tile))
        {
            tile.GetComponent<DraggableObject>().enabled = false;
            return true;
        }
        return false;
    }
}