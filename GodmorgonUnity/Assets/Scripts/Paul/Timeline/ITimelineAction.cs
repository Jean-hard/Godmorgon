using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public interface ITimelineAction
    {
        void Execute();

        void Finish();
    }
}