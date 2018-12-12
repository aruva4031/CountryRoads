using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int carDamage;
    public float speed;
    public float rvm;
    public RCC_CarControllerV3 carController;

	// Use this for initialization
	void Start () {
        carDamage = 10;
        carController = GetComponent<RCC_CarControllerV3>();

	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public int decreaseBy(float magnitude)
    {
        if (magnitude > 15)
        {
            return 3;
        }
        else if (magnitude > 5)
        {
            return 2;
        }
        return 1;
    }

    public void decreaseCarDamage(float magnitude)
    {
        Debug.Log("magnitude: " + magnitude);
        int decreasingAmount = decreaseBy(magnitude);
        if (carDamage > 0)
        {
            carDamage-=decreasingAmount;
        }
        if (carDamage < 0)
        {
            carDamage = 0;
        }
        if (carDamage == 0)
        {
            //speed is zero
            carController.maxspeed = 0f;
        }
        else if (carDamage == 5)
        {
            //speed is half
            carController.maxspeed *= 0.5f;
        }
        else if (carDamage == 7)
        {
            //speed is 3/4
            carController.maxspeed *= 0.75f;
        }
    }
}
