using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using GodMorgon.StateMachine;


public class PlayerManager : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase accessibleTile;
    public Vector3Int[,] spots;
    private List<Vector3Int> spotsList;
    
    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;

    private bool playerCanMove = false;
    private int spotIndex;
    private List<Spot> playerPathArray;
    private List<Vector3Int> accessibleSpots = new List<Vector3Int>();

    public float playerSpeed = 1f;
    public AnimationCurve playerMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    Vector3Int endPos;

    // nombre de tiles parcourues pour 1 move
    private int nbTilesToMove = 3;

    private List<Vector3Int> nearestTilesList = new List<Vector3Int>();
    Vector3Int currentTileCoordinate;

    public GameObject moveTileEffect;
    public Transform effectsParent;
    private bool effectInstantiated = false;

    #region Singleton Pattern
    private static PlayerManager _instance;

    public static PlayerManager Instance { get { return _instance; } }
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
    }

    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        spotsList = new List<Vector3Int>();
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                    spotsList.Add(new Vector3Int(x, y, 0));
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), accessibleTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }


    /**
     * Détermine la liste de tiles (=path) à parcourir jusqu'à l'endroit où la carte mouvement est déposée
     */
    public bool SetPlayerPath()
    {
        //Transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        endPos = walkableTilemap.WorldToCell(mouseWorldPos);

        //Si la tile sélectionnée fait partie de la liste des tiles accessibles
        if (accessibleSpots.Contains(endPos))
        {
            Vector3 playerPos = this.transform.position;
            Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell

            playerPathArray = new List<Spot>();

            spotIndex = 0;

            //int nbMoves = 2;

            if (roadPath != null && roadPath.Count > 0) //reset le roadpath
                roadPath.Clear();

            //création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path
            //roadPath est une liste de spots = une liste de positions de tiles
            roadPath = astar.CreatePath(spots, new Vector2Int(playerCellPos.x, playerCellPos.y), new Vector2Int(endPos.x, endPos.y), nbTilesToMove);    // * nbMoves

            foreach (Spot spot in roadPath)
            {
                playerPathArray.Add(spot);     //on ajoute les spots par lesquels le player va devoir passer sauf celui où il est

                spotIndex++;
            }

            playerPathArray.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
            playerPathArray.RemoveAt(0);

            /*
            foreach (Spot spot in playerPathArray)
            {
                Debug.Log(spot.X + " " + spot.Y);
            }*/

            playerCanMove = true;  //on autorise le player à bouger
        }
        else
            playerCanMove = false;

        spotIndex = 0;
        return playerCanMove;
        
    }

    public void MovePlayer()
    {
        if(!playerCanMove)
        {
            return;
        }

        //la prochaine position est le spot parmi la liste de spots
        Vector3 nextPos = walkableTilemap.CellToWorld(new Vector3Int(playerPathArray[spotIndex].X, playerPathArray[spotIndex].Y, 0))
            + new Vector3(0, 0.4f, 0);   //on ajoute 0.4 pour que le player passe bien au milieu de la tile, la position de la tile étant en bas du losange             


        if (Vector3.Distance(this.transform.position, nextPos) < 0.001f)
        {
            //si on arrive à la tile finale où le player peut se rendre
            if (spotIndex == playerPathArray.Count - 1)
            {
                //WIP : si on arrive dans une room où un enemy se situe
                /*if (enemiesCloseToPlayer.Contains(enemiesArray[enemyIndex]))
                {
                    enemiesArray[enemyIndex].GetComponent<EnemyDisplay>().SetIsInPlayersRoom(true); //on set l'état de cet enemy dans la room
                }*/

                playerCanMove = false;
                spotIndex = 0;
                StartCoroutine(WaitForRingMasterTurn());
            }
            else if (spotIndex < playerPathArray.Count - 1)
            {
                spotIndex++;    //on passe à la tile suivante tant qu'on a pas atteint la dernière
            }
        }
        else
        {
            float ratio = (float)spotIndex / (playerPathArray.Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
            ratio = playerMoveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
            float speed = playerSpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse du player
            this.transform.position = Vector2.MoveTowards(this.transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    /**
     * Donne les cases les plus proches du joueur vers lesquelles il peut se déplacer
     */
    public void UpdateAccessibleSpotsList()
    {
        nearestTilesList.Clear();   //Clear la liste de tiles avant de placer les nouvelles
        currentTileCoordinate = walkableTilemap.WorldToCell(transform.position);   //On transpose la position scène du player en position grid 
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x + nbTilesToMove, currentTileCoordinate.y, 0));   //Ajoute les 4 cases voisines
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x - nbTilesToMove, currentTileCoordinate.y, 0));   
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y + nbTilesToMove, 0));
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y - nbTilesToMove, 0));

        accessibleSpots.Clear();
        Vector3 playerPos = this.transform.position;
        Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell
        
        //Pour tous les spots walkable, on regarde s'il fait partie des 4 cases (room) voisines, et si le chemin vers cette room est direct, dans ce cas on met le spot dans la liste des accessibles
        foreach (Vector3Int spot in spotsList)
        {
            roadPath = astar.CreatePath(spots, new Vector2Int(playerCellPos.x, playerCellPos.y), new Vector2Int(spot.x, spot.y), 100);
            
            if (nearestTilesList.Contains(spot) && roadPath.Count < 5)
            {
                accessibleSpots.Add(spot);
            }
        }
    }

    /**
     * Montre les tiles sur lesquelles le joueur peut se déplacer
     */
    public void ShowAccessibleSpot()
    {
        UpdateAccessibleSpotsList();

        for (int i = 0; i < accessibleSpots.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(accessibleSpots[i].x, accessibleSpots[i].y, 0), accessibleTile); //On applique le sprite de tile sur les spots accessibles
            Vector2 moveTileEffectPos = walkableTilemap.CellToWorld(accessibleSpots[i]) + new Vector3(0, 0.2f, 0);
            if (!effectInstantiated)
            {
                Instantiate(moveTileEffect, moveTileEffectPos, Quaternion.identity, effectsParent); //on lance l'effet de drop de carte sur la map, qui sera en enfant de effectsParent
            }
        }
        effectInstantiated = true;
    }

    /**
     * Cache les tiles sur lesquelles le joueur peut se déplacer
     */
    public void HideAccessibleSpot()
    {
        roadMap.ClearAllTiles();
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
        effectInstantiated = false;
    }

    // WIP
    IEnumerator WaitForRingMasterTurn()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Ringmaster turn");
        GameEngine.Instance.SetState(StateMachine.STATE.RINGMASTER_TURN);
    }
}
