using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using GodMorgon.Sound;

[Serializable]
public class Bounds
{
    public int x = 10;
    public int y = 10;
}

public class FogTile
{
    public Vector3Int roomPos = new Vector3Int();
    public GameObject fogParticule;
}

public class FogMgr : MonoBehaviour
{
    public static FogMgr Instance;

    [Header("Sight Card Settings")]
    [SerializeField]
    private float timeAfterAction = 2f;

    [Header("Sight Action Settings")]
    public Bounds fogBounds;
    public TileBase fogTile;

    private Tilemap fogTilemap = null;

    private Color transparentColor = new Color(1, 1, 1, 0);
    private Color opaqueColor = new Color(1, 1, 1, 1);

    private bool hasUpdatedFog = false;
    private bool hasBeenRevealed = false;
    
    private int revealRange = 1;

    [Header("Version with particules")]
    public List<GameObject> fogParticulePrefabs = new List<GameObject>();
    public Transform fogParent;
    public List<Vector3Int> roomsClearedAtStart = new List<Vector3Int>();

    public List<Vector3Int> positionsToSpawn = new List<Vector3Int>();
    


    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        fogTilemap = this.GetComponent<Tilemap>();
    }

    public void InitFog()
    {
        //Créé du fog sur toutes les rooms
        foreach(RoomData room in RoomEffectManager.Instance.roomsDataArr)
        {
            AddFogParticuleOnRoom(room);
        }

        foreach(Vector3Int room in roomsClearedAtStart)
        {
            RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(room);
            StopFogParticuleOnRoom(currentRoomData);
        }

        hasUpdatedFog = false;
    }

    private void Update()
    {
        //UpdateFogToPlayer();

        UpdateFogParticules();
    }

    private void AddFogParticuleOnRoom(RoomData room)
    {
        Vector3Int roomCellPos = new Vector3Int(room.x, room.y, 0);
        Vector3 worldSpawnPos = TilesManager.Instance.roomTilemap.CellToWorld(roomCellPos) + new Vector3(0f, 1f, 0f);

        int random = UnityEngine.Random.Range(0, 3);

        room.fogParticule = Instantiate(fogParticulePrefabs[0], worldSpawnPos, Quaternion.identity, fogParent);
        room.isRoomCleared = false;
    }

    private void PlayFogParticuleOnRoom(RoomData room)
    {
        room.fogParticule.GetComponent<ParticleSystem>().Play();
        room.isRoomCleared = false;
    }

    private void StopFogParticuleOnRoom(RoomData room)
    {
        room.fogParticule.GetComponent<ParticleSystem>().Stop();
        room.isRoomCleared = true;
    }

    /**
     * Clear les rooms autour du player
     * Appelée lorsque le joueur est en mouvement
     */
    private void UpdateFogParticules()
    {
        if (PlayerManager.Instance.IsPlayerMoving())
        {
            hasUpdatedFog = false;
            return;
        }

        if (hasUpdatedFog) return;
        
        Vector3Int currentPlayerPos = PlayerManager.Instance.GetPlayerRoomPosition();

        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(currentPlayerPos);

        List<RoomData> nearRoomDatas = GetNearRoomData(currentPlayerPos, 1);

        TilesManager.Instance.CreateGrid();
        TilesManager.Instance.UpdateAccessibleTilesList();

        foreach (RoomData room in nearRoomDatas)   //Pour toutes les rooms à coté du player
        {
            foreach (RoomData accessibleRoom in TilesManager.Instance.GetAccessibleRooms())
            {
                //Debug.Log(room.x + "/" + room.y + " contre " + showableRoom.x + "/" + showableRoom.y);
                
                if (room == accessibleRoom)
                {
                    if (!room.isRoomCleared)   //Si la tile n'est pas transparente
                    {
                        StopFogParticuleOnRoom(room);
                    }
                }
            }
        }

        StopFogParticuleOnRoom(currentRoomData);

        hasUpdatedFog = true;
    }

    
    /**
     * Renvoie la liste des rooms à côté d'une position room donnée
     */
    private List<RoomData> GetNearRoomData(Vector3Int roomPos, int revealRange)
    {
        if (null == PlayerManager.Instance) return null;

        List<RoomData> nearRooms = new List<RoomData>();

        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(roomPos);

        for (int i = 0; i < revealRange; ++i)
        {
            //On parcours les room data car on veut se contenter seulement des rooms du jeu, et pas au-delà (sinon on ajouterait directement les positions dans la liste)
            foreach (RoomData room in RoomEffectManager.Instance.roomsDataArr)
            {
                if ((room.x == currentRoomData.x + i + 1 && room.y == currentRoomData.y)     //Si on a une room à + ou - 1 en x et y de la room actuelle
                    || (room.x == currentRoomData.x && room.y == currentRoomData.y + i + 1)
                    || (room.x == currentRoomData.x -i - 1 && room.y == currentRoomData.y)
                    || (room.x == currentRoomData.x && room.y == currentRoomData.y - i - 1))
                {
                    nearRooms.Add(room);
                }
            }
        }

        return nearRooms;
    }

    
    /**
     * Clear les rooms autour d'une position donnée, avec une certaine range
     * Penser à faire un SetRevealRange avant d'appeler la GSA Sight pour que la valeur de la carte soit prise en compte
     */
    public void RevealRoomAtPosition(Vector3Int baseRoomPosition, int cardRevealRange)
    {
        List<RoomData> nearRooms = GetNearRoomData(baseRoomPosition, cardRevealRange);

        nearRooms.Add(RoomEffectManager.Instance.GetRoomData(baseRoomPosition));


        foreach (RoomData roomPos in nearRooms)
        {
            StopFogParticuleOnRoom(roomPos);
        }

        StartCoroutine(TimedAction(timeAfterAction));

        //SFX fog clear
        MusicManager.Instance.PlayFogClear();
    }

    

    /**
     * Check si les positions ont été reveal
     */
    public bool RevealDone()
    {
        if (!hasBeenRevealed) return false;

        hasBeenRevealed = false;
        return true;
    }

    /**
     * On attend un peu avant de terminer l'action
     */
    IEnumerator TimedAction(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        hasBeenRevealed = true;
    }

    /**
    * Recouvre toute la map de Fog sauf où il y a le player et des cases voisines
    */
    public void CoverEntireMapWithParticules()
    {
        hasUpdatedFog = false;  //Le fog n'a pas encore été updaté

        foreach(RoomData room in RoomEffectManager.Instance.roomsDataArr)
        {
            if(!TilesManager.Instance.GetAccessibleRooms().Contains(room))
            {
                PlayFogParticuleOnRoom(room);
            }
        }
    }

    #region Old Version with tiles instead of particules

    /**
     * Recouvre toute la map de Fog sauf où il y a le player et des cases voisines
     */
    public void CoverEntireMap()
    {
        hasUpdatedFog = false;  //Le fog n'a pas encore été updaté

        Vector3Int currentPlayerPos = PlayerManager.Instance.GetPlayerRoomPosition();
        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(currentPlayerPos);
        Vector3Int currentRoomDataPos = new Vector3Int(currentRoomData.x, currentRoomData.y, 0);

        List<Vector3Int> nearRoomsTiles = UpdateNearRooms(currentPlayerPos, 1);

        //On parcourt toutes les tiles du fog (gérer les bounds dans l'inspector)
        for (int x = 0; x < fogBounds.x; ++x)
        {
            for(int y = 0; y < fogBounds.y; ++y)
            {
                Vector3Int fogTilePos = new Vector3Int(x, y, 0);   //Récup la coordonnée de la tile actuelle
                if(fogTilePos != currentRoomDataPos && !nearRoomsTiles.Contains(fogTilePos))
                {
                    fogTilemap.SetTile(fogTilePos, fogTile);    //On applique la tile fog sur toutes les cases

                    StartCoroutine(FadeFog(false, fogTilePos));
                }
                    
            }
        }
    }

    /**
     * Clear les rooms autour du player
     * Appelée lorsque le joueur est en mouvement
     */
    private void UpdateFogToPlayer()
    {
        if (PlayerManager.Instance.IsPlayerMoving())
        {
            hasUpdatedFog = false;
            return;
        }

        if (hasUpdatedFog) return;

        Vector3Int currentPlayerPos = PlayerManager.Instance.GetPlayerRoomPosition();
        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(currentPlayerPos);
        Vector3Int currentRoomDataPos = new Vector3Int(currentRoomData.x, currentRoomData.y, 0);

        List<Vector3Int> nearRoomsTiles = UpdateNearRooms(currentPlayerPos, 1);
        TilesManager.Instance.CreateGrid();
        TilesManager.Instance.UpdateAccessibleTilesList();

        foreach (Vector3Int room in nearRoomsTiles)   //Pour toutes les rooms à coté du player
        {
            foreach (Vector3Int tile in TilesManager.Instance.showableTilesList)
            {
                Vector3 worldPos = TilesManager.Instance.walkableTilemap.CellToWorld(tile);
                Vector3Int tempRoomPos = fogTilemap.WorldToCell(worldPos);
                if (room == tempRoomPos)
                {
                    if (fogTilemap.GetColor(room).a > 0.99f)   //Si la tile n'est pas transparente
                    {
                        StartCoroutine(FadeFog(true, room));
                    }
                }
            }
        }

        if (fogTilemap.GetColor(currentRoomDataPos).a > 0.99f)
        {
            StartCoroutine(FadeFog(true, currentRoomDataPos));
        }

        hasUpdatedFog = true;
    }

    /**
     * Clear le fog ou rajoute du fog en fade 
     * TRUE pour aller au transparent
     * FALSE pour aller à l'opaque
     */
    IEnumerator FadeFog(bool fadeAway, Vector3Int originPos)
    {
        fogTilemap.SetTileFlags(originPos, TileFlags.None);   //On rend possible le changement de couleur

        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                // set color with i as alpha
                Color color = new Color(1, 1, 1, i);

                fogTilemap.SetColor(originPos, color);
                yield return null;
            }
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // set color with i as alpha
                Color color = new Color(1, 1, 1, i);

                fogTilemap.SetColor(originPos, color);
                yield return null;
            }
        }
    }

    /**
     * Renvoie la liste des tiles à côté d'une position room donnée
     */
    private List<Vector3Int> UpdateNearRooms(Vector3Int roomPos, int revealRange)
    {
        if (null == PlayerManager.Instance) return null;

        List<Vector3Int> nearRoomsTiles = new List<Vector3Int>();

        RoomData currentRoomData = RoomEffectManager.Instance.GetRoomData(roomPos);

        Vector3Int currentRoomDataPos = new Vector3Int(currentRoomData.x, currentRoomData.y, 0);

        for (int i = 0; i < revealRange; ++i)
        {
            //On parcours les room data car on veut se contenter seulement des rooms du jeu, et pas au-delà (sinon on ajouterait directement les positions dans la liste)
            foreach (RoomData room in RoomEffectManager.Instance.roomsDataArr)
            {
                Vector3Int currRoom = new Vector3Int(room.x, room.y, 0);
                if (currRoom == currentRoomDataPos + new Vector3Int(i + 1, 0, 0)     //Si on a une room à + ou - 1 en x et y de la room actuelle
                    || currRoom == currentRoomDataPos + new Vector3Int(0, i + 1, 0)
                    || currRoom == currentRoomDataPos + new Vector3Int(-i - 1, 0, 0)
                    || currRoom == currentRoomDataPos + new Vector3Int(0, -i - 1, 0))
                {
                    nearRoomsTiles.Add(currRoom);
                }
            }
        }

        return nearRoomsTiles;
    }

    /**
     * Clear les rooms autour d'une position donnée, avec une certaine range
     * Penser à faire un SetRevealRange avant d'appeler la GSA Sight pour que la valeur de la carte soit prise en compte
     */
    public void RevealAtPosition(Vector3Int baseRoomPosition)
    {
        List<Vector3Int> nearRoomsTiles = UpdateNearRooms(baseRoomPosition, revealRange);

        nearRoomsTiles.Add(baseRoomPosition);


        foreach (Vector3Int roomPos in nearRoomsTiles)
        {
            StartCoroutine(FadeFog(true, roomPos));
        }

        StartCoroutine(TimedAction(timeAfterAction));

        //SFX fog clear
        MusicManager.Instance.PlayFogClear();
    }

    /**
     * Set la reveal range en fonction de la valeur de la carte
     */
    public void SetRevealRange(int rangeValue)
    {
        revealRange = rangeValue;
    }

    #endregion
}
