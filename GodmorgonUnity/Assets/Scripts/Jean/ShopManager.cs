using GodMorgon.Models;
using GodMorgon.Player;
using GodMorgon.Timeline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GodMorgon.Sound;


namespace GodMorgon.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public Text playerGoldText;     //Texte affichant les golds du player

        //liste des cartes du magasin
        [SerializeField]
        private ShopContent shopContent = null;

        //liste actuel des cartes dans le magasin
        private List<BasicCard> actualContent = new List<BasicCard>();

        //liste des gameObject des cartes à afficher
        [SerializeField]
        private List<CardDisplay> shopCards = new List<CardDisplay>();

        //liste des gameObject texte pour le prix de chaque cartes
        [SerializeField]
        private List<Text> tickets = new List<Text>();


        private bool isPlayerGoldTextUpdated = false;   //Bool gérant la mise à jour du texte des gold du player
        private bool hasPlayerBuy = false; //pour savoir si le joueur a acheté quelque chose

        //au start on enlève le composant 
        public void Awake()
        {
            //on initialize la liste actuiel de carte
            foreach(BasicCard card in shopContent.cards)
            {
                actualContent.Add(card);
            }

            //on retire le composant de drag de toutes les cartes display
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
            isPlayerGoldTextUpdated = false;    //Reset le bool de la misa à jour du nb de gold du player pour la prochaine ouverture
            GameManager.Instance.OnCloseShop();

            //si le joueur a acheté quelque chose, on lance le tour du ringmaster en quittant le shop
            if (hasPlayerBuy)
            {
                TimelineManager.Instance.SetRingmasterActionRemain(1);
                GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.RINGMASTER_TURN);
            }
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
                if (i < actualContent.Count)
                {
                    shopCards[i].gameObject.SetActive(true);
                    shopCards[i].UpdateCard(actualContent[i]);
                    tickets[i].text = actualContent[i].price.ToString();
                }
                else
                {
                    shopCards[i].gameObject.SetActive(false);
                    tickets[i].text = "";
                }
            }

            hasPlayerBuy = false;
        }

        /**
         * button in-game
         * appelé lorsque choisit un carte du magasin
         */
        public void SelectCard(CardDisplay card)
        {
            //si le player à suffisamment d'argent et si la carte n'a pas déjà été acheté
            if (PlayerData.Instance.goldValue > card.card.price && card.gameObject.activeSelf)
            {
                GameManager.Instance.AddCardInHand(card.card);
                PlayerData.Instance.goldValue -= card.card.price;
                isPlayerGoldTextUpdated = false;
                card.gameObject.SetActive(false);
                actualContent.Remove(card.card);
                PlayerManager.Instance.UpdateGoldText();    //Met à jour les golds sur l'écran de jeu

                //SFX card buy
                MusicManager.Instance.PlayCardsBuy();

                hasPlayerBuy = true;
            }
        }
    }
}