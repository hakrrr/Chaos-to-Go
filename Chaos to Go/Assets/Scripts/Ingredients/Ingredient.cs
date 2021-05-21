using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    enum movementType
    {
        wait, topdown, downtop, leftright, rightleft, topleft, topright, downleft, downright, lefttop, leftdown, righttop, rightdown
    }

    private movementType move;
    

    // Start is called before the first frame update
    void Start()
    {
        //check where are you find_tile()
        //set as current tile
    }

    // Update is called once per frame
    void Update()
    {
        //if waiting
        if (move == movementType.wait)
        {
            //check if newtile changed
            
            //can move further -> nowait, change new to current, move
        }
        //else move
            //if out of bounds
            //where are you?
                //nowhere -> die
                //newtile
                    //compare: can move further?
                        //YES: switch new to current, move accordingly
                        //NO: wait till it changes 
                //goal -> yay -> die
    }

    void find_tile(Vector2 position)
    {
        GameObject gameBoard = GameObject.Find("GameBoard");
        GameBoard gameBoardScript = gameBoard.GetComponent<GameBoard>();
        GameBoardTile tile = gameBoardScript.FindClosestTileTo(new Vector3(position.x, 0, position.y));

        move = movementType.wait;
    }
}
