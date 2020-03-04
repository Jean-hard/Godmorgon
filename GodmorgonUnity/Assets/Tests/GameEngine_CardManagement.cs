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
        [Test]
        public void CanDrawCardFromDraftDeck()
        {
            BasicCard card = ScriptableObject.CreateInstance<BasicCard>(); // TADAAAA !
        }
    }
}
