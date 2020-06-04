using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using GodMorgon.Models;
using GodMorgon.StateMachine;
using GodMorgon.Timeline;
using GodMorgon.Player;
using GodMorgon.Shop;

public class GameManager : MonoBehaviour
{
    /**
     * HandManager object
     */
    [SerializeField]
    private HandManager handManager = null;

    [SerializeField]
    private GameObject downPanelBlock = null;

    [SerializeField]
    private ShopManager shopManager = null;

    //Animations signalant les différents tours
    public Animation playerTurnAnimation = null;
    public Animation ringmasterTurnAnimation = null;

    //color advertising by default
    private Color advertisingDefaultColor;

    //bool pour savoir si le player à passer son tour précédemment
    private bool lastPlayerTurnPassed = false;

    #region Singleton Pattern
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Start()
    {
        //on sauvegarde l'alpha des images pour les animations, si on doit les stopper
        advertisingDefaultColor = playerTurnAnimation.gameObject.GetComponent<Image>().color;
    }

    /**
     * At the beginning of turn,
     * the player discard all his currents cards and draw new one.
     */
    public void PlayerDraw()
    {
        handManager.DiscardAllCard();
        for(int i = 0; i < GameEngine.Instance.GetSettings().MaxHandCapability; i++)
        {
            BasicCard cardDrawn = GameEngine.Instance.DrawCard();
            handManager.AddCard(cardDrawn);
        }
    }

    //discard toutes les cartes dans la main
    public void DiscardHand()
    {
        handManager.DiscardAllCard();
    }

    #region IN-GAME BUTTON FUNCTION
    /**
     * Draw a card from the player Deck
     */
    public void DrawCardButton()
    {
        BasicCard cardDrawn = GameEngine.Instance.DrawCard();
        handManager.AddCard(cardDrawn);
    }

    //Passe le tour du player ce qui lui permettra de tirer une carte supplémentaire
    public void SkipPlayerTurn()
    {
        lastPlayerTurnPassed = true;
        TimelineManager.Instance.SetRingmasterActionRemain(1);
        GameEngine.Instance.SetState(StateMachine.STATE.RINGMASTER_TURN);
    }

    /**
     * Affiche le shop
     * Appelé par le bouton token
     */
    public void OpenShop()
    {
        //Si c'est au tour du joueur et qu'il nous reste des token
        if (GameEngine.Instance.GetState() == StateMachine.STATE.PLAYER_TURN && PlayerData.Instance.token > 0)
        {
            PlayerManager.Instance.TakeOffToken(); //Retire un token au player
            shopManager.gameObject.SetActive(true);  //Affiche le shop
            shopManager.ShopOpening();//on prépare les cartes pour le magasin
            handManager.gameObject.SetActive(false);
        }
    }

    #endregion


    //on réactive l'affichage de la main quand on ferme le shop
    public void OnCloseShop()
    {
        handManager.gameObject.SetActive(true);
    }

    //add nbCard to hand
    public void DrawCard(int nbCard)
    {
        for (int i = 0; i < nbCard; i++)
        {
            BasicCard cardDrawn = GameEngine.Instance.DrawCard();
            handManager.AddCard(cardDrawn);
        }
        //on met à jour les infos dès qu'on pioche une carte
        handManager.UpdateCardDataDisplay();
    }

    //add card to hand from shop
    public void AddCardInHand(BasicCard card)
    {
        GameEngine.Instance.hand.AddCard(card);
        handManager.AddCard(card);
        handManager.UpdateCardDataDisplay();
    }

    /**
    * Discard the card
    * Call by the handManager
    */
    public void DiscardHandCard(BasicCard cardDiscarded)
    {
        GameEngine.Instance.DiscardCard(cardDiscarded);
    }

    /**
    * Discard a card (not from hand)
    * Call by the RoomEffectManager
    */
    public void AddCardToDiscardPile(BasicCard cardDiscarded)
    {
        GameEngine.Instance.AddCardToDiscardPile(cardDiscarded);
    }

    /**
     * active panel to block dawn panel during the ringmaster turn
     */
    public void DownPanelBlock(bool isPanelBlock)
    {
        downPanelBlock.SetActive(isPanelBlock);
    }

    //Update les infos de toutes les cartes
    public void UpdateCardDataDisplay()
    {
        handManager.UpdateCardDataDisplay();
    }

    

    //Pioche une carte supplémentaire si le tour précédent à été passé
    public void CheckForCardToDraw()
    {
        if(lastPlayerTurnPassed)
        {
            lastPlayerTurnPassed = false;
            DrawCard(1);
        }
    }

    //affiche le texte signalant le tour du Player
    public void ShowPlayerTurnImage()
    {
        if (ringmasterTurnAnimation.isPlaying)
        {
            ringmasterTurnAnimation.Stop();
            ringmasterTurnAnimation.gameObject.GetComponent<Image>().color = advertisingDefaultColor;
        }
        playerTurnAnimation.Play();
    }

    //affiche le texte signalant le tour du Ringmaster
    public void ShowRingmasterTurnImage()
    {
        if (playerTurnAnimation.isPlaying)
        {
            playerTurnAnimation.Stop();
            playerTurnAnimation.gameObject.GetComponent<Image>().color = advertisingDefaultColor;
        }
        ringmasterTurnAnimation.Play();
    }
}
