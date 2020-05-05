using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using GodMorgon.Models;
using GodMorgon.StateMachine;


/**
 * Présent sur chaque carte
 * Gère l'affichage des infos Name, Description, Artwork et Template sur le prefab Card
 * Gère aussi le drag and drop de la carte, et les effets qui en découlent
 * On peut déclencher des évènements liés au drag and drop dans les autres scripts grâce aux delegate
 */
public class CardDisplay : MonoBehaviour
{
    public BasicCard card;

    public int cardId;

    public Text nameText;
    public Text descriptionText;

    public Image artworkImage;
    public Image template;
    

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
        }

        UpdateDescription();
    }

    //met à jour le descriptif de la carte en continue
    public void Update()
    {
        UpdateDescription();
    }

    /**
     * update the card gameObject using the card data
     */
    public void UpdateCard(BasicCard cardData)
    {
        //Save the scriptableObject used by this card gameObject
        card = cardData;

        nameText.text = cardData.name;
        descriptionText.text = cardData.description;
        if (cardData.template)
            template.sprite = cardData.template;
        if (cardData.artwork)
            artworkImage.sprite = cardData.artwork;

        UpdateDescription();
    }

    /**
     * met les données dans la description à jour :
     * 1)met à jour la data par rapport aux buffs du player (exemple : killer instinct...)
     * 2)met à jour la data par rapport aux effets de la carte ( exemple : shiver, trust....)
     */
    public void UpdateDescription()
    {
        //damage
        //met à jour la data par rapport aux buffs du player (exemple : killer instinct...)
        int actualDamage = BuffManager.Instance.getModifiedDamage(card.GetRealDamage());

        //met à jour la data par rapport aux effets de la carte(exemple : shiver, trust....)
        descriptionText.text = descriptionText.text.Replace("[nbDamage]", actualDamage.ToString());

        //block
        int actualBlock = BuffManager.Instance.getModifiedBlock(card.GetRealBlock());
        descriptionText.text = descriptionText.text.Replace("[nbBlock]", actualBlock.ToString());

        //Move
        int actualMove = BuffManager.Instance.getModifiedMove(card.GetRealMove());
        descriptionText.text = descriptionText.text.Replace("[nbMove]", actualMove.ToString());

        //Heal
        int actualHeal = BuffManager.Instance.getModifiedHeal(card.GetRealHeal());
        descriptionText.text = descriptionText.text.Replace("[nbHeal]", actualHeal.ToString());

        //carte à piocher
        descriptionText.text = descriptionText.text.Replace("[nbCardToDraw]", card.GetRealNbDraw().ToString());
    }
}
