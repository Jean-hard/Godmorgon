using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInteractions : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public delegate void CardDragDelegate(CardInteractions card, PointerEventData eventData);

    public CardDragDelegate oncardDragBeginDelegate;
    public CardDragDelegate onCardDragDelegate;
    public CardDragDelegate onCardDragEndDelegate;

    public void OnBeginDrag(PointerEventData eventData)
    {
        oncardDragBeginDelegate?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onCardDragDelegate?.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onCardDragEndDelegate?.Invoke(this, eventData);
    }
}
