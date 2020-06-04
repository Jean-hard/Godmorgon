using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GodMorgon.Enemy;

namespace GodMorgon.Timeline
{
    public class Attack : Action
    {
        public override IEnumerator Execute()
        {
            //Debug.Log("ACTION attack");
            EnemyManager.Instance.Attack();
            while (!EnemyManager.Instance.EnemiesAttackDone())
            {
                yield return null;
            }
            Debug.Log("Action Attack Done : next");
            yield return new WaitForSeconds(2f);
        }

        public override void Finish()
        {

        }
    }
}
