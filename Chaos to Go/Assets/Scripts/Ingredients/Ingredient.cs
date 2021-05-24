using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    enum movementType
    {
        none, 
        topdown, 
        downtop, 
        leftright, 
        rightleft, 
        topleft, 
        topright, 
        downleft, 
        downright, 
        lefttop, 
        leftdown, 
        righttop, 
        rightdown
    }

    private bool wait;

    struct Tile
    {
        public movementType movement;
        public string name;
        public Vector3 position;
    }
    private bool lastTimeSingleHit;
    private Tile currentTile;
    private Tile nextTile;

    [SerializeField]
    float speed = 1f;
    int debugCounter = 0;

    float offset = 16 / (4 * 2);

    Game gameScript;
   

    // Start is called before the first frame update
    void Start()
    {
        wait = false;
        currentTile.movement = movementType.none;
        lastTimeSingleHit = true;
        GameObject game = GameObject.Find("Game");
        gameScript = game.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        //find_colisions, return array full of important collided objects
        List<GameObject> hit = find_tile(new Vector2(transform.position.x, transform.position.z));

        //Initialize, on start, but after the tiles where initialized (could be moved to on start, after testing)
        if (currentTile.movement == movementType.none)
        {
            updateCurrentTile(hit[0]);
            //Debug.Log("Initialized: " + currentTile.movement);
            //Debug.Log("CurrentTile initialized: " + currentTile.name);
        }

        //nothing there
        if (hit.Count == 0)
        {
            //die
            gameScript.AddScore(-100);
            Destroy(gameObject);
            Debug.Log("DESTROYED: fell out of board");
        }
        //one tile there
        if(hit.Count == 1)
        {
            //detected tile is not the same as current -> it was swaped
            if (!hit[0].name.Equals(currentTile.name))
            {
                //die
                gameScript.AddScore(-100);
                Destroy(gameObject);
                //Debug.Log("DESTROYED: smashed with currentTile " + currentTile.name + " and hit " + hit[0].name);
            }
            move(currentTile.movement);
            lastTimeSingleHit = true;
        }
        //two tiles there - on the boundary
        if(hit.Count == 2)
        {
            if(wait)
            {
                //Debug.Log("waiting...");
                //if both not current 
                if (!hit[0].name.Equals(currentTile.name) && !hit[1].name.Equals(currentTile.name))
                {
                    //then current tile swaped -> die
                    gameScript.AddScore(-100);
                    Destroy(gameObject);
                    //Debug.Log("DESTROYED: smashed 2");
                }
                //if one current but second not next
                else if (hit[0].name.Equals(currentTile.name) && !hit[1].name.Equals(nextTile.name))
                {
                    //then next swaped
                    //update next
                    updateNextTile(hit[1]);
                    //Debug.Log("NextTile swaped1: " + nextTile.name);
                    moveFurtherIfPossible(); 
                }
                //same in other direction
                else if (hit[1].name.Equals(currentTile.name) && !hit[0].name.Equals(nextTile.name))
                {
                    //then next swaped
                    //update next
                    updateNextTile(hit[0]);
                    //Debug.Log("NextTile swaped2: " + nextTile.name);
                    moveFurtherIfPossible();
                }
            }
            else
            {
                //Debug.Log("Not waiting");
                //check if new tile hit or still hitting previous tile
                //first time detection
                if (lastTimeSingleHit)
                {
                    //Debug.Log("First detection");
                    wait = true;
                    //if both not current 
                    if (!hit[0].name.Equals(currentTile.name) && !hit[1].name.Equals(currentTile.name))
                    {
                        //then current tile swaped -> die
                        gameScript.AddScore(-100);
                        Destroy(gameObject);
                        //Debug.Log("DESTROYED: smashed 3");
                    }
                    //check which is new, which old
                    //if one current but second not
                    else if (hit[0].name.Equals(currentTile.name) && !hit[1].name.Equals(currentTile.name))
                    {
                        //update next with new hit
                        updateNextTile(hit[1]);
                        //Debug.Log("NextTile first hit1: " + nextTile.name);
                        moveFurtherIfPossible();
                    }
                    //same in other direction
                    else if (hit[1].name.Equals(currentTile.name) && !hit[0].name.Equals(currentTile.name))
                    {
                        //update next with new hit
                        updateNextTile(hit[0]);
                        //Debug.Log("NextTile first hit1: " + nextTile.name);
                        moveFurtherIfPossible();
                    }
                }
                //passing-by detection
                else
                {
                    //like in one hit
                    //check if current changed
                    if (!hit[0].name.Equals(currentTile.name) && !hit[1].name.Equals(currentTile.name))
                    {
                        //then current tile swaped -> die
                        gameScript.AddScore(-100);
                        Destroy(gameObject);
                        //Debug.Log("DESTROYED: smashed 4");
                    }
                    //if not, move further 
                    move(currentTile.movement);
                }
            }
            lastTimeSingleHit = false;
        }
        if(hit.Count > 2)
        {
            Debug.LogError("Too many tiles detected! Am I (" + gameObject + ") in the corner?");
        }    
    }

    void moveFurtherIfPossible()
    {
        //if current and next movement match it is possible to move
        if(movementfit(currentTile.movement, nextTile.movement))
        {
            //make next the current, move accordingly and wait no more
            currentTile = nextTile;
            //Debug.Log("CAN MOVE, so CurrentTile: " + currentTile.name);
            move(currentTile.movement);
            wait = false;
        }
    }

    void updateNextTile(GameObject g)
    {
        nextTile.name = g.name;
        string start = g.GetComponent<BaseTile>().getStart();
        string end = g.GetComponent<BaseTile>().getEnd();
        nextTile.movement = whichMovType(start, end);
        nextTile.position = g.transform.position;
        //Debug.Log("UpdateNextTile call: " + nextTile.movement);
    }
    void updateCurrentTile(GameObject g)
    {
        currentTile.name = g.name;
        string start = g.GetComponent<BaseTile>().getStart();
        string end = g.GetComponent<BaseTile>().getEnd();
        currentTile.movement = whichMovType(start, end);
        currentTile.position = g.transform.position;
        //Debug.Log("UpdateCurrentTile call: " + currentTile.movement);
    }

    List<GameObject> find_tile(Vector2 position)
    {
        List<GameObject> output = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(new Vector3(position.x, (float)0.75, position.y), 2);//make it smaller but make tiles colliders bigger
        Debug.Log("Kolidiert : " + colliders);

        foreach (Collider hit in colliders)
        {
            Collider col = hit.GetComponent<Collider>();
            if (col.gameObject.GetComponent<BaseTile>())
            {                
                output.Add(col.gameObject);
            }
            if (col.gameObject.GetComponent<CookingPlace>())
            {
                //goal found: add ingredient to pot, then die
                Recipes.eIngredients ingredient = Recipes.eIngredients.empty;
                string name = gameObject.name;
                switch(name)
                {
                    case "tomato(Clone)":
                        ingredient = Recipes.eIngredients.tomato;
                        break;
                    case "onion(Clone)":
                        ingredient = Recipes.eIngredients.onion;
                        break;
                    case "carrot(Clone)":
                        ingredient = Recipes.eIngredients.carrot;
                        break;
                    case "chicken(Clone)":
                        ingredient = Recipes.eIngredients.chicken;
                        break;
                    case "asparagus(Clone)":
                        ingredient = Recipes.eIngredients.asparagus;
                        break;
                }
                col.gameObject.GetComponent<CookingPlace>().AddIngredient(ingredient);
                Debug.Log("DESTROYED: cooked" + ingredient);
                Destroy(gameObject);
            }
        }
        return output;
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
                transform.RotateAround(currentTile.position + new Vector3(-offset, 0, offset), Vector3.up, speed/(offset*Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.topright:
                transform.RotateAround(currentTile.position + new Vector3(offset, 0, offset), Vector3.up, -speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.downleft:
                transform.RotateAround(currentTile.position + new Vector3(-offset, 0, -offset), Vector3.up, -speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.downright:
                transform.RotateAround(currentTile.position + new Vector3(offset, 0, -offset), Vector3.up, speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.lefttop:
                transform.RotateAround(currentTile.position + new Vector3(-offset, 0, offset), Vector3.up, -speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.leftdown:
                transform.RotateAround(currentTile.position + new Vector3(-offset, 0, -offset), Vector3.up, speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.righttop:
                transform.RotateAround(currentTile.position + new Vector3(offset, 0, offset), Vector3.up, speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);
                break;
            case movementType.rightdown:
                transform.RotateAround(currentTile.position + new Vector3(offset, 0, -offset), Vector3.up, -speed / (offset * Mathf.Deg2Rad) * Time.deltaTime);;
                break;
        }
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
