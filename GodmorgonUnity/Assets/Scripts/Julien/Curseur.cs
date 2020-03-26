using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curseur : MonoBehaviour
{
    
    public Texture2D cursorTexture;
    public Texture2D defaultTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    public float m_Speed = 1.0f;
    Vector2 m_MousePosition;
    private void Update()
    {
        m_MousePosition = Input.mousePosition;
       // m_MousePosition.z = -10;
        m_MousePosition = Camera.main.ScreenToWorldPoint(m_MousePosition);
        transform.position = m_MousePosition;
    }

    private void OnMouseEnter()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        //transform.SetPositionAndRotation(hotSpot, null );
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
