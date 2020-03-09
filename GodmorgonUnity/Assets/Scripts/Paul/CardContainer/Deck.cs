using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardContainer
{
    public class Deck : CardList
    {
        // ==== Attributes

        protected new Stack<BasicCard> cards = new Stack<BasicCard>();

        // ==== Methods

        public new void AddCard(BasicCard newCard)
        {
            if (newCard != null)
                cards.Push(newCard);
            else
                throw new InconsistentCardExecption();
        }

        public new BasicCard DrawCard()
        {
            return cards.Count > 0 ? cards.Pop() : null;
        }

        // For debug only
        public new void ClearCards()
        {
            cards.Clear();
        }

        public new int Count() { return cards.Count; }
    }
}
