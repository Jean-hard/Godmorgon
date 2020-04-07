using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Enemy;

namespace GodMorgon.Timeline
{
    public class MoveEnemy : Action
    {
        public override IEnumerator Execute()
        {
            //Debug.Log("ACTION Move Enemy");
            EnemyManager.Instance.MoveEnemies();
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