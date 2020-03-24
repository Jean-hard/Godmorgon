using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         * List containing all the actions in order.
         */
        private List<Action> actionlist = new List<Action>();

        //Current index of the list of action
        private int indexCurrentAction = 0;

        #region Singleton Pattern
        private static TimelineManager instance;

        public static TimelineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimelineManager();
                }

                return instance;
            }
        }
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            actionlist = settings.GetActionList();
            SetDisplay();
        }

        /**
         * Set the display of the Timeline
         */
        public void SetDisplay()
        {

        }



        // Update is called once per frame
        void Update()
        {

        }
    }
}