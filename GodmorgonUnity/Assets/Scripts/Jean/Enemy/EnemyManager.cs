using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using GodMorgon.Models;

namespace GodMorgon.Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [Header("General Settings")]
        public Grid grid;
        public Tilemap walkableTilemap;
        public Tilemap roadMap;
        public TileBase roadTile;
        public GameObject player;


        private List<EnemyView> enemiesList;
        private List<EnemyView> enemiesInPlayersRoom = new List<EnemyView>();
        [System.NonSerialized]
        public List<Vector3Int> attackableEnemiesTiles = new List<Vector3Int>();
        private List<EnemyView> farEnemiesList;  //Tableau des ennemis présents sur la map mais hors de la room du player
        private bool enemiesHaveMoved = false;
        //private bool enemiesHaveAttacked = false;


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

        /**
         * Mets dans un tableau TOUS les enemies enfant du gameobject EnemyManager sur la map
         */
        private void UpdateEnemiesList()
        {
            enemiesList = new List<EnemyView>();
            for (int i = 0; i < transform.childCount; i++)
            {
                enemiesList.Add(transform.GetChild(i).gameObject.GetComponent<EnemyView>()); 
            }
        }

        /**
         * Mets dans une liste SEULEMENT les ennemis enfant du gameobject EnemyManager HORS DE LA ROOM DU PLAYER
         */
        private void UpdateFarEnemiesList()
        {
            farEnemiesList = new List<EnemyView>();
            for (int i = 0; i < transform.childCount; i++)
            {
                //s'il n'est pas dans la room du player, on ne l'ajoute pas dans la liste des ennemis
                if (transform.GetChild(i).gameObject.GetComponent<EnemyView>().enemyData.inPlayersRoom == false)
                    farEnemiesList.Add(transform.GetChild(i).gameObject.GetComponent<EnemyView>());
            }
        }

        /**
         * Retourne un ennemi par son id
         */
        public EnemyView GetEnemyView(string id)
        {
            UpdateEnemiesList();

            if (enemiesList.Count > 0)
            {
                foreach (EnemyView enemyView in enemiesList)
                {
                    if (enemyView.enemyData.enemyId == id)
                    {
                        return enemyView;
                    }
                }
            }

            return null;
        }

        /**
         * Retourne un ennemi par sa position cellule
         */
        public EnemyView GetEnemyViewByPosition(Vector3Int tilePosition)
        {
            UpdateEnemiesList();

            if (enemiesList.Count > 0)
            {
                foreach (EnemyView enemyView in enemiesList)
                {
                    Vector3Int enemyViewCellPos = walkableTilemap.WorldToCell(enemyView.transform.position);
                    if (enemyViewCellPos == tilePosition)
                    {
                        return enemyView;
                    }
                }
            }

            return null;
        }

        /**
         * Renvoie la liste des ennemis présents dans la room du player
         * Met à jour en même temps la listes des tiles sur lesquelles sont les ennemis présents dans la room du player
         */
        public List<EnemyView> GetEnemiesInPlayersRoom()
        {
            UpdateEnemiesList();

            List<EnemyView> attackableEnemies = new List<EnemyView>();
            attackableEnemiesTiles.Clear();
            enemiesInPlayersRoom.Clear();

            foreach (EnemyView enemy in enemiesList)
            {
                if(enemy.enemyData.inPlayersRoom)
                {
                    attackableEnemies.Add(enemy);

                    Vector3Int enemyCellPos = walkableTilemap.WorldToCell(enemy.transform.position);
                    attackableEnemiesTiles.Add(enemyCellPos);
                }
            }
            return attackableEnemies;
        }

        /**
        * Montre les ennemis attaquables en activant l'effet (cible) enfant du gameObject de l'ennemi
        */
        public void ShowAttackableEnemies()
        {
            enemiesInPlayersRoom = GetEnemiesInPlayersRoom();
            foreach (EnemyView enemy in enemiesInPlayersRoom)
            {
                Debug.Log("Active target of " + enemy.enemyData.enemyId);
                enemy.transform.Find("Target").gameObject.SetActive(true);
            }
        }

        /**
        * Désactive l'effet (cible) enfant du gameObject de l'ennemi
        */
        public void HideAttackableEnemies()
        {
            enemiesInPlayersRoom = GetEnemiesInPlayersRoom();

            foreach (EnemyView enemy in enemiesInPlayersRoom)
            {
                enemy.transform.Find("Target").gameObject.SetActive(false);
            }
        }

        /**
         * Lance le mouvement des ennemis
         */
        public void MoveEnemies()
        {
            enemiesHaveMoved = false;

            UpdateFarEnemiesList();   //Recup les ennemis présents sur la map
            StartCoroutine(TimedEnemiesMove());   //Lance la coroutine qui applique un par un le mouvement de chaque ennemi
        }

        /**
         * Permet de lancer le move des ennemis loin du player, l'un après l'autre
         */
        IEnumerator TimedEnemiesMove()
        {
            foreach (EnemyView enemy in farEnemiesList)    //Pour chaque ennemi de la liste
            {
                enemy.MoveToPlayer();  //On lance le mouvement de l'ennemi
                while (!enemy.IsMoveFinished()) //Tant qu'il n'ont pas tous bougé on continue
                {
                    yield return null;
                }
            }

            enemiesHaveMoved = true;
        }

        /**
         * Renvoie true si le mouvement des ennemis est terminé
         */
        public bool EnemiesMoveDone()
        {
            if (enemiesHaveMoved)
                return true;
            else
                return false;
        }

        /**
         * Prend un ennemi présent dans la room du player et le recentre au milieu de la room
         * Cela peut arriver si le joueur fuit une room avec des ennemis dedans
         */
        public void RecenterEnemies()
        {
            enemiesInPlayersRoom = GetEnemiesInPlayersRoom();

            if (enemiesInPlayersRoom.Count > 0)
            {
                Debug.Log("Un ennemi doit se recentrer");
                enemiesInPlayersRoom[0].RecenterEnemy();    //L'ennemi se recentre en avançant d'une case vers le player
            }
        }

        /**
         * Lance l'attaque des ennemis
         */
        public void Attack()
        {
            UpdateEnemiesList();
            StartCoroutine(TimedAttacks());
        }        

        /**
         * Applique les effets de l'attaque tous les [attackDuration] secondes
         */
        
       IEnumerator TimedAttacks()
       {
           foreach (EnemyView enemy in enemiesList)
           {
               //Si l'ennemi est dans la room du player
               if (enemy.enemyData.inPlayersRoom)
               {
                   //Lance anim d'attack
                   enemy.Attack();
               }
               while (!enemy.IsAttackFinished()) //Tant qu'il n'ont pas tous bougé on continue
               {
                   yield return null;
               }

           }
       }
    }
}
