using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;
    
    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    BoundsInt bounds;

    private bool playerCanMove = false;
    private int spotIndex;
    private List<Spot> playerPathArray;

    // Start is called before the first frame update
    void Start()
    {
        walkableTilemap.CompressBounds();
        roadMap.CompressBounds();
        bounds = walkableTilemap.cellBounds;
        camera = Camera.main;


        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

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
        if (Input.GetMouseButton(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);
            start = new Vector2Int(gridPos.x, gridPos.y);
        }
        if (Input.GetMouseButton(2))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
        if (Input.GetMouseButton(0))
        {
            Debug.Log("hello");
            CreateGrid();

            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkableTilemap.WorldToCell(world);

            if (roadPath != null && roadPath.Count > 0)
                roadPath.Clear();

            roadPath = astar.CreatePath(spots, start, new Vector2Int(gridPos.x, gridPos.y), 1000);
            if (roadPath == null)
                return;

            DrawRoad();
            start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
            playerCanMove = true;
        }


        if(playerCanMove)
        {
            float ratio = (float)spotIndex / (playerPathArray.Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
            ratio = enemyMoveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
            float speed = enemySpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse de l'enemy
            this.transform.position = Vector2.MoveTowards(this.transform.position,);
        }
    }

    public void SetPlayerPath(Vector3 tileClicked)
    {
        Vector3 playerPos = this.transform.position;
        Vector3Int endPos = walkableTilemap.WorldToCell(tileClicked);    //position d'arrivée (player) en format cellule

        playerPathArray = new List<Spot>();

        foreach (GameObject enemy in enemiesArray)
        {
            spotIndex = 0;

            int nbMoves = enemy.GetComponent<EnemyDisplay>().GetNbMoves();  //on récupère le nombre de room que peut parcourir un enemy (propre à chacun)

            Vector3Int enemyPos = walkableTilemap.WorldToCell(enemy.transform.position);
            if (roadPath != null && roadPath.Count > 0) //reset le roadpath
                roadPath.Clear();

            //création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path -> dépend de l'ennemi
            //roadPath est une liste de spots = une liste de positions de tiles
            roadPath = astar.CreatePath(spots, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(endPos.x, endPos.y), nbTilesToMove * nbMoves);

            if (roadPath == null)
            {
                return;
            }

            foreach (Spot spot in roadPath)
            {
                bool hasPlayerOnPath = false;

                //on met dans un tableau les enemies qui vont se rendre dans la room du player pour pouvoir les faire s'arreter une tile avant le player
                if (playerCellPos.x == spot.X && playerCellPos.y == spot.Y)
                {
                    hasPlayerOnPath = true;
                    enemiesCloseToPlayer.Add(enemiesArray[enemyIndex]);
                }

                if (!hasPlayerOnPath)
                {
                    playerPathArray[enemyIndex].Add(spot);     //on ajoute pour tel enemy les spots par lesquels il va devoir passer sauf celui où il y a le player
                }

                spotIndex++;
            }

            playerPathArray[enemyIndex].Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
            playerPathArray[enemyIndex].RemoveAt(0);

            /*
                * Affiche les coordonnées de tiles des paths que les enemies doivent parcourir
            foreach(Spot spot in enemiesPathArray[enemyIndex])
            {
                Debug.Log(spot.X + " / " + spot.Y);
            }*/

            enemyIndex++;
        }
        

        playerCanMove = true;  //on autorise les enemies à bouger
        spotIndex = 0;
    }
}
