using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwappingTileBehavior : MonoBehaviour
{
    private static SwappingTileBehavior selected = null;

    private uint x, y;
    private GameBoardTile tile;

    private InputStates btnSelect = new InputStates(KeyCode.Mouse0);


    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<GameBoardTile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DraggableObject.IsPlayerDraggingObject() && selected == this)
        {
            OnRelease();
            selected = null;
        }
        x = tile.x;
        y = tile.y;
    }


    private bool IsTileOccupated(GameBoardTile tile)
    {
        if (!GameBoard.GAME_BOARD.Contains(tile))
            return false;

        if (GameBoard.GAME_BOARD.IsTileBlocked(tile.x, tile.y))
            return true;

        Ingredient[] ingredients = FindObjectsOfType<Ingredient>();
        foreach(Ingredient ingr in ingredients)
        {
            if (ingr.IsBlockingTile(tile))
            {
                return true;
            }
        }

        return false;
    }


    private void OnSelect()
    {
        Mark();
    }


    private void OnRelease()
    {
        Unmark();
    }


    private void Mark()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            try
            {
                Material material = renderer.material;
                material.SetColor("_MarkedCol", new Color(0.38f, 0.64f, 1.0f));
                material.SetInt("_Marked", 1);
            }
            catch (Exception) {}
        }
    }


    private void Unmark()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            try
            {
                Material material = renderer.material;
                material.SetInt("_Marked", 0);
            }
            catch (Exception) { }
        }
    }


    void OnMouseOver()
    {
        if (PauseMenu.PAUSED)
        {
            return;
        }
        if (!Game.BOARD.Contains(tile))
        {
            return;
        }
        if (DraggableObject.IsPlayerDraggingObject())
        {
            return;
        }
        if (IsTileOccupated(tile))
        {
            return;
        }

        if (btnSelect.Check() == InputStates.InputState.JustPressed)
        {
            if(selected == null)
            {
                OnSelect();
                selected = this;
            }
            else if(selected != this)
            {
                if ((Math.Abs((int)selected.x - (int)x) == 1 && selected.y == y) || (Math.Abs((int)selected.y - (int)y) == 1 && selected.x == x))
                {
                    if (!IsTileOccupated(selected.tile))
                    {
                        //Debug.Log("§§-§§");
                        //Debug.Log("> T1: " + new Vector2(x, y));
                        //Debug.Log("> T2: " + new Vector2(selected.x, selected.y));
                        GameBoard.GAME_BOARD.AddTile(selected.tile, x, y);
                        GameBoard.GAME_BOARD.AddTile(tile, selected.x, selected.y);
                        GameBoard.GAME_BOARD.Build();
                        selected.OnRelease();
                        OnRelease();
                        selected = null;
                    }
                }
                else
                {
                    selected.OnRelease();
                    OnSelect();
                    selected = this;
                }
            }
            else
            {
                OnRelease();
                selected = null;
            }
        }
    }
}
