
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenerateRoads : MonoBehaviour {

    public GameObject precursorPoint;
    public bool special;
    public string direction;
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


	/// Alex's Code ////////////////////////////////////////////////////////////////////
	//public GameObject endingPoint;
	public GameObject ensureDirectionPoint;
	public GameObject startingPoint;

	public GameObject testingPeice;
	private GameObject movingTestingPeice;
	public GameObject[] testinPoints;

	private bool reachedEndPoint = false;
	private int i = 0;
	private bool SmartConnectionbegin = false;

	//public float bottomContstraint, topConstraint, leftConstraint, rightConstraint;
	private float[] distances = new float[3];
	private float lastDistance = 9999;
	public GameObject[] generatedRoads;
	public int Limiter;
	private Vector3 firstTarget;
	private bool[] smartPeiceValidator = new bool[3];
	//left is position 0
	//straight is position 1
	//right is position 2
	public bool test = false;
	private bool hasGenerationCompleted(){
		
		//Debug.Log (generatedRoads.Length);
		//Debug.Log (generatedRoads [generatedRoads.Length - 1].GetComponent<roadscript> ().isRoadconnected ());
	
		return false;
	}

	private int smartConnection(Vector3 targetPosition){
		int selectedPeice = 0;
		int counter = 0;
		float shortestDistance = 9999;


		foreach (GameObject point in testinPoints){
			if(point.GetComponent<RoadCollisionDetector>().connectionMade == true){
				SmartConnectionbegin = false;
				selectedPeice = counter;
				return selectedPeice;
			}
			if (point.GetComponent<RoadCollisionDetector> ().hitARoad) {
				smartPeiceValidator [counter] = false;
				distances [counter] = 9999;
			} 
			else {
				distances [counter] = Vector3.Distance (point.transform.position, targetPosition);
			}
			counter++;
		}
		for(int i = 0; i < distances.Length; i++){
			if (distances [i] != 9999) {
				if (shortestDistance == 9999) {
					shortestDistance = distances [i];
					selectedPeice = i;
				} 
				else {
					if (distances [i] < shortestDistance) {
						shortestDistance = distances [i];
						selectedPeice = i;
					}
				}
			}
		}
		return selectedPeice;

	}


	private void moveTestPeice(){
		int counter = 0;
		if (movingTestingPeice != null) {
			Destroy (movingTestingPeice);
		}
		foreach(GameObject point in testinPoints){
			smartPeiceValidator [counter] = true;
			distances [counter] = 9999;
			counter++;
		}
			

		if (previousRoad.CompareTag ("straightRoad")) {
			movingTestingPeice = Instantiate (testingPeice, getPoint1, previousRoad.transform.rotation);
		}

		if (previousRoad.CompareTag ("curvedroadright")) {
			rotation = Quaternion.Euler (0f, 45f, 0f) * previousRoad.transform.rotation;

			movingTestingPeice = Instantiate (testingPeice, getPoint1, rotation);
		}

		if (previousRoad.CompareTag ("curvedroadleft")) {
			rotation = Quaternion.Euler (0f, 135f, 0f) * previousRoad.transform.rotation;

			movingTestingPeice = Instantiate (testingPeice, getPoint1, rotation);
		}
		testinPoints [0] = movingTestingPeice.gameObject.transform.GetChild (0).gameObject;
		testinPoints [1] = movingTestingPeice.gameObject.transform.GetChild (1).gameObject;
		testinPoints [2] = movingTestingPeice.gameObject.transform.GetChild (2).gameObject;

	}


	private GameObject[] AppentoArray(GameObject[] OldArray, GameObject newItem){
		int newLength = OldArray.Length + 1;
		GameObject[] newArray = new GameObject[newLength];
		for (int i = 0; i < OldArray.Length; i++) {
			newArray [i] = OldArray [i];
		}
		newItem.GetComponent<roadscript> ().generator = this.gameObject;
		newArray [newLength-1] = newItem;
		return newArray;
	}

	private GameObject[] ResizeArray(GameObject[] OldArray, int newSize){
		int newLength = newSize + 1;
		//Debug.Log (newLength);
		//Debug.Log (OldArray.Length);
		GameObject[] newArray = new GameObject[newLength];
		for (int i = 0; i < newLength; i++) {
			newArray [i] = OldArray [i];
			//Debug.Log (i);
		}


		return newArray;
	}
		
	/// Alex's Code ////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {
		firstTarget = ensureDirectionPoint.transform.position;
		smartPeiceValidator [0] = true;
		smartPeiceValidator [1] = true;
		smartPeiceValidator [2] = true;
		flagged = 0;
		point1 = Vector3.zero;
		point2 = Vector3.zero;

		rotation =  Quaternion.Euler (0f, 0f, 0f);

	}

	// Update is called once per frame
	void Update () {
		if(!reachedEndPoint){
			while (!SmartConnectionbegin) {
				//set the flag to zero
				flagged = 0;

				//choose a random road piece 
				index = Random.Range (-2, roads.Length);
				if (index < 0) {
					index = 1;
				}

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
					i++;
                    //	while(roads[index].CompareTag("curvedroadleft") )
                    //	{
                    //		index = Random.Range (0, roads.Length);
                    //	}
                    position = startingPoint.transform.position;
                    position.y = 0.1f;
                    //store the instantiated clone of the object in copy road
                    index = 1;
                    if (direction == "up")
                    {
                        rotation = roads[index].transform.rotation;
                       
                    }
                    else if (direction == "left")
                    {
                        rotation = Quaternion.Euler(0f, -90f, 0f) * roads[index].transform.rotation;
                    }
                    else if (direction == "down")
                    {

                        rotation = Quaternion.Euler(0f, 180f, 0f) * roads[index].transform.rotation;
                    }
                    else
                    {
                        rotation = Quaternion.Euler(0f, 90f, 0f) * roads[index].transform.rotation;
                    }
                    copy_road = Instantiate (roads [index], position, rotation);

					//get the position of the end of the road 
					getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;
					generatedRoads = AppentoArray (generatedRoads, copy_road);

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
						generatedRoads = AppentoArray (generatedRoads, copy_road);
					}

					//get the location of the where the next road piece should be instantiated 
					getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;
					//				if (copy_road.CompareTag ("curvedroadleft")|| copy_road.CompareTag ("curvedroadright")) {
					//					storeTurn (copy_road.tag);
					//				}

				}

				if(generatedRoads.Length >= Limiter){
					previousRoad = copy_road;
					SmartConnectionbegin = true;
				}
			}
			//if two lefts add a right
			//if two rights add a left
			int leftcounter = 0;
			int rightcounter = 0;
			for (int j = 0; j < tagTurns.Length; j++){
				if(tagTurns[j] == "curvedroadright"){
					rightcounter++;
				}
				else if(tagTurns[j] == "curvedroadleft"){
					leftcounter++;
				}
			}
			if (leftcounter == 2){
				index = 2;
				getPoint1 = new Vector3(getPoint1.x, 0.1f, getPoint1.z);
				previousRoad = copy_road;

				rightCurvedRoad ();
				previousStraightRoad();
				leftCurvedRoad();

				previousRoad = copy_road;
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;
			}
			else if(rightcounter == 2){
				index = 0;
				getPoint1 = new Vector3(getPoint1.x, 0.1f, getPoint1.z);
				previousRoad = copy_road;

				rightCurvedRoad ();
				previousStraightRoad();
				leftCurvedRoad();

				previousRoad = copy_road;
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;
			}
			int infinteLoop = -20;
			while(SmartConnectionbegin && !test){
				if(hasGenerationCompleted() || test){
					SmartConnectionbegin = false;
					break;
				}
				flagged = 0;
				//Debug.Log ("SmartConnectionStarted");
				moveTestPeice ();
				index = smartConnection (firstTarget);
				//Debug.Log (index + " on loop " + infinteLoop);
				if(index == -1 || infinteLoop == 50){
					SmartConnectionbegin = false;
					break;
				}
				getPoint1 = new Vector3(getPoint1.x, 0.1f, getPoint1.z);
				previousRoad = copy_road;

				rightCurvedRoad ();
				previousStraightRoad();
				leftCurvedRoad();


				if (flagged == 0) {
					copy_road = Instantiate (roads [index], getPoint1, previousRoad.transform.rotation );
					generatedRoads = AppentoArray (generatedRoads, copy_road);
				}
				previousRoad = copy_road;
				getPoint1 = copy_road.gameObject.transform.GetChild(0).transform.position;

				infinteLoop++;
			}
			infinteLoop = 0;
			reachedEndPoint = true;
			flagged = 0;
		}
		if(reachedEndPoint && flagged == 0){
			int firstconnection = 9999;
			for(int k = 0; k < generatedRoads.Length; k++) {
				if(generatedRoads[k].GetComponent<roadscript>().isRoadconnected()){
					firstconnection = k;
					break;
				}
			}
			for(int k = firstconnection+1; k < generatedRoads.Length; k++) {
				Destroy (generatedRoads[k]);
			}
			if(firstconnection != 9999){
				generatedRoads = ResizeArray (generatedRoads, firstconnection);
                Destroy(testingPeice);
				flagged++;
			}

		}
			
	}

	//--------------------------Turn Restrictions---------------------------------

	//STORES THE TURNS IN ARRAY AND ACTS LIKE A QUEUE 
	public void storeTurn(string tag)
	{

		if (tagTurns [0] == null) {
			tagTurns [0] = tag;
			//Debug.Log ("first tag was null ");
			//Debug.Log ("Now its: " + tagTurns [0]);

		} else if (tagTurns [1] == null) {
			tagTurns [1] = tag;
			//Debug.Log ("second tag was null ");
			//Debug.Log ("Now its: " + tagTurns [0]);
		} else if (tagTurns [2] == null) {
			if (tagTurns [0] == tagTurns [1]) {
				rerollRoadPiece (tagTurns [0]);
				tagTurns[2] = tag; 
			} else {
				tagTurns [2] = tag;
			}
			//Debug.Log ("third tag was null ");
			//Debug.Log ("Now its: " + tagTurns [0]);

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
			//Debug.Log ("One of the elements was empty");
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
				generatedRoads = AppentoArray (generatedRoads, copy_road);
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
				generatedRoads = AppentoArray (generatedRoads, copy_road);
				//call the flag
				flagged++;
			}

			//the current road is going to be a curved right road 
			if (roads [index].CompareTag ("curvedroadright")) {
				//rotate 
				rotation = Quaternion.Euler (0f, 180f, 0f) * previousRoad.transform.rotation;
				//instantiate
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				generatedRoads = AppentoArray (generatedRoads, copy_road);
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
				generatedRoads = AppentoArray (generatedRoads, copy_road);
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
				generatedRoads = AppentoArray (generatedRoads, copy_road);
				//call the flag
				flagged++;

			} 
			//or the current road is going to be a curved left road
			if(roads[index].CompareTag("curvedroadleft")) {
				//and the current road is going to be a curved road rotate the road accordinatly 
				rotation = Quaternion.Euler (0f, 180f, 0f) * previousRoad.transform.rotation;
				//instantaite the copy road with the new rotation 
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				generatedRoads = AppentoArray (generatedRoads, copy_road);
				//call the flag
				flagged++;
			}

			//or the current road is going to be a curved right road
			if(roads[index].CompareTag("curvedroadright")) {
				//and the current road is going to be a curved road rotate the road accordinatly 
				rotation = Quaternion.Euler (0f, 90f, 0f) * previousRoad.transform.rotation;
				//instantaite the copy road with the new rotation 
				copy_road = Instantiate (roads [index], getPoint1, rotation);
				generatedRoads = AppentoArray (generatedRoads, copy_road);
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
				generatedRoads = AppentoArray (generatedRoads, copy_road);
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

