using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        if (GameObject.FindObjectsOfType<DontDestroy>().Length > 1)
            Destroy(gameObject);
        else DontDestroyOnLoad(this.gameObject);
    }
  
}
