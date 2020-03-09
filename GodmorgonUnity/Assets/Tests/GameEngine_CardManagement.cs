﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using GodMorgon.Models;
using GodMorgon.CardContainer;


namespace Tests
{
    public class GameEngine_CardManagement
    {
        [SetUp]
        public void SetUpCardManagement()
        {
            GameEngine.Instance.InitializePlayerDeck();
            Debug.Log("Setup Done");
        }

        #region Draft
        //[Test]
        //public void CanDrawCardFromDraftDeck()
        //{
        //    BasicCard card = ScriptableObject.CreateInstance<BasicCard>();
        //}

        //[Test]
        //public void DraftPickerChooseTheNumberOfCardFromTheSettings()
        //{

        //}

        //[Test]
        //public void UserCanOnlyChooseOneCardFromTheSettings()
        //{

        //}

        //[Test]
        //public void DraftTurnIsLimitedByTheSettings ()
        //{

        //}

        //[Test]//pas sur
        //public void DraftCardIsAddedToTheChosenDeckAndBackToTheDraftDeck()
        //{

        //}

        //[Test]
        //public void NonDraftCardIsAddedToTheChosenDeck()
        //{

        //}

        //[Test]
        //public void WhenNoDraftSequenceRemainsGameEngineIsSetToPlayMode()
        //{

        //}
        #endregion


        [Test]
        public void CantDrawCardWhenDeckIsEmpty()
        {
            // Low level
            GameEngine.Instance.ClearPlayerDeck(); // Remove all cards! Debug only
            BasicCard myCard = GameEngine.Instance.playerDeckSTACK.DrawCard();
            Assert.IsNull(myCard);

            // High Level
            //GameEngine.Instance.ClearPlayerDeck(); ///si ça envoie l'exception le test renvoie erreur alors que... bas c'est bon du coup... non ?
            //Assert.Throws<DeckIsEmptyException>(() =>
            //{
            //    GameEngine.Instance.DrawCard();
            //});
        }

        [Test]
        public void CanDrawFromTheDeckWhenNotEmpty()
        {
            GameEngine.Instance.ClearPlayerDeck();
            BasicCard myCard = ScriptableObject.CreateInstance<BasicCard>();
            GameEngine.Instance.AddCardToPlayerDeck(myCard);
            BasicCard myDrawedCard = GameEngine.Instance.playerDeckSTACK.DrawCard();
            Assert.AreSame(myCard, myDrawedCard);
        }

        [Test]
        public void AlwaysDrawTheTopOfTheDeck()
        {
            BasicCard cardA = ScriptableObject.CreateInstance<BasicCard>();
            BasicCard cardB = ScriptableObject.CreateInstance<BasicCard>();
            GameEngine.Instance.AddCardToPlayerDeck(cardA);
            GameEngine.Instance.AddCardToPlayerDeck(cardB);
            BasicCard myDrawedCard = GameEngine.Instance.playerDeckSTACK.DrawCard();
            Assert.AreSame(cardB, myDrawedCard);
        }

        [Test]
        public void CantDiscardFromTheHandWhenEmpty()
        {
            BasicCard myCardToRemove = ScriptableObject.CreateInstance<BasicCard>();
            GameEngine.Instance.hand.ClearCards();
            GameEngine.Instance.disposalPile.ClearCards();
            GameEngine.Instance.DiscardCard(myCardToRemove);

            Assert.AreEqual(0, GameEngine.Instance.disposalPile.Count());

            GameEngine.Instance.DiscardCard(); // Blindly remove a card

            Assert.AreEqual(0, GameEngine.Instance.disposalPile.Count());
        }

        [Test]
        public void CanDiscardFromTheHandWhenNotEmpty()
        {
            BasicCard myCardToRemove = ScriptableObject.CreateInstance<BasicCard>();
            GameEngine.Instance.hand.ClearCards();
            GameEngine.Instance.hand.AddCard(myCardToRemove);

            GameEngine.Instance.disposalPile.ClearCards();

            // Main action!
            GameEngine.Instance.DiscardCard(myCardToRemove);
            // Results are:
            // - The card should no more be in hand! (Hand must be empty)
            // - the card should be found in the disposal

            Assert.AreEqual(0, GameEngine.Instance.hand.Count());
            Assert.AreSame(myCardToRemove, GameEngine.Instance.disposalPile.DrawCard());

            // Rebelote
            GameEngine.Instance.hand.AddCard(myCardToRemove);
            GameEngine.Instance.DiscardCard(); // Blindly remove a card (but only one, then... ;) )

            Assert.AreEqual(0, GameEngine.Instance.hand.Count());
            Assert.AreSame(myCardToRemove, GameEngine.Instance.disposalPile.DrawCard());
        }

        [Test]
        public void CanShuffleDeckWhenEmpty()
        {

            GameEngine.Instance.ClearPlayerDeck();
            GameEngine.Instance.disposalPile.ClearCards();
            GameEngine.Instance.disposalPile.AddCard(ScriptableObject.CreateInstance<BasicCard>());

            GameEngine.Instance.ShuffleDeck();

            Assert.AreEqual(0, GameEngine.Instance.disposalPile.Count());
            Assert.AreEqual(1, GameEngine.Instance.playerDeckSTACK.Count());
        }

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
