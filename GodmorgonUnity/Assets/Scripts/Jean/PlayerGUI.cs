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

    // Start is called before the first frame update
    void Start()
    {
        
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
    public void AddBonus(GameObject addedCard)
    {
        var addedStuffCard = CardsDataBase.GetCard<StuffCard>(addedCard.GetComponent<CardDisplay>().cardId);

        //check si la carte existe bien
        if (!addedStuffCard)
        {
            Debug.LogErrorFormat("Card ID: {0} is not a card or was not found!", addedCard.GetComponent<CardDisplay>().cardId);
            return;
        }

        PlayerMgr.Instance.lifeMax += addedStuffCard.lifeBonus;
        PlayerMgr.Instance.power += addedStuffCard.powerBonus;

        lifeNbText.text = PlayerMgr.Instance.life.ToString() + " / " + PlayerMgr.Instance.lifeMax.ToString();
        powerNbText.text = PlayerMgr.Instance.power.ToString();
    }

    //EN ATTENDANT DE SETUP CA PLUS PROPREMENT
    public IEnumerator WaitForGUIStatsSetup()
    {
        yield return new WaitForSeconds(3f);
        lifeNbText.text = PlayerMgr.Instance.lifeMax.ToString() + " / " + PlayerMgr.Instance.lifeMax.ToString();
        powerNbText.text = PlayerMgr.Instance.power.ToString();
    }
}
