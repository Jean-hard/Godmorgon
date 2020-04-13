using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.CardEffect;

namespace GodMorgon.GameSequencerSpace
{
    public abstract class GameSequencerAction
    {
        public abstract IEnumerator ExecuteAction(GameContext context);
    }
}