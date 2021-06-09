using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : BaseTile
{
    [SerializeField]
    private GameObject mesh;
    [SerializeField]
    private ScrollTexture scrollTex;


    new void Start()
    {
        base.Start();
        scrollTex.scrollY = -0.5f;
        scrollTex.gameObject.GetComponent<Renderer>().material.SetFloat("_TexScaleY", -1.0f);
        //mesh.transform.Rotate(new Vector3(0, ((float) GetStart()) * 90.0f));

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
        Vector3 centerPos = new Vector3(transform.position.x, Y_VALUE, transform.position.z);
        movePattern = new SpawnPointMovePattern(eDirection.down, centerPos, Game.BOARD.GetTileLengths().x / 2.0f, 0.01f, new Vector3(0, 1.5f, -0.5f), new Vector3(0, 0.1f, 0));
    }
}