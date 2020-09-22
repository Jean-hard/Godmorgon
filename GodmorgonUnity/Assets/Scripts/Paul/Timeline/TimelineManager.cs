using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GodMorgon.Models;

using GodMorgon.StateMachine;
using GodMorgon.Sound;

namespace GodMorgon.Timeline
{
    public class TimelineManager : MonoBehaviour
    {
        /**
         * Settings for the timeline.
         */
        [SerializeField]
        private TimelineSettings settings = null;

        /**
         * The action image on the timeline;
         */
        [SerializeField]
        private List<Image> actionLogoList = null;

        /**
         * ActionCursor script manage the animation of the cursor
         */
        [SerializeField]
        private ActionCursor cursorAction = null;

        //text indiquant le nombre de d'action restantes
        [SerializeField]
        private Text nbRemainingActionText = null;

        //Tell if the current action is running
        [System.NonSerialized]
        public bool isRunning = false;

        /**
         * List containing all the actions in order.
         */
        private List<Action> actionlist = new List<Action>();

        //Current index of the list of action
        private int indexCurrentAction = 0;

        //nombre d'action restantes que le ringmaster doit executer
        [System.NonSerialized]
        public int nbRingmasterActionRemain = 0;

        //numéro de l'action actuel de la timeline
        [System.NonSerialized]
        public int nbActualAction = 0;

        //valeur du block gagné pour l'action defend
        public int nbBlockGain = 5;

        //liste de tous les engrenages de la timeline pour les animations
        public List<Animation> actionGearAnimations = new List<Animation>();

        //particule pour l'engrenage de l'action en cour
        public GameObject gearParticle = null;

        //actionLogo destination position
        public Transform logoDestination = null;

        //temps de l'animation
        public float actionLogoTime = 2;

        //position de tout les logos (pour pouvoir les remettres à leur places après l'animation)
        //private List<Transform> actionLogoBasePos = null;

        //particule actuel
        public Transform particulePos = null;

        #region Singleton Pattern
        private static TimelineManager _instance;

        public static TimelineManager Instance { get { return _instance; } }
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

            actionlist = settings.GetActionList();
        }

        // Start is called before the first frame update
        void Start()
        {
            //set the nb remaining action text
            nbRemainingActionText.text = nbRingmasterActionRemain.ToString();

            //launche the first gear anmation
            //actionGearAnimations[0].Play();
            gearParticle.transform.position = actionGearAnimations[0].transform.position;

            particulePos.localPosition = actionGearAnimations[0].transform.localPosition;

            //particulePos.gameObject.SetActive(false);
            particulePos.gameObject.GetComponent<ParticleSystem>().Stop();
        }

        //Init the Timeline, function call in Initialization_Maze state
        public void InitTimeline()
        {
            SetTimeline();
        }

        /**
         * Set the display of the Timeline
         * We take the picture of the 4 next actions in the timeline
         * On réactive les logos
         * -------------TODO : peut en coroutine pour faire une animation---------------------------
         */
        public void SetTimeline()
        {
            nbActualAction = 1;

            //actionGearAnimations[3].Stop();
            //actionGearAnimations[0].Play();
            gearParticle.transform.position = actionGearAnimations[0].transform.position;

            int idx = indexCurrentAction;
            idx = SetNextActions(actionLogoList[0], idx);
            actionLogoList[0].gameObject.SetActive(true);

            idx = SetNextActions(actionLogoList[1], idx);
            actionLogoList[1].gameObject.SetActive(true);

            idx = SetNextActions(actionLogoList[2], idx);
            actionLogoList[2].gameObject.SetActive(true);

            idx = SetNextActions(actionLogoList[3], idx);
            actionLogoList[3].gameObject.SetActive(true);
        }

