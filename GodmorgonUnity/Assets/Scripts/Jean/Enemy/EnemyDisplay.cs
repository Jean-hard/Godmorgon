using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Models;
public class EnemyDisplay : MonoBehaviour
{
    public Enemy enemy;
    
    public int enemyId;

    public Sprite enemySprite;

    public bool isInPlayersRoom;

    public void Start()
    {
        enemyId = enemy.id;
    }

    //Méthode récupérant le nombre de room que l'enemy peut parcourir en une fois
    public int GetNbMoves()
    {
        return enemy.nbMoves;
    }


    public bool GetIsInPlayersRoom()
    {
        return isInPlayersRoom;
    }

    public void SetIsInPlayersRoom(bool value)
    {
        isInPlayersRoom = value;
    }
}
