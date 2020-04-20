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

        [Header("Movement Settings")]
        public float moveSpeed = 5f;    //Vitesse de l'ennemi
        public AnimationCurve moveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);   //Curve liée à la vitesse de l'ennemi (pour lui donner un pattern de move)
        private List<Spot> tilesList = new List<Spot>();  //Tableau contenant la liste de tiles à parcourir
        private int tileIndex;  //Index des tiles utilisé lors du parcours de la liste des tiles
        private int nbTilesPerMove = 3;  //Nombre de tiles pour 1 move
        private bool isMoveFinished = false;

        private bool isAttackFinished = false;

        #region Astar Settings
        Astar astar;
        List<Spot> roadPath = new List<Spot>();
        BoundsInt bounds;
        public Vector3Int[,] walkableTilesArray;
        #endregion

        [Header("World Settings")]
        private Grid grid;   //Grid du jeu
        private Tilemap walkableTilemap;    //Tilemap contenant tous les chemins de tiles walkable
        //private Tilemap roadMap;

        private PlayerManager player;
        private Animator _animator;
        private HealthBar _healthBar;
        public EnemyData enemyData = new EnemyData();
        private bool canMove;
        [System.NonSerialized]
        public bool canRecenter = false;

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
                enemyData.enemyView = this;
            }
            else
            {
                Debug.LogError("Impossible de charger les datas d'un ennemi. Vérifier son scriptable object.");
            }
        }

        private void Start()
        {
            player = FindObjectOfType<PlayerManager>();
            grid = EnemyManager.Instance.grid;
            walkableTilemap = EnemyManager.Instance.walkableTilemap;
            _animator = this.GetComponent<Animator>();
            _healthBar = this.GetComponentInChildren<HealthBar>();

            #region Astar Start Setup
            walkableTilemap.CompressBounds();   // réduit la taille de la tilemap à là où des tiles existent
            //roadMap.CompressBounds();
            bounds = walkableTilemap.cellBounds;
            CreateGrid();
            astar = new Astar(walkableTilesArray, bounds.size.x, bounds.size.y);
            #endregion
        }

        private void Update()
        {
            if (canMove)
                LaunchMoveMechanic();

            if (canRecenter)
                LaunchRecenterMechanic();

            // ============ WIP ================== 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(20);
                ShowDamageEffect();
            }
            // ===================================
        }

        #region CreateGrid --> Astar
        /**
         * Ajoute dans un tableau de Vector3 les positions des tiles de la tilemap Walkable
         */
        public void CreateGrid()
        {
            walkableTilesArray = new Vector3Int[bounds.size.x, bounds.size.y];
            for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
            {
                for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
                {
                    if (walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        walkableTilesArray[i, j] = new Vector3Int(x, y, 0);
                    }
                    else
                    {
                        walkableTilesArray[i, j] = new Vector3Int(x, y, 1);
                    }
                }
            }
        }
        #endregion

        
        public void MoveToPlayer()
        {
            tilesList = new List<Spot>();

            //position d'arrivée (player) en format cellule
            Vector3 playerPos = player.transform.position;
            Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);    

            //Si l'ennemi n'est pas dans la room du player
            if (!enemyData.inPlayersRoom)
            {
                tileIndex = 0;

                //Position cellule de l'enemy
                Vector3Int enemyPos = walkableTilemap.WorldToCell(transform.position);
                
                if (roadPath != null && roadPath.Count > 0) //reset le roadpath
                    roadPath.Clear();

                //création du path, prenant en paramètre la position des tiles, le point de départ, le point d'arrivée, et la longueur en tiles du path -> dépend de l'ennemi
                //roadPath est une liste de spots = une liste de positions de tiles
                roadPath = astar.CreatePath(walkableTilesArray, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(playerCellPos.x, playerCellPos.y), nbTilesPerMove * enemyData.nbMoves);

                if (roadPath == null)
                {
                    return;
                }

                //On ajoute les tiles à parcourir dans une liste, en vérifiant si le joueur est sur le path
                foreach (Spot tile in roadPath)
                {
                    bool isPlayerOnPath = false;

                    //on ajoute les tiles par lesquelles il va devoir passer sauf celle où il y a le player
                    if (playerCellPos.x == tile.X && playerCellPos.y == tile.Y)
                    {
                        isPlayerOnPath = true;
                        enemyData.inPlayersRoom = true; //L'ennemi sera présent dans la room
                    }
                    if (!isPlayerOnPath)
                    {
                        tilesList.Add(tile);     
                    }

                    tileIndex++;
                }

                tilesList.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
                tilesList.RemoveAt(0);

                //Affiche les coordonnées de tiles des paths que l'ennemi doit parcourir
                /*foreach(Spot spot in tilesList)
                {
                    Debug.Log(spot.X + " / " + spot.Y);
                }*/
            }

            tileIndex = 0;
            canMove = true; //On autorise le mouvement lancé par l'Update
            isMoveFinished = false; //Le bool retournera false tant que le mouvement n'est pas fini
        }

        private void LaunchMoveMechanic()
        {
            //La prochaine position est la tile suivante
            Vector3 nextTilePos = walkableTilemap.CellToWorld(new Vector3Int(tilesList[tileIndex].X, tilesList[tileIndex].Y, 0))
                + new Vector3(0, 0.4f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange             

            //Si on atteint la tile
            if (Vector3.Distance(transform.position, nextTilePos) < 0.001f)
            {
                //Si on arrive à la tile finale où l'ennemi peut se rendre
                if (tileIndex == tilesList.Count - 1)
                {
                    tileIndex = 0;

                    canRecenter = false;
                    canMove = false;    //On arrête d'autoriser le mouvement
                    isMoveFinished = true;  //Le mouvement est terminé (pour la fonction qui retourne ce bool)
                    tilesList = new List<Spot>();   //Reset de la liste
                }
                //Sinon, s'il reste des tiles à parcourir
                else if (tileIndex < tilesList.Count - 1)
                {
                    tileIndex++;    //On passe à la tile suivante
                }
            }
            else
            {
                float ratio = (float)tileIndex / (tilesList.Count - 1);   //ratio varie entre 0 et 1, 0 pour le spot le plus proche et 1 pour le spot final
                ratio = moveCurve.Evaluate(ratio);     //on le lie à notre curve pour le modifier dans l'inspector à notre guise
                float speed = moveSpeed * ratio;   //on le lie à la vitesse pour que la curve ait un impact sur la vitesse de l'enemy
                transform.position = Vector2.MoveTowards(transform.position, nextTilePos, speed * Time.deltaTime);   //on avance jusqu'à la prochaine tile
            }
        }

        public bool IsMoveFinished()
        {
            return isMoveFinished;
        }

        public void PlayAnim(string animName)
        {
            _animator.Play(animName);
        }

        public bool IsAnimFinished(string animName)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                return true;
            }

            return false;
        }

        public void TakeDamage(int damage)
        {
            enemyData.health -= damage; //On applique les damages à la vie
            
            if (enemyData.health < 0)   //Empêche que la vie soit inférieur à 0
                enemyData.health = 0;
        }

        /**
         * Affiche les effets lors de damage
         */
        public void ShowDamageEffect()
        {
            _healthBar.SetHealth(enemyData.health);
        }

        /**
         * Lance l'attaque (effet visuel + calcul)
         */
        public void Attack()
        {
            if(enemyData.inPlayersRoom)
            {
                ShowAttackEffect();
                PlayerManager.Instance.TakeDamage(enemyData.attack);
            }
        }

        /**
         * Affiche les effets d'une attaque
         */
        public void ShowAttackEffect()
        {
            PlayAnim("Attack");
        }

        public bool IsAttackFinished()
        {
            if (IsAnimFinished("Attack"))
                isAttackFinished = true;
            else isAttackFinished = false;
            
            return isAttackFinished;
        }

        /**
         * Replace l'ennemi au milieu de la room où il est
         * Lancé si le player fuit une room où l'enemy était
         */
        public void RecenterEnemy()
        {
            //position d'arrivée (player) en format cellule
            Vector3 playerPos = player.transform.position;
            Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);
            
            //Position cellule de l'enemy
            Vector3Int enemyPos = walkableTilemap.WorldToCell(transform.position);

            if (roadPath != null && roadPath.Count > 0) //reset le roadpath
                roadPath.Clear();

            //création du path
            roadPath = astar.CreatePath(walkableTilesArray, new Vector2Int(enemyPos.x, enemyPos.y), new Vector2Int(playerCellPos.x, playerCellPos.y), 1);

            if (roadPath == null)
            {
                return;
            }

            foreach (Spot tile in roadPath)
            {
                tilesList.Add(tile);
                Debug.Log(tile.X + " : " + tile.Y);
            }

            tilesList.Reverse();
            tilesList.RemoveAt(0);
            
            
            canRecenter = true;
            enemyData.inPlayersRoom = false;
        }

        private void LaunchRecenterMechanic()
        {
            Vector3 nextTilePos = walkableTilemap.CellToWorld(new Vector3Int(tilesList[tileIndex].X, tilesList[tileIndex].Y, 0))
                + new Vector3(0, 0.4f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange

            float speed = 0.5f;
            transform.position = Vector2.MoveTowards(transform.position, nextTilePos, speed * Time.deltaTime);   //on avance jusqu'à la prochaine tile
        
            if(transform.position == nextTilePos)
            {
                Debug.Log("Enemy recenterd");
                canRecenter = false;
                enemyData.inPlayersRoom = false;
            }
        }
    }
}
