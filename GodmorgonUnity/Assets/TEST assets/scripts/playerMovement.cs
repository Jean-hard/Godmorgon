using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerMovement : MonoBehaviour
{
    private float moveH, moveV;
    private Vector3 direction;

    bool hasMoved;

    private Vector3Int playerTilePosition;

    private void Start()
    {
        transform.position = TilemapManager.Instance.GetCurrentTileCenterPosition(transform.position);

        TilemapManager.Instance.PlayerTileOnColor(transform.position);
        //PlayerTileOnColor();
    }

    void Update()
    {
        moveH = Input.GetAxisRaw("Horizontal");
        moveV = Input.GetAxisRaw("Vertical");

        if (moveH == 0)
        {
            hasMoved = false;
        }
        else if (moveH != 0 && !hasMoved)
        {
            hasMoved = true;

            GetMovementDirection();
        }

    }

    public void GetMovementDirection()
    {
        direction = Vector3.zero;

        if (moveH < 0)
        {
            if (moveV > 0)
            {
                direction = new Vector3(-0.5f, 0.25f);
            }
            else if (moveV < 0)
            {
                direction = new Vector3(-0.5f, -0.25f);
            }
            transform.position += direction;
        }

        else if (moveH > 0)
        {
            if (moveV > 0)
            {
                direction = new Vector3(0.5f, 0.25f);
            }
            else if (moveV < 0)
            {
                direction = new Vector3(0.5f, -0.25f);
            }
            transform.position += direction;
        }

        TilemapManager.Instance.PlayerTileOnColor(transform.position);
        //PlayerTileOnColor();
    }


    public enum directionOrientation
    {
        UP_LEFT = 1,
        UP_RIGHT,
        DOWN_LEFT,
        DOWN_RIGHT
    }

    public void Move(Vector3 direction)
    {
        transform.position += direction;
    }

    public void DirectionButton(int orientation)
    {
        direction = Vector3.zero;
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

        transform.position += direction;
        TilemapManager.Instance.PlayerTileOnColor(transform.position);

    }

    ////to know on wich tile the player is
    //private void PlayerTileOnColor()
    //{
    //    playerTilePosition = ground.WorldToCell(transform.position);
    //    playerTilePosition = TilemapManager.Instance.GetTilePosition(transform.position);

    //    ground.SetTileFlags(playerTilePosition, TileFlags.None);
    //    ground.SetColor(playerTilePosition, Color.black);
    //}
}
