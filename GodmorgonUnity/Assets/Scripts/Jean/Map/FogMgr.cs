using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogMgr : MonoBehaviour
{
    public static FogMgr Instance;

    [Header("Sight Card Settings")]
    [SerializeField]
    private float timeAfterAction = 2f;

    private Tilemap fogTilemap = null;

    private Color transparentColor = new Color(1, 1, 1, 0);

    private bool hasUpdatedFog = false;
    private bool hasBeenRevealed = false;
    
    private int revealRange = 1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        fogTilemap = this.GetComponent<Tilemap>();
    }

    private void Update()
    {
        UpdateFogToPlayer();
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
        TilesManager.Instance.UpdateAccessibleTilesList(1);

        Debug.Log("is Updating fog");

        foreach (Vector3Int room in nearRoomsTiles)   //Pour toutes les rooms à coté du player
        {
            foreach (Vector3Int tile in TilesManager.Instance.accessibleTiles)
            {
                Vector3 worldPos = TilesManager.Instance.walkableTilemap.CellToWorld(tile);
                Vector3Int tempRoomPos = fogTilemap.WorldToCell(worldPos);
                if (room == tempRoomPos)
                {
                    if (fogTilemap.GetColor(room).a > 0.5f)   //Si la tile n'est pas transparente
                    {                     
                        fogTilemap.SetTileFlags(room, TileFlags.None);   //On rend possible le changement de couleur
                        fogTilemap.SetColor(room, transparentColor);     //On la rend transparente
                    }
                }
            }
        }

        if (fogTilemap.GetColor(currentRoomDataPos).a > 0.5f)
        {
            fogTilemap.SetTileFlags(currentRoomDataPos, TileFlags.None);   //On rend possible le changement de couleur
            fogTilemap.SetColor(currentRoomDataPos, transparentColor);     //On la rend transparente
        }

        hasUpdatedFog = true;
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

        for(int i = 0; i < revealRange; ++i)
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
        
        //---------- WIP ------------- Gérer le cas où on dépose la carte sur une case déjà clear
        
        foreach(Vector3Int roomPos in nearRoomsTiles)
        {
            fogTilemap.SetTileFlags(roomPos, TileFlags.None);
            fogTilemap.SetColor(roomPos, transparentColor);
        }

        StartCoroutine(TimedAction(timeAfterAction));
    }

    /**
     * Set la reveal range en fonction de la valeur de la carte
     */
    public void SetRevealRange(int rangeValue)
    {
        revealRange = rangeValue;
    }

    /**
     * Chech si les positions ont été reveal
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
}
