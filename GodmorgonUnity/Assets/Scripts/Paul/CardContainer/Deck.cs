using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardContainer
{
    /**
     * object containing a stack of card to be use as the deck of the player
     */
    public class Deck : CardList
    {
        // ==== Attributes

        protected new Stack<BasicCard> cards = new Stack<BasicCard>();

        // ==== Methods

        //add a card on top of the stack
        public new void AddCard(BasicCard newCard)
        {
            if (newCard != null)
            {
                cards.Push(newCard);
            }
            else
                throw new InconsistentCardExecption();
        }

        //draw the card on top of the stack
        public new BasicCard DrawCard()
        {
            return cards.Count > 0 ? cards.Pop() : null;
        }

        // For debug only
        public new void ClearCards()
        {
            cards.Clear();
        }

        //Return the count in the Stack
        public new int Count() { return cards.Count; }
    }
}
