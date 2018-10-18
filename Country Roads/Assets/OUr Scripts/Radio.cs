using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    // is radio switch on?
    public bool radioOn;

    // Use this for initialization
    void Start()
    {
        this.radioOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (radioOn)
        {
            Debug.Log("The radio is ON");
        }
        else if (!radioOn)
        {
            Debug.Log("The radio is OFF");
        }

        if (Input.GetButton("joystick button 0"))
        {
            this.switchRadio();
        }
    }

    // turn radio on or off
    public void switchRadio()
    {
        // negate whatever starting point the radio was at
        this.radioOn = !this.radioOn;
    }
}
