using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;

    // ============== A recup du GameEngine
    public GameObject player;
    public List<GameObject> enemiesList;

    private List<List<Spot>> enemiesPathList;

    private bool enemiesCanMove = false;
    private bool pathFound = false;
    
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

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
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

            Debug.Log(roadPath.Count);
            // ajoute un sprite sur les tiles du path
            DrawRoad();
            // le point d'arrivée de vient le nouveau point de départ
            start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
        }

        MoveEnemies(SetEnemyPath());

    }

    public void MoveEnemies(List<List<Spot>> pathTilesList)
    {
        //  DOIT BOUGER CASE PAR CASE DE LA LIST ROADPATH

        //transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime); //avance jusqu'à la tile cliquée

        // ajoute un sprite sur les tiles du path
        DrawRoad();
        // le point d'arrivée de vient le nouveau point de départ
        start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
    }

    /*
     * Renvoie la liste des tiles sur lequelles l'enemy doit bouger
     */
    public List<List<Spot>> SetEnemyPath()
    {
        Vector3 playerPos = player.transform.position;
        Vector3Int endPos = walkableTilemap.WorldToCell(playerPos);    // position d'arrivée (player) en format cellule

        enemiesPathList = null;

        // s'il y a des ennemis
        if (enemiesList.Count > 0)
        {
            // RECUP LA POSITION CELLULE DE CHAQUE ENEMY
            foreach (GameObject enemy in enemiesList)
            {
                Vector3Int enemyPos = walkableTilemap.WorldToCell(enemy.transform.position);
                if (roadPath != null && roadPath.Count > 0)
                    roadPath.Clear();

                // création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path
                // roadPath est une liste de spots = une liste de positions de tiles
                roadPath = astar.CreatePath(spots, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(endPos.x, endPos.y), nbMoves);
                
                if (roadPath == null)
                    return null;

                enemiesPathList.Add(roadPath);
            }
        }

        return enemiesPathList;
    }
}
