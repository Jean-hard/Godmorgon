using GodMorgon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPanel : MonoBehaviour
{
    public List<CardDisplay> cardObjects = new List<CardDisplay>();

    public List<BasicCard> draftCards = new List<BasicCard>();

    public int draftNb = 0;

    public void UpdateDraft()
    {
        

        int index = 0;

        if (draftNb > 2) draftNb = 0;

        //Choisit à partir de quel index de la liste on choisit les 3 cartes
        switch(draftNb)
        {
            case 0:
                index = 0;
                break;
            case 1:
                index = 3;
                break;
            case 2:
                index = 6;
                break;
        }

        ++draftNb;

        //On applique à chacune des trois cartes les SO de la liste suivant l'index
        foreach (CardDisplay card in cardObjects)
        {
            card.UpdateCard(draftCards[index]);
            index++;
        }

    }

    /**
     * Fonction appelée lorsqu'on clique sur une carte du draft panel
     */
    public void ChooseCard(CardDisplay card)
    {
        GameManager.Instance.AddCardToDiscardPile(card.card);
        GameManager.Instance.DraftPanelActivation(false);
    }
}
