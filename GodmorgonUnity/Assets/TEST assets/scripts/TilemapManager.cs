using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TEST
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField]
        private playerMovement player;

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

        public Vector3 GetCurrentTileCenterPosition(Vector3 worldPosition)
        {
            Vector3 newPosition;
            Vector3Int currentTilePos = ground.WorldToCell(worldPosition);

            newPosition = ground.CellToWorld(currentTilePos) + new Vector3(0, 0.25f);
            Debug.Log("position : " + newPosition);

            return newPosition;
        }

        public Vector3Int GetTilePosition(Vector3 worldPosition)
        {
            return ground.WorldToCell(worldPosition);
        }

        public void PlayerTileOnColor(Vector3 worldPlayerPosition)
        {
            //to give back it's original color to last tile where the player was
            if (playerTilePosition != null)
                ground.SetColor(playerTilePosition, Color.white);

            playerTilePosition = ground.WorldToCell(worldPlayerPosition);

            ground.SetTileFlags(playerTilePosition, TileFlags.None);
            ground.SetColor(playerTilePosition, Color.black);
        }

        public void DirectionForPlayer(int orientation)
        {
            Vector3 direction = Vector3.zero;
            switch (orientation)
            {
                //UP_LEFT
                case 1:
                    direction = new Vector3(-0.5f, 0.25f);
                    break;
                //UP_RIGHT
                case 2:
                    direction = new Vector3(0.5f, 0.25f);
                    break;
                //DOWN_LEFT
                case 3:
                    direction = new Vector3(-0.5f, -0.25f);
                    break;
                //DOWN_RIGHT
                case 4:
                    direction = new Vector3(0.5f, -0.25f);
                    break;
            }

            player.Move(direction);
            PlayerTileOnColor(player.transform.position);

        }
    }
}