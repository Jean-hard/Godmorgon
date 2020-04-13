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
        private List<EnemyView> farEnemiesList;  //Tableau des ennemis présents sur la map mais hors de la room du player
        private bool enemiesHaveMoved = false;
        private bool enemiesHaveAttacked = false;


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
        private void UpdateEnemiesArray()
        {
            enemiesList = new List<EnemyView>();
            for (int i = 0; i < transform.childCount; i++)
            {
                enemiesList.Add(transform.GetChild(i).gameObject.GetComponent<EnemyView>()); 
            }
        }

        /**
         * Mets dans un tableau SEULEMENT les ennemis enfant du gameobject EnemyManager HORS DE LA ROOM DU PLAYER
         */
        private void UpdateFarEnemiesArray()
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
            foreach (EnemyView enemyView in enemiesList)
            {
                if (enemyView.enemyData.enemyId == id)
                {
                    return enemyView;
                }
            }

            return null;
        }


        /**
         * Lance le mouvement des ennemis
         */
        public void MoveEnemies()
        {
            enemiesHaveMoved = false;

            UpdateEnemiesArray();   //Recup les ennemis présents sur la map
            StartCoroutine(TimedEnemiesMove());   //Lance la coroutine qui applique un par un le mouvement de chaque ennemi
        }

        /**
         * Permet de lancer le move des ennemis l'un après l'autre
         */
        IEnumerator TimedEnemiesMove()
        {
            foreach (EnemyView enemy in enemiesList)    //Pour chaque ennemi de la liste
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
         * Lance l'attaque des ennemis
         */
        public void Attack()
        {
            UpdateEnemiesArray();
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
