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

    // Start is called before the first frame update
    void Start()
    {
        //SpawnIngredient("chicken", new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //or strings or ID for ingredient and position, what do we like to have here?
    void SpawnIngredient(string type, Vector3 spawnPoint)
    {
        switch (type)
        {
            case "tomato":
                Instantiate(tomato, spawnPoint, Quaternion.identity, transform);
                break;
            case "onion":
                Instantiate(onion, spawnPoint, Quaternion.identity, transform);
                break;
            case "carrot":
                Instantiate(carrot, spawnPoint, Quaternion.identity, transform);
                break;
            case "asparagus":
                Instantiate(asparagus, spawnPoint, Quaternion.identity, transform);
                break;
            case "chicken":
                Instantiate(chicken, spawnPoint, Quaternion.identity, transform);
                break;
            default:
                break;
        }
    }
}
