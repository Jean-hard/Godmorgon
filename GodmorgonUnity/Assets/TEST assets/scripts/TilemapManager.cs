using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TEST
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField]
        private Tilemap ground;

        private Vector3Int playerTilePosition;

        private static TilemapManager _instance;

        public static TilemapManager Instance { get { return _instance; } }
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
    }
}