using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Transform bar;
    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("Bar");
    }

    public void SetHealth(float newHealthValue)
    {
        bar.localScale = new Vector3(newHealthValue * 0.01f, 1f);
    }
}
