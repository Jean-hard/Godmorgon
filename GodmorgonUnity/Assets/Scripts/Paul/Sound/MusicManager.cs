using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Sound
{
    /**
     * This class managed the different music theme in the game
     */
    public class MusicManager : MonoBehaviour
    {
        public AK.Wwise.Event musicEvent;
        public AK.Wwise.Event dropCardEvent;

        public AK.Wwise.State PlayerState;
        public AK.Wwise.State RingmasterState;

        #region Singleton Pattern
        private static MusicManager _instance;

        public static MusicManager Instance { get { return _instance; } }
        #endregion

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            musicEvent.Post(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
                PlayerState.SetValue();
            else if (Input.GetKeyDown(KeyCode.Keypad2))
                RingmasterState.SetValue();
        }

        public void PlayPlayerTurnTheme()
        {
            PlayerState.SetValue();
        }

        public void PlayRingmasterTurnTheme()
        {
            RingmasterState.SetValue();
        }

        public void PlayDropCard()
        {
            dropCardEvent.Post(gameObject);
        }
    }
}