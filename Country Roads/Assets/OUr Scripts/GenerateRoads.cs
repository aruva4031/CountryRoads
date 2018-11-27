using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateRoads : MonoBehaviour {

	public GameObject[] roads; 

	private int index = 0;
	private Vector3 position;
	private int previousIndex;

	private Vector3 point1;
	private Vector3 point2;

	private Vector3 getPoint1;

	private GameObject copy_road;
    public GameObject starting_road;
	private GameObject previousRoad;
	private int currentIndex;

	private Quaternion rotation;

	private int flagged;


	// Use this for initialization
	void Start () {
		flagged = 0;
		point1 = Vector3.zero;
		point2 = Vector3.zero;

		rotation =  Quaternion.Euler (0f, 0f, 0f);

		for (int i = 0; i < 10; i++) {
			//set the flag to zero
			flagged = 0;

			//choose a random road piece 
			index = Random.Range (0, roads.Length);

			//save the previous road prefab
			previousIndex = index;

			//ensure the y position never changes 
			position = new Vector3(50f, 0.1f,0f);

			//if it is the first road piece 
			if (i == 0) {
				//store the instantiated clone of the object in copy road 
				copy_road = Instantiate (roads [index], position, roads [index].transform.rotation);

				//get the position of the end of the road 
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;

			}
			else {
				//store the previous clone of the road
				previousRoad = copy_road;

				//get the position of the end of the road and ensure the y variable is never changed and above the terrain 
				getPoint1 = new Vector3(getPoint1.x, 0.1f, getPoint1.z);

				//CHECK ROAD CONDITIONS 
				curvedRoad ();
				previousStraightRoad ();

				//if none of the road conditions are met then instantate a road
				if (flagged == 0) {
					//instanted the randomly selected road using the previous end of road position and the rotation of the previously instantiated road 
					copy_road = Instantiate (roads [index], getPoint1, previousRoad.transform.rotation );
				}

				//get the location of the where the next road piece should be instantiated 
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;

			}
		}

	}

	// Update is called once per frame
	void Update () {

	}

	//--------------------------CHECK ROAD CONDITIONS ----------------------------------------

	public void curvedRoad()
	{
		//if the previous road is a curved road
		if (previousRoad.CompareTag ("curvedroad")) {
			//and the current raod is going to be a straight road
			if (roads [index].CompareTag ("straightRoad")) {
				//rotate the straight road to fit the curved road
				rotation = Quaternion.Euler (0f, 45f, 0f) * previousRoad.transform.rotation;
				//instantiate with the clone and with the new rotation
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;

				//CREATE A LEFT ROAD CONFIGURATION
			} else {
				//and the current road is going to be a curved road rotate the road accordinatly 
				rotation = Quaternion.Euler (0f, 90f, 0f) * previousRoad.transform.rotation;
				//instantaite the copy road with the new rotation 
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}
		}
	}

	public void previousStraightRoad()
	{
		//if the previous road was a straight road 
		if(previousRoad.CompareTag("straightRoad"))
		{
			//and if the current road is going to be a curved road 
			if (roads [index].CompareTag ("curvedroad")) {
				//roatate the road accordinatly 
				rotation = Quaternion.Euler(0f,45f,0f) * previousRoad.transform.rotation;
				//instantiate the road
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}

		}
	}



}
