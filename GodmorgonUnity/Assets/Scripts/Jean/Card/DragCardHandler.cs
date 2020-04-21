using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using GodMorgon.Sound;
using GodMorgon.CardEffect;
using GodMorgon.Models;

public class DragCardHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private float cardWidth, cardHeight;

    private GameObject player;
    private BasicCard _card;
    private Transform movingCardParent;
    private Transform effectsParent;
    private Transform hand;
    private GameContext context;

    public delegate void CardDragDelegate(GameObject draggedCard, PointerEventData eventData);

    public CardDragDelegate onCardDragBeginDelegate;
    public CardDragDelegate onCardDragDelegate;
    public CardDragDelegate onCardDragEndDelegate;

    public GameObject dropEffect;

    private DropPositionManager dropPosManager = new DropPositionManager();

    

    // Start is called before the first frame update
    void Start()
    {
        cardWidth = this.GetComponent<RectTransform>().sizeDelta.x;
        cardHeight = this.GetComponent<RectTransform>().sizeDelta.y;

        // WIP : essayer de les récupérer autrement
        player = GameObject.Find("Player");
        movingCardParent = GameObject.Find("MovingCardParent").transform;
        hand = GameObject.Find("Hand").transform;
        effectsParent = GameObject.Find("EffectsParent").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //fonction lancée au drag d'une carte
    public void OnBeginDrag(PointerEventData eventData)
    {
        //onCardDragBeginDelegate?.Invoke(this.gameObject, eventData);

        startPosition = this.transform.position;
        this.transform.SetParent(movingCardParent);

        _card = eventData.pointerDrag.GetComponent<CardDisplay>().card;

        context = new GameContext
        {
            card = _card
        };

        //On montre les positions disponibles pour le drop de la carte
        dropPosManager.ShowPositionsToDrop(_card);
    }

    //fonction lancée lorsqu'on a une carte en main
    public void OnDrag(PointerEventData eventData)
    {
        //onCardDragDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = eventData.position;   //La carte prend la position de la souris
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth / 3, cardHeight / 3);  //On réduit la taille de la carte lors du drag
      
    }

    //fonction lancée au drop d'une carte
    public void OnEndDrag(PointerEventData eventData)
    {
        //onCardDragEndDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = startPosition;    //Par défaut, la carte retourne dans la main
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);  //La carte récupère sa taille normale

        Vector3 dropPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        Vector3Int dropCellPosition = PlayerManager.Instance.walkableTilemap.WorldToCell(dropPosition);


        //Vérifie si la position du drop est valide
        dropPosManager.GetDropCardContext(_card, dropCellPosition, context);
        if (context.isDropValidate)
        {
            //Joue la carte
            CardEffectManager.Instance.PlayCard(eventData.pointerDrag.GetComponent<CardDisplay>().card, context);
            
            //Effect + delete card
            //Instantiate(dropEffect, dropPosition, Quaternion.identity, effectsParent);
            this.gameObject.SetActive(false);

            //Cache les positions accessibles
            dropPosManager.HidePositionsToDrop(_card);
            
            //sound
            
            MusicManager.Instance.PlayDropCard();
            //MusicManager.Instance.PlayRollingKart();  //pas ici qu'il doit être activé
        }

        this.transform.SetParent(hand);
    }
}
