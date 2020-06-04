using GodMorgon.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Timeline
{
    public class SpawnEnemy : Action
    {
        public override IEnumerator Execute()
        {
            Debug.Log("ACTION spawn enemy");
            EnemyManager.Instance.SpawnEnemiesList();

            yield return new WaitForSeconds(2f);

            //yield return null;
        }

        public override void Finish()
        {

        }
    }
}
