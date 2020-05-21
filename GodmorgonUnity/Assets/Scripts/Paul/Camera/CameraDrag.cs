using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{

    /*
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

    [Header("keyboard parameter")]
    public float mainSpeed = 100.0f; //regular speed
    public float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    [Header("mouse parameter")]
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    public bool cameraDragging = true;
    public float outerLeft = -10f;
    public float outerRight = 10f;
    public float outerDown = -10f;
    public float outerUp = 10f;

    private bool isDragging = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > 285)
            isDragging = true;

        //Input.GetMouseButtonDown(0)

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging)
            MoveCameraByDraging();

        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        transform.Translate(p);
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.Z))
        {
            p_Velocity += new Vector3(0, 1 * mainSpeed, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1 * mainSpeed, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            p_Velocity += new Vector3(-1 * mainSpeed, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1 * mainSpeed, 0, 0);
        }
        return p_Velocity;
    }

    private void MoveCameraByDraging()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            transform.Translate(move, Space.World);
            dragOrigin = Input.mousePosition;
            return;
        }
    }
}