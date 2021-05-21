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
    //WRONG: positions of tiles are relative!!!11!!1
    Vector3[] spawnPoints = new Vector3[] {new Vector3(-6,0,6), new Vector3(-2, 0, 6), new Vector3(2, 0, 6), new Vector3(6, 0, 6), new Vector3(0, 0, 0) };

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

    //or strings or ID for ingredient and position, what do we like to have here?
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
