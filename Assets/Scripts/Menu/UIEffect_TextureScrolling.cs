using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect_TextureScrolling : MonoBehaviour
{
    public Vector2 MeshMaterialPosition;
    MeshRenderer Mesh = null;

    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        Mesh.material.mainTextureOffset += MeshMaterialPosition * Time.deltaTime * 0.2f;
    }
}
