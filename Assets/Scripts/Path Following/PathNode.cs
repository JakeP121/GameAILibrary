using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour {
    public Mesh mesh; // The mesh (only used if visible)
    public Material material; // The material (only used if visible)
    public bool visible = false; // Should the node be rendered? 

    public void Update()
    {
        if (visible) // If node should be visible
            Graphics.DrawMesh(mesh, transform.position, Quaternion.identity, material, 0); // Render the mesh 
    }
}
