using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "NodeMap")]
public class NodeMap : ScriptableObject
{
    public string mapName;
    public Vector2Int mapSize;
    public Node[] nodeList;
}
