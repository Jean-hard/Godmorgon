using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using GodMorgon.Models;

namespace GodMorgon.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Enemy Settings")]
        public Models.Enemy enemy;  //Scriptable object Enemy
        public string enemyId = ""; //Nom de l'ennemi

        [Header("Movement Settings")]
        public float moveSpeed = 5f;    //Vitesse de l'ennemi
        public AnimationCurve moveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);   //Curve liée à la vitesse de l'ennemi (pour lui donner un pattern de move)
        private Vector2Int nextPos = new Vector2Int();
        private bool isMoveFinished = false;

        [Header("General Settings")]
        public Grid grid;
        private Tilemap walkableTilemap;
        private Tilemap roadMap;

        private PlayerManager player;
        private Animator _animator;
        public EnemyData enemyData = new EnemyData();


        public void Awake()
        {
            if (enemy != null && enemyData != null)
            {
                enemyData.enemyId = enemy.enemyId;
                enemyData.health = enemy.health;
                enemyData.attack = enemy.attack;
                enemyData.defense = enemy.defense;
                enemyData.nbMoves = enemy.nbMoves;
                enemyData.speed = enemy.speed;
                enemyData.skin = enemy.skin; 
                enemyData.inPlayersRoom = false;
            }
            else
            {
                Debug.LogError("Impossible de charger les datas d'un ennemi. Vérifier son scriptable object.");
            }
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerManager>();
            grid = FindObjectOfType<Grid>();
            //walkableTilemap = grid.GetComponentInChildren<Tilemap>("Walkable");
            
        }

        private void Update()
        {
            LaunchEnemyMove();
        }



        public void PlayAnim(string animName)
        {
            _animator.Play(animName);
        }

        public void ShowDamageEffect(int damage)
        {

        }

        public bool IsAnimFinished()
        {
            return true;
        }

        /*
        public void MoveToPosition()
        {
            Vector3 playerPos = player.transform.position;
            Vector3Int endPos = walkableTilemap.WorldToCell(playerPos);    //position d'arrivée (player) en format cellule

            enemyIndex = 0;

            UpdateFarEnemiesArray();

            enemiesPathArray = new List<Spot>[farEnemiesList.Count];
            for (int i = 0; i < enemiesPathArray.Length; i++)
            {
                enemiesPathArray[i] = new List<Spot>();
            }

            //s'il y a des ennemis
            if (farEnemiesList.Count > 0)
            {
                Vector3Int playerCellPos = walkableTilemap.WorldToCell(player.transform.position);

                enemiesCloseToPlayer = new List<GameObject>();   //Init la liste des/du enemies dans la room du joueur

                foreach (GameObject enemy in farEnemiesList)
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
                            enemiesCloseToPlayer.Add(farEnemiesList[enemyIndex]);
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
                    /*
                    enemyIndex++;
                }
            }

            enemiesCanMove = true;  //on autorise les enemies à bouger
            enemyIndex = 0;
            spotIndex = 0;
        }*/

        private void LaunchEnemyMove()
        {

        }

        public bool IsMoveFinished()
        {
            return isMoveFinished;
        }
    }
}
