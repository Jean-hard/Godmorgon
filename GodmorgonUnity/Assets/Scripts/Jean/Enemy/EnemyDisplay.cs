using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Models;

namespace GodMorgon.Enemy
{
    public class EnemyDisplay : MonoBehaviour
    {
        public Models.Enemy enemy;

        public int enemyId;

        public Sprite enemySprite;

        public bool isInPlayersRoom;

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
        public bool GetIsInPlayersRoom()
        {
            return isInPlayersRoom;
        }

        //Set le bool de la présence de l'enemy dans la room du player
        public void SetIsInPlayersRoom(bool value)
        {
            isInPlayersRoom = value;
        }
    }
}
