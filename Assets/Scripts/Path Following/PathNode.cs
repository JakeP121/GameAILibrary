using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {
    public Mesh mesh;
    public Material material;
    public bool visible = false;

    public void Update()
    {
        if (visible)
            Graphics.DrawMesh(mesh, transform.position, Quaternion.identity, material, 0);
    }
}
