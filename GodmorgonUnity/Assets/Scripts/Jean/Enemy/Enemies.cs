using GodMorgon.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "NewEnemiesList", menuName = "Enemies List")]
    public class Enemies : ScriptableObject
    {
        public List<Enemy> enemiesSOList = new List<Enemy>();
    }
}
