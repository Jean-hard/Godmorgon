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


    public int GetNbMoves()
    {
        return enemy.nbMoves;
    }

    public bool IsInPlayersRoom()
    {
        return isInPlayersRoom;
    }

    public void SetIsInPlayersRoom(bool value)
    {
        isInPlayersRoom = value;
    }
}
