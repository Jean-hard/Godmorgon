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
        
        [Header("Movement Settings")]
        public float enemySpeed = 1f;
        public AnimationCurve enemyMoveCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);   //Curve liée à la vitesse des ennemies (pour leur donner un pattern de move)

        [Header("Attack Settings")]
        public float attackDuration = 3f; 

        private List<GameObject> farEnemiesList;  //Tableau des ennemis présents sur la map mais hors de la room du player
        public Vector3Int[,] spots;
        private bool enemiesHaveMoved = false;

        private List<EnemyView> enemiesList;


        #region Singleton Pattern
        private static EnemyManager _instance;

        public static EnemyManager Instance { get { return _instance; } }
        #endregion

        /*
        public EnemyView GetEnemyView(string id)
        {
            foreach (EnemyView enemyView in farEnemiesList)
            {
                if (enemyView.enemyId == id)
                {
                    return enemyView;
                }
            }

            return null;
        }*/

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
         * Mets dans un tableau SEULEMENT les enemies enfant du gameobject EnemyManager HORS DE LA ROOM DU PLAYER
         */
        private void UpdateFarEnemiesArray()
        {
            farEnemiesList = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                //s'il n'est pas dans la room du player, on ne l'ajoute pas dans le tableau des enemies
                if (transform.GetChild(i).gameObject.GetComponent<EnemyDisplay>().IsInPlayersRoom() == false)
                    farEnemiesList.Add(transform.GetChild(i).gameObject);
            }
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


        public void MoveEnemies()
        {
            enemiesHaveMoved = false;

            UpdateEnemiesArray();
            Debug.Log("Enemy child count : " + enemiesList.Count);
            StartCoroutine(TimedEnemyMove());
        }

        IEnumerator TimedEnemyMove()
        {
            foreach (EnemyView enemy in enemiesList)
            {
                Debug.Log("Launch MoveEnemy()");
                enemy.MoveEnemy();
                while (!enemy.IsMoveFinished())
                {
                    yield return null;
                }
            }

            enemiesHaveMoved = true;
        }

        /**
         * Lance l'attaque des ennemis
         */
        public void Attack()
        {
            UpdateEnemiesArray();

            //StartCoroutine(TimedAttacks(attackDuration));
        }

        /**
         * Applique les effets de l'attaque tous les [attackDuration] secondes
         */
         /*
        IEnumerator TimedAttacks(float attackDuration)
        {
            foreach (GameObject enemy in enemiesList)
            {
                //Si l'ennemi est dans la room du player
                if (enemy.GetComponent<EnemyDisplay>().IsInPlayersRoom())
                {
                    //Lance anim d'attack

                    //Fait des damages au player
                    PlayerManager.Instance.HitPlayer(3);
                }
                
                yield return new WaitForSeconds(attackDuration);    //Attend X temps pour passer à l'ennemi suivant
            }
        }*/
    }
}
