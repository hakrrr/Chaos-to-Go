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
    Vector3[] spawnPoints = new Vector3[] {new Vector3(-6,0,6), new Vector3(-2, 0, 6), new Vector3(2, 0, 6), new Vector3(6, 0, 6), new Vector3(6, 0, 6), new Vector3(6, 0, 6) };

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
    public void SpawnIngredient(string type, int spawnPointID)
    {
        Debug.Log("(i) Spawning " + type + " at " + spawnPointID);
        spawnPointID = spawnPointID - 1;
        switch (type)
        {
            case "tomato":
                GameObject tomatoObj = Instantiate(tomato, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                tomatoObj.GetComponent<Ingredient>().PleaseDontForgetToInitMe(spawnPointID, 5, Recipes.eIngredients.tomato);
                tomatoObj.transform.position = Game.BOARD.GetTile(spawnPointID, 5).GetMovePattern().GetStart(tomatoObj.GetComponent<Ingredient>());
                break;
            case "onion":
                GameObject onionObj = Instantiate(onion, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                onionObj.GetComponent<Ingredient>().PleaseDontForgetToInitMe(spawnPointID, 5, Recipes.eIngredients.onion);
                onionObj.transform.position = Game.BOARD.GetTile(spawnPointID, 5).GetMovePattern().GetStart(onionObj.GetComponent<Ingredient>());
                break;
            case "carrot":
                GameObject carrotObj = Instantiate(carrot, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                carrotObj.GetComponent<Ingredient>().PleaseDontForgetToInitMe(spawnPointID, 5, Recipes.eIngredients.carrot);
                carrotObj.transform.position = Game.BOARD.GetTile(spawnPointID, 5).GetMovePattern().GetStart(carrotObj.GetComponent<Ingredient>());
                break;
            case "asparagus":
                GameObject asparagusObj = Instantiate(asparagus, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                asparagusObj.GetComponent<Ingredient>().PleaseDontForgetToInitMe(spawnPointID, 5, Recipes.eIngredients.asparagus);
                asparagusObj.transform.position = Game.BOARD.GetTile(spawnPointID, 5).GetMovePattern().GetStart(asparagusObj.GetComponent<Ingredient>());
                break;
            case "chicken":
                GameObject chickenObj = Instantiate(chicken, transform.localPosition + spawnPoints[spawnPointID], Quaternion.identity, transform);
                chickenObj.GetComponent<Ingredient>().PleaseDontForgetToInitMe(spawnPointID, 5, Recipes.eIngredients.chicken);
                chickenObj.transform.position = Game.BOARD.GetTile(spawnPointID, 5).GetMovePattern().GetStart(chickenObj.GetComponent<Ingredient>());
                break;
            default:
                break;
        }
    }
}
