using GodMorgon.Models;
using GodMorgon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text playerGoldText;     //Texte affichant les golds du player
    public List<Text> pricesTextList = new List<Text>();    //Liste des textes pour le prix
    public List<BasicCard> cardsList = new List<BasicCard>();   //Liste des cartes à vendre

    private bool isPlayerGoldTextUpdated = false;   //Bool gérant la mise à jour du texte des gold du player

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf && !isPlayerGoldTextUpdated)  //si le shop est affiché et que le nb de gold du joueur n'est pas à jour
        {
            UpdatePlayerGoldText(); //On met à jour le nombre de gold
            isPlayerGoldTextUpdated = true; //Le nb de gold est à jour
        }
    }

    /**
     * Update le text des gold du player 
     */
    private void UpdatePlayerGoldText()
    {
        playerGoldText.text = PlayerData.Instance.goldValue.ToString(); //Récup le nb de gold de player data et l'affiche
    }

    /**
     * Ferme le shop et reset les prix
     */
    public void CloseShop()
    {
        this.gameObject.SetActive(false);   //Désactive le shop
        pricesTextList = new List<Text>();  //Reset la liste de prix
        isPlayerGoldTextUpdated = false;    //Reset le bool de la misa à jour du nb de gold du player pour la prochaine ouverture
    }
}
