using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxShader : MonoBehaviour {

    private Renderer rend;
    public Vector2 currentPos = new Vector2(0,0);
    public Vector2 stepSize = new Vector2(0.2f, 0);
    void Start()
    {
        rend = GetComponent<Renderer>();
       // rend.material.shader = Shader.Find("Specular");
    }
    void Update()
    {
        currentPos += stepSize;
        currentPos = Limit(currentPos);
        rend.material.SetFloat("_PosX", currentPos.x);
        rend.material.SetFloat("_PosY", currentPos.y);
    }
    private Vector2 Limit(Vector2 vec) {
        Vector2 ret = vec;
        ret.x = ret.x > 1 ? ret.x - 1f : ret.x;
        ret.y = ret.y > 1 ? ret.y - 1f : ret.y;
        return ret;
    }
}

