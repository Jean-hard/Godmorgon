using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GodMorgon.Enemy;

namespace GodMorgon.Timeline
{
    public class Defend : Action
    {
        //tout les enemies de la scene gagne de la defense
        public override IEnumerator Execute()
        {
            List<EnemyView> enemyInScene = EnemyManager.Instance.GetAllEnemies();
            if(enemyInScene.Count > 0)
                foreach(EnemyView enemy in enemyInScene)
                    enemy.enemyData.AddBlock(TimelineManager.Instance.nbBlockGain);

            Debug.Log("ACTION defend");

            yield return new WaitForSeconds(2f);

            //yield return null;
        }

        public override void Finish()
        {

        }
    }
}
