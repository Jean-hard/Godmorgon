using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GodMorgon.Models
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class Enemy : ScriptableObject
    {
        public new string name;
        public Vector3 nextPosition;

    }
}