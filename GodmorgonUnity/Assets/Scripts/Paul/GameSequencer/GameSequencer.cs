﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.GameSequencerSpace
{
    /**
     * This class manage the animation and the movement of the player or the enemy in the scene.
     */
    public class GameSequencer : MonoBehaviour
    {
        #region Singleton Pattern
        private static GameSequencer _instance;

        public static GameSequencer Instance { get { return _instance; } }
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

        /**
         * List of action complete by effect of card or the timeline.
         */
        private List<GameSequencerAction> _actionsList = new List<GameSequencerAction>();

        /**
         * We add an action when we play effect of card or when we execute action timeline.
         */
        public void AddAction(GameSequencerAction action)
        {
            _actionsList.Add(action);
        }

        /**
         * Execute all the action in the list and when done,
         * change turn.
         */
        public void ExecuteActions()
        {
            StartCoroutine(_CoroutineExecuteActions());
        }

        /**
         * Execute the action one by one.
         */
        private IEnumerator _CoroutineExecuteActions()
        {
            foreach (GameSequencerAction action in _actionsList)
            {
                yield return action.ExecuteAction();
            }

            GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.RINGMASTER_TURN);
            _actionsList.Clear();
        }
    }
}