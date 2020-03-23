using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public abstract class Action : ITimelineAction
    {
        public virtual void Execute()
        {

        }

        public virtual void Finish()
        {

        }
    }
}