using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.Enemy;

namespace GodMorgon.CardEffect
{
    public class DropPositionManager
    {
        public GameContext GetDropCardContext(BasicCard droppedCard, Vector3Int dropPosition, GameContext context)
        {
            //will contain all the information needed for the require effect
            //GameContext context = new GameContext();

            switch (droppedCard.cardType)
            {
                case BasicCard.CARDTYPE.MOVE:
                    //PlayerManager.Instance.UpdateAccessibleTilesList(droppedCard.effectsData[0].nbMoves);
                    if (PlayerManager.Instance.accessibleTiles.Contains(dropPosition))
                        context.isDropValidate = true;
                    break;
                case BasicCard.CARDTYPE.ATTACK:
                    if (EnemyManager.Instance.attackableEnemiesTiles.Contains(dropPosition))
                    {
                        context.targets = EnemyManager.Instance.GetEnemyViewByPosition(dropPosition).enemyData;
                        context.isDropValidate = true;
                    }
                    break;
                case BasicCard.CARDTYPE.DEFENSE:
                    if (PlayerManager.Instance.GetPlayerPosition() == dropPosition)
                    {
                        context.isDropValidate = true;
                    }
                    break;
                case BasicCard.CARDTYPE.POWER_UP:
                    if (PlayerManager.Instance.GetPlayerPosition() == dropPosition)
                    {
                        context.isDropValidate = true;
                    }
                    break;
                case BasicCard.CARDTYPE.SPELL:
                    if (PlayerManager.Instance.GetPlayerPosition() == dropPosition)
                    {
                        context.isDropValidate = true;
                        Debug.Log("SPELL");
                    }
                    break;
            }

            return context;
        }

        public void ShowPositionsToDrop(BasicCard draggedCard)
        {
            switch (draggedCard.cardType)
            {
                case BasicCard.CARDTYPE.MOVE:
                    PlayerManager.Instance.UpdateAccessibleTilesList(draggedCard.effectsData[0].nbMoves);
                    PlayerManager.Instance.ShowAccessibleTiles();
                    break;
                case BasicCard.CARDTYPE.ATTACK:
                    Debug.Log("Show positions for attack");
                    EnemyManager.Instance.ShowAttackableEnemies();
                    break;
                case BasicCard.CARDTYPE.DEFENSE:
                    break;
            }
        }

        public void HidePositionsToDrop(BasicCard draggedCard)
        {
            switch (draggedCard.cardType)
            {
                case BasicCard.CARDTYPE.MOVE:
                    PlayerManager.Instance.HideAccessibleTiles();
                    break;
                case BasicCard.CARDTYPE.ATTACK:
                    EnemyManager.Instance.HideAttackableEnemies();
                    break;
                case BasicCard.CARDTYPE.DEFENSE:
                    break;
            }
        }
    }
}