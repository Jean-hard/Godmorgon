using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Models;

namespace GodMorgon.Enemy
{
    public class EnemyDisplay : MonoBehaviour
    {
        public Models.Enemy enemy;  //Scriptable object Enemy

        [System.NonSerialized]
        public int enemyId;
        [System.NonSerialized]
        public Sprite enemySprite;

        //public bool isInPlayersRoom;

        public EnemyData enemyData;

        public void Awake()
        {
            if (enemy != null)
            {
                enemyData.name = enemy.name;
                enemyData.health = enemy.health;
                enemyData.attack = enemy.attack;
                enemyData.defense = enemy.defense;
                enemyData.nbMoves = enemy.nbMoves;
                enemyData.skin = enemy.skin;
                enemyData.inPlayersRoom = false;
            }
        }

        public void Start()
        {
            enemyId = enemy.id;
        }

        //Récupére le nombre de room que l'enemy peut parcourir en une fois
        public int GetNbMoves()
        {
            return enemy.nbMoves;
        }

        //Retourne true si l'enemy est dans la room du player
        public bool IsInPlayersRoom()
        {
            return enemyData.inPlayersRoom;
        }

        //Set le bool de la présence de l'enemy dans la room du player
        public void SetInPlayersRoomBool(bool value)
        {
            enemyData.inPlayersRoom = value;
        }
    }
}
