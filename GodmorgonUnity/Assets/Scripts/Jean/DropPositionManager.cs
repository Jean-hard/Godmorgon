using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
    public class DropPositionManager
    {
        public GameContext GetDropCardContext(BasicCard droppedCard, Vector3Int dropPosition)
        {
            //will contain all the information needed for the require effect
            GameContext context = new GameContext();

            switch (droppedCard.cardType)
            {
                case BasicCard.CARDTYPE.MOVE:
                    PlayerManager.Instance.UpdateAccessibleTilesList();
                    if (PlayerManager.Instance.accessibleTiles.Contains(dropPosition))
                        context.isDropValidate = true;
                    break;
                case BasicCard.CARDTYPE.ATTACK:
                    break;
                case BasicCard.CARDTYPE.DEFENSE:
                    break;
            }

            return context;
        }

        public void ShowAvailableTilesToDrop(BasicCard draggedCard)
        {
            switch (draggedCard.cardType)
            {
                case BasicCard.CARDTYPE.MOVE:
                    PlayerManager.Instance.ShowAccessibleTiles();
                    break;
                case BasicCard.CARDTYPE.ATTACK:
                    break;
                case BasicCard.CARDTYPE.DEFENSE:
                    break;
            }
        }
    }
}