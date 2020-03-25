using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GodMorgon.Models;

namespace GodMorgon.Timeline
{
    public class TimelineManager : MonoBehaviour
    {
        /**
         * Settings for the timeline.
         */
        [SerializeField]
        private TimelineSettings settings;

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
         * List containing all the actions in order.
         */
        private List<Action> actionlist = new List<Action>();

        //Current index of the list of action
        private int indexCurrentAction = 0;

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

        public void DoAction()
        {

        }


        //return the id of the current action in the list
        public int GetIndexCurrentAction()
        {
            return indexCurrentAction;
        }
    }
}