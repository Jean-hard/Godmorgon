using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RingMasterManager : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;

    private GameObject player;
    private GameObject[] enemiesArray;
    private List<Spot>[] enemiesPathArray;
    private int enemyIndex;
    private int spotIndex;

    private bool enemiesCanMove = false;

    public float enemySpeed = 1.0f;

    // A recup dans les futurs scriptables object des enemies
    public int nbMoves;

    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    BoundsInt bounds;
    
    
    // Start is called before the first frame update
    void Start()
    {
        walkableTilemap.CompressBounds();   // réduit la taille de la tilemap à là où des tiles existent
        roadMap.CompressBounds();   // réduit la taille de la tilemap à là où des tiles existent
        bounds = walkableTilemap.cellBounds;
        camera = Camera.main;

        player = GameObject.FindGameObjectWithTag("Player");
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

    

    // Update is called once per frame
    public Vector2Int start;
    void Update()
    {
        // Set la position de départ du path avec le clique droit
        if (Input.GetMouseButton(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }

        // Efface le chemin tracé avec le bouton molette
        if (Input.GetMouseButton(2))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }

        // Trace le chemin entre le start et la tile cliquée avec le clique gauche
        if (Input.GetMouseButtonDown(0))    //GetMouseButtonDown ici car appelé pendant 1 seule frame
        {
            CreateGrid();

            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);    // position d'arrivée en format tile

            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();

            // création du path, prenant en compte la position des tiles walkable, le point de départ, le point d'arrivée, et la longueur en tiles du path
            // roadPath est une liste de spots = une liste de positions de tiles
            roadPath = astar.CreatePath(spots, start, new Vector2Int(gridPos.x, gridPos.y), 3);
            if (roadPath == null)
                return;

            //Debug.Log(roadPath.Count);
            // ajoute un sprite sur les tiles du path
            DrawRoad();
            // le point d'arrivée de vient le nouveau point de départ
            start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
        }

        MoveEnemies();

    }

    /*
     * Ajoute dans un tableau de Vector3 les positions des tiles de la tilemap Walkable
     */
    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    /*
     * Ajoute un sprite sur les tile d'un path
     */
    private void DrawRoad()
    {
        for (int i = 0; i < roadPath.Count; i++)
        {
            roadMap.SetTile(new Vector3Int(roadPath[i].X, roadPath[i].Y, 0), roadTile);
        }
    }

    /*
     * Lance le mouvement des ennemis
     * Prend en paramètre un tableau de listes de positions de tiles
     */
    public void MoveEnemies()
    {
        if (enemiesArray.Length <= 0 || !enemiesCanMove)
        {
            Debug.Log("Quit MoveEnemies");
            return;
        }

        // FAIRE UNE SECURITE SI UN ENEMY A LA MEME POSITION QUE LA CASE OU CE REND L'ENEMY EN COURS
        
        if (enemyIndex < enemiesArray.Length)
        {
            Debug.Log("Taille pathTilesArray : " + enemiesPathArray[enemyIndex].Count);
            Debug.Log("Enemy Index : " + enemyIndex);
            Vector3 nextPos = walkableTilemap.CellToWorld(new Vector3Int(enemiesPathArray[enemyIndex][spotIndex].X, enemiesPathArray[enemyIndex][spotIndex].Y, 0));    // la prochaine position est le spot de l'enemy correspondant
            enemiesArray[enemyIndex].transform.position = Vector3.MoveTowards(enemiesArray[enemyIndex].transform.position, nextPos, enemySpeed * Time.deltaTime); 
            
            if (Vector3.Distance(enemiesArray[enemyIndex].transform.position, nextPos) < 0.001f)
            {
                if(spotIndex == (nbMoves - 1))  //si on arrive à la tile finale où l'enemy peut se rendre
                {
                    enemyIndex++;   //on passe à l'enemy suivant
                    spotIndex = 0;  //reset de l'index des spots pour que l'enemy suivant avance sur une case à coté de lui 
                }
                else if(spotIndex < (nbMoves - 1))
                    spotIndex++;    //on passe à la tile suivante
                
                if (enemyIndex >= enemiesArray.Length)   //s'il ne reste plus d'enemies à bouger
                {
                    enemiesCanMove = false;
                    enemyIndex = 0;
                    spotIndex = 0;
                }
            }
        }
    }

    /*
     * Renvoie la liste des tiles sur lequelles l'enemy doit bouger
     */
    public void SetEnemyPath()
    {
        Vector3 playerPos = player.transform.position;
        Vector3Int endPos = walkableTilemap.WorldToCell(playerPos);    // position d'arrivée (player) en format cellule

        enemyIndex = 0;
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesPathArray = new List<Spot>[enemiesArray.Length];
        

        // s'il y a des ennemis
        if (enemiesArray.Length > 0)
        {
            // RECUP LA POSITION CELLULE DE CHAQUE ENEMY
            foreach (GameObject enemy in enemiesArray)
            {
                Vector3Int enemyPos = walkableTilemap.WorldToCell(enemy.transform.position);
                if (roadPath != null && roadPath.Count > 0)
                    roadPath.Clear();

                // création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path
                // roadPath est une liste de spots = une liste de positions de tiles
                roadPath = astar.CreatePath(spots, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(endPos.x, endPos.y), nbMoves);

                if (roadPath == null)
                    return;

                enemiesPathArray[enemyIndex] = roadPath;
                
                enemyIndex++;
            }
        }

        enemiesCanMove = true;  //on autorise les enemies à bouger
        enemyIndex = 0;
        spotIndex = 0;
    }
}