        /**
         * Set the next action and the display
         */
        private int SetNextActions(Image image, int initIdx)
        {
            //si initIdx > actionlist.Count, alors idx = 0
            int idx = (initIdx >= actionlist.Count) ? 0 : initIdx;
            image.sprite = actionlist[idx++].actionLogo;//ici idx = idx;
            //ici idx = idx + 1; 
            return idx;
        }

        /**
         * Launch the current target action of the timeline
         */
        public void DoAction()
        {
            nbRemainingActionText.text = nbRingmasterActionRemain.ToString();
            StartCoroutine(ActionExecution());
        }

        /**
         * Function that know when the execution of the action is finished and launch the player turn
         */
        public IEnumerator ActionExecution()
        {
            //particulePos.gameObject.SetActive(true);
            particulePos.gameObject.GetComponent<ParticleSystem>().Play();

            actionGearAnimations[nbActualAction - 1].Play();

            //SFX ringmaster laugh
            MusicManager.Instance.PlayRingMasterEndTurn();


            isRunning = true;
            //wait for the action to finish
            yield return actionlist[indexCurrentAction].Execute();
            isRunning = false;

            //on désactive le logo de l'action actuel
            actionLogoList[nbActualAction - 1].gameObject.SetActive(false);

            //actualise le numéro pour l'action actuel et l'index de la list d'action;
            nbActualAction++;
            indexCurrentAction++;



            //si on arrive au bout des 4 actions affichées
            if (indexCurrentAction % 4 == 0)
            {
                SetTimeline();
                //le joueur jette sa main et repioche des cartes
                GameManager.Instance.PlayerDraw();

                //le player perd ces bonus à la fin du tour
                BuffManager.Instance.ResetAllBonus();

                actionGearAnimations[3].Stop();

                GameManager.Instance.ShowNewTurnImage();

                //SFX end ringmaster Turn
                MusicManager.Instance.PlayCursorEnd();
            }
            else
            {
                //actionGearAnimations[nbActualAction - 1].Play();
                gearParticle.transform.position = actionGearAnimations[nbActualAction - 1].transform.position;

                particulePos.localPosition = actionGearAnimations[nbActualAction - 1].transform.localPosition;

                actionGearAnimations[nbActualAction - 2].Stop();
            }

            //at the end of the action, we set the cursor
            cursorAction.RunCursorAnim();

            nbRingmasterActionRemain--;
            //si il reste des action pour le ringmaster, on relance son tour
            if (nbRingmasterActionRemain > 0)
            {
                //yield return new WaitForSeconds(0.5f);
                GameEngine.Instance.RestartState();
            }
            //sinon on lance le tour du joueur
            else
                GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.PLAYER_TURN);

            //actionGearAnimations[nbActualAction - 2].Stop();

            particulePos.gameObject.GetComponent<ParticleSystem>().Stop();

            nbRemainingActionText.text = nbRingmasterActionRemain.ToString();
        }


        //return the id of the current action in the list
        public int GetIndexCurrentAction()
        {
            return indexCurrentAction;
        }

        //indique le nombre d'actions que le ringmaster va executer
        public void SetRingmasterActionRemain(int nbTurn)
        {
            nbRingmasterActionRemain = nbTurn;
        }

        /**
         * Animation à chaque tour du ringmaster
         * le logo s'agrandit
         */
        public IEnumerator ActionLogoAnimation()
        {
            Vector3 originalScale = actionLogoList[nbActualAction - 1].transform.localScale;
            Vector3 destinationScale = new Vector3(2f, 2f, 0);

            float currentTime = 0.0f;

            while (currentTime <= actionLogoTime)
            {
                actionLogoList[nbActualAction - 1].transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime);
                currentTime += Time.deltaTime * 4;
                yield return null;
            }
            yield return new WaitForSeconds(1f);

            currentTime = 0;
            while (currentTime <= actionLogoTime)
            {
                actionLogoList[nbActualAction - 1].transform.localScale = Vector3.Lerp(destinationScale, originalScale, currentTime);
                currentTime += Time.deltaTime * 4;
                yield return null;
            }
        }
    }
}