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

    public delegate void CardDragDelegate(GameObject draggedCard, PointerEventData eventData);

    public CardDragDelegate onCardDragBeginDelegate;
    public CardDragDelegate onCardDragDelegate;
    public CardDragDelegate onCardDragEndDelegate;

    private Vector3 startPosition;
    private float cardWidth, cardHeight;

    private GameObject player;

    //Load the data of the card in the gameObject at start, if the card exist.
    void Start()
    {
        player = GameObject.Find("Player");

        if (card)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
            cardId = card.id;
        }

        cardWidth = this.GetComponent<RectTransform>().sizeDelta.x;
        cardHeight = this.GetComponent<RectTransform>().sizeDelta.y;
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
        //onCardDragBeginDelegate?.Invoke(this.gameObject, eventData);

        startPosition = this.transform.position;
        
    }

    //fonction lancée lorsqu'on a une carte en main
    public void OnDrag(PointerEventData eventData)
    {
        //onCardDragDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = eventData.position;
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth / 3, cardHeight / 3);

        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.name == "Mouvement")
        {
            //Montre les tiles accessibles
            PlayerManager.Instance.ShowAccessibleSpot();
        }
    }

    //fonction lancée au drop d'une carte
    public void OnEndDrag(PointerEventData eventData)
    {
        //onCardDragEndDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = startPosition;
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);  //récupère sa taille normale

        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.name == "Mouvement") 
        {
            //Cache les tiles accessibles
            PlayerManager.Instance.HideAccessibleSpot();

            bool moveValidate = player.GetComponent<PlayerManager>().SetPlayerPath();
            if (moveValidate)
            {
                GameEngine.Instance.DiscardCard(eventData.pointerDrag.GetComponent<CardDisplay>().card);   //on place la carte dans la disposal pile une fois utilisée
                Destroy(this.gameObject);// WIP : retirer de la main plutot que détruire
            }
            //supprime la carte si la carte est droppée dans une case voisine de celle du player
            /*bool moveValidate = player.GetComponent<PlayerMove>().UseMoveCard();
            if (moveValidate)
            {
                GameEngine.Instance.DiscardCard(eventData.pointerDrag.GetComponent<CardDisplay>().card);   //on place la carte dans la disposal pile une fois utilisée
                Destroy(this.gameObject);// WIP : retirer de la main plutot que détruire
            } */
        }
    }
}
