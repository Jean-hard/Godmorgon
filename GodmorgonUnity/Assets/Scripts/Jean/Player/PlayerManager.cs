using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

using GodMorgon.StateMachine;
using GodMorgon.Player;
using GodMorgon.Enemy;

public class PlayerManager : MonoBehaviour
{
    [Header("General Settings")]
    public Text healthText;

    [Header("Movement Settings")]
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase accessibleTile;
    public Vector3Int[,] spots;
    private List<Vector3Int> tilesList;
    public float playerSpeed = 1f;
    public AnimationCurve playerMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;

    private bool playerCanMove = false;
    private bool playerHasMoved = false;
    private int spotIndex;
    private List<Spot> playerPathArray;
    public List<Vector3Int> accessibleTiles = new List<Vector3Int>();

    Vector3Int endPos;

    // nombre de tiles parcourues pour 1 move
    private int nbTilesToMove = 3;

    private List<Vector3Int> nearestTilesList = new List<Vector3Int>();
    Vector3Int currentTileCoordinate;

    public GameObject moveTileEffect;
    public Transform effectsParent;
    private bool effectInstantiated = false;

    private bool isFirstInRoom = true;

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

        UpdateHealthText(PlayerData.Instance.healthMax);

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
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
        LaunchMoveMechanic();
    }


    /**
     * Détermine la liste de tiles (=path) à parcourir jusqu'à l'endroit où la carte mouvement est déposée
     */
    public void MovePlayer()
    {
        //Transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0,0,10);
        endPos = walkableTilemap.WorldToCell(mouseWorldPos);

        //Si la tile sélectionnée fait partie de la liste des tiles accessibles
        //if (accessibleTiles.Contains(endPos))
        //{
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

        bool isEnemyOnPath = false;

        foreach (Spot tile in roadPath)
        {
            bool isEnemyOnTile = false;

            //on ajoute les tiles par lesquelles il va devoir passer sauf celle où il y a un enemy
            if (null != EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(tile.X, tile.Y, 0)))
            {
                EnemyManager.Instance.GetEnemyViewByPosition(new Vector3Int(tile.X, tile.Y, 0)).enemyData.inPlayersRoom = true;
                isEnemyOnTile = true;
                isEnemyOnPath = true;
                isFirstInRoom = false;
            }

            if(!isEnemyOnTile)
                playerPathArray.Add(tile);     //on ajoute les spots par lesquels le player va devoir passer sauf celui où il est

            spotIndex++;
        }

        //Si on ETAIT le premier arrivé dans la room, alors un ennemi présent dans la room doit se recentrer au milieu de la room
        if (isFirstInRoom)
            EnemyManager.Instance.RecenterEnemies();

        //Si on n'a pas d'ennemi sur le chemin, on est le premier arrivé dans la room
        if (!isEnemyOnPath)
        {
            isFirstInRoom = true;
            foreach (EnemyView enemy in EnemyManager.Instance.GetEnemiesInPlayersRoom())
            {
                enemy.enemyData.inPlayersRoom = false;
            }
        }

        playerPathArray.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
        playerPathArray.RemoveAt(0);

        /*    
        foreach (Spot spot in playerPathArray)
        {
            Debug.Log(spot.X + " " + spot.Y);
        }*/

        playerCanMove = true;  //on autorise le player à bouger

        spotIndex = 0;    
    }

    /**
     * Lance le mouvement du player
     */
    private void LaunchMoveMechanic()
    {
        if(!playerCanMove)
        {
            return;
        }

        //la prochaine position est le spot parmi la liste de spots
        Vector3 nextPos = walkableTilemap.CellToWorld(new Vector3Int(playerPathArray[spotIndex].X, playerPathArray[spotIndex].Y, 0))
            + new Vector3(0, 0.3f, 0);   //on ajoute 0.x pour que le player passe bien au milieu de la tile, la position de la tile étant en bas du losange             


        if (Vector3.Distance(this.transform.position, nextPos) < 0.001f)
        {
            //si on arrive à la tile finale où le player peut se rendre
            if (spotIndex == playerPathArray.Count - 1)
            {

                playerCanMove = false;
                playerHasMoved = true;
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
    * Renvoie true si le mouvement des ennemis est terminé
    */
    public bool PlayerMoveDone()
    {
        if (playerHasMoved)
            return true;
        else
            return false;
    }

    #region TILES_MANAGER

    /**
     * Donne les cases les plus proches du joueur vers lesquelles il peut se déplacer
     */
    public void UpdateAccessibleTilesList()
    {
        nearestTilesList.Clear();   //Clear la liste de tiles avant de placer les nouvelles
        currentTileCoordinate = walkableTilemap.WorldToCell(transform.position);   //On transpose la position scène du player en position grid 
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x + nbTilesToMove, currentTileCoordinate.y, 0));   //Ajoute les 4 cases voisines
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x - nbTilesToMove, currentTileCoordinate.y, 0));   
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y + nbTilesToMove, 0));
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y - nbTilesToMove, 0));

        accessibleTiles.Clear();
        Vector3 playerPos = this.transform.position;
        Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);  //Position du player en format cell
        
        //Pour toutes les tiles walkable, on regarde si elle fait partie des 4 cases (room) voisines, et si le chemin vers cette room est direct, dans ce cas on met la tile dans la liste des accessibles
        foreach (Vector3Int tile in tilesList)
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
                if(!isEnemyOnPath)
                {
                    //Si le joueur n'est pas au centre de la room actuelle car il n'était pas arrivé en premier (ennemi déjà présent)
                    if(!isFirstInRoom)
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

    /**
     * Montre les tiles sur lesquelles le joueur peut se déplacer
     */
    public void ShowAccessibleTiles()
    {
        UpdateAccessibleTilesList();

        for (int i = 0; i < accessibleTiles.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(accessibleTiles[i].x, accessibleTiles[i].y, 0), accessibleTile); //On applique le sprite de tile sur les spots accessibles
            Vector2 moveTileEffectPos = walkableTilemap.CellToWorld(accessibleTiles[i]) + new Vector3(0, 0.2f, 0);
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
    public void HideAccessibleTiles()
    {
        roadMap.ClearAllTiles();
        for (int i = 0; i < effectsParent.childCount; i++)
        {
            Destroy(effectsParent.GetChild(i).gameObject);
        }
        effectInstantiated = false;
    }

    public void GetPlayerPosition()
    {

    }

    #endregion

    // WIP
    IEnumerator WaitForRingMasterTurn()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Ringmaster turn");
        //GameEngine.Instance.SetState(StateMachine.STATE.RINGMASTER_TURN);
        playerHasMoved = false;
    }

    /**
     * Inflige des damages au player
     */
    public void TakeDamage(int damage)
    {
        //WIP : considérer le shield du player

        int newHealth = PlayerData.Instance.health - damage;
        PlayerData.Instance.SetHealth(newHealth);
        UpdateHealthText(newHealth);
        Debug.Log("Update player's life ");
    }

    private void UpdateHealthText(int healthValue)
    {
        healthText.text = healthValue.ToString();
    }
}
