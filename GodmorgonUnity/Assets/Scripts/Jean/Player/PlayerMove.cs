﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Grid roomsGrid;
    private List<Vector3Int> nearestTilesList = new List<Vector3Int>();

    private float moveH, moveV;
    private float gridX, gridY;

    private bool hasMoved;
    private bool canMove;
    private bool moveValidate;

    Vector3Int nextTileCoordinate;
    Vector3Int currentTileCoordinate;

    public float speed = 1.0f;

    void Start()
    {
        gridX = roomsGrid.gameObject.GetComponent<Grid>().cellSize.x;
        gridY = roomsGrid.gameObject.GetComponent<Grid>().cellSize.y;

        UpdateNearestTilesList();
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

        if (canMove)
        {
            Move(nextTileCoordinate);
        }
    }

    /**
     * Lancée dans le GameManager lorsque la carte movement est droppée qqpart
     * Active le mouvement du player si la carte est droppée sur une tile voisine du player
     */
    public bool UseMoveCard()
    {
        //Transpose la position de la souris au moment du drop de carte en position sur la grid, ce qui donne donc la tile sur laquelle on a droppé la carte
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        nextTileCoordinate = roomsGrid.WorldToCell(mouseWorldPos);

        //Transpose la position scène du player en position grid 
        currentTileCoordinate = roomsGrid.WorldToCell(transform.position);

        if (nearestTilesList.Contains(nextTileCoordinate))
        {
            canMove = true;
            Debug.Log("Carte move droppée dans une case proche");
        }
        else
        {
            canMove = false;
            Debug.Log("Carte move droppée dans une case : " + nextTileCoordinate);
        }
        return canMove;
    }

    //Fonction pour se déplacer sur une tile en cliquant dessus
    public void Move(Vector3Int tileDirection)
    {
        //la position de la tile étant en bas du losange, on ajoute 0.2f en hauteur pour cibler le milieu de la tile
        Vector3 nextPos = roomsGrid.CellToWorld(tileDirection) + new Vector3(0, gridY / 2 + 0.2f, 10);
        
        //si le joueur arrive sur la target position
        if (Vector3.Distance(transform.position, nextPos) < 0.001f)
        {
            UpdateNearestTilesList();   //on update les tiles voisines
            canMove = false;
            hasMoved = true;
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime); //avance jusqu'à la tile cliquée       
    }

    /**
     * Donne les 4 cases les plus proches du joueur 
     * Lancée au Start et lorsque le joueur arrive sur une nouvelle tile
     */
    public void UpdateNearestTilesList()
    {
        nearestTilesList.Clear();   //Clear la liste de tiles avant de placer les nouvelles
        currentTileCoordinate = roomsGrid.WorldToCell(transform.position);   //On transpose la position scène du player en position grid 
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x + 1, currentTileCoordinate.y, currentTileCoordinate.z - 10));   //Ajoute la case en +1 en x dans la liste des cases voisines
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x - 1, currentTileCoordinate.y, currentTileCoordinate.z - 10));   //Ne pas oubliez le -10 en z car la grid est à -10 en z
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y + 1, currentTileCoordinate.z - 10));
        nearestTilesList.Add(new Vector3Int(currentTileCoordinate.x, currentTileCoordinate.y - 1, currentTileCoordinate.z - 10));
    }

    /**
     * Set la direction pour se déplacer avec les touches du clavier
     * Lancée dans l'update lors d'un appui sur les flèches directionnelles
     */
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
