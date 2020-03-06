using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Grid moveGrid;
    private float moveH, moveV;
    private float gridX, gridY;

    private bool hasMoved;
    private bool canMove;

    Vector3Int nextTileCoordinate;
    Vector3Int currentTileCoordinate;

    public float speed = 1.0f;

    void Start()
    {
        gridX = moveGrid.gameObject.GetComponent<Grid>().cellSize.x;
        gridY = moveGrid.gameObject.GetComponent<Grid>().cellSize.y;
    }

    void Update()
    {
        // INPUT CLAVIER
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
        /*
        // CLICK ON TILE
        if (Input.GetMouseButton(0) && !hasMoved)
        {
            //transpose la position de la souris au moment du clique en position sur la grid, ce qui donne donc la tile sur laquelle on a cliqué
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nextTileCoordinate = moveGrid.WorldToCell(mouseWorldPos);
            currentTileCoordinate = moveGrid.WorldToCell(transform.position);
            
            //check si on clique bien sur une case à côté de nous
            if (currentTileCoordinate.x != nextTileCoordinate.x && currentTileCoordinate.y != nextTileCoordinate.y)
                return;
            if (Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) <= 1 && Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) >= -1
                && Mathf.Abs(nextTileCoordinate.y) - Mathf.Abs(currentTileCoordinate.y) <= 1 && Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) >= -1)
                canMove = true;
        }*/

        if (canMove)
        {
            Move(nextTileCoordinate);
        }
    }

    //CORRIGER LA LIMITATION AUX CASES VOISINES
    public void UseMoveCard()
    {
        //transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nextTileCoordinate = moveGrid.WorldToCell(mouseWorldPos);
        currentTileCoordinate = moveGrid.WorldToCell(transform.position);

        if (currentTileCoordinate.x != nextTileCoordinate.x && currentTileCoordinate.y != nextTileCoordinate.y)
        {
            Debug.Log("Carte move droppée dans une case trop éloignée");
            return;
        }
        //check si on clique bien sur une case à côté de nous
        if (Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) <= 1 && Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) >= -1
            && Mathf.Abs(nextTileCoordinate.y) - Mathf.Abs(currentTileCoordinate.y) <= 1 && Mathf.Abs(nextTileCoordinate.x) - Mathf.Abs(currentTileCoordinate.x) >= -1)
            canMove = true;
    }

    //Fonction pour se déplacer sur une tile en cliquant dessus
    public void Move(Vector3Int tileDirection)
    {
        //la position de la tile étant en bas du losange, on ajoute 0.2f en hauteur pour cibler le milieu de la tile
        Vector3 nextPos = moveGrid.CellToWorld(tileDirection) + new Vector3(0, gridY / 2 + 0.2f, 10);
        
        if (Vector3.Distance(transform.position, nextPos) < 0.001f)
        {
            canMove = false;
            hasMoved = true;
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime); //avance jusqu'à la tile cliquée       
    }

    //Fonction pour se déplacer avec les touches du clavier
    public void SetDirection()
    {
        Vector3 direction = Vector3.zero;

        if (moveH < 0)
        {
            //si on veut bouger en haut à gauche
            if (moveV > 0)
                direction = new Vector3(-gridX / 2, gridY / 2);

            //si on veut bouger en bas à gauche
            else if (moveV < 0)
                direction = new Vector3(-gridX / 2, -gridY / 2);

            transform.position += direction;
        }

        else if (moveH > 0)
        {
            //si on veut bouger en haut à droite
            if (moveV > 0)
                direction = new Vector3(gridX / 2, gridY / 2);

            //si on veut bouger en bas à droite
            else if (moveV < 0)
                direction = new Vector3(gridX / 2, -gridY / 2);

            transform.position += direction;
        }
    }
}
