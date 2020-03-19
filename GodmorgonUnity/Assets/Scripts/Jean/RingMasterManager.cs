﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RingMasterManager : MonoBehaviour
{
    public Grid grid;
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

    public float enemySpeed = 0.5f;

    // A recup dans les futurs scriptables object des enemies : nombre de room que l'ennemi peut parcourir en une fois
    public int nbMoves = 1;

    // 
    public int nbTilesToMove = 4;

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
    void Update()
    {
        

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
     * Renvoie la liste des tiles sur lequelles l'enemy doit bouger
     */
    public void SetEnemyPath()
    {
        Vector3 playerPos = player.transform.position;
        Vector3Int endPos = walkableTilemap.WorldToCell(playerPos);    // position d'arrivée (player) en format cellule

        enemyIndex = 0;
        enemiesArray = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesPathArray = new List<Spot>[enemiesArray.Length];
        for (int i = 0; i < enemiesPathArray.Length; i++)
        {
            enemiesPathArray[i] = new List<Spot>();
        }

        // s'il y a des ennemis
        if (enemiesArray.Length > 0)
        {
            foreach (GameObject enemy in enemiesArray)
            {
                spotIndex = 0;

                Vector3Int enemyPos = walkableTilemap.WorldToCell(enemy.transform.position);
                if (roadPath != null && roadPath.Count > 0) //reset le roadpath
                    roadPath.Clear();

                // création du path, prenant en compte la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path -> dépend de l'ennemi
                // roadPath est une liste çde spots = une liste de positions de tiles
                roadPath = astar.CreatePath(spots, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(endPos.x, endPos.y), nbTilesToMove * nbMoves);

                if (roadPath == null)
                {
                    return;
                }

                foreach (Spot spot in roadPath)
                {
                    if (spotIndex > 0)  //on zappe le premier spot car correspond à la tile sur laquelle l'enemy est
                    {
                        enemiesPathArray[enemyIndex].Add(spot);     //on ajoute pour tel enemy les 3 spots par lesquels il va devoir passer
                    }
                    spotIndex++;
                }

                enemiesPathArray[enemyIndex].Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée

                enemyIndex++;
            }
        }

        enemiesCanMove = true;  //on autorise les enemies à bouger
        enemyIndex = 0;
        spotIndex = 0;
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

        // FAIRE UNE SECURITE SI 2 ENEMY SE CROISENT
        
        if (enemyIndex < enemiesArray.Length)
        {
            // la prochaine position est le spot parmi la liste de spot de l'enemy concerné
            Vector3 nextPos = walkableTilemap.CellToWorld(new Vector3Int(enemiesPathArray[enemyIndex][spotIndex].X, enemiesPathArray[enemyIndex][spotIndex].Y, 0)) 
                + new Vector3(0, 0.4f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange             
            
            if (Vector3.Distance(enemiesArray[enemyIndex].transform.position, nextPos) < 0.001f)
            {
                if(spotIndex == (nbTilesToMove * nbMoves - nbMoves))  //si on arrive à la tile finale où l'enemy peut se rendre
                {
                    enemyIndex++;   //on passe à l'enemy suivant
                    spotIndex = 0;  //reset de l'index des spots pour que l'enemy suivant avance sur une case à coté de lui 
                }
                else if(spotIndex < (nbTilesToMove * nbMoves - nbMoves))
                    spotIndex++;    //on passe à la tile suivante
                
                if (enemyIndex >= enemiesArray.Length)   //s'il ne reste plus d'enemies à bouger
                {
                    enemiesCanMove = false;
                    enemyIndex = 0;
                    spotIndex = 0;
                }
            }
            else
            {
                enemiesArray[enemyIndex].transform.position = Vector3.MoveTowards(enemiesArray[enemyIndex].transform.position, nextPos, enemySpeed * Time.deltaTime);   //on avance jusqu'à la prochaine tile
            }
        }
    }
}
