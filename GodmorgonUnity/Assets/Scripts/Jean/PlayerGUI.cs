using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    public Text lifeNbText;
    public Text powerNbText;

    public Image hatSlot;
    public Image chestSlot;
    public Image pantsSlot;
    public Image shoesSlot;

    private Color transparentColor = new Color(1f, 1f, 1f, 0);
    private Color notTransparentColor = new Color(1f, 1f, 1f, 1f);

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

    //ajoute les bonus aux stats de la GUI
    public void UseStuffCard(GameObject addedCard)
    {
        var addedStuffCard = CardsDataBase.GetCard<StuffCard>(addedCard.GetComponent<CardDisplay>().cardId);

        //check si la carte existe bien
        if (!addedStuffCard)
        {
            Debug.LogErrorFormat("Card ID: {0} is not a card or was not found!", addedCard.GetComponent<CardDisplay>().cardId);
            return;
        }

        //applique les bonus de la carte
        PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
        PlayerMgr.Instance.power += addedStuffCard.powerBonus;

        //update les textes des valeurs
        lifeNbText.text = PlayerMgr.Instance.life.ToString() + " / " + PlayerMgr.Instance.lifeMax.ToString();
        powerNbText.text = PlayerMgr.Instance.power.ToString();

        //update l'image
        switch(addedStuffCard.StuffType)
        {
            case StuffCard.StuffTypes.HAT :
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
