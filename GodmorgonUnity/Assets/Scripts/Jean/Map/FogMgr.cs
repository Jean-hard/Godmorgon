using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Bounds
{
    public int x = 10;
    public int y = 10;
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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
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
        TilesManager.Instance.UpdateAccessibleTilesList();

        foreach (Vector3Int room in nearRoomsTiles)   //Pour toutes les rooms à coté du player
        {
            foreach (Vector3Int tile in TilesManager.Instance.showableTilesList)
            {
                Vector3 worldPos = TilesManager.Instance.walkableTilemap.CellToWorld(tile);
                Vector3Int tempRoomPos = fogTilemap.WorldToCell(worldPos);
                if (room == tempRoomPos)
                {
                    if (fogTilemap.GetColor(room).a > 0.5f)   //Si la tile n'est pas transparente
                    {
                        //fogTilemap.SetTileFlags(room, TileFlags.None);   //On rend possible le changement de couleur
                        //fogTilemap.SetColor(room, transparentColor);     //On la rend transparente

                        StartCoroutine(FadeFog(true, room));
                    }
                }
            }
        }

        if (fogTilemap.GetColor(currentRoomDataPos).a > 0.5f)
        {
            //fogTilemap.SetTileFlags(currentRoomDataPos, TileFlags.None);   //On rend possible le changement de couleur
            //fogTilemap.SetColor(currentRoomDataPos, transparentColor);     //On la rend transparente

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
            Debug.Log("FADE to transparent");
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
            //fogTilemap.SetTileFlags(roomPos, TileFlags.None);
            //fogTilemap.SetColor(roomPos, transparentColor);

            StartCoroutine(FadeFog(true, roomPos));
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

    /**
     * Recouvre toute la map de Fog sauf où il y a le player et des cases voisines
     */
    public void CoverEntireMap()
    {
        hasUpdatedFog = false;  //Le fog n'a pas encore été updaté

        //On parcourt toutes les tiles du fog (gérer les bounds dans l'inspector)
        for(int x = 0; x < fogBounds.x; ++x)
        {
            for(int y = 0; y < fogBounds.y; ++y)
            {
                Vector3Int fogTilePos = new Vector3Int(x, y, 0);   //Récup la coordonnée de la tile actuelle
                fogTilemap.SetTile(fogTilePos, fogTile);    //On applique la tile fog sur toutes les cases
                fogTilemap.SetTileFlags(fogTilePos, TileFlags.None);   //Autorise le changement de color
                fogTilemap.SetColor(fogTilePos, opaqueColor);  //Applique l'alpha au max
            }
        }

        UpdateFogToPlayer();    //On clear la tile du player et autour
    }
}
