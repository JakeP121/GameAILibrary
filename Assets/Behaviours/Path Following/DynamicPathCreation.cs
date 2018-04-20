using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPathCreation : MonoBehaviour
{

    public float frequency = 0.2f;
    public float maxNodeLifeTime = 3.0f;
    public float proximityThreshold = 0.2f;

    private float timeSinceLastNode = 0.0f;

    private Path path;
    private List<float> lifeTimeCounter = new List<float>();

    // Use this for initialization
    void Start()
    {
        path = Instantiate(Resources.Load<Path>("Path")); // Create a path attached to this game object
        path.transform.position = Vector3.zero;
        path.transform.name = this.transform.name + "'s Trail";
    }

    // Update is called once per frame
    void Update()
    {
        increaseNodeTimeCount();

        removeOldNodes();

        addNewNode();
    }

    private void increaseNodeTimeCount()
    {
        for (int i = 0; i < path.nodes.Count; i++)
        {
            lifeTimeCounter[i] += Time.deltaTime;
        }
    }

    private void removeOldNodes()
    {
        if (maxNodeLifeTime > 0.0f && path.nodes.Count > 0) // If nodes have been given a lifetime and there are nodes created
        {
            if (lifeTimeCounter[0] > maxNodeLifeTime) // We'll only need to check the first value since thats the oldest
            {
                lifeTimeCounter.RemoveAt(0);
                Destroy(path.nodes[0].gameObject);
                path.nodes.RemoveAt(0);
            }
        }
    }

    private void addNewNode()
    {
        bool tooClose = false;

        if (path.nodes.Count > 0)
        {
            Vector3 lastNodePos = path.nodes[path.nodes.Count - 1].transform.position;
            Vector3 newNodePos = transform.position;

            float distance = (lastNodePos - newNodePos).magnitude;

            if (distance <= proximityThreshold)
                tooClose = true;
        }

        if (timeSinceLastNode > frequency && !tooClose)
        {
            PathNode newNode = Instantiate(Resources.Load<PathNode>("Path Node")); // Spawn a PathNode prefab object
            lifeTimeCounter.Add(0.0f);

            newNode.transform.parent = path.transform; // Assign path as parent of PathNode
            newNode.transform.position = this.transform.position;

            path.nodes.Add(newNode); // Add node to path

            timeSinceLastNode = 0.0f; // Reset time since last node
        }
        else
            timeSinceLastNode += Time.deltaTime;
    }

    public Path getPath()
    {
        return path;
    }
}