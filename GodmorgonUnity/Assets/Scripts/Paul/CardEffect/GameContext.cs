using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Models;

namespace GodMorgon.CardEffect
{
    /**
     * This class contain all the information necessary
     * to apply any card effect
     */
    public class GameContext
    {
        public bool isDropValidate = false;
        public BasicCard card;
        public Entity owner;
        public Entity targets;
        public RoomData targetRoom;
    }
}