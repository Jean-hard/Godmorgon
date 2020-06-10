using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using GodMorgon.Models;
using GodMorgon.VisualEffect;
using GodMorgon.Sound;

namespace GodMorgon.Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [Header("Enemy View")]
        public List<Sprite> spriteList = new List<Sprite>();
        private SpriteRenderer spriteRenderer;

        [Header("Enemy Settings")]
        public Models.Enemies enemies;
        public Models.Enemy _enemy;  //Scriptable object Enemy

        [Header("Movement Settings")]
        public float moveSpeed = 5f;    //Vitesse de l'ennemi
        public AnimationCurve moveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);   //Curve liée à la vitesse de l'ennemi (pour lui donner un pattern de move)
        private List<Spot> tilesList = new List<Spot>();  //Tableau contenant la liste de tiles à parcourir
        private int tileIndex;  //Index des tiles utilisé lors du parcours de la liste des tiles
        private int nbTilesPerMove = 3;  //Nombre de tiles pour 1 move
        private bool isMoveFinished = false;
        public List<EnemyView> enemiesInRoom = new List<EnemyView>();

        private bool isAttackFinished = false;

        #region Astar Settings
        Astar astar;
        List<Spot> roadPath = new List<Spot>();
        BoundsInt bounds;
        public Vector3Int[,] walkableTilesArray;
        #endregion

        [Header("World Settings")]
        public CameraShaker shaker;     //Script du shake posé sur la cam
        public float shakeDuration = 0.1f;
        private Camera mainCamera;
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

        [Header("Visual Effect")]
        public ParticleSystemScript enemyHit;
        public GameObject spawnParticule;
        public GameObject deathParticule;

        public void Awake()
        {
            //_enemy = enemies.enemiesSOList[Random.Range(0, enemies.enemiesSOList.Count)];

            if (_enemy != null && enemyData != null)
            {
                enemyData.enemyId = _enemy.enemyId;
                enemyData.health = _enemy.health;
                enemyData.attack = _enemy.attack;
                enemyData.defense = _enemy.defense;
                enemyData.nbMoves = _enemy.nbMoves;
                enemyData.speed = _enemy.speed;
                enemyData.skin = _enemy.skin; 
                enemyData.inPlayersRoom = false;
                enemyData.inOtherEnemyRoom = false;
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
            _animator = this.transform.GetComponentInChildren<Animator>();

            _healthBar = this.GetComponentInChildren<HealthBar>();
            _healthBar.SetBarPoints(enemyData.health, enemyData.defense);

            if (this.gameObject.GetComponentInChildren<SpriteRenderer>().gameObject.name == "EnemySprite")
                spriteRenderer = this.gameObject.GetComponentInChildren<SpriteRenderer>();
            else Debug.Log("Sprite of player not found");

            mainCamera = Camera.main;
            if (mainCamera)
                shaker = mainCamera.GetComponent<CameraShaker>();

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

        
        public Vector3Int GetRoomPosition()
        {
            return TilesManager.Instance.roomTilemap.WorldToCell(this.transform.position);
        }

        public void MoveToPlayer()
        {
            tilesList = new List<Spot>();

            //position d'arrivée (player) en format cellule
            Vector3 playerPos = player.transform.position;
            Vector3Int playerCellPos = walkableTilemap.WorldToCell(playerPos);    

            //Si l'ennemi n'est pas dans la room du player
            if (!enemyData.inPlayersRoom && !enemyData.inOtherEnemyRoom)
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
                    bool isOtherEnemyOnPath = false;

                    //on ajoute les tiles par lesquelles il va devoir passer sauf celle où il y a le player ou un autre ennemi
                    if (playerCellPos.x == tile.X && playerCellPos.y == tile.Y)
                    {
                        isPlayerOnPath = true;
                        enemyData.inPlayersRoom = true; //L'ennemi sera présent dans la room
                    }

                    //On regarde si on a un autre ennemi sur le path parmis tous les ennemis existants
                    foreach(EnemyView enemy in EnemyManager.Instance.GetAllEnemies())
                    {
                        //Position cellule de l'ennemi
                        Vector3Int _enemyCellPos = walkableTilemap.WorldToCell(enemy.transform.position);

                        //On check seulement les ennemis autres que celui qu'on veut bouger (qui porte ce script)
                        if(enemy != this)
                        {
                            //Si l'ennemi est sur une tile
                            if (_enemyCellPos.x == tile.X && _enemyCellPos.y == tile.Y)
                            {
                                isOtherEnemyOnPath = true;
                                enemyData.inOtherEnemyRoom = true; //L'ennemi sera présent dans la room
                                //Debug.Log("L'ennemi " + this.name + " a un ennemi sur sa route : " + enemy.name);
                            }
                        }
                    }

                    if (!isPlayerOnPath && !isOtherEnemyOnPath)
                    {
                        tilesList.Add(tile);     
                    }

                    tileIndex++;
                }

                enemiesInRoom.Clear();  //Clear la liste d'ennemis présents dans la future room

                Vector3 targetTilePos = TilesManager.Instance.walkableTilemap.CellToWorld(new Vector3Int(roadPath[0].X, roadPath[0].Y, 0));
                Vector3Int targetRoom = TilesManager.Instance.roomTilemap.WorldToCell(targetTilePos);   //Récup la room future en prenant la position de la tile la plus éloignée du path
                
                foreach (EnemyView enemy in EnemyManager.Instance.GetAllEnemies())  //Pour chaque ennemi de la map
                {
                    if (enemy != this)  //Si c'est un autre que celui actuel
                    {
                        if(enemy.GetRoomPosition() == targetRoom)   //On check si l'autre ennemi est dans la future room
                        {
                            enemiesInRoom.Add(enemy);   //On ajoute donc l'ennemi dans la liste
                        }
                    }
                }

                tilesList.Reverse(); //on inverse la liste pour la parcourir de la tile la plus proche à la plus éloignée
                tilesList.RemoveAt(0);

                Vector3Int enemyCellPos = TilesManager.Instance.walkableTilemap.WorldToCell(this.transform.position);
                

                //On update le sprite du player en fonction de sa direction
                if (tilesList[0].Y > enemyCellPos.y)
                {
                    UpdateEnemySprite("haut_gauche");
                }
                else if (tilesList[0].X > enemyCellPos.x)
                {
                    UpdateEnemySprite("haut_droite");
                }
                else if (tilesList[0].X < enemyCellPos.x)
                {
                    UpdateEnemySprite("bas_gauche");
                }
                else if (tilesList[0].Y < enemyCellPos.y)
                {
                    UpdateEnemySprite("bas_droite");
                }

                //Affiche les coordonnées de tiles des paths que l'ennemi doit parcourir
                /*foreach(Spot spot in tilesList)
                {
                    Debug.Log(spot.X + " / " + spot.Y);
                }*/
            }

            tileIndex = 0;
            canMove = true; //On autorise le mouvement lancé par l'Update
            isMoveFinished = false; //Le bool retournera false tant que le mouvement n'est pas fini

            //SFX enemy move
            MusicManager.Instance.PlayEnemyMoving();
        }

        private void LaunchMoveMechanic()
        {
            //La prochaine position est la tile suivante
            Vector3 nextTilePos = walkableTilemap.CellToWorld(new Vector3Int(tilesList[tileIndex].X, tilesList[tileIndex].Y, 0))
                + new Vector3(0, 0.2f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange             

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

                    if(enemyData.inPlayersRoom || enemiesInRoom.Count > 0)
                    {
                        Attack();   //L'ennemi attaque s'il y a qqun dans la room d'arrivée
                    }
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

        /**
         * Joue l'anim avec tel nom
         */
        public void PlayAnim(string animName)
        {
            _animator.Play(animName);
        }

        /*
         * Renvoie un booleen true si l'anim est finie
         */
        public bool IsAnimFinished()
        {
            
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime > 1f && !stateInfo.loop)
            {
                return true;
            }

            return false;
        }

        /**
        * Actualise le sprite de l'ennemi en fonction de sa direction
        */
        public void UpdateEnemySprite(string direction)
        {
            switch (direction)
            {
                case "haut_gauche":
                    spriteRenderer.sprite = spriteList[0];
                    break;
                case "haut_droite":
                    spriteRenderer.sprite = spriteList[1];
                    break;
                case "bas_gauche":
                    spriteRenderer.sprite = spriteList[2];
                    break;
                case "bas_droite":
                    spriteRenderer.sprite = spriteList[3];
                    break;
            }
        }

        /**
         * Update the healthBar
         */
        public void UpdateHealthBar(float health, float defense)
        {
            _healthBar.UpdateHealthBar(defense, health);
        }

        /**
         * Launch hit visual effect on enemy
         */
        public void OnDamage()
        {
            enemyHit.launchParticle();  //Lance la particule de hit
            //SFX enemy hit
            MusicManager.Instance.PlayEnemyHit();
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
            //ShowAttackEffect(); //Décommenter qd on aura l'anim d'attaque
            StartCoroutine(AttackEffect());

            if(enemyData.inPlayersRoom) //Si le player est dans la room
                PlayerManager.Instance.TakeDamage(enemyData.attack);
            if(enemiesInRoom.Count > 0) //S'il y a des ennemis dans la room
            {
                foreach(EnemyView enemy in enemiesInRoom)
                {
                    enemy.enemyData.TakeDamage(enemyData.attack, false);
                }
            }
            //prend des dégats si le counter est activé
            enemyData.TakeDamage(PlayerManager.Instance.Counter(), false);
        }

        /**
         * Affiche les effets d'une attaque
         */
        public void ShowAttackEffect()
        {
            //_animator.SetTrigger("LaunchAttack");
            Animation anim = this.transform.GetComponentInChildren<Animation>();
            
            foreach (AnimationState state in anim)
            {
                if (state.name == "Enemy_Attack")
                {
                    anim.Play(state.name);
                    
                    //Debug.Log("anim played");
                }
            }
        }

        /**
         * Anim d'attaque
         */
        public IEnumerator AttackEffect()
        {
            shaker.Shake(shakeDuration);
            yield return new WaitForSeconds(1f);
            isAttackFinished = true;
        }

        public bool IsAttackFinished()
        {
            /*
            if (IsAnimFinished())
            {
                isAttackFinished = true;
            }
            else isAttackFinished = false;
            
            return isAttackFinished;*/

            if (isAttackFinished)
            {
                isAttackFinished = false;
                return true;
            }
            
            return false;
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
                //Debug.Log(tile.X + " : " + tile.Y);
            }

            tilesList.Reverse();
            tilesList.RemoveAt(0);
            
            
            canRecenter = true;
            enemyData.inPlayersRoom = false;
        }

        private void LaunchRecenterMechanic()
        {
            Vector3 nextTilePos = walkableTilemap.CellToWorld(new Vector3Int(tilesList[tileIndex].X, tilesList[tileIndex].Y, 0))
                + new Vector3(0, 0.2f, 0);   //on ajoute 0.4 pour que l'enemy passe bien au milieu de la tile, la position de la tile étant en bas du losange

            float speed = 0.5f;
            transform.position = Vector2.MoveTowards(transform.position, nextTilePos, speed * Time.deltaTime);   //on avance jusqu'à la prochaine tile
        
            if(transform.position == nextTilePos)
            {
                Debug.Log("Enemy recentered");
                canRecenter = false;
                enemyData.inPlayersRoom = false;
            }
        }

        /**
         * Tue un ennemi donné après une durée correspondant à la durée de la particule du hit
         */
        public void KillEnemy(float hitAnimDuration)
        {
            StartCoroutine(TimedDeath(hitAnimDuration));
            EnemyManager.Instance.UpdateEnemiesList();    //Update la liste des ennemis sur la map
            PlayerManager.Instance.AddGold(15); //Add gold to player

            //SFX enemy death
            MusicManager.Instance.PlayEnemyDeath();
        }

        IEnumerator TimedDeath(float duration)
        {
            yield return new WaitForSeconds(duration);   //On attend que la particule de hit soit terminée


            foreach(Transform child in this.transform)
            {
                if(child.gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    child.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }
            }

            Instantiate(deathParticule, this.transform.position, Quaternion.identity, EnemyManager.Instance.effectParent);

            if (enemyData.killedByPlayer)
            {
                yield return new WaitForSeconds(1f);
                GameManager.Instance.DraftPanelActivation(true);
                Debug.Log("Tué directement par le player, donc lance le draft");
            }

            

            Destroy(this.gameObject);    //Détruit le gameobject de l'ennemi
        }
    }
}
