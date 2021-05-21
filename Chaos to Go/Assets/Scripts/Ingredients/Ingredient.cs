using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    enum movementType
    {
        none, topdown, downtop, leftright, rightleft, topleft, topright, downleft, downright, lefttop, leftdown, righttop, rightdown
    }

    private bool wait;

    struct Tile
    {
        public movementType movement;
        public int boundtop;
        public int bounddown;
        public int boundleft;
        public int boundright;
    }

    private Tile currentTile;
    private Tile nextTile;


    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Chicken!!!");
        //check where are you find_tile()
        find_tile(new Vector2(transform.position.x, transform.position.z));
        //set as current tile (INHERITANCE PROBLEM)

        //nowait
        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tileChanged(currentTile))
        {
            //HULK SMASH!
            Destroy(gameObject);
            //update negative score for destroying ingredients
        }

        //if waiting
        if (wait)
        {
            //check if newtile changed
            if (tileChanged(nextTile))
            {
                //if possible to move further after change
                if (movementfit(currentTile.movement, nextTile.movement))
                {
                    //replace current with next and move accordingly to movement pattern and wait no more
                    currentTile = nextTile;
                    move(currentTile.movement);
                    wait = false;
                }
            }
        }
        else
        {
            move(currentTile.movement);
        }

        if (outOfBounds())
        {
            //find where are you
                //nowhere -> die
                    //Destroy(gameObject);
                    //update score for destroying
                //goal -> yay -> die
                    //check if correct recipe
                    //update score accordingly
                    //Destroy(gameObject);
                //newtile
                    //wait = true;
                    //nextTile = found tile
                    //compare: can move further?
                    //YES: switch new to current, move accordingly
                    //if (movementfit(currentTile.movement, nextTile.movement))
                    //{
                        //replace current with next and move accordingly to movement pattern and wait no more
                        //currentTile = nextTile;
                        //move(currentTile.movement);
                        //wait = false;
                    //}
        }

    }

    void find_tile(Vector2 position)
    {
        Debug.Log("Here!");
        GameObject gameBoard = GameObject.Find("GameBoard");
        GameBoard gameBoardScript = gameBoard.GetComponent<GameBoard>();
        GameBoardTile tile = gameBoardScript.FindClosestTileTo(new Vector3(position.x, 0, position.y));

        //I want to have base tile here!
        //How to do that? Inheritance :/
        Debug.Log("Tile : " + tile.name);

        currentTile.movement = movementType.none;
    }

    //move according to the movement pattern
    void move(movementType pattern)
    {
        switch (pattern)
        {
            case movementType.none:
                break;
            case movementType.topdown:
                break;
            case movementType.downtop:
                break;
            case movementType.leftright:
                break;
            case movementType.rightleft:
                break;
            case movementType.topleft:
                break;
            case movementType.topright:
                break;
            case movementType.downleft:
                break;
            case movementType.downright:
                break;
            case movementType.lefttop:
                break;
            case movementType.leftdown:
                break;
            case movementType.righttop:
                break;
            case movementType.rightdown:
                break;
        }
    }

    //check if ingredient moved out of current conveyor belt tile
    bool outOfBounds()
    {
        if (transform.position.x >= currentTile.boundright || transform.position.x <= currentTile.boundleft ||
            transform.position.z >= currentTile.boundtop || transform.position.z <= currentTile.bounddown)
            return true;
        return false;
    }

    //check if corresponding tile has been replaced and update local variables
    bool tileChanged(Tile t)
    {
        //find tile at position
        //compare with local variable (compare names or some kind of IDs so that the movement pattern is not misleading
        return false;
    }

    bool movementfit(movementType curr, movementType next)
    {
        //down to top
        if (curr == movementType.topdown || curr == movementType.rightdown || curr == movementType.leftdown)
            if (next == movementType.topdown || next == movementType.topright || next == movementType.topleft)
                return true;
        //top to down
        if (curr == movementType.downtop || curr == movementType.righttop || curr == movementType.lefttop)
            if (next == movementType.downtop || next == movementType.downright || next == movementType.downleft)
                return true;
        //left to right
        if (curr == movementType.topleft || curr == movementType.downleft || curr == movementType.rightleft)
            if (next == movementType.righttop || next == movementType.rightdown || next == movementType.rightleft)
                return true;
        //right to left
        if (curr == movementType.topright || curr == movementType.downright || curr == movementType.leftright)
            if (next == movementType.lefttop || next == movementType.leftdown || next == movementType.leftright)
                return true;
        return false;
    }
}
