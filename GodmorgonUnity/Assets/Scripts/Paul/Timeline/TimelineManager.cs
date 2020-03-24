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
            //Debug.Log("nb action in list : " + actionlist.Count);
            //SetDisplay();
        }

        //Init the Timeline, function call in Initialization_Maze state
        public void InitTimeline()
        {
            //actionlist = settings.GetActionList();
            Debug.Log("nb action in list : " + actionlist.Count);
            SetDisplay();
        }

        /**
         * Set the display of the Timeline
         * We take the picture of the 4 next actions in the timeline
         * ----------------------------------------------------------TODO : préparer le cas où l'index est au bout de la liste
         */
        public void SetDisplay()
        {
            Debug.Log("AAAAAAAAAAALLLLOOOOOOO !!!!");

            actionLogo1.sprite = actionlist[indexCurrentAction].actionLogo;
            actionLogo2.sprite = actionlist[indexCurrentAction + 1].actionLogo;
            actionLogo3.sprite = actionlist[indexCurrentAction + 2].actionLogo;
            actionLogo4.sprite = actionlist[indexCurrentAction + 3].actionLogo;
        }
    }
}