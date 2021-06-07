using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : BaseTile
{
    [SerializeField]
    private GameObject mesh;


    new void Start()
    {
        base.Start();
        mesh.transform.Rotate(new Vector3(0, ((float) GetStart()) * 90.0f));

        switch (GetStart())
        {
            case eDirection.up:
                SetDirections(eDirection.up, eDirection.down);
                break;
            case eDirection.down:
                SetDirections(eDirection.down, eDirection.up);
                break;
            case eDirection.left:
                SetDirections(eDirection.left, eDirection.right);
                break;
            case eDirection.right:
                SetDirections(eDirection.right, eDirection.left);
                break;
        }
    }


    public override void InitMovementPattern()
    {
        movePattern = new SpawnPointMovePattern(eDirection.down, transform.position + new Vector3(0, 2.0f, 0), Game.BOARD.GetTileLengths().x / 2.0f, 0.01f, new Vector3(0, 3.0f, 0));
    }
}