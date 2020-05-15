using GodMorgon.GameSequencerSpace;
using GodMorgon.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    private Tilemap roomTilemap;

    [Header("Effect Settings")]
    public List<GameObject> roomFxList = new List<GameObject>();
    [SerializeField]
    private Transform roomEffectsParent;
    private BasicCard cursedCard = null;
    private bool isRoomEffectDone = false;
    private RoomData currentRoom = null;

    [Header("Curse Settings")]
    [SerializeField]
    private int distancePlayerToCurse = 0;

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
            DontDestroyOnLoad(_instance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (RoomData room in roomsDataArr)
        {
            room.effectLaunched = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRoomEffectToSequencer(RoomData room)
    {
        currentRoom = room;
        //Debug.LogWarning("found the room you entered in : " + room.roomEffect + " : " + room.x + "/" + room.y);

        if (!room.effectLaunched)
        {
            switch (room.roomEffect) //On regarde son effet
            {
                case RoomEffect.EMPTY:

                    break;
                case RoomEffect.CURSE:
                    Debug.Log(room.roomEffect + " added to sequencer");
                    //Ajoute l'action Curse de la room au sequencer
                    GSA_CurseRoom curseRoomAction = new GSA_CurseRoom();
                    GameSequencer.Instance.AddAction(curseRoomAction);
                    break;
                case RoomEffect.SHOP:
                    Debug.Log(room.roomEffect + " added to sequencer");
                    break;
                case RoomEffect.REST:

                    break;
                case RoomEffect.REMOVE:

                    break;
                case RoomEffect.CHEST:
                    GSA_ChestRoom chestRoomAction = new GSA_ChestRoom();
                    GameSequencer.Instance.AddAction(chestRoomAction);
                    Debug.Log(room.roomEffect + " added to sequencer");
                    break;
                case RoomEffect.START:

                    break;
                case RoomEffect.EXIT:

                    break;
            }

            room.effectLaunched = true;
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

        Debug.Log("Room tilemap generated");
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

        //On lance les particules de Curse sur la room
        Instantiate(roomFxList[1], currentRoomWorldPos, Quaternion.identity, roomEffectsParent);

        //Ajoute la gold à la bourse du joueur
        PlayerManager.Instance.AddGold(goldInChest);

        StartCoroutine(TimedRoomEffect());
    }

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

    public RoomData GetRoomData(Vector3Int position)
    {
        foreach (RoomData room in roomsDataArr)
        {
            if (position.x == room.x && position.y == room.y)  //Si une room correspond à la position du player
            {
                return room;
            }
        }

        return null;
    }

    public void CurseRandomRoom()
    {
        Vector3Int playerRoomPos = PlayerManager.Instance.GetPlayerRoomPosition();
        RoomData roomToCurse = null;
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

        roomToCurse = cursableRooms[UnityEngine.Random.Range(0, cursableRooms.Count)];  //Sélectionne une room au hasard parmi les cursable
        for(int i = 0; i < roomsDataArr.Length; i++)
        {
            if (roomsDataArr[i] == roomToCurse)
                roomsDataArr[i].roomEffect = RoomEffect.CURSE; 
        }

        GenerateRoomsView();    //Update la room tilemap
        
        //Afficher effet au dessus de la room
    }

    
}
