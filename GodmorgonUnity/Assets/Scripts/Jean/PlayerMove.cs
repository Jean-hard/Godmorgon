using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Grid moveGrid;
    private float moveH, moveV;
    private float gridX, gridY;
    private Vector3 direction;

    bool hasMoved;
    /*
    public void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int coordinate = moveGrid.WorldToCell(mouseWorldPos);
            this.gameObject.transform.position = coordinate;
            Debug.Log(coordinate);
        }
    }*/

    void Start()
    {
        gridX = moveGrid.gameObject.GetComponent<Grid>().cellSize.x;
        gridY = moveGrid.gameObject.GetComponent<Grid>().cellSize.y;
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

            SetDirection();
        }
    }

    public void SetDirection()
    {
        direction = Vector3.zero;

        if (moveH < 0)
        {
            //si on veut bouger en haut à gauche
            if (moveV > 0)
            {
                direction = new Vector3(-gridX / 2, gridY / 2);
            }
            //si on veut bouger en bas à gauche
            else if (moveV < 0)
            {
                direction = new Vector3(-gridX / 2, -gridY / 2);
            }
            transform.position += direction;
        }

        else if (moveH > 0)
        {
            //si on veut bouger en haut à droite
            if (moveV > 0)
            {
                direction = new Vector3(gridX / 2, gridY / 2);
            }
            //si on veut bouger en bas à droite
            else if (moveV < 0)
            {
                direction = new Vector3(gridX / 2, -gridY / 2);
            }
            transform.position += direction;
        }

        TilemapManager.Instance.PlayerTileOnColor(transform.position);
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
        //TilemapManager.Instance.PlayerTileOnColor(transform.position);
    }
}
