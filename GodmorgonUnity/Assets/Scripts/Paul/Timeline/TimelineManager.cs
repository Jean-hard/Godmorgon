﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GodMorgon.Models;
////    ????????????????????
using GodMorgon.StateMachine;

namespace GodMorgon.Timeline
{
    public class TimelineManager : MonoBehaviour
    {
        /**
         * Settings for the timeline.
         */
        [SerializeField]
        private TimelineSettings settings = null;

        /**
         * The action image on the timeline;
         */
        [SerializeField]
        private Image actionLogo1 = null;
        [SerializeField]
        private Image actionLogo2 = null;
        [SerializeField]
        private Image actionLogo3 = null;
        [SerializeField]
        private Image actionLogo4 = null;

        /**
         * ActionCursor script manage the animation of the cursor
         */
        [SerializeField]
        private ActionCursor cursorAction = null;

        //Tell if the current action is running
        [System.NonSerialized]
        public bool isRunning = false;

        /**
         * List containing all the actions in order.
         */
        private List<Action> actionlist = new List<Action>();

        //Current index of the list of action
        private int indexCurrentAction = 0;

        //numéro de l'action actuel de la timeline
        [System.NonSerialized]
        public int nbActualAction = 0;

        #region Singleton Pattern
        private static TimelineManager _instance;

        public static TimelineManager Instance { get { return _instance; } }
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

        // Start is called before the first frame update
        void Start()
        {
            actionlist = settings.GetActionList();
        }

        //Init the Timeline, function call in Initialization_Maze state
        public void InitTimeline()
        {
            SetTimeline();
        }

        /**
         * Set the display of the Timeline
         * We take the picture of the 4 next actions in the timeline
         */
        public void SetTimeline()
        {
            nbActualAction = 1;

            int idx = indexCurrentAction;
            idx = SetNextActions(actionLogo1, idx);
            idx = SetNextActions(actionLogo2, idx);
            idx = SetNextActions(actionLogo3, idx);
            idx = SetNextActions(actionLogo4, idx);
        }

        /**
         * Set the next action and the display
         */
        private int SetNextActions(Image image, int initIdx)
        {
            //si initIdx > actionlist.Count, alors idx = 0
            int idx = (initIdx > actionlist.Count) ? 0 : initIdx;
            image.sprite = actionlist[idx++].actionLogo;//ici idx = idx;
            //ici idx = idx + 1; 
            return idx;
        }

        /**
         * Launch the current target action of the timeline
         */
        public void DoAction()
        {
            StartCoroutine(ActionExecution());
        }

        /**
         * Function that know when the execution of the action is finished and launch the player turn
         */
        public IEnumerator ActionExecution()
        {
            isRunning = true;
            //wait for the action to finish
            yield return actionlist[indexCurrentAction].Execute();
            isRunning = false;

            //actualise le numéro pour l'action actuel;
            nbActualAction++;

            indexCurrentAction++;

            //si on arrive au bout des 4 actions affichées
            if (indexCurrentAction % 4 == 0)
            {
                SetTimeline();
                //le joueur jette sa main et repioche des cartes
                GameManager.Instance.PlayerDraw();

                //le player perd ces bonus à la fin du tour
                PlayerManager.Instance.ResetBonus();
            }

            //at the end of the action, we set the cursor
            cursorAction.RunCursorAnim();

            GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.PLAYER_TURN);
        }


        //return the id of the current action in the list
        public int GetIndexCurrentAction()
        {
            return indexCurrentAction;
        }
    }
}