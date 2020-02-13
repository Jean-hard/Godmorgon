using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField]
    private Tilemap ground;

    [SerializeField]
    Vector3Int playerTilePosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTilePosition = ground.WorldToCell(transform.position);
        //transform.position = ground.GetCellCenterWorld(playerTilePosition);
        transform.position = ground.CellToWorld(playerTilePosition);
        Debug.Log(" START player tilemap position : " + playerTilePosition);
        Debug.Log(" START player position By Cell center : " + ground.GetCellCenterWorld(playerTilePosition));
        Debug.Log(" START player position By Cell : " + ground.CellToWorld(playerTilePosition));
    }

    // Update is called once per frame
    void Update()
    {
        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;

        playerTilePosition = ground.WorldToCell(transform.position);
        //Debug.Log("player tilemap position : " + playerTilePosition);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveH, moveV);
        //transform.position = ground.GetCellCenterWorld(playerTilePosition);
        transform.position = ground.CellToWorld(playerTilePosition);
        //Debug.Log("player position By Cell : " + ground.GetCellCenterWorld(playerTilePosition));
    }
}
