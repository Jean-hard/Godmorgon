using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TreeEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<Transform> nodePrefabList = new List<Transform>();
    public NodeMap map;
    private Node[] tempNodes;

    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < map.mapSize.x; x++)
        {
            for (int y = 0; y < map.mapSize.y; y++)
            {
                Vector3 nodePosition = new Vector3(x*3, 0, y*3);
                Transform newNode = Instantiate(nodePrefabList[0], nodePosition, Quaternion.identity) as Transform;
                newNode.parent = mapHolder;
            }
        }
    }
}
