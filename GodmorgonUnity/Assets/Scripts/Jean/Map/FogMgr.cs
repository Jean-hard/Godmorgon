using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogMgr : MonoBehaviour
{
    private List<Vector3Int> nearRoomsTiles = new List<Vector3Int>();   //Liste des tile/room autour du player + celle du player
    private Tilemap fogTilemap = null;

    private Color transparentColor = new Color(1, 1, 1, 0);

    private void Start()
    {
        InitFog();
    }

    private void Update()
    {
        UpdateFogToPlayer();
    }

    /**
     * Initialise le fog autour du player au start
     */
    private void InitFog()
    {
        fogTilemap = this.GetComponent<Tilemap>();
        
        UpdateNearRooms();

        foreach (Vector3Int tilePos in nearRoomsTiles)   //Pour toutes les rooms à coté du player
        {
            if (fogTilemap.GetColor(tilePos).a > 0.5f)   //Si la tile n'est pas transparente
            {                
                fogTilemap.SetTileFlags(tilePos, TileFlags.None);   //On rend possible le changement de couleur
                fogTilemap.SetColor(tilePos, transparentColor);     //On la rend transparente
            }
        }


    }

    /**
     * Clear les rooms autour du player
     * Appelée lorsque le joueur est en mouvement
     */
    private void UpdateFogToPlayer()
    {
        if (PlayerManager.Instance.IsPlayerMoving()) return;

        UpdateNearRooms();
        
        foreach(Vector3Int tilePos in nearRoomsTiles)   //Pour toutes les rooms à coté du player
        {
            if(fogTilemap.GetColor(tilePos).a > 0.5f)   //Si la tile n'est pas transparente
            {
                fogTilemap.SetTileFlags(tilePos, TileFlags.None);
                fogTilemap.SetColor(tilePos, transparentColor); //On la rend transparente
            }
        }
        
    }

    /**
     * Update la liste des tiles à côté de celle du player + celle du player 
     */
    private void UpdateNearRooms()
    {
        if (null == PlayerManager.Instance) return;

        nearRoomsTiles = new List<Vector3Int>();

        Vector3Int currentPlayerPos = PlayerManager.Instance.GetPlayerRoomPosition();
        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(currentPlayerPos);
        Vector3Int currentRoomDataPos = new Vector3Int(currentRoomData.x, currentRoomData.y, 0);

        nearRoomsTiles.Add(currentRoomDataPos);

        foreach(RoomData room in RoomEffectManager.Instance.roomsDataArr)
        {
            Vector3Int currRoom = new Vector3Int(room.x, room.y, 0);
            if(currRoom == currentRoomDataPos + new Vector3Int(1, 0, 0)     //Si on a un room à + ou - 1 en x et y de la room actuelle
                || currRoom == currentRoomDataPos + new Vector3Int(0, 1, 0)
                || currRoom == currentRoomDataPos + new Vector3Int(-1, 0, 0)
                || currRoom == currentRoomDataPos + new Vector3Int(0, -1, 0))
            {
                nearRoomsTiles.Add(currRoom);
            }
        }
    }
}
