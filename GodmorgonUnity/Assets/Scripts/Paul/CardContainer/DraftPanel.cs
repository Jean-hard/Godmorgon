using GodMorgon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftPanel : MonoBehaviour
{
    public List<CardDisplay> cardObjects = new List<CardDisplay>();

    public List<BasicCard> draftCards = new List<BasicCard>();

    private int draftNb = 1;

    public void UpdateDraft()
    {
        int index = 0;

        if (draftNb == 3) draftNb = 1;

        //Choisit à partir de quel index de la liste on choisit les 3 cartes
        switch(draftNb)
        {
            case 1:
                index = 0;
                break;
            case 2:
                index = 3;
                break;
            case 3:
                index = 6;
                break;
        }

        //On applique à chacune des trois cartes les SO de la liste suivant l'index
        foreach (CardDisplay card in cardObjects)
        {
            card.UpdateCard(draftCards[index]);
            index++;
        }

        draftNb++;
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
