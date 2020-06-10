using GodMorgon.GameSequencerSpace;
using GodMorgon.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GodMorgon.Sound;

public enum RoomEffect
{
    EMPTY,
    CURSE,
    SHOP,
    REST,
    REMOVE,
    CHEST,
    START,
    EXIT
}

[System.Serializable]
public class RoomData
{
    public int x = 0;
    public int y = 0;
    public RoomEffect roomEffect = RoomEffect.EMPTY;
    public bool effectLaunched = false;

    public GameObject fogParticule;
    public bool isRoomCleared = false;
}

public class RoomEffectManager : MonoBehaviour
{
    [Header("Editor Settings")]
    public int sizeX = 0;
    public int sizeY = 0;
    public List<TileBase> effectTilesList = new List<TileBase>();

    [Header("General Settings")]
    public RoomData[] roomsDataArr;
    [SerializeField]
    private Tilemap roomTilemap = null;

    [Header("Effect Settings")]
    public List<GameObject> roomFxList = new List<GameObject>();
    [SerializeField]
    private Transform roomEffectsParent = null;
    private BasicCard cursedCard = null;
    private bool isRoomEffectDone = false;
    
    [NonSerialized]
    public RoomData currentRoom = null;

    [Header("Curse Settings")]
    [SerializeField]
    private int distancePlayerToCurse = 0;
    public Vector3Int cursedRoom = new Vector3Int();

    [Header("Chest Settings")]
    [SerializeField]
    private int goldInChest = 40;

    #region Singleton Pattern
    private static RoomEffectManager _instance;

    public static RoomEffectManager Instance { get { return _instance; } }
    #endregion

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

