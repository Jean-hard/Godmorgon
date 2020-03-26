using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    /**
     * abstract class mother for all the different type of action of the ringmaster
     */
    public abstract class Action : ITimelineAction
    {
        public Sprite actionLogo;

        public virtual IEnumerator Execute()
        {
            yield return null;
        }

        public virtual void Finish()
        {

        }
    }
}