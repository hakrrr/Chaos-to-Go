using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsManager : MonoBehaviour
{

    public GameObject tomato;
    public GameObject onion;
    public GameObject carrot;
    public GameObject asparagus;
    public GameObject chicken;

    private GameObject tilesArray;

    //array of spawn points
    //teporarily declared here, but should be declared on start, according to level topology
    Vector3[] spawnPoints = new Vector3[] {new Vector3(-6,0,6), new Vector3(-2, 0, 6), new Vector3(2, 0, 6), new Vector3(6, 0, 6), new Vector3(6, 0, 6) };

    // Start is called before the first frame update
    void Start()
    {
        //int i = 0;
        //SpawnIngredient("chicken", i);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Spawns ingredient of type "type" at spawn point with "spawnPointID
    //--------------------------TODO------------------------------
    //               proper scaling and rotation
    //                       coliders
    //------------------------------------------------------------
    public void SpawnIngredient(string type, int spawnPointID)
    {
        switch (type)
        {
            case "tomato":
                Instantiate(tomato, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                break;
            case "onion":
                Instantiate(onion, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                break;
            case "carrot":
                Instantiate(carrot, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                break;
            case "asparagus":
                Instantiate(asparagus, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                break;
            case "chicken":
                Instantiate(chicken, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                break;
            default:
                break;
        }
    }
}
