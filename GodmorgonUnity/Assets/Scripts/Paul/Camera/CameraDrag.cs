using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour
{

    /*
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    */
    public float minScroll = 2;
    public float maxScroll = 4;


    [Header("keyboard parameter")]
    public float mainSpeed = 100.0f; //regular speed

    [Header("mouse parameter")]
    public float dragSpeed = 2;
    private Vector3 dragOrigin;
    public bool cameraDragging = true;

    private bool isDragging = false;
    private Camera gameCamera = null;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
    }

    void Update()
    {
        //deal with the mouse scroll to zoom
        if(Input.mouseScrollDelta.y != 0)
            ZoomCam();


        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            isDragging = true;
        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging)
            MoveCameraByDraging();

        //Keyboard commands
        //float f = 0.0f;
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

    //DeZoom and zoom between 2 and 4
    private void ZoomCam()
    {
        if (gameCamera.orthographicSize >= minScroll && gameCamera.orthographicSize <= maxScroll)
        {
            gameCamera.orthographicSize -= Input.mouseScrollDelta.y * 0.2f;
            //security
            if (gameCamera.orthographicSize < minScroll)
                gameCamera.orthographicSize = minScroll;
            if (gameCamera.orthographicSize > maxScroll)
                gameCamera.orthographicSize = maxScroll;
        }
    }
}