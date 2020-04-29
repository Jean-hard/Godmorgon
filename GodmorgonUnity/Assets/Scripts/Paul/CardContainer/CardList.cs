using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardContainer
{
    /**
     * abstract class for all type of list of card use during the game
     */
    public abstract class CardList : ICardContainer 
    {
        //list of cards contains in this object.
        protected List<BasicCard> cards = new List<BasicCard>();

        //add a card to list.
        public void AddCard(BasicCard newCard)
        {
            if (newCard != null)
            {
                cards.Add(newCard);
            }
            else
                Debug.Log("carte à ajouter inexistante ---- GO ENVOYER UNE EXECPTION !");
                //throw new InconsistentCardException();
        }

        //Draw a card randomly from the list.
        public BasicCard DrawCard()
        {
            if (cards.Count > 0)
            {
                BasicCard toRemove = cards[Random.Range(0, cards.Count)];
                cards.Remove(toRemove);
                return toRemove;
            }
            return null;
        }

        //Draw one precise card from the list.
        public BasicCard DrawCard(BasicCard toDraw)
        {
            //Debug.Log("Drawing: " + toDraw.ToString());
            if (cards.Contains(toDraw))
            {
                //Debug.Log("    Found!");
                cards.Remove(toDraw);
                return toDraw;
            }
            return null;
        }

        /**
         * For debug only.
         * Clear all the card from the list.
         */
        public void ClearCards()
        {
            cards.Clear();
        }

        //Count all the card in the list.
        public int Count()
        {
            return cards.Count;
        }

        //Get the cards list
        public List<BasicCard> GetCards()
        {
            return cards;
        }
    }
}