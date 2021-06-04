using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ingredient : MonoBehaviour
{
    private int boardX, boardY;
    private GameBoardTile tile;
    private IngredientsManager manager;
    private Recipes.eIngredients ingredientType;
    private bool wait;
    private bool died = true;


    public void PleaseDontForgetToInitMe(int boardX, int boardY, Recipes.eIngredients ingredientType)
    {
        this.boardX = boardX;
        this.boardY = boardY;
        this.ingredientType = ingredientType;

        wait = false;
        tile = Game.BOARD.GetTile(boardX, boardY);
    }


    public bool IsBlocked()
    {
        if (tile == null)
        {
            return false;
        }

        Vector3 p1 = transform.position + tile.GetMovePattern().Step(transform.position);
        for (int i = 0; i < manager.transform.childCount; i++)
        {
            Ingredient ingredient = manager.transform.GetChild(i).GetComponent<Ingredient>();
            if (ingredient != null && ingredient != this)
            {
                Vector3 p2 = ingredient.transform.position;
                if((p1 - p2).magnitude < 2.0f)
                {
                    return true;
                }
            }
        }

        return false;
    }


    public void Start()
    {
        manager = GameObject.Find("IngredientsManager").GetComponent<IngredientsManager>();
    }


    public void Update()
    {
        if (tile == null)
        {
            if(died) IngredientKillEffects.PlayDeathEffect((int)ingredientType - 1, transform.position, transform.parent);
            Game.GAME.AddScore(-100);
            Destroy(gameObject);
            Debug.Log("DESTROYED: fell out of board");
            return;
        }

        if (IsBlocked())
            return;

        IBoardMovePattern movePattern = tile.GetMovePattern();

        if (wait)
        {
            int nextX = boardX + (int)movePattern.NextTile().x;
            int nextY = boardY + (int)movePattern.NextTile().y;
            GameBoardTile nextTile = Game.BOARD.GetTile(nextX, nextY);
            if(tile is BaseTile && nextTile is BaseTile)
            {
                if (IsConveyerBeltAdjacent((BaseTile) tile, (BaseTile) nextTile))
                {
                    boardX = nextX;
                    boardY = nextY;
                    tile = nextTile;
                    transform.position = tile.GetMovePattern().GetStart();
                    wait = false;
                }
            }
            return;
        }

        transform.position += tile.GetMovePattern().Step(transform.position);
        transform.Rotate(tile.GetMovePattern().RotStep());
        if (movePattern.ReachedDestination(transform.position))
        {
            int nextX = boardX + (int) movePattern.NextTile().x;
            int nextY = boardY + (int)movePattern.NextTile().y;

            // This needs to go somewhere else!
            if (nextX == 0 && nextY == -1)
            {
                died = false;
                CookingPlace pot = GameObject.Find("CookingPot1").GetComponent<CookingPlace>();
                pot.AddIngredient(ingredientType);
            }
            else if (nextX == 2 && nextY == -1)
            {
                died = false;
                CookingPlace pot = GameObject.Find("CookingPot2").GetComponent<CookingPlace>();
                pot.AddIngredient(ingredientType);
            }

            GameBoardTile nextTile = Game.BOARD.GetTile(nextX, nextY);
            if (nextTile == null)
            {
                tile = null;
            }
            else
            {
                wait = true;
            }
        }
    }


    private bool IsConveyerBeltAdjacent(BaseTile endingTile, BaseTile startingTile)
    {
        return endingTile.GetEnd() == BaseTile.eDirection.left && startingTile.GetStart() == BaseTile.eDirection.right ||
            endingTile.GetEnd() == BaseTile.eDirection.right && startingTile.GetStart() == BaseTile.eDirection.left ||
            endingTile.GetEnd() == BaseTile.eDirection.down && startingTile.GetStart() == BaseTile.eDirection.up ||
            endingTile.GetEnd() == BaseTile.eDirection.up && startingTile.GetStart() == BaseTile.eDirection.down;
    }
}