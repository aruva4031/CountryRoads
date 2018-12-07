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
	private GameObject previousRoad;
	private int currentIndex;

	private Quaternion rotation;

	private int flagged;

	private string [] tagTurns = new string[3];
	


	// Use this for initialization
	void Start () {

		flagged = 0;
		point1 = Vector3.zero;
		point2 = Vector3.zero;

		rotation =  Quaternion.Euler (0f, 0f, 0f);

		for (int i = 0; i < 100; i++) {
			//set the flag to zero
			flagged = 0;

			//choose a random road piece 
			index = Random.Range (0, roads.Length);

		//	Debug.Log ("loop #:" + i);


			///////////////////////////////FIX lIMITATION (BELOW) ///////////////////////////

			checkTurns ();


			//Fix the way the roads is being stored

			if (roads[index].CompareTag ("curvedroadleft")|| roads[index].CompareTag ("curvedroadright")) {
				storeTurn (roads[index].tag);
			}
				

			//////////////////////FIX LIMITATION (ABOVE)//////////////////////

			//save the previous road prefab
			previousIndex = index;

			//ensure the y position never changes 
			position = new Vector3(50f, 0.1f,0f);

			//if it is the first road piece 
			if (i == 0) {

			//	while(roads[index].CompareTag("curvedroadleft") )
			//	{
			//		index = Random.Range (0, roads.Length);
			//	}

				//store the instantiated clone of the object in copy road 
				copy_road = Instantiate (roads [index], position, roads [index].transform.rotation);

				//get the position of the end of the road 
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;

//				if (copy_road.CompareTag ("curvedroadleft")|| copy_road.CompareTag ("curvedroadright")) {
//					storeTurn (copy_road.tag);
//				}

			}
			else {
				//store the previous clone of the road
				previousRoad = copy_road;

				//get the position of the end of the road and ensure the y variable is never changed and above the terrain 
				getPoint1 = new Vector3(getPoint1.x, 0.1f, getPoint1.z);

				//CHECK ROAD CONDITIONS 
				rightCurvedRoad ();
				previousStraightRoad();
				leftCurvedRoad();

				//if none of the road conditions are met then instantate a road
				if (flagged == 0) {
					//instanted the randomly selected road using the previous end of road position and the rotation of the previously instantiated road 
					copy_road = Instantiate (roads [index], getPoint1, previousRoad.transform.rotation );
				}

				//get the location of the where the next road piece should be instantiated 
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;
//				if (copy_road.CompareTag ("curvedroadleft")|| copy_road.CompareTag ("curvedroadright")) {
//					storeTurn (copy_road.tag);
//				}

			}


		}

	}

	// Update is called once per frame
	void Update () {

	}

	//--------------------------Turn Restrictions---------------------------------

	//STORES THE TURNS IN ARRAY AND ACTS LIKE A QUEUE 
	public void storeTurn(string tag)
	{

		if (tagTurns [0] == null) {
			tagTurns [0] = tag;
			Debug.Log ("first tag was null ");
			Debug.Log ("Now its: " + tagTurns [0]);

		} else if (tagTurns [1] == null) {
			tagTurns [1] = tag;
			Debug.Log ("second tag was null ");
			Debug.Log ("Now its: " + tagTurns [0]);
		} else if (tagTurns [2] == null) {
			if (tagTurns [0] == tagTurns [1]) {
				rerollRoadPiece (tagTurns [0]);
				tagTurns[2] = tag; 
			} else {
				tagTurns [2] = tag;
			}
			Debug.Log ("third tag was null ");
			Debug.Log ("Now its: " + tagTurns [0]);

		} else {
			tagTurns [0] = tagTurns [1];
			tagTurns [1] = tagTurns [2];
			tagTurns [2] = tag;
		}





	}

	//REROLL PIECE GIVEN A PIECE NOT TO SELECT
	public int rerollRoadPiece(string tag)
	{
		while (roads [index].CompareTag(tag)) {
			index = Random.Range (0, roads.Length);
		}
		return index;
	}

	//CHECK SO THAT THE TURNS ARE NOT GOING TO BE IN A CIRCLE
	public void checkTurns()
	{
		int rightcount = 0;
		int leftcount = 0;

		if (tagTurns [0] == null || tagTurns [1] == null ) {
			Debug.Log ("One of the elements was empty");
			return;
		}

		foreach (string tag in tagTurns) {
			if (tag == "curvedroadleft") {
				leftcount++;
			}
			if (tag == "curvedroadright") {
				rightcount++;
			}
		}


		if (leftcount >= 2) {
			rerollRoadPiece ("curvedroadleft");
		}
		if (rightcount >= 2) {
			rerollRoadPiece ("curvedroadright");
		}
						
	}



	//--------------------------CHECK ROAD CONDITIONS ----------------------------------------

	public void leftCurvedRoad()
	{
		////////////////////////////FIX ROTATION/////////////////////////////////////////////////////////
		//if the previous road is a straight Road
		if (previousRoad.CompareTag ("straightRoad")) {
			//if the current road is going to be a curved left road
			if (roads [index].CompareTag ("curvedroadleft")) {
				//rotate
				rotation = Quaternion.Euler (0f, 140f, 0f) * previousRoad.transform.rotation;
				//instantiate
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}

		}


		//////////////////////TESTING////////////////////////////////
		//if the previous road is a curved left road 
		if (previousRoad.CompareTag ("curvedroadleft")) {
			//the current road is going to be a curved left road 
			if (roads [index].CompareTag ("curvedroadleft")) {
				//rotate 
				rotation = Quaternion.Euler (0f, 275f, 0f) * previousRoad.transform.rotation;
				//instantiate
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}

			//the current road is going to be a curved right road 
			if (roads [index].CompareTag ("curvedroadright")) {
				//rotate 
				rotation = Quaternion.Euler (0f, 180f, 0f) * previousRoad.transform.rotation;
				//instantiate
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}
		}

		///////////////////////////TESTING/////////////////////////
		if (previousRoad.CompareTag ("curvedroadleft")) {
			//the current road is going to be a straight road
			if (roads [index].CompareTag ("straightRoad")) {
				//rotate 
				rotation = Quaternion.Euler (0f, 140f, 0f) * previousRoad.transform.rotation;
				//instantiate
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}
		}



	}

	public void rightCurvedRoad()
	{
		//if the previous road is a curved road
		if (previousRoad.CompareTag ("curvedroadright")) {
			//and the current raod is going to be a straight road
			if (roads [index].CompareTag ("straightRoad")) {
				//rotate the straight road to fit the curved road
				rotation = Quaternion.Euler (0f, 45f, 0f) * previousRoad.transform.rotation;
				//instantiate with the clone and with the new rotation
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;

			} 
			//or the current road is going to be a curved left road
			if(roads[index].CompareTag("curvedroadleft")) {
				//and the current road is going to be a curved road rotate the road accordinatly 
				rotation = Quaternion.Euler (0f, 180f, 0f) * previousRoad.transform.rotation;
				//instantaite the copy road with the new rotation 
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}

			//or the current road is going to be a curved right road
			if(roads[index].CompareTag("curvedroadright")) {
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
			//or if the current road is going to be a curved right road 
			if (roads [index].CompareTag ("curvedroadright")) {
				//roatate the road accordinatly 
				rotation = Quaternion.Euler(0f,45f,0f) * previousRoad.transform.rotation;
				//instantiate the road
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				//call the flag
				flagged++;
			}

			//////////////////////TESTING////////////////////////////////

//			//or if the current road is going to be a curve left road 
//			if (roads [index].CompareTag ("curvedroadleft")) {
//				//rotate the road accordunatly 
//				rotation = Quaternion.Euler (0f, 45f, 0f) * previousRoad.transform.rotation;
//				//instantiate the road
//				copy_road = Instantiate (roads [index], getPoint1, rotation);
//				//call the flag
//				flagged++;
//			}

		}
	}



}
