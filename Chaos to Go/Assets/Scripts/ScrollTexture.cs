using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollY = 0.075f;
    private float offsetY = 0.0f;
    //public bool flip = false;

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.PAUSED) return;
        offsetY += Time.deltaTime * scrollY;
        if(offsetY > 1.0f)
        {
            offsetY -= 1.0f;
        }
        else if(offsetY < -1.0f)
        {
            offsetY += 1.0f;
        }

        GetComponent<Renderer>().material.SetFloat("_TexOffsetY", offsetY);
    }
}
