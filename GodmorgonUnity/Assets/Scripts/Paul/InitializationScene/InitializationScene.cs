using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/////// ?????????????????
using GodMorgon.StateMachine;

namespace GodMorgon.IniatializationScene
{
    /**
     * object dont destroy on load to assure good initialization of the game scene
     */
    public class InitializationScene : MonoBehaviour
    {
        [SerializeField]
        private float timeBeforeInitializationMaze = 2.0f;

        #region Singleton Pattern
        private static InitializationScene _instance;

        public static InitializationScene Instance { get { return _instance; } }
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

        public void Start()
        {
            DontDestroyOnLoad(this);
        }

        //Wait some time to assure that the game object in the game scene have time to initialize
        public void LaunchTimerGameScene()
        {
            StartCoroutine(TimerInitializationMaze());
        }
        public IEnumerator TimerInitializationMaze()
        {
            yield return new WaitForSeconds(timeBeforeInitializationMaze);
            GameEngine.Instance.SetState(StateMachine.StateMachine.STATE.INITIALIZATION_MAZE);
        }
    }
}