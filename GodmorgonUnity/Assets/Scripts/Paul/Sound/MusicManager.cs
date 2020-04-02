using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public AK.Wwise.Event musicEvent;

    public AK.Wwise.State PlayerState;
    public AK.Wwise.State RingmasterState;

    // Start is called before the first frame update
    void Start()
    {
        musicEvent.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            PlayerState.SetValue();
        else if (Input.GetKeyDown(KeyCode.Keypad2))
            RingmasterState.SetValue();
    }

    public void 

    
}
