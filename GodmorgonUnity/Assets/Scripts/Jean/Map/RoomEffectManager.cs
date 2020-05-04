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
        Debug.Log(" a été ajouté dans la room ");
    }

    public void GenerateRoomsView()
    {

    }
}
