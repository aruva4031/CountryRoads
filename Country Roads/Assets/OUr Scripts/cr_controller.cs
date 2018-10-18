using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cr_controller : MonoBehaviour {

	public GameObject car;

	//
	private float xbox_lsHaxis;
	private float xbox_lsVAxis;
	private bool xbox_ls;
	//
	private float xbox_rsHAxis;
	private float xbox_rsVAxis;
	private bool xbox_rs;
	//
	private float xbox_lt;
	private float xbox_rt;
	private float xbox_triggers;
	//
	private float dhaxis;
	private float dvaxis;
	//
	private bool xbox_a;
	private bool xbox_b;
	private bool xbox_x;
	private bool xbox_y;
	private bool xbox_lb;
	private bool xbox_rb;
	private bool xbox_back;
	private bool xbox_start;  


	void Update()
	{
		//
		xbox_lsHaxis = Input.GetAxis("HorizontalLS");
		xbox_lsVAxis = Input.GetAxis("VerticalLS") * -1;
		xbox_ls = Input.GetButton("XboxLS");
		//
		xbox_rsHAxis = Input.GetAxis("HorizontalRS");
		xbox_rsVAxis = Input.GetAxis("VerticalRS") * -1;
		xbox_rs = Input.GetButton("XboxRS");
		//
		xbox_lt = Input.GetAxis("XboxLT");
		xbox_rt = Input.GetAxis("XboxRT");
		xbox_triggers = Input.GetAxis ("XboxTriggers") * -1;
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

		car.GetComponent<RCC_CarControllerV3> ().gasInput = xbox_rt;
		car.GetComponent<RCC_CarControllerV3> ().brakeInput = xbox_lt;
		car.GetComponent<RCC_CarControllerV3> ().steerInput = xbox_lsHaxis;


	}
}
