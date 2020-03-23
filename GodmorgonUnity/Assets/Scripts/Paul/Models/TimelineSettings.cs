using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Timeline;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "Timeline", menuName = "AddTimeLineSettings")]
    public class TimeLineSettings : ScriptableObject
    {
        public List<Action> ActionInTimeline;
    }

}
