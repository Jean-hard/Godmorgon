﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;
using GodMorgon.CardContainer;

/**
 * class that hold all the gameObject card in the hand In-Game
 */
public class HandManager : MonoBehaviour
{
    //card prefab
    [SerializeField]
    private GameObject cardDisplayPrefab;

    [SerializeField]
    private float cardWidth = 175f;
    [SerializeField]
    private float cardHeight = 300f;

    //list of CardDisplay in the hand
    private List<CardDisplay> CardDisplayList = new List<CardDisplay>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /**
     * Create a card gameObject in the hand and add it to the list
     * Use when a card is draw
     */
    public void AddCard(BasicCard cardDraw)
    {
        CardDisplay cardDisplay = Instantiate(cardDisplayPrefab, this.transform).GetComponent<CardDisplay>();

        //Set the display of the card
        cardDisplay.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
        cardDisplay.UpdateCard(cardDraw);
        CardDisplayList.Add(cardDisplay);
    }

    /**
     * Update the gameObject in the hand
     * Modify, create or delete the card according to the list from the GameEngine
     */
    public void HandUpdate()
    {
        int i = 0;

        for (i = 0; i < GameEngine.Instance.GetHandCards().Count; i++)
        {
            //if there is missing card, we create it
            if(CardDisplayList[i] == null)
            {
                //----mettre les bonnes dimensions plus tard !
                CardDisplay cardDisplay = Instantiate(cardDisplayPrefab, this.transform).GetComponent<CardDisplay>();

                cardDisplay.UpdateCard(GameEngine.Instance.GetHandCards()[i]);
            }
            //if the cardDisplayed is different we modify it
            else if(CardDisplayList[i].card != GameEngine.Instance.GetHandCards()[i])
                CardDisplayList[i].UpdateCard(GameEngine.Instance.GetHandCards()[i]);

        }

        //we delete the extra cards
        if (i >= GameEngine.Instance.GetHandCards().Count)
        {
            
        }
    }
}
