using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEST
{
    public class Card : MonoBehaviour
    {
        public void OnUse()
        {
            Destroy(gameObject);
        }

        public void MovingEffect(int orientionChoice)
        {
            TilemapManager.Instance.DirectionForPlayer(orientionChoice);

            OnUse();
        }
    }
}