    // Start is called before the first frame update
    void Start()
    {
        foreach (RoomData room in roomsDataArr)
        {
            room.effectLaunched = false;
        }
        currentRoom = GetRoomData(PlayerManager.Instance.GetPlayerRoomPosition());
        FogMgr.Instance.InitFog();   //Initialise le tableau room du FogMgr
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchRoomEffect(Vector3Int roomPos)
    {
        foreach(RoomData room in roomsDataArr)
        {
            if(roomPos.x == room.x && roomPos.y == room.y)
            {
                currentRoom = room;
            }
        }
        
        //Debug.LogWarning("found the room you entered in : " + room.roomEffect + " : " + room.x + "/" + room.y);

        if (!currentRoom.effectLaunched)
        {
            switch (currentRoom.roomEffect) //On regarde son effet
            {
                case RoomEffect.EMPTY:

                    break;
                case RoomEffect.CURSE:
                    LaunchCurseRoomEffect();
                    
                    break;
                case RoomEffect.SHOP:

                    break;
                case RoomEffect.REST:

                    break;
                case RoomEffect.REMOVE:

                    break;
                case RoomEffect.CHEST:
                    LaunchChestRoomEffect();

                    break;
                case RoomEffect.START:

                    break;
                case RoomEffect.EXIT:

                    break;
            }

            currentRoom.effectLaunched = true;
        }
    }

    public void SetRoomEffect(RoomEffect roomEffect, Vector2Int roomCoord)
    {
        //Debug.Log(roomEffect + " a été ajouté dans la room " + roomCoord.x + "/" + roomCoord.y);
    }

    #region Generate Room Tilemap
    //Génère les tiles associées aux effets de room sur la tilemap RoomTilemap
    public void GenerateRoomsView()
    {
        //Pour chaque room du tableau créé avec l'editor
        foreach (RoomData room in roomsDataArr)
        {
            TileBase currentTileBase = null;
            
            //On check l'effet de la room
            switch(room.roomEffect)
            {
                case RoomEffect.EMPTY:
                    currentTileBase = GetTileBase("Empty"); //On récupère le visuel de la tile correspondant à l'effet
                    break;
                case RoomEffect.CURSE:
                    currentTileBase = GetTileBase("Curse");
                    break;
                case RoomEffect.SHOP:
                    currentTileBase = GetTileBase("Shop");
                    break;
                case RoomEffect.REST:
                    currentTileBase = GetTileBase("Rest");
                    break;
                case RoomEffect.REMOVE:
                    currentTileBase = GetTileBase("Remove");
                    break;
                case RoomEffect.CHEST:
                    currentTileBase = GetTileBase("Chest");
                    break;
                case RoomEffect.START:
                    currentTileBase = GetTileBase("Start");
                    break;
                case RoomEffect.EXIT:
                    currentTileBase = GetTileBase("Exit");
                    break;
            }

            //Si aucun visuel ne correspond à l'effet d'une room
            if (currentTileBase == null)
            {
                Debug.Log("No TileBase corresponding to the room effect");
                return;
            }

            //On applique sur la tilemap des rooms le visuel de l'effet de room à la coordonnée de la room
            //On inverse car les coordonnées du tableau et de la tilemap sont inversées
            roomTilemap.SetTile(new Vector3Int(room.x, room.y, 0), currentTileBase);  
        }
    }

    //Renvoie le visuel de la tile en fonction de l'effet
    private TileBase GetTileBase(string effect)
    {
        //Pour chaque visuel de la liste de tiles visuelles
        foreach(TileBase tile in effectTilesList)
        {
            //S'il correspond à l'effet
            if(tile.name == effect)
            {
                return tile;    //On retourne le visuel
            }
        }
        return null;
    }
    #endregion

    /**
     * Affiche l'effet visuel de curse et lance l'effet 
     */
    public void LaunchCurseRoomEffect()
    {
        if (null == currentRoom) return;
        Vector3 currentRoomWorldPos = roomTilemap.CellToWorld(new Vector3Int(currentRoom.x, currentRoom.y, 0)) + new Vector3(0, 0.75f, 0);
        
        //On lance les particules de Curse sur la room 
        Instantiate(roomFxList[0], currentRoomWorldPos, Quaternion.identity, roomEffectsParent);
        if(null != cursedCard)
            GameManager.Instance.AddCardToDiscardPile(cursedCard);
        StartCoroutine(TimedRoomEffect());
    }

    /**
     * Affiche l'effet visuel de chest et lance l'effet 
     */
    public void LaunchChestRoomEffect()
    {
        if (null == currentRoom) return;
        Vector3 currentRoomWorldPos = roomTilemap.CellToWorld(new Vector3Int(currentRoom.x, currentRoom.y, 0)) + new Vector3(0, 0.75f, 0);

        //On lance les particules de CHest sur la room
        Instantiate(roomFxList[1], currentRoomWorldPos, Quaternion.identity, roomEffectsParent);

        //Ajoute la gold à la bourse du joueur
        PlayerManager.Instance.AddGold(goldInChest);

        //SFX chest room
        MusicManager.Instance.PlayFeedbackChest();

        StartCoroutine(TimedRoomEffect());
    }

    //WIP
    public void LaunchRestRoomEffect()
    {

    }

    //Façon dégueulasse de timer le temps de l'effet de la room
    IEnumerator TimedRoomEffect()
    {
        yield return new WaitForSeconds(3f);
        isRoomEffectDone = true;
        yield return new WaitForSeconds(3f);
        foreach (Transform child in roomEffectsParent)  //On clear les effets de room instantiés sur la scène
            DestroyImmediate(child.gameObject);
        isRoomEffectDone = false;
    }

    public bool RoomEffectDone()
    {
        if (isRoomEffectDone)
            return true;
        else
            return false;
    }

    /**
     * Renvoie la roomData correspondant à une position cellule
     */
    public RoomData GetRoomData(Vector3Int position)
    {
        foreach (RoomData room in roomsDataArr)
        {
            if (position.x == room.x && position.y == room.y)  //Si une room correspond à la position donnée en param
            {
                return room;
            }
        }

        return null;
    }

    public void CurseRandomRoom()
    {
        Vector3Int playerRoomPos = PlayerManager.Instance.GetPlayerRoomPosition();
        List<RoomData> cursableRooms = new List<RoomData>();
        foreach (RoomData room in roomsDataArr)
        {
            if ((room.x == playerRoomPos.x + distancePlayerToCurse && room.y == playerRoomPos.y) 
                || (room.y == playerRoomPos.y + distancePlayerToCurse && room.x == playerRoomPos.x)
                || (room.x == playerRoomPos.x - distancePlayerToCurse && room.y == playerRoomPos.y)
                || (room.y == playerRoomPos.y - distancePlayerToCurse && room.x == playerRoomPos.x))  //Si une room correspond à la position du player
            {
                if(room.roomEffect == RoomEffect.EMPTY)
                    cursableRooms.Add(room);
            }
        }

        //On réitère si aucune des rooms n'était empty
        if (cursableRooms.Count == 0)
        {
            distancePlayerToCurse++;
            CurseRandomRoom();
            return;
        }

        RoomData roomToCurse = cursableRooms[UnityEngine.Random.Range(0, cursableRooms.Count)];  //Sélectionne une room au hasard parmi les cursable
        for(int i = 0; i < roomsDataArr.Length; i++)
        {
            if (roomsDataArr[i] == roomToCurse)
                roomsDataArr[i].roomEffect = RoomEffect.CURSE; 
        }

        GenerateRoomsView();    //Update la room tilemap
        
        //Afficher effet au dessus de la room
    }

    //Curse une room dont on aura renseigné les coordonnées dans l'inspector
    public void CurseSpecificRoom()
    {
        if (cursedRoom.x == 0 && cursedRoom.y == 0) return;

        
        //On parcourt toutes les rooms jusqu'à trouver celle renseignée dans l'inspector
        for (int i = 0; i < roomsDataArr.Length; i++)
        {
            if (roomsDataArr[i].x == cursedRoom.x && roomsDataArr[i].y == cursedRoom.y)
                roomsDataArr[i].roomEffect = RoomEffect.CURSE;
        }

        Vector3 cursedRoomWorldPos = roomTilemap.CellToWorld(cursedRoom) + new Vector3(0, 0.75f, 0);
        
        //On lance les particules de Curse sur la room 
        GameObject curseParticules = Instantiate(roomFxList[3], cursedRoomWorldPos, Quaternion.identity, roomEffectsParent);
        curseParticules.transform.localScale = new Vector3(.5f, .5f, 0);
        GenerateRoomsView();    //Update la room tilemap
    }
}
