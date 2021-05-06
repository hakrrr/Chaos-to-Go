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
    private Transform tileRoot;

    private InstanceMatrix<GameBoardTile> tileMatrix;


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
                tileMatrix.Set(i, j, tile);
            }
        }
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
            }
        }
    }


    public void AddTile(GameBoardTile tile, uint x, uint y)
    {
        tileMatrix.Set(x, y, tile);
    }


    // Start is called before the first frame update
    void Start()
    {
        tileMatrix = new InstanceMatrix<GameBoardTile>(x, y);
        FillEmpty();
        Build();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
