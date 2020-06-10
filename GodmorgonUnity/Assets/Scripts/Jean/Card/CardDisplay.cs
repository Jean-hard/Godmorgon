using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

using GodMorgon.Models;
using GodMorgon.StateMachine;


/**
 * Présent sur chaque carte
 * Gère l'affichage des infos Name, Description, Artwork et Template sur le prefab Card
 * Gère aussi le drag and drop de la carte, et les effets qui en découlent
 * On peut déclencher des évènements liés au drag and drop dans les autres scripts grâce aux delegate
 */
public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BasicCard card;

    public int cardId;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    public Image artworkImage;
    public Image template;

    public bool isHover = false;
    public float timeHover = 1.0f;
    private static bool cardIsDragging = false;

    public GameObject display = null;

    /**
     * Load the data of the card in the gameObject at start, if the card exist.
     */
    void Start()
    {
        if (card)
        {
            nameText.text = card.name;
            descriptionText.text = card.description;
            artworkImage.sprite = card.artwork;
            cardId = card.id;
            costText.text = card.actionCost.ToString();
        }
        UpdateDescription();
    }

    /**
     * update the card gameObject using the card data
     */
    public void UpdateCard(BasicCard cardData)
    {
        //Debug.Log("on lolololol : " + cardData.name);
        //Save the scriptableObject used by this card gameObject
        card = cardData;

        nameText.text = cardData.name;
        descriptionText.text = cardData.description;
        if (cardData.template)
            template.sprite = cardData.template;
        if (cardData.artwork)
            artworkImage.sprite = cardData.artwork;
        costText.text = card.actionCost.ToString();

        UpdateDescription();
    }

    /**
     * met les données dans la description à jour :
     * 1)met à jour la data par rapport aux buffs du player (exemple : killer instinct...)
     * 2)met à jour la data par rapport aux effets de la carte ( exemple : shiver, trust....)
     */
    public void UpdateDescription()
    {
        if (card != null)
        {
            string cardDescription = card.description;

            //damage
            int actualDamage = BuffManager.Instance.getModifiedDamage(card.GetRealDamage());
            cardDescription = cardDescription.Replace("[nbDamage]", "<b>" + actualDamage.ToString() + "</b>");

            //block
            int actualBlock = BuffManager.Instance.getModifiedBlock(card.GetRealBlock());
            cardDescription = cardDescription.Replace("[nbBlock]", "<b>" + actualBlock.ToString() + "</b>");

            //Move
            int actualMove = BuffManager.Instance.getModifiedMove(card.GetRealMove());
            cardDescription = cardDescription.Replace("[nbMove]", "<b>" + actualMove.ToString() + "</b>");

            //Heal
            int actualHeal = BuffManager.Instance.getModifiedHeal(card.GetRealHeal());
            cardDescription = cardDescription.Replace("[nbHeal]", "<b>" + actualHeal.ToString() + "</b>");

            //carte à piocher
            cardDescription = cardDescription.Replace("[nbCardToDraw]", "<b>" + card.GetRealNbDraw().ToString() + "</b>");

            descriptionText.text = cardDescription;
        }
    }

    /**
     * doit activer l'animation de l'agrandissement de la carte
     */
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(!cardIsDragging)
        {
            isHover = true;
            StartCoroutine(ScaleCardIn());
        }
    }

    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!cardIsDragging)
        {
            isHover = false;
            StartCoroutine(ScaleCardOut());
        }
    }

    public IEnumerator ScaleCardIn()
    {
        GetComponent<Canvas>().sortingOrder = 1;

        Vector3 originalScale = display.transform.localScale;
        Vector3 destinationScale = new Vector3(2.0f, 2.0f, 0);

        Vector3 originalPosition = display.transform.localPosition;
        Vector3 destinationPosition = new Vector3(0, 215, -100);

        float currentTime = 0.0f;

        while (currentTime <= timeHover && isHover)
        {
            display.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime);
            display.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime);
            currentTime += Time.deltaTime * 4;
            yield return null;
        }
    }

    public IEnumerator ScaleCardOut()
    {
        GetComponent<Canvas>().sortingOrder = 0;

        Vector3 originalScale = display.transform.localScale;
        Vector3 destinationScale = new Vector3(1, 1, 1);

        Vector3 originalPosition = display.transform.localPosition;
        Vector3 destinationPosition = new Vector3(0, 0, 0);

        float currentTime = 0.0f;

        while (currentTime <= timeHover && !isHover)
        {
            display.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime);
            display.transform.localPosition = Vector3.Lerp(originalPosition, destinationPosition, currentTime);
            currentTime += Time.deltaTime * 4;
            yield return null;
        }
    }

    //quand la carte est drag, elle reprend sa taille normale
    public void OnCardDrag(bool isCardDrag)
    {
        if (isCardDrag)
        {
            cardIsDragging = true;
            isHover = false;
            StopCoroutine(ScaleCardIn());
            StopCoroutine(ScaleCardOut());

            display.transform.localScale = new Vector3(1, 1, 1);
            display.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
            cardIsDragging = false;
    }
}
