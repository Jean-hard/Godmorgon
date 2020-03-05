using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using GodMorgon.Models;

public class PlayerGUI : MonoBehaviour, IDropHandler
{
    public Text lifeNbText;
    public Text powerNbText;

    [SerializeField]
    private Image hatSlot;
    [SerializeField]
    private Image chestSlot;
    [SerializeField]
    private Image pantsSlot;
    [SerializeField]
    private Image shoesSlot;

    private Color transparentColor = new Color(1f, 1f, 1f, 0);
    private Color notTransparentColor = new Color(1f, 1f, 1f, 1f);

    private StuffCard currentHatStuff;
    private StuffCard currentChestStuff;
    private StuffCard currentPantsStuff;
    private StuffCard currentShoesStuff;

    //Delegate à utiliser dans d'autres scripts si on veut qu'il se passe qqchose qd on drop une carte dans la fiche joueur
    public delegate void DropDelegate(CardDropArea cardDropArea, PointerEventData eventData);
    public DropDelegate onDropDelegate;

    // Start is called before the first frame update
    void Start()
    {
        //set les slots de stuff à transparent pour que les slots soient vides
        hatSlot.color = chestSlot.color = pantsSlot.color = shoesSlot.color = transparentColor;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //modifie la santé en fonction lors d'évènements (combats, ...)
    public void UpdateLife()
    {

    }
    public void OnDrop(PointerEventData eventData)
    {
        //onDropDelegate?.Invoke(this, eventData);
        //Debug.Log("object dropped: " + eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name);
        
        //ne drop l'équipement seulement si c'est une carte équipement. Elle retourne direct à la main dans le cas contraire
        if (eventData.pointerDrag.GetComponent<CardDisplay>().card.GetType().Name == "StuffCard")
        {
            UseStuffCard(eventData.pointerDrag);
            Destroy(eventData.pointerDrag);
        }
    }


    //ajoute les bonus aux stats de la GUI
    public void UseStuffCard(GameObject addedCard)
    {
        StuffCard addedStuffCard = CardsDataBase.GetCard<StuffCard>(addedCard.GetComponent<CardDisplay>().cardId);

        //check si la carte existe bien
        if (!addedStuffCard)
        {
            Debug.LogErrorFormat("Card ID: {0} is not a card or was not found!", addedCard.GetComponent<CardDisplay>().cardId);

            return;
        }

        //vérifie s'il y a déjà un équipement du même type déjà porté, dans ce cas on retire le bonus de l'ancien pour mettre celui de la nouvelle carte 
        //sinon, on ajoute celui de la nouvelle carte
        switch (addedStuffCard.StuffType)
        {
            case StuffCard.StuffTypes.HAT:
                if (currentHatStuff != null)
                {
                    //retire le bonus de l'équipement déjà présent
                    PlayerMgr.Instance.lifeMax -= currentHatStuff.lifeBonus;
                    PlayerMgr.Instance.power -= currentHatStuff.powerBonus; ;
                    //applique les bonus de la nouvelle carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                else
                {
                    //applique les bonus de la carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                //set l'équipement porté à : celui de la carte qu'on vient de poser
                currentHatStuff = addedStuffCard;

                Debug.Log("Equipement HAT ajouté : " + currentHatStuff.name);

                break;
            case StuffCard.StuffTypes.CHEST:
                if (currentChestStuff != null)
                {
                    //retire le bonus de l'équipement déjà présent
                    PlayerMgr.Instance.lifeMax -= currentChestStuff.lifeBonus;
                    PlayerMgr.Instance.power -= currentChestStuff.powerBonus; ;
                    //applique les bonus de la nouvelle carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                else
                {
                    //applique les bonus de la carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                //set l'équipement porté à : celui de la carte qu'on vient de poser
                currentChestStuff = addedStuffCard;

                Debug.Log("Equipement CHEST ajouté : " + currentChestStuff.name);

                break;
            case StuffCard.StuffTypes.PANTS:
                if (currentPantsStuff != null)
                {
                    //retire le bonus de l'équipement déjà présent
                    PlayerMgr.Instance.lifeMax -= currentPantsStuff.lifeBonus;
                    PlayerMgr.Instance.power -= currentPantsStuff.powerBonus; ;
                    //applique les bonus de la nouvelle carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                else
                {
                    //applique les bonus de la carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                //set l'équipement porté à : celui de la carte qu'on vient de poser
                currentPantsStuff = addedStuffCard;

                Debug.Log("Equipement PANTS ajouté : " + currentPantsStuff.name);

                break;
            case StuffCard.StuffTypes.SHOES:
                if (currentShoesStuff != null)
                {
                    //retire le bonus de l'équipement déjà présent
                    PlayerMgr.Instance.lifeMax -= currentShoesStuff.lifeBonus;
                    PlayerMgr.Instance.power -= currentShoesStuff.powerBonus; ;
                    //applique les bonus de la nouvelle carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                else
                {
                    //applique les bonus de la carte
                    PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
                    PlayerMgr.Instance.power += addedStuffCard.powerBonus;
                }
                //set l'équipement porté à : celui de la carte qu'on vient de poser
                currentShoesStuff = addedStuffCard;

                Debug.Log("Equipement SHOES ajouté : " + currentShoesStuff.name);

                break;
        }



        //update les textes des valeurs
        lifeNbText.text = PlayerMgr.Instance.life.ToString() + " / " + PlayerMgr.Instance.lifeMax.ToString();
        powerNbText.text = PlayerMgr.Instance.power.ToString();

        //mets l'image du stuff ajouté dans le slot associé
        switch (addedStuffCard.StuffType)
        {
            case StuffCard.StuffTypes.HAT:
                hatSlot.sprite = addedStuffCard.artwork;
                hatSlot.color = notTransparentColor;
                break;
            case StuffCard.StuffTypes.CHEST:
                chestSlot.sprite = addedStuffCard.artwork;
                chestSlot.color = notTransparentColor;
                break;
            case StuffCard.StuffTypes.PANTS:
                pantsSlot.sprite = addedStuffCard.artwork;
                pantsSlot.color = notTransparentColor;
                break;
            case StuffCard.StuffTypes.SHOES:
                shoesSlot.sprite = addedStuffCard.artwork;
                shoesSlot.color = notTransparentColor;
                break;
        }
    }

    //EN ATTENDANT DE SETUP CA PLUS PROPREMENT
    public IEnumerator WaitForGUIStatsSetup()
    {
        yield return new WaitForSeconds(3f);
        lifeNbText.text = PlayerMgr.Instance.lifeMax.ToString() + " / " + PlayerMgr.Instance.lifeMax.ToString();
        powerNbText.text = PlayerMgr.Instance.power.ToString();
    }

}
