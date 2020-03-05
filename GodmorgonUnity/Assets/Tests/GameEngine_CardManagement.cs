using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using GodMorgon.Models;


namespace Tests
{
    public class GameEngine_CardManagement
    {
        #region Draft
        [Test]
        public void CanDrawCardFromDraftDeck()
        {
            BasicCard card = ScriptableObject.CreateInstance<BasicCard>();
        }

        [Test]
        public void DraftPickerChooseTheNumberOfCardFromTheSettings()
        {

        }

        [Test]
        public void UserCanOnlyChooseOneCardFromTheSettings()
        {

        }

        [Test]
        public void DraftTurnIsLimitedByTheSettings ()
        {

        }

        [Test]//pas sur
        public void DraftCardIsAddedToTheChosenDeckAndBackToTheDraftDeck()
        {

        }

        [Test]
        public void NonDraftCardIsAddedToTheChosenDeck()
        {

        }

        [Test]
        public void WhenNoDraftSequenceRemainsGameEngineIsSetToPlayMode()
        {

        }
        #endregion


        //[Test]
        //public void CantDrawCardWhenDeckIsEmpty()
        //{
        //    // Low level
        //    GameEngine.Instance.playerDeck.ClearCards(); // Remove all cards! Debug only
        //    BasicCard myCard = GameEngine.Instance.playerDeck.DrawCard();
        //    Assert.IsNull(myCard);

        //    // High Level
        //    GameEngine.deck.ClearCards();
        //    Assert.Throws<DeckIsEmptyException>(() =>
        //    {
        //        GameEngine.DrawCard();
        //    });
        //}

        //[Test]
        //public void CanDrawFromTheDeckWhenNotEmpty()
        //{
        //    GameEngine.deck.ClearCards();
        //    Card myCard = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.deck.AddCard(myCard);
        //    Card myDrawedCard = GameEngine.deck.DrawCard();
        //    Assert.AreSame(myCard, myDrawedCard);
        //}

        //[Test]
        //public void AlwaysDrawTheTopOfTheDeck()
        //{
        //    Card cardA = ScriptableObject.CreateInstance<Card>();
        //    Card cardB = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.deck.AddCard(cardA);
        //    GameEngine.deck.AddCard(cardB);
        //    Card myDrawedCard = GameEngine.deck.DrawCard();
        //    Assert.AreSame(cardB, myDrawedCard);
        //}

        //[Test]
        //public void CantDiscardFromTheHandWhenEmpty()
        //{
        //    Card myCardToRemove = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.hand.ClearCards();
        //    GameEngine.disposalPile.ClearCards();
        //    GameEngine.DiscardCard(myCardToRemove);

        //    Assert.AreEqual(0, GameEngine.disposalPile.Count());

        //    GameEngine.DiscardCard(); // Blindly remove a card

        //    Assert.AreEqual(0, GameEngine.disposalPile.Count());
        //}

        //[Test]
        //public void CanDiscardFromTheHandWhenNotEmpty()
        //{
        //    Card myCardToRemove = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.hand.ClearCards();
        //    GameEngine.hand.AddCard(myCardToRemove);

        //    GameEngine.disposalPile.ClearCards();

        //    // Main action!
        //    GameEngine.DiscardCard(myCardToRemove);
        //    // Results are:
        //    // - The card should no more be in hand! (Hand must be empty)
        //    // - the card should be found in the disposal

        //    Assert.AreEqual(0, GameEngine.hand.Count());
        //    Assert.AreSame(myCardToRemove, GameEngine.disposalPile.DrawCard());

        //    // Rebelote
        //    GameEngine.hand.AddCard(myCardToRemove);
        //    GameEngine.DiscardCard(); // Blindly remove a card (but only one, then... ;) )

        //    Assert.AreEqual(0, GameEngine.hand.Count());
        //    Assert.AreSame(myCardToRemove, GameEngine.disposalPile.DrawCard());
        //}

        //[Test]
        //public void CanShuffleDeckWhenEmpty()
        //{

        //    GameEngine.deck.ClearCards();
        //    GameEngine.disposalPile.ClearCards();
        //    GameEngine.disposalPile.AddCard(ScriptableObject.CreateInstance<Card>());

        //    GameEngine.ShuffleDeck();

        //    Assert.AreEqual(0, GameEngine.disposalPile.Count());
        //    Assert.AreEqual(1, GameEngine.deck.Count());
        //}

        //[Test]
        //public void CantShuffleDeckWhenNotEmpty()
        //{
        //    Card cardA = ScriptableObject.CreateInstance<Card>();
        //    Card cardB = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.deck.ClearCards();
        //    GameEngine.disposalPile.ClearCards();
        //    GameEngine.disposalPile.AddCard(cardA);
        //    GameEngine.deck.AddCard(cardB);

        //    GameEngine.ShuffleDeck();

        //    Assert.AreSame(cardA, GameEngine.disposalPile.DrawCard());
        //    Assert.AreSame(cardB, GameEngine.deck.DrawCard());
        //}

        //[Test]
        //public void CanUseACardFromHand()
        //{

        //}

        //[Test]
        //public void CantDrawMoreCardWhenHandIsFull()
        //{
        //    // No Low level test with hand...
        //    GameEngine.deck.ClearCards(); // Remove all cards! Debug only
        //    Card myCard = GameEngine.deck.DrawCard();
        //    Assert.IsNull(myCard);

        //    // We DO have cards to draw
        //    Card cardA = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.deck.AddCard(cardA);

        //    // Hand is full!
        //    Settings settings = ScriptableObject.CreateInstance<Settings>();
        //    Player player = new Player();
        //    GameEngine.hand.ClearCards();
        //    int FullHand = player.HandOverflow + settings.MaxHandCapability;
        //    while (GameEngine.hand.Count() < FullHand)
        //    {
        //        Card newCard = ScriptableObject.CreateInstance<Card>();
        //        GameEngine.hand.AddCard(newCard);
        //    }

        //    // High Level
        //    Assert.Throws<HandIsFullException>(() => {
        //        GameEngine.DrawCard();
        //    });
        //    // No card actually drawn
        //    Assert.AreEqual(1, GameEngine.deck.Count());
        //    Assert.AreEqual(FullHand, GameEngine.hand.Count());

        //}

        //[Test]
        //public void CanDrawMoreCardWhenHandIsNotFull()
        //{
        //    // We DO have cards to draw
        //    Card cardA = ScriptableObject.CreateInstance<Card>();
        //    GameEngine.deck.ClearCards(); // Remove all cards! Debug only
        //    GameEngine.deck.AddCard(cardA);

        //    // Hand is NOT full (actually empty!)
        //    GameEngine.hand.ClearCards();

        //    // High Level
        //    GameEngine.DrawCard();

        //    // - Deck should be empty
        //    // - cardA should be in hand
        //    Assert.AreEqual(0, GameEngine.deck.Count());
        //    Assert.AreSame(cardA, GameEngine.hand.DrawCard());
        //}

        /*
        [Test]
        public void () {

        }*/
    }
}
