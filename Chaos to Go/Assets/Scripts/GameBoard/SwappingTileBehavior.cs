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
    private AudioSource[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<GameBoardTile>();
        try
        {
            sounds = GameObject.Find("Audio").GetComponents<AudioSource>();
        }
        catch (Exception) { }
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

        // This is ugly but it will work for now!
        if (selected != this)
        {
            Unmark();
            if (selected != null)
            {
                selected.MarkNeighbors(new Color(0, 1, 0));
            }
        }
        else
        {
            if (IsTileOccupated(tile))
            {
                OnRelease();
                selected = null;
            }
        }
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
        //Mark(new Color(0.38f, 0.64f, 1.0f));
        Mark(new Color(0.0f, 0.5f, 0.15f));
        if(sounds != null) sounds[0].Play();
    }


    private void OnRelease()
    {
        Unmark();
        if (sounds != null)  sounds[2].Play();
    }


    private void MarkNeighbors(Color color)
    {
        GameBoard board = GameBoard.GAME_BOARD;
        List<GameBoardTile> neighbors = new List<GameBoardTile>();
        if (board.GetTile((int)x - 1, (int)y) != null) neighbors.Add(board.GetTile((int)x - 1, (int)y));
        if (board.GetTile((int)x + 1, (int)y) != null) neighbors.Add(board.GetTile((int)x + 1, (int)y));
        if (board.GetTile((int)x, (int)y - 1) != null) neighbors.Add(board.GetTile((int)x, (int)y - 1));
        if (board.GetTile((int)x, (int)y + 1) != null) neighbors.Add(board.GetTile((int)x, (int)y + 1));
        foreach (GameBoardTile tile in neighbors)
        {
            if (!IsTileOccupated(tile))
            {
                tile.GetComponent<SwappingTileBehavior>().Mark(color);
            }
            else
            {
                tile.GetComponent<SwappingTileBehavior>().Unmark();
            }
        }
    }


    private void Mark(Color color)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            try
            {
                Material material = renderer.material;
                material.SetColor("_MarkedCol", color);
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
