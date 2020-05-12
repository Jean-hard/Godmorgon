using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * class managing the all the different state of the game
 */
namespace GodMorgon.StateMachine
{
    public class StateMachine
    {
        //enum containing the state of the game
        public enum STATE
        {
            TITLE,
            DRAFT,
            LOAD,
            INITIALIZATION_MAZE,
            PLAYER_TURN,
            RINGMASTER_TURN,
            TRANSITION_PHASE,
            VICTORY,
            DEFEAT
        }

        //current state of the game
        private STATE currentState;

        //current object state of the game
        private State currentActifState;

        /**
         * Dictionary with all the state object
         */
        private Dictionary<STATE, State> DictionaryState;

        /**
         * State machine constructor
         * Initialize all the state and put them in the dictionary
         */
        public StateMachine()
        {
            DictionaryState = new Dictionary<STATE, State>();

            DictionaryState.Add(STATE.DRAFT, new Draft());
            DictionaryState.Add(STATE.LOAD, new Load());
            DictionaryState.Add(STATE.INITIALIZATION_MAZE, new Initialization_Maze());
            DictionaryState.Add(STATE.PLAYER_TURN, new Player_Turn());
            DictionaryState.Add(STATE.RINGMASTER_TURN, new RingMaster_Turn());
        }

        //return the current State of the game
        public STATE GetCurrentState()
        {
            return currentState;
        }

        /**
         * On each change of state, we end the current state and start the new one
         */
        public void SetState(STATE newState)
        {
            if(currentActifState != null)
                currentActifState.OnEndState();

            currentActifState = DictionaryState[newState];
            if (currentActifState == null)
                Debug.Log("Cet état n'existe pas !!");

            currentState = newState;
            currentActifState.OnStartState();
        }

        //relance l'état
        public void RestartState()
        {
            currentActifState.OnEndState();
            Debug.Log("restart de l'état : " + currentState);
            currentActifState.OnStartState();
        }
    }
}