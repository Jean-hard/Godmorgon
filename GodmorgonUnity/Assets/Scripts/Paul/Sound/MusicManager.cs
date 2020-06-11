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
        

        public AK.Wwise.Event Player_Hit;
        public AK.Wwise.Event Player_Move;
        public AK.Wwise.Event Feedback_Chest;

        public AK.Wwise.Event Cards_Play;
        public AK.Wwise.Event Cards_Attack;
        public AK.Wwise.Event Cards_Defens;
        public AK.Wwise.Event Cards_Move;
        public AK.Wwise.Event Cards_PowerUp;
        public AK.Wwise.Event Cards_Spell;
        public AK.Wwise.Event Cards_Buy;

        public AK.Wwise.Event Mechanical;
        public AK.Wwise.Event Park_Theme;

        public AK.Wwise.Event Enemy_Hit;
        public AK.Wwise.Event Enemy_Death;
        public AK.Wwise.Event Enemy_Moving;

        public AK.Wwise.Event Ringmaster_EndofTurn;

        public AK.Wwise.Event Fog_Clear;

        public AK.Wwise.Event cursorEnd;

        //public AK.Wwise.Event PlayerTheme;
        //public AK.Wwise.State RingmasterState;

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
            //MusicManager.Instance.PlayPlayerTurnTheme();
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Keypad1))
            //    PlayerState.SetValue();
            //else if (Input.GetKeyDown(KeyCode.Keypad2))
            //    RingmasterState.SetValue();
        }

        public void PlayPlayerTurnTheme()
        {
            //PlayerTheme.Post(gameObject);
        }

        //public void PlayRingmasterTurnTheme()
        //{
        //    RingmasterState.SetValue();
        //}

        public void PlayPlayerMove()
        {
            Player_Move.Post(gameObject);
        }

        public void PlayEnemyDeath()
        {
            Enemy_Death.Post(gameObject);
        }

        public void PlayFeedbackChest()
        {
            Feedback_Chest.Post(gameObject);
        }

        public void PlayCardsPlay()
        {
            Cards_Play.Post(gameObject);
        }

        public void PlayCardsAttack()
        {
            Cards_Attack.Post(gameObject);
        }

        public void PlayCardsDefens()
        {
            Cards_Defens.Post(gameObject);
        }

        public void PlayCardsMove()
        {
            Cards_Move.Post(gameObject);
        }

        public void PlayCardsPowerUp()
        {
            Cards_PowerUp.Post(gameObject);
        }

        public void PlayCardsSpell()
        {
            Cards_Spell.Post(gameObject);
        }

        public void PlayCardsBuy()
        {
            Cards_Buy.Post(gameObject);
        }

        public void PlayMechanical()
        {
            Mechanical.Post(gameObject);
        }

        public void PlayParkTheme()
        {
            Park_Theme.Post(gameObject);
        }

        public void PlayEnemyHit()
        {
            Enemy_Hit.Post(gameObject);
        }

        public void PlayPlayerHit()
        {
            Player_Hit.Post(gameObject);
        }

        public void PlayRingMasterEndTurn()
        {
            Ringmaster_EndofTurn.Post(gameObject);
        }

        public void PlayFogClear()
        {
            Fog_Clear.Post(gameObject);
        }

        public void PlayEnemyMoving()
        {
            Enemy_Moving.Post(gameObject);
        }

        public void PlayCursorEnd()
        {
            cursorEnd.Post(gameObject);
        }
    }
}