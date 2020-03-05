using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardContainer
{
    public interface ICardContainer
    {
        // ==== Methods
        void AddCard(BasicCard newCard);
        BasicCard DrawCard();

        // For debug only
        void ClearCards();
        int Count();
    }
}
