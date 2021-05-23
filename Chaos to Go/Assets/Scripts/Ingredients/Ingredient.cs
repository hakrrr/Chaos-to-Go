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
        public string name;
        public int boundtop;
        public int bounddown;
        public int boundleft;
        public int boundright;
    }

    private Tile currentTile;
    private Tile nextTile;

    Vector2[][] topology;

    float angle = 0;
    float speed = (2 * Mathf.PI) / 5; //2*PI in degress is 360, so you get 5 seconds to complete a circle
    float radius = 5;   


    // Start is called before the first frame update
    void Start()
    {
        /*topology[0][0] = new Vector2(-6, 6);
        topology[0][1] = new Vector2(-6, 2);
        topology[0][2] = new Vector2(-6, -2);
        topology[0][3] = new Vector2(-6, -6);

        topology[1][0] = new Vector2(-2, 6);
        topology[1][1] = new Vector2(-2, 2);
        topology[1][2] = new Vector2(-2, -2);
        topology[1][3] = new Vector2(-2, -6);

        topology[2][0] = new Vector2(2, 6);
        topology[2][1] = new Vector2(2, 2);
        topology[2][2] = new Vector2(2, -2);
        topology[2][3] = new Vector2(2, -6);

        topology[3][0] = new Vector2(6, 6);
        topology[3][1] = new Vector2(6, 2);
        topology[3][2] = new Vector2(6, -2);
        topology[3][3] = new Vector2(6, -6);*/

        //nowait
        wait = false;
        currentTile.movement = movementType.none;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTile.movement == movementType.none)
        {
            find_tile(new Vector2(transform.position.x, transform.position.z));
            Debug.Log("Update: " + currentTile.movement);
        }

        

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
        /*Debug.Log("Here!");
        GameObject gameBoard = GameObject.Find("GameBoard");
        GameBoard gameBoardScript = gameBoard.GetComponent<GameBoard>();
        Debug.Log("There!");
        GameBoardTile tile = gameBoardScript.FindClosestTileTo(new Vector3(0, 0, 0));*/
        //I want to have base tile here!
        //How to do that? Inheritance :/
        //Debug.Log("Tile : " + tile.name);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
        Debug.Log("Kolidiert : " + colliders);

        foreach (Collider hit in colliders)
        {
            Collider col = hit.GetComponent<Collider>();
            if (col.gameObject.GetComponent<BaseTile>())
            {
                Debug.Log("Hit:" + hit);
                //currentTile.name = col.gameObject.name;
                string start = col.gameObject.GetComponent<BaseTile>().getStart();
                string end = col.gameObject.GetComponent<BaseTile>().getEnd();
                currentTile.movement = whichMovType(start, end);
                Debug.Log("MOVE: " + currentTile.movement);
            }

        }
    }

    movementType whichMovType(string s, string e)
    {
        if(s=="top")
        {
            if (e == "down")
                return movementType.topdown;
            if (e == "left")
                return movementType.topleft;
            if (e == "right")
                return movementType.topright;
        }
        if (s == "down")
        {
            if (e == "top")
                return movementType.downtop;
            if (e == "left")
                return movementType.downleft;
            if (e == "right")
                return movementType.downright;
        }
        if (s == "left")
        {
            if (e == "top")
                return movementType.lefttop;
            if (e == "down")
                return movementType.leftdown;
            if (e == "right")
                return movementType.leftright;
        }
        if (s == "right")
        {
            if (e == "top")
                return movementType.righttop;
            if (e == "down")
                return movementType.rightdown;
            if (e == "left")
                return movementType.rightleft;
        }
        return movementType.none;
    }

    //move according to the movement pattern
    void move(movementType pattern)
    {
        switch (pattern)
        {
            case movementType.none:
                break;
            case movementType.topdown:
                transform.position = transform.position + new Vector3(0, 0, -1) * speed * Time.deltaTime;
                break;
            case movementType.downtop:
                transform.position = transform.position + new Vector3(0, 0, 1) * speed * Time.deltaTime;
                break;
            case movementType.leftright:
                transform.position = transform.position + new Vector3(1, 0, 0) * speed * Time.deltaTime;
                break;
            case movementType.rightleft:
                transform.position = transform.position + new Vector3(-1, 0, 0) * speed * Time.deltaTime;
                break;
            case movementType.topleft:
                angle -= speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.topright:
                angle += speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.downleft:
                angle += speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.downright:
                angle -= speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.lefttop:
                angle += speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.leftdown:
                angle -= speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.righttop:
                angle -= speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                break;
            case movementType.rightdown:
                angle += speed * Time.deltaTime;
                transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
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
