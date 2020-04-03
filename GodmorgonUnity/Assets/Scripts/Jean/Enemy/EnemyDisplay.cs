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

        public bool inPlayersRoom;

        public EnemyData enemyData = new EnemyData();

        public void Awake()
        {
            if (enemy != null && enemyData != null)
            {
                //enemyData.id = enemy.id;
                enemyData.health = enemy.health;
                enemyData.attack = enemy.attack;
                enemyData.defense = enemy.defense;
                enemyData.nbMoves = enemy.nbMoves;
                enemyData.skin = enemy.skin;
                
                inPlayersRoom = false;
            }
        }

        //Récupére le nombre de room que l'enemy peut parcourir en une fois
        public int GetNbMoves()
        {
            return enemy.nbMoves;
        }

        //Retourne true si l'enemy est dans la room du player
        public bool IsInPlayersRoom()
        {
            return inPlayersRoom;
        }

        //Set le bool de la présence de l'enemy dans la room du player
        public void SetInPlayersRoomBool(bool value)
        {
            inPlayersRoom = value;
        }
    }
}
