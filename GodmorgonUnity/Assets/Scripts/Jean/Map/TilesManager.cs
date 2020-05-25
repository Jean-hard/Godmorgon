using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

using GodMorgon.Player;
using GodMorgon.Enemy;

public class TilesManager : MonoBehaviour
{
    [Header("Tilemaps")]
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public Tilemap roomTilemap;

    public Vector3Int[,] spots;
    private List<Vector3Int> tilesList;

    [System.NonSerialized]
    public List<Vector3Int> accessibleTiles = new List<Vector3Int>();

    private List<Vector3Int> nearestTilesList = new List<Vector3Int>();
    Vector3Int currentTileCoordinate;

    [Header("Effects Settings")]
    public GameObject moveTileEffect;
    public Transform effectsParent;
    private bool effectInstantiated = false;


    // nombre de tiles parcourues pour 1 move
    private int nbTilesToMove = 3;

    public Astar astar;
    public List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;

    #region Singleton Pattern
    private static TilesManager _instance;

    public static TilesManager Instance { get { return _instance; } }
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
        walkableTilemap.CompressBounds();
        roadMap.CompressBounds();
        bounds = walkableTilemap.cellBounds;

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);

        UpdateAccessibleTilesList(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        tilesList = new List<Vector3Int>();
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                    tilesList.Add(new Vector3Int(x, y, 0));
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    /**
     * Donne les cases les plus proches du joueur vers lesquelles il peut se déplacer
     */
    public void UpdateAccessibleTilesList(int nbMoves)
    {
        nearestTilesList.Clear();   //Clear la liste de tiles avant de placer les nouvelles
        currentTileCoordinate = walkableTilemap.WorldToCell(PlayerManager.Instance.transform.position);   //On transpose la position scène du player en position grid 
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x + nbTilesToMove, currentTileCoordinate.y, 0));   //Ajoute les 4 cases voisines
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x - nbTilesToMove, currentTileCoordinate.y, 0));
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y + nbTilesToMove, 0));
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y - nbTilesToMove, 0));

        accessibleTiles.Clear();
        Vector3 playerPos = PlayerManager.Instance.transform.position;
        Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell

        if (null != tilesList)
        {
            //On regarde si les cases proches sont walkable, si non : on les retire de la liste
            List<Vector3Int> tempNearTilesList = new List<Vector3Int>();            
            foreach (Vector3Int tile in nearestTilesList)
            {
                if (tilesList.Contains(tile))
                    tempNearTilesList.Add(tile);
            }
            nearestTilesList = tempNearTilesList;


            //Si le chemin vers les rooms proches est direct, dans ce cas on met la tile dans la liste des accessibles
            foreach (Vector3Int tile in nearestTilesList)
            { 
                roadPath = astar.CreatePath(spots, new Vector2Int(playerCellPos.x, playerCellPos.y), new Vector2Int(tile.x, tile.y), 100);

                bool isEnemyOnPath = false;

                //Si la tile fait partie de celles voisines (room) et que le chemin est direct (moins de 5 tiles pour y accéder)
                if (nearestTilesList.Contains(tile) && roadPath.Count < 5)
                {
                    //On check en plus si on a un ennemi sur le chemin 
                    foreach (Spot _tile in roadPath)
                    {
                        //Si un ennemi est sur une tile du roadPath
                        if (null != EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(_tile.X, _tile.Y, 0)))
                        {
                            //Si cet ennemi est dans la room
                            if (EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(_tile.X, _tile.Y, 0)).enemyData.inPlayersRoom)
                                isEnemyOnPath = true;
                        }
                    }

                    //Si aucun ennemi est sur le path jusqu'à cette tile, c'est une tile accessible
                    if (!isEnemyOnPath)
                    {
                        //Si le joueur n'est pas au centre de la room actuelle car il n'était pas arrivé en premier (ennemi déjà présent)
                        if (!PlayerManager.Instance.isFirstInRoom)
                        {
                            //La tile accessible est l'avant dernière tile du path (l'index 0 étant la tile la plus éloignée, on prend celle d'avant = index 1) 
                            //pour que le joueur arrive au milieu de la prochaine room (vu qu'il n'était pas centré dans la room)
                            Vector3Int _newTile = new Vector3Int(roadPath[1].X, roadPath[1].Y, 0);
                            accessibleTiles.Add(_newTile);

                        }
                        //Sinon si le joueur était bien au milieu d'une room car arrivé en premier dedans
                        else
                            accessibleTiles.Add(tile);
                    }

                }
            }
        }
    }

    /**
     * Montre les tiles sur lesquelles le joueur peut se déplacer
     */
    public void ShowAccessibleTiles()
    {
        //Debug.Log("Accessible Tiles : " + accessibleTiles.Count);

        for (int i = 0; i < accessibleTiles.Count; i++)
        {
            Vector2 moveTileEffectPos = walkableTilemap.CellToWorld(accessibleTiles[i]) + new Vector3(0, 0.25f, 0);
            if (!effectInstantiated)
            {
                //on lance l'effet de drop de carte sur la map, qui sera en enfant de effectsParent
                GameObject effectObject = Instantiate(moveTileEffect, moveTileEffectPos, Quaternion.identity, effectsParent);
                effectObject.transform.rotation = Quaternion.Euler(-60, 0, 0);
            }
        }
        effectInstantiated = true;
    }

    /**
     * Cache les tiles sur lesquelles le joueur peut se déplacer
     */
    public void HideAccessibleTiles()
    {
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
        effectInstantiated = false;
    }
}
