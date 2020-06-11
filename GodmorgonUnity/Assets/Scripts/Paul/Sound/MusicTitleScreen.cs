using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Sound
{
    public class MusicTitleScreen : MonoBehaviour
    {
        public AK.Wwise.Event Title_Screen;
        public AK.Wwise.Event Mus_Menu;
        public AK.Wwise.Event Mus_None;

        public void Start()
        {
            Mus_Menu.Post(gameObject);
        }

        public void StopMusic()
        {
            Mus_None.Post(gameObject);
        }
    }
}