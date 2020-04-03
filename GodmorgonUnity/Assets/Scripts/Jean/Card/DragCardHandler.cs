using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using GodMorgon.Sound;

public class DragCardHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 startPosition;
    private float cardWidth, cardHeight;

    private GameObject player;
    private Transform movingCardParent;
    private Transform effectsParent;
    private Transform hand;

    public delegate void CardDragDelegate(GameObject draggedCard, PointerEventData eventData);

    public CardDragDelegate onCardDragBeginDelegate;
    public CardDragDelegate onCardDragDelegate;
    public CardDragDelegate onCardDragEndDelegate;

    public GameObject dropEffect;

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
    }

    //fonction lancée lorsqu'on a une carte en main
    public void OnDrag(PointerEventData eventData)
    {
        //onCardDragDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = eventData.position;   //La carte prend la position de la souris
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth / 3, cardHeight / 3);  //On réduit la taille de la carte lors du drag

        //Si la carte draggée est de type Mouvement, on montre les tiles accessibles
        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.name == "Mouvement")
        {
            PlayerManager.Instance.ShowAccessibleSpot();
        }
    }

    //fonction lancée au drop d'une carte
    public void OnEndDrag(PointerEventData eventData)
    {
        //onCardDragEndDelegate?.Invoke(this.gameObject, eventData);

        this.transform.position = startPosition;    //
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);  //La carte récupère sa taille normale

        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.name == "Mouvement")
        {
            //Cache les tiles accessibles
            PlayerManager.Instance.HideAccessibleSpot();

            bool moveValidate = player.GetComponent<PlayerManager>().SetPlayerPath();
            if (moveValidate)
            {
                GameEngine.Instance.DiscardCard(eventData.pointerDrag.GetComponent<CardDisplay>().card);   //on place la carte dans la disposal pile une fois utilisée

                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Instantiate(dropEffect, mouseWorldPos, Quaternion.identity, effectsParent);
                this.gameObject.SetActive(false);

                //sound
                //MusicManager.Instance.PlayDropCard();
                MusicManager.Instance.PlayRollingKart();
            }
        }

        this.transform.SetParent(hand);
    }
}
