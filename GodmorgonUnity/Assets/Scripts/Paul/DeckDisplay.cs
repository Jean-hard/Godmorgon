using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GodMorgon.Models;

public class DeckDisplay : MonoBehaviour
{
    public DeckContent deck;

    public int deckId;

    public Image artwork;

    public Text nameText;
    public Text descriptionText;

    //Load the data of the deck in the gameObject at start, if the deck exist.
    void Start()
    {
        if (deck)
        {
            nameText.text = deck.name;
            //descriptionText.text = deck.description;
            deckId = deck.id;
        }
    }

    /**
     * update the deck gameObject using the deck data
     */
    public void UpdateDeck(DeckContent deckData)
    {
        //Save the scriptableObject use by this deck gameObject
        deck = deckData;
        deckId = deck.id;

        nameText.text = deckData.name;
        //descriptionText.text = deckData.description;

        if (deckData.cards.Count != 0)
             deck.cards = deckData.cards;
        if (deckData.artwork)
            deck.artwork = deckData.artwork;
    }

}

