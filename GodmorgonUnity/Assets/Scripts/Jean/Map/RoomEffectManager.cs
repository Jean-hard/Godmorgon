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
}

public class RoomEffectManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap roomTilemap;

    public int sizeX = 0;
    public int sizeY = 0;
    public RoomData[] roomsDataArr;
    public int offsetX = 0;
    public int offsetY = 0;

    public List<TileBase> effectTilesList = new List<TileBase>();

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRoomEffect(RoomEffect roomEffect, Vector2Int roomCoord)
    {
        //Debug.Log(roomEffect + " a été ajouté dans la room " + roomCoord.x + "/" + roomCoord.y);
    }

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
}
