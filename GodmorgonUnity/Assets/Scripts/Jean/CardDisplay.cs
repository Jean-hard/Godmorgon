using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using GodMorgon.Models;

/**
 * Présent sur chaque carte
 * Gère l'affichage des infos Name, Description, Artwork et Template sur le prefab Card
 * Gère aussi le drag and drop de la carte, et les effets qui en découlent
 * On peut déclencher des évènements liés au drag and drop dans les autres scripts grâce aux delegate
 */
public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BasicCard card;

    public int cardId;

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    public Image template;

    public delegate void CardDragDelegate(BasicCard draggedCard, PointerEventData eventData);

    public CardDragDelegate onCardDragBeginDelegate;
    public CardDragDelegate onCardDragDelegate;
    public CardDragDelegate onCardDragEndDelegate;

    private Vector3 startPosition;

    private GameObject player;

    //Load the data of the card in the gameObject at start, if the card exist.
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //Récupère le player pour utiliser ses propriétés/fonctions

        if (card)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
            cardId = card.id;
        }
    }

    /**
     * update the card gameObject using the card data
     */
    public void UpdateCard(BasicCard cardData)
    {
        //Save the scriptableObject use by this card gameObject
        card = cardData;

        nameText.text = cardData.name;
        descriptionText.text = cardData.description;
        if (cardData.template)
            template.sprite = cardData.template;
        if (cardData.artwork)
            artworkImage.sprite = cardData.artwork;

    }

    //fonction lancée au drag d'une carte
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = this.transform.position;
        onCardDragBeginDelegate?.Invoke(card, eventData);
    }

    //fonction lancée lorsqu'on a une carte en main
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        onCardDragDelegate?.Invoke(card, eventData);
    }

    //fonction lancée au drop d'une carte
    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.position = startPosition;
        onCardDragEndDelegate?.Invoke(card, eventData);

        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name == "MoveCard")
        {
            bool moveValidate = player.GetComponent<PlayerMove>().UseMoveCard();
            if (moveValidate)
                GameEngine.Instance.DiscardCard(card);   //on place la carte dans la disposal pile une fois utilisée
        }
        
        //on place la carte dans la disposal pile une fois utilisée
            /*
            if (eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name == "MoveCard")
            {
                bool moveValidate = player.GetComponent<PlayerMove>().UseMoveCard();
                if (moveValidate)
                    GameEngine.Instance.DiscardCard(choosedCard);   //on place la carte dans la disposal pile une fois utilisée
            }*/
        }
}
