using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Sound
{
    public class MusicTitleScreen : MonoBehaviour
    {
        public AK.Wwise.Event Title_Screen;

        public void Start()
        {
            Title_Screen.Post(gameObject);
        }
    }
}