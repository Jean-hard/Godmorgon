using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "New Shop Content", menuName = "Shop_Content")]

    /**
     * Contain the list of card forming this deck
     */
    public class ShopContent : ScriptableObject
    {
        public string _name;

        public List<BasicCard> cards;
    }
}