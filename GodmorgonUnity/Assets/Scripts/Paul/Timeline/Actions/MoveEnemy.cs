using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class MoveEnemy : Action
    {
        public override IEnumerator Execute()
        {
            //Debug.Log("ACTION Move Enemy");
            EnemyManager.Instance.SetEnemyPath();
            while(!EnemyManager.Instance.EnemiesMoveDone())
            {
                yield return null;
            }
        }

        public override void Finish()
        {

        }
    }
}