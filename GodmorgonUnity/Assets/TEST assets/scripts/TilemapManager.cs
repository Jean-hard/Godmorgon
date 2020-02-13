using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap ground;

    private Vector3Int playerTilePosition;

    private static TilemapManager _instance;

    public static TilemapManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public Vector3 GetCurrentTileCenterPosition(Vector3 worldPosition)
    {
        Vector3 newPosition;
        Vector3Int currentTilePos = ground.WorldToCell(worldPosition);

        newPosition = ground.CellToWorld(currentTilePos) + new Vector3(0, 0.25f);
        Debug.Log("position : " + newPosition);

        return newPosition;
    }

    public Vector3Int GetTilePosition(Vector3 worldPosition)
    {
        return ground.WorldToCell(worldPosition);
    }

    public void PlayerTileOnColor(Vector3 worldPlayerPosition)
    {
        Debug.Log("YOLO ?");
        //to give back it's original color to last tile where the player was
        if (playerTilePosition != null)
            ground.SetColor(playerTilePosition, Color.white);

        playerTilePosition = ground.WorldToCell(worldPlayerPosition);

        ground.SetTileFlags(playerTilePosition, TileFlags.None);
        ground.SetColor(playerTilePosition, Color.black);
    }
}
