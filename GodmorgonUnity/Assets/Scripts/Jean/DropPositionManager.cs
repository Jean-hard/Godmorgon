using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

public class DropPositionManager
{
    public bool CheckDroppedCardPosition(BasicCard droppedCard, Vector3Int dropPosition)
    {
        switch (droppedCard.cardType)
        {
            case BasicCard.CARDTYPE.MOVE:
                PlayerManager.Instance.UpdateAccessibleTilesList();
                if (PlayerManager.Instance.accessibleTiles.Contains(dropPosition))
                    return true;
                break;
            case BasicCard.CARDTYPE.ATTACK:
                break;
            case BasicCard.CARDTYPE.DEFENSE:
                break;
        }

        return false;
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
