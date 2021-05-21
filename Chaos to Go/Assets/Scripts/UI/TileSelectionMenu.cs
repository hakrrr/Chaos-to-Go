using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelectionMenu : MonoBehaviour
{
    [SerializeField]
    private BaseTile baseTilePrefab;
    [SerializeField]
    private Transform tileRoot;

    private GameBoardTile[] tiles;



    public void AddBaseTile(int idx, BaseTile.eDirection start, BaseTile.eDirection end)
    {
        if (tiles[idx] != null && tiles[idx].transform.parent == tileRoot)
        {
            Destroy(tiles[idx].gameObject);
        }

        BaseTile newTile = Instantiate(baseTilePrefab);
        newTile.name = "BaseTile" + ((int)Random.Range(0.0f, 128.0f));
        newTile.SetDirections(start, end);
        newTile.transform.parent = tileRoot;
        newTile.transform.position = new Vector3(10.0f, 0, 10.0f + 3.0f * idx);
        tiles[idx] = newTile;
    }

    private void AddNewRandomBaseTile(int idx)
    {
        BaseTile newTile = Instantiate(baseTilePrefab);
        newTile.name = "BaseTile" + ((int)Random.Range(0.0f, 128.0f));
        int dirStart = (int)Random.Range(0.0f, 4.0f);
        int dirEnd = (int)Random.Range(0.0f, 4.0f);
        if (dirStart == dirEnd)
        {
            if (dirStart == 3)
                dirStart = 1;
            dirEnd = 3;
        }
        newTile.SetDirections((BaseTile.eDirection)dirStart, (BaseTile.eDirection)dirEnd);
        newTile.transform.parent = tileRoot;
        newTile.transform.position = new Vector3(10.0f, 0, 10.0f + 3.0f * idx);
        tiles[idx] = newTile;
    }


    private void CheckChildren()
    {
        if (tileRoot.childCount == 4)
            return;
        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].transform.parent != tileRoot)
            {
                AddNewRandomBaseTile(i);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameBoardTile[4];
        for(int i = 0; i < tiles.Length; i++)
        {
            AddNewRandomBaseTile(i);
        }
    }


    // Update is called once per frame
    void Update()
    {
        CheckChildren();
    }
}
