/**
 *  Title:       PublicObjects.cs
 *  Description: The purpose of this script is mainly to make objects within the scene easier accessible for procedurally generated AIs and any scripts,
 *  but especially those that are not in the scene from the beginning.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicObjects : MonoBehaviour {

    public GameObject hikerPose2;       //the pose the ghost hitchhiker has when sitting in the car
    public GameObject musicianPose2;    //the pose the ghost musician has when sitting in the car
    public GameObject carPosition;      //the position the ghost hitchhiker has when sitting in the car
    public GameObject carPosition2;     //the position the ghost musician has when sitting in the car
    public GameObject radio;            //the radio
}
