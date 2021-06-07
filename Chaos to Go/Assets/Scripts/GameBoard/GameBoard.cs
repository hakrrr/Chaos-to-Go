using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private uint x = 4, y = 4;
    [SerializeField]
    private Vector3 botleftPos = new Vector2(-6f, -6f);
    [SerializeField]
    private Vector2 tileDim = new Vector2(4f, 4f);
    [SerializeField]
    private GameBoardTile emptyTilePrefab;
    [SerializeField]
    private BaseTile baseTilePrefab;
    [SerializeField]
    private SpawnPoint spawnPointPrefab;
    [SerializeField]
    private Transform tileRoot;

    private InstanceMatrix<GameBoardTile> tileMatrix;
    private InstanceMatrix<bool> blockedTiles;


    public void FillEmpty()
    {
        Fill(emptyTilePrefab);
    }


    public void Fill(GameBoardTile prefab)
    {
        for (uint i = 0; i < x; i++)
        {
            for (uint j = 0; j < y; j++)
            {
                GameBoardTile tile = Instantiate(prefab);
                tile.name = "Tile" + i + "_" + j;
                AddTile(tile, i, j);
            }
        }
    }


    public void FillRandom(BaseTile prefab)
    {
        for (uint i = 0; i < x; i++)
        {
            for (uint j = 0; j < y; j++)
            {
                BaseTile tile = Instantiate(prefab);
                int dirStart = (int)Random.Range(0.0f, 3.0f);
                int dirEnd = (int)Random.Range(0.0f, 3.0f);
                if(dirEnd == 1) dirEnd = 3;

                if (dirStart == dirEnd)
                {
                    if (dirStart == 3)
                        dirStart = 1;
                    dirEnd = 3;
                }
                tile.SetDirections((BaseTile.eDirection)dirStart, (BaseTile.eDirection)dirEnd);
                //tile.SetDirections(BaseTile.eDirection.left, BaseTile.eDirection.right);
                tile.name = "Tile" + i + "_" + j;
                AddTile(tile, i, j);
                Destroy(tile.GetComponent<DraggableObject>());
            }
        }
    }


    public void AddSpawnPoint(uint x, uint y, BaseTile.eDirection direction, GameObject prefab)
    {
        SpawnPoint spawnPoint = Instantiate(prefab).GetComponent<SpawnPoint>();
        spawnPoint.name = "SpawnPoint";
        GameBoardTile oldTile = tileMatrix.Get(x, y);
        AddTile(spawnPoint, x, y);
        Destroy(oldTile.gameObject);
    }


    public void Build()
    {
        for(uint i = 0; i < x; i++)
        {
            for(uint j = 0; j < y; j++)
            {
                GameBoardTile tile = tileMatrix.Get(i, j);
                tile.transform.parent = tileRoot;
                tile.transform.localPosition = new Vector3(botleftPos.x + i * tileDim.x, 0, botleftPos.y + j * tileDim.y);
                tile.InitMovementPattern();
            }
        }
    }


    public void AddTile(GameBoardTile tile, uint x, uint y)
    {
        if (blockedTiles.Get(x, y))
            return;
        //tile.GetComponent<Collider>().enabled = false;
        tileMatrix.Set(x, y, tile);
        tile.x = x;
        tile.y = y;
    }


    public GameBoardTile GetTile(int x, int y)
    {
        if (x >= this.x || y >= this.y || x < 0 || y < 0)
            return null;
        return tileMatrix.Get((uint) x, (uint) y);
    }


    public bool FindAndReplace(GameBoardTile newTile, float maxDist = 2.0f)
    {
        GameBoardTile oldTile = FindClosestTileTo(newTile.transform.position);
        Vector3 v = (oldTile.transform.position - newTile.transform.position);
        Vector2 v_xz = new Vector2(v.x, v.z);
        float d = v_xz.magnitude;
        if(d <= maxDist)
        {
            if (IsTileBlocked(oldTile.x, oldTile.y))
            {
                return false;
            }
            AddTile(newTile, oldTile.x, oldTile.y);
            Destroy(oldTile.gameObject);
            Build();
            return true;
        }
        return false;
    }


    public GameBoardTile FindClosestTileTo(Vector3 position)
    {
        float minDist = float.PositiveInfinity;
        GameBoardTile closestTile = null;

        for (uint i = 0; i < x; i++)
        {
            for (uint j = 0; j < y; j++)
            {
                GameBoardTile tile = tileMatrix.Get(i, j);
                Vector3 v = tile.transform.position - position;
                float dist = v.magnitude;
                if(dist < minDist)
                {
                    minDist = dist;
                    closestTile = tile;
                }
            }
        }

        return closestTile;
    }


    public void BlockTile(uint x, uint y)
    {
        blockedTiles.Set(x, y, true);
    }


    public void FreeTile(uint x, uint y)
    {
        blockedTiles.Set(x, y, false);
    }


    public bool IsTileBlocked(uint x, uint y)
    {
        return blockedTiles.Get(x, y);
    }


    public Vector2 GetTileLengths()
    {
        return tileDim;
    }


    // Start is called before the first frame update
    void Start()
    {
        Game.BOARD = this;
        tileMatrix = new InstanceMatrix<GameBoardTile>(x, y);
        blockedTiles = new InstanceMatrix<bool>(x, y);
        for(uint i = 0; i < x; i++)
        {
            for(uint j = 0; j < y; j++)
            {
                blockedTiles.Set(i, j, false);
            }
        }
        FillRandom(baseTilePrefab);
        for(uint i = 0; i < 5; i++)
        {
            AddSpawnPoint(i, 5, BaseTile.eDirection.down, spawnPointPrefab.gameObject);
            blockedTiles.Set(i, 5, true);
        }
        Build();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
