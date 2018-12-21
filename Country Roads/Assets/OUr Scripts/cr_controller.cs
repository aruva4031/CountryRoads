using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityE.UI;

public class cr_controller : MonoBehaviour
{
    public CanvasGroup pause;
    public GameObject radioObject;

    public GameObject car;
    public AudioSource Radio;
	//power is used to see if the cars engine is on for events
    private bool Power = true;

    //  Left Analog Stick Axises
    private float xbox_lsHaxis;
    private float xbox_lsVAxis;
    private bool xbox_ls;

    // Right Analog Stick Axises
    private float xbox_rsHAxis;
    private float xbox_rsVAxis;
    private bool xbox_rs;

    // Trigger Buttons
    private float xbox_lt;
    private float xbox_rt;
    private float xbox_triggers;

    // D-Pad Buttons
    private float dhaxis;
    private float dvaxis;

    // The Other XBox buttons
    private bool xbox_a;
    private bool xbox_b;
    private bool xbox_x;
    private bool xbox_y;
    private bool xbox_lb;
    private bool xbox_rb;
    private bool xbox_back;
    private bool xbox_start;

    //if car is manipulated by hitchhiker ghost, this will be true
    public bool car_manipulated;
    public float randomSteeringValue;
    public float accelerate;

    // when the Start Button is pressed, make sure the Pause menu stays open
    public bool stayPaused = false;

    public void pauseMenuOn()
    {
        pause.alpha = 1;
        pause.interactable = true;
        Time.timeScale = 0f;
        stayPaused = true;
    }

    public void pauseMenuOff()
    {
        pause.alpha = 0;
        pause.interactable = false;
        Time.timeScale = 1f;
        stayPaused = false;
    }


    public void CarShutdown(float time)
    {
        StartCoroutine(PowerOff(time));
        car_manipulated = false;
        randomSteeringValue = 0;
        accelerate = 0;
    }
    void Update()
    {
        //
        xbox_lsHaxis = Input.GetAxis("HorizontalLS") + randomSteeringValue;
        xbox_lsVAxis = (Input.GetAxis("VerticalLS") * -1) + randomSteeringValue;
        xbox_ls = Input.GetButton("XboxLS");
        //
        xbox_rsHAxis = Input.GetAxis("HorizontalRS");
        xbox_rsVAxis = Input.GetAxis("VerticalRS") * -1;
        xbox_rs = Input.GetButton("XboxRS");
        //
        xbox_lt = Input.GetAxis("XboxLT");
        xbox_rt = Input.GetAxis("XboxRT");
        xbox_triggers = Input.GetAxis("XboxTriggers") * -1;
        //
        dhaxis = Input.GetAxis("XboxDPADH");
        dvaxis = Input.GetAxis("XboxDPADV");

        //
        xbox_a = Input.GetButton("XboxA");
        xbox_b = Input.GetButton("XboxB");
        xbox_x = Input.GetButton("XboxX");
        xbox_y = Input.GetButton("XboxY");
        xbox_lb = Input.GetButton("XboxLB");
        xbox_rb = Input.GetButton("XboxRB");
        xbox_back = Input.GetButton("XboxBack");
        xbox_start = Input.GetButton("XboxStart");

        
        // pause the game
        if (xbox_start)
        {
            stayPaused = true;
            
        }

        // pause time itself!
        if (stayPaused)
        {
            pauseMenuOn();
        }
        //if there is power to the car then it would take in inputs normaly
        if (Power)
        {

            car.GetComponent<RCC_CarControllerV3>().gasInput = xbox_rt + accelerate;
            if (!car_manipulated)
            {
                car.GetComponent<RCC_CarControllerV3>().brakeInput = xbox_lt;
            }
            else
            {
                car.GetComponent<RCC_CarControllerV3>().brakeInput = 0;
            }
            car.GetComponent<RCC_CarControllerV3>().steerInput = xbox_lsHaxis;

			//this would control power to the lights that would intreact with other objects using physics
            if (xbox_lb)
            {

                if (car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn == true)
                {
                    car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = false;
                }
                else
                {
                    car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = true;
                }
            }
        }
		//if there is no powere then only steering and braking would work with the car
        else if (!Power)
        {
            car.GetComponent<RCC_CarControllerV3>().steerInput = xbox_lsHaxis;
            if (!car_manipulated)
            {
                car.GetComponent<RCC_CarControllerV3>().handbrakeInput = xbox_lt;
            }
            else
            {
                car.GetComponent<RCC_CarControllerV3>().handbrakeInput = 0;
            }

        }





    }

	//this courotine controls how long the car should be turned off as well as turning off the radio with the ghost car interaction
    private IEnumerator PowerOff(float duration)
    {
        car.GetComponent<RCC_CarControllerV3>().gasInput = 0.0f;
        car.GetComponent<RCC_CarControllerV3>().highBeamHeadLightsOn = false;
        Power = false;
        radioObject.GetComponent<Radio>().carOn = false;
        yield return new WaitForSecondsRealtime(duration);
        Power = true;
        radioObject.GetComponent<Radio>().carOn = true;
    }
}
