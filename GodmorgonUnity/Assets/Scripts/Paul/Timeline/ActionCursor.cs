using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    /// <summary>
    /// Manage the animation of the cursor of the timeline
    /// </summary>
    public class ActionCursor : MonoBehaviour
    {
        //The animation component containing the different animation of the cursor.
        [SerializeField]
        private Animation anim = null;

        //Index to current animation to play
        private int animIndex = 0;

        /**
         * Call by the TimelineManager
         * launch the animation of the cursor
         */
        public void RunCursorAnim()
        {
            //We use PlayQueued to assure that the animation play only one by one, waiting for the other to finish
            if (animIndex == 0)
                anim.PlayQueued("Cursor animation 1", QueueMode.CompleteOthers);
            else if (animIndex == 1)
                anim.PlayQueued("Cursor animation 2", QueueMode.CompleteOthers);
            else if (animIndex == 2)
                anim.PlayQueued("Cursor animation 3", QueueMode.CompleteOthers);
            else
                anim.PlayQueued("Cursor animation 4", QueueMode.CompleteOthers);

            if (animIndex >= 3)
                animIndex = 0;
            else
                animIndex++;
        }
    }
}