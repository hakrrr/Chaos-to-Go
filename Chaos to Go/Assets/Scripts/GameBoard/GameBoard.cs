using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private uint x = 8, y = 8;

    private InstanceMatrix<GameBoardTile> tileMatrix;


    // Start is called before the first frame update
    void Start()
    {
        tileMatrix = new InstanceMatrix<GameBoardTile>(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
