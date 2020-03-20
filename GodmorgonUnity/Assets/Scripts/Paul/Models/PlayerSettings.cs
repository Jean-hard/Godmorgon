using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "Player", menuName = "AddPlayerSettings")]
    public class PlayerSettings : ScriptableObject
    {
        public int lifeMax;
        public int life;
        public int power;
    }

}
