using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollY = 0.075f;
    //public bool flip = false;

    // Update is called once per frame
    void Update()
    {
        float offsetY = Time.time * scrollY;

        GetComponent<Renderer>().material.SetFloat("_TexOffsetY", offsetY);
    }
}
