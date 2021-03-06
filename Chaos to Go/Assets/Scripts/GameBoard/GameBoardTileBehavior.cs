using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameBoardTileBehavior : DraggableObject.IDragBehavior
{
    private GameBoard board;
    private GameBoardTile tile;
    private GameBoardTile prevClosest = null;
    private AudioSource[] sounds;

    public GameBoardTileBehavior(GameBoard board, GameBoardTile tile)
    {
        this.board = board;
        this.tile = tile;
        try
        {
            this.sounds = GameObject.Find("Audio").GetComponents<AudioSource>();
        }
        catch (Exception) {  }
    }


    public void OnDrag()
    {
        GameBoardTile closest = board.FindClosestTileTo(tile.transform.position);

        if (prevClosest != null && prevClosest != closest)
        {
            MeshRenderer[] renderers = prevClosest.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                try
                {
                    Material material = renderer.material;
                    material.SetInt("_Marked", 0);
                }
                catch (Exception) { continue; }
            }
        }
        prevClosest = closest;

        Vector3 v = closest.transform.position - tile.transform.position;
        v.y = 0.0f;
        if (v.magnitude < 2.0f)
        {
            MeshRenderer[] renderers = closest.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer renderer in renderers){
                try
                {
                    Material material = renderer.material;
                    if (board.IsTileBlocked(closest.x, closest.y))
                    {
                        material.SetColor("_MarkedCol", new Color(1, 0, 0));
                        material.SetInt("_Marked", 1);
                    }
                    else
                    {
                        material.SetColor("_MarkedCol", new Color(0, 1, 0));
                        material.SetInt("_Marked", 1);
                    }
                }
                catch (Exception){ continue; }
            }
        }
        else
        {
            MeshRenderer[] renderers = closest.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                try
                {
                    Material material = renderer.material;
                    material.SetInt("_Marked", 0);
                }
                catch (Exception) { continue; }
            }
        }
    }


    public void OnPickup()
    {
        if(sounds != null)
            sounds[0].Play();
    }

    public void OnAbort()
    {
        GameBoardTile closest = board.FindClosestTileTo(tile.transform.position);

        if (prevClosest != null && prevClosest != closest)
        {
            MeshRenderer[] renderers = prevClosest.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                try
                {
                    Material material = renderer.material;
                    material.SetInt("_Marked", 0);
                    if (sounds != null)
                        sounds[1].Play();
                }
                catch (Exception) { continue; }
            }
        }

        MeshRenderer[] renderers2 = closest.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers2)
        {
            try
            {
                Material material = renderer.material;
                material.SetInt("_Marked", 0);
                if (sounds != null)
                    sounds[1].Play();
            }
            catch (Exception) { continue; }
        }
    }

    public bool OnRelease()
    {
        if (board.FindAndReplace(tile))
        {
            tile.GetComponent<DraggableObject>().enabled = false;
            if (sounds != null)
                sounds[2].Play();
            return true;
        }
        return false;
    }
}