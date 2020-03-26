﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GodMorgon.Models;

public class EnemyManager : MonoBehaviour
{
    public Grid grid;
    public Tilemap walkableTilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;

    public GameObject player;
    private List<GameObject> enemiesArray;
    private List<Spot>[] enemiesPathArray;
    private int enemyIndex;
    private int spotIndex;
    private List<GameObject> enemiesCloseToPlayer;

    private bool enemiesCanMove = false;
    private bool enemiesHaveMoved = false;

    public float enemySpeed = 1f;
    public AnimationCurve enemyMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    // nombre de tiles parcourues pour 1 move
    private int nbTilesToMove = 3;

    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    BoundsInt bounds;

    #region Singleton Pattern
    private static EnemyManager _instance;

    public static EnemyManager Instance { get { return _instance; } }
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
        walkableTilemap.CompressBounds();   // réduit la taille de la tilemap à là où des tiles existent
        roadMap.CompressBounds();   // réduit la taille de la tilemap à là où des tiles existent
        bounds = walkableTilemap.cellBounds;

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
     * Mets dans un tableau les enemies enfant du gameobject EnemyManager qui ne sont pas dans la room du player
     */
    private void UpdateEnemiesArray()
    {
        enemiesArray = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            //s'il n'est pas dans la room du player, on ne l'ajoute pas dans le tableau des enemies
            if (transform.GetChild(i).gameObject.GetComponent<EnemyDisplay>().GetIsInPlayersRoom() == false)
                enemiesArray.Add(transform.GetChild(i).gameObject);
        }
    }

    /*
     * Renvoie la liste des tiles sur lequelles l'enemy doit bouger
     */
    public void SetEnemyPath()
    {
        enemiesHaveMoved = false;

        Vector3 playerPos = player.transform.position;
        Vector3Int endPos = walkableTilemap.WorldToCell(playerPos);    //position d'arrivée (player) en format cellule

        enemyIndex = 0;
        
        UpdateEnemiesArray();

        enemiesPathArray = new List<Spot>[enemiesArray.Count];
        for (int i = 0; i < enemiesPathArray.Length; i++)
        {
            enemiesPathArray[i] = new List<Spot>();
        }

        //s'il y a des ennemis
        if (enemiesArray.Count > 0)
        {
            Vector3Int playerCellPos = walkableTilemap.WorldToCell(player.transform.position);
            
            enemiesCloseToPlayer = new List<GameObject>();   //Init la liste des/du enemies dans la room du joueur

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
                        enemiesPathArray[enemyIndex].Add(spot);     //on ajoute pour tel enemy les spots par lesquels il va devoir passer sauf celui où il y a le player
                    }

                    spotIndex++;
                }

                enemiesPathArray[enemyIndex].Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
                enemiesPathArray[enemyIndex].RemoveAt(0);

                /*
                    * Affiche les coordonnées de tiles des paths que les enemies doivent parcourir
                foreach(Spot spot in enemiesPathArray[enemyIndex])
                {
                    Debug.Log(spot.X + " / " + spot.Y);
                }*/

                enemyIndex++;
            }
        }

        enemiesCanMove = true;  //on autorise les enemies à bouger
        enemyIndex = 0;
        spotIndex = 0;
    }

    /*
     * Lance le mouvement des ennemis
     */
    public void MoveEnemies()
    {
        if (enemiesArray == null || enemiesArray.Count == 0 || !enemiesCanMove)
        {
            return;
        }

        //FAIRE UNE SECURITE SI 2 ENEMY SE CROISENT

        if (enemyIndex < enemiesArray.Count)
        {
            //la prochaine position est le spot parmi la liste de spot de l'enemy concerné
            Vector3 nextPos = walkableTilemap.CellToWorld(new Vector3Int(enemiesPathArray[enemyIndex][spotIndex].X, enemiesPathArray[enemyIndex][spotIndex].Y, 0))
                + new Vector3(0, 0.4f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange             


            if (Vector3.Distance(enemiesArray[enemyIndex].transform.position, nextPos) < 0.001f)
            {
                //on passe à l'ennemy suivant si on arrive à la tile finale où l'enemy peut se rendre
                if (spotIndex == enemiesPathArray[enemyIndex].Count - 1)
                {
                    //si l'enemy avait sur son chemin le player (il est donc dans sa room à se moment là)
                    if(enemiesCloseToPlayer.Contains(enemiesArray[enemyIndex]))
                    {
                        enemiesArray[enemyIndex].GetComponent<EnemyDisplay>().SetIsInPlayersRoom(true); //on set l'état de cet enemy dans la room
                    }
                        
                    enemyIndex++;   //on passe à l'enemy suivant
                    spotIndex = 0;  //reset de l'index des spots pour que l'enemy suivant avance sur une case à coté de lui 
                }
                else if (spotIndex < enemiesPathArray[enemyIndex].Count - 1)
                {
                    spotIndex++;    //on passe à la tile suivante tant qu'on a pas atteint la dernière
                }

                if (enemyIndex >= enemiesArray.Count)   //s'il ne reste plus d'enemies à bouger
                {
                    enemiesCanMove = false;
                    enemiesHaveMoved = true;
                    enemyIndex = 0;
                    spotIndex = 0;
                }
            }
            else
            {
                float ratio = (float)spotIndex / (enemiesPathArray[enemyIndex].Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
                ratio = enemyMoveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
                float speed = enemySpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse de l'enemy
                enemiesArray[enemyIndex].transform.position = Vector2.MoveTowards(enemiesArray[enemyIndex].transform.position, nextPos, speed * Time.deltaTime);   //on avance jusqu'à la prochaine tile
            }
        }
        else 
            enemyIndex++;   //on passe à l'enemy suivant si un enemy a été retiré de la liste pcq dans la room du player
        
    }

    public bool EnemiesMoveDone()
    {
        if (enemiesHaveMoved)
            return true;
        else
            return false;
    }
}
