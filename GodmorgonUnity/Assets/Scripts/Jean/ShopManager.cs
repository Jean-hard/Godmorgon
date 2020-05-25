using GodMorgon.Models;
using GodMorgon.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GodMorgon.Models;

namespace GodMorgon.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public Text playerGoldText;     //Texte affichant les golds du player
        public List<Text> pricesTextList = new List<Text>();    //Liste des textes pour le prix
        public List<BasicCard> cardsList = new List<BasicCard>();   //Liste des cartes à vendre

        //liste des cartes du magasin
        [SerializeField]
        private ShopContent shopContent = null;
        //liste des gameObject des cartes à afficher
        [SerializeField]
        private List<CardDisplay> shopCards = new List<CardDisplay>();


        private bool isPlayerGoldTextUpdated = false;   //Bool gérant la mise à jour du texte des gold du player

        //au start on enlève le composant 
        public void Start()
        {
            foreach (CardDisplay card in shopCards)
            {
                Destroy(card.gameObject.GetComponent<DragCardHandler>());
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (this.gameObject.activeSelf && !isPlayerGoldTextUpdated)  //si le shop est affiché et que le nb de gold du joueur n'est pas à jour
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

        //à l'ouverture du magasin, on affiche toutes les cartes
        public void ShopOpening()
        {
            //on sort s'il manque le shop content
            if (shopContent == null)
            {
                Debug.Log("il manque un shop content dans le shop panel");
                return;
            }

            //on load une par une toutes les cartes
            for (int i = 0; i < shopCards.Count; i++)
            {
                shopCards[i].UpdateCard(shopContent.cards[i]);
            }
        }

        /**
         * button in-game
         * appelé lorsque choisit un carte du magasin
         */
        public void SelectCard(CardDisplay card)
        {
            GameManager.Instance.AddCardInHand(card.card);
        }
    }
}