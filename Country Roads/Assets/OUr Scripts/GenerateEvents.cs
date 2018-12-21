/**
 *  Title:       GenerateEvents.cs
 *  Description: Used to procedurally generate events on the road, basd off of probability and distance between certain events.
 *  Outcomes addressed: Procedural generation, Difficulty management
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEvents : MonoBehaviour
{
    System.Random rand = new System.Random();                                       //to generate random numbers used to generate a random event
    public GameObject[] roads;                                                      //to hold the roads that events need to be generated on
    public bool[] hasEvent;                                                         //to store whether each of the roads has an event, used to check distances between events
    public int eventNumber;                                                         //to store the number of the event generated
    public bool eventsGenerated;                                                    //to determine if events have been generated or not
    public int normalProbability, supernaturalProbability;                          //to hold the probabilities of certain events happening
    public GameObject[] normalEvents;                                               //to store the normal event prefabs
    public GameObject[] supernaturalEvents;                                         //to store the supernatural event prefabs

    public static int treeCounter=0;                                                //to count the number of fallen trees in the game
    public int instanceTreeCounter;                                                 //to count the number of fallen trees in the instance

    // Use this for initialization
    void Start()
    {
        //no trees are counted and no events have been generated
        treeCounter = 0;
        eventsGenerated = false;
        //start the function to set the event probability based on the player's chosen difficulty
        setProbabilities();
        //start a coroutine to generate the events after some time so the roads can be generated first
        StartCoroutine(waitForTrigger());
    }

    //function to set the event probability based on the player's chosen difficulty
    //Outcome addressed: Difficulty management
    void setProbabilities()
    {
        //get the difficulty from a script that preserves the data from the starting scene
        int difficulty = DifficultyChosen.getDifficultyLevel();
        //set the probability based on the difficulty number: the more difficult, the higher is the event probability
        switch (difficulty)
        {
            case 0:
                normalProbability = 20;
                supernaturalProbability = 25;
                break;
            case 1:
                normalProbability = 30;
                supernaturalProbability = 35;
                break;
            case 2:
                normalProbability = 35;
                supernaturalProbability = 60;
                break;
        }
    }

    //function to activate the events after the roads have been generated in order to generate events on them
    IEnumerator waitForTrigger()
    {
        //wait for a little bit to ensure the roads have been generated
        yield return new WaitForSeconds(0.2f);
        //get the roads from the GenerateRoads script
        roads = GetComponent<GenerateRoads>().generatedRoads;
        //create a new array of the roads length for the hasEvent array
        hasEvent = new bool[roads.Length];
        //set all elements of the array to false
        for (int i = 0; i < hasEvent.Length; i++)
        {
            hasEvent[i] = false;
        }
        //call the generateEvents() function to generate events
        generateEvents();
    }

    //function to deactivate all tree meshes: might not be in use any more
    void deactivateAllTrees()
    {
        //find all trees
        GameObject[] trees = GameObject.FindGameObjectsWithTag("tree");
        //go through all the trees
        foreach(GameObject tree in trees)
        {
            //disable their mesh renderer
            tree.GetComponent<MeshRenderer>().enabled = false;
            //if they have a child, disable the child's mesh renderer
            if (tree.gameObject.transform.childCount>0)
            {
                tree.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    //function to generate events
    //outcome addressed: procedural generation
    void generateEvents()
    {
        //go through all the roads
        for (int i = 0; i < roads.Length; i++)
        {
            //if there is enough distance between the last event or it is the first road piece
            if ((i > 0 && enoughRoadDistance(i)) || (i == 0))
            {
                //generate a random integer between 0 and 100, representing 100%
                int randomChance = rand.Next(0, 100);
                //if the random chance is in the range of the normal probability, generate a normal event
                if (randomChance <= normalProbability)
                {
                    generateNormalEvent(i);
                    //determine that the road piece has an event, used for checking the road distance
                    hasEvent[i] = true;
                }
                //if the random chance is in the range of the supernatural probability, generate a supernatural event
                else if (randomChance <= (normalProbability+supernaturalProbability))
                {
                    generateSupernaturalEvent(i);
                    //determine that the road piece has an event, used for checking the road distanc
                    hasEvent[i] = true;
                }
            }
        }
        //since the events have been generated, set the eventsGenerated boolean to true
        eventsGenerated = true;
    }

    //function to check whether there is enough distance between the road
    bool enoughRoadDistance(int index)
    {
        //if the index is greater than or equal 2, the distance is hecked, otherwise it would be out of bounds
        if (index >= 2)
        {
            //if any of the two previous events has an event, return false
            if (hasEvent[index - 1] || hasEvent[index - 2] )
            {
                return false;
            }
            //otherwise, if none of the two previous events has an event, return true
            else if (!(hasEvent[index - 1]) && !(hasEvent[index - 2]))
            {
                return true;
            }
        }
        //otherwise, return false
        return false;
    }

    //function to generate a normal event
    void generateNormalEvent(int index)
    {
        //create a GameObject to store the instantiated event in for modifications after instantiation
        GameObject instance;
        //set the randomEvent integer to -1
        int randomEvent = -1;
        //if there are no fallen trees in total is less than 2 and in the instance (road until next cross piece) is less than 1, the fallen tree can be an event
        if (treeCounter < 2 && instanceTreeCounter < 1)
        {
            randomEvent = rand.Next(0, normalEvents.Length);
        }
        //otherwise, the fallen tree can't be an event
        else
        {
            randomEvent = rand.Next(0, normalEvents.Length - 1);
        }
        //if the event is a fallen tree, increase the counters for fallen trees
        if (randomEvent == 2)
        {
            treeCounter++;
            instanceTreeCounter++;
        }
        //if the random event is a deer, instantiate it appropriately and make it look at a transform pointing in the right direction
        if (randomEvent == 0)
        {
            instance = Instantiate(normalEvents[randomEvent], roads[index].transform.GetChild(0).transform.position, /*normalEvents[randomEvent].transform.rotation*/Quaternion.Euler(0, normalEvents[randomEvent].transform.rotation.y, normalEvents[randomEvent].transform.rotation.z));
            instance.transform.LookAt(roads[index].transform.GetChild(5));
            //rotate the deer by 90 degrees
            instance.transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
        }
        //if the random event is a fog, instantiate it with the rotation of (-90,0,0), as it will be the correct rotation for fogs
        else if (randomEvent == 1)
        {
            instance = Instantiate(normalEvents[randomEvent], roads[index].transform.GetChild(0).transform.position, Quaternion.Euler(-90,0,0));
        }
        //if the random event is a fallen tree, instantiate it appropriately and make it look at a transform pointing in the right direction
        else
        {
            instance = Instantiate(normalEvents[randomEvent], roads[index].transform.GetChild(1).transform.position, Quaternion.Euler(0,roads[index].transform.GetChild(1).transform.rotation.y, roads[index].transform.GetChild(1).transform.rotation.z));
            instance.transform.LookAt(roads[index].transform.GetChild(2));
        }
        //call the instanceSetup() function on the instance object
        instanceSetup(instance, index);
    }

    //function to generate a supernatural event
    void generateSupernaturalEvent(int index)
    {
        //create an instance object for storing and modifying the instantiated event temporarily
        GameObject instance=null;
        //create an object to store which object the event should look at
        GameObject lookAt = null;
        //set the randomEvent ineger to -1
        int randomEvent = -1;
        //if the ghost car can spawn at this point, include it in the event choice
        if (ghostCarCanSpawnHere(index))
        {
            randomEvent = rand.Next(0, supernaturalEvents.Length);
        }
        //if not, choose any other event
        else
        {
            randomEvent = rand.Next(0, supernaturalEvents.Length - 1);
        }
        //do a switch of the random event
        switch (randomEvent)
        {
            //depending on which event is created, choose the appropriate child to look at
            case 0:
                lookAt = roads[index].transform.GetChild(4).gameObject;
                break;
            case 1:
                lookAt = roads[index].transform.GetChild(3).gameObject;
                break;
            case 2:
                lookAt = roads[index].transform.GetChild(2).gameObject;
                break;
            case 3:
                lookAt = roads[index].transform.GetChild(5).gameObject;
                break;
        }
        //if the random event is the ghost child, ensure it is spawned in the middle of the road
        if (randomEvent == 0)
        {
            instance = Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x + supernaturalEvents[randomEvent].transform.rotation.x, roads[index].transform.GetChild(0).transform.rotation.y + supernaturalEvents[randomEvent].transform.rotation.y, roads[index].transform.GetChild(0).transform.rotation.z + supernaturalEvents[randomEvent].transform.rotation.z));        }
        //if the random event is the ghost car event
        else if (randomEvent == supernaturalEvents.Length - 1)
        {
            //choose an appropriate rotation depending on the type of road and spawn it in the middle of the road
            if (roads[index].gameObject.tag == "straightRoad")
            {
                instance = Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x, supernaturalEvents[randomEvent].transform.GetChild(0).transform.rotation.y - 90, roads[index].transform.GetChild(0).transform.rotation.z));
                instanceSetup(instance, index);
            }
            else if (roads[index].gameObject.tag == "curvedroad")
            {
                instance = Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x, supernaturalEvents[randomEvent].transform.GetChild(0).transform.rotation.y + 225, roads[index].transform.GetChild(0).transform.rotation.z));
                instanceSetup(instance, index);
            }
            else
            {
                instance = Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x + supernaturalEvents[randomEvent].transform.rotation.x, roads[index].transform.GetChild(0).transform.rotation.y + supernaturalEvents[randomEvent].transform.rotation.y, roads[index].transform.GetChild(0).transform.rotation.z + supernaturalEvents[randomEvent].transform.rotation.z));
            }
            lookAt.transform.position = new Vector3(lookAt.transform.position.x, instance.transform.position.y, lookAt.transform.position.z);
        }
        //in any other case, spawn at the spawn point of the road, which is on the side of the road
        else
        {
            instance = Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(1).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(1).transform.position.z), Quaternion.Euler(0,0,0));
        }
        //call the instanceSetup function for the event
        instanceSetup(instance, index);
        //make the object look at the lookAt object, ensuring the correct rotation for the instance object
        instance.transform.LookAt(lookAt.transform);

    }

    //function to set up index and event generator of the instance
    void instanceSetup(GameObject instance,int index)
    {
        //if the instance exists
        if (instance != null)
        {
            //set the index of the observeable manager to the current index of the road
            instance.GetComponent<ObserveableManager>().index = index;
            //set the event generator of the observeable manager to the current script, used for accessing the function to generate a new event at the same place
            instance.GetComponent<ObserveableManager>().eventGenerator = this.gameObject.GetComponent<GenerateEvents>();
        }
    }

    //function to check if the ghost car can spawn at a point
    bool ghostCarCanSpawnHere(int index)
    {
        //the index has to be at least 2, since the ghost car needs 2 straight roads to drive on
        if (index >= 2)
        {
            //if the two previous roads are straight roads, return true
            if ((roads[index - 1].gameObject.tag == "straightRoad") && (roads[index - 2].gameObject.tag == "straightRoad"))
            {
                return true;
            }
        }
        //otherwise, return false
        return false;
    }

    //function to generate a single event after a supernatural event has happened, used to cause randomness
    public void generateSingleEvent(int index)
    {
        //generate a random integer between 0 and 100, representing 100%
        int randomChance = rand.Next(0, 100);
        //if the random chance is in the range of the normal probability, generate a normal event
        if (randomChance <= normalProbability)
        {
            generateNormalEvent(index);
            //make sure the road having an event is still set to true
            hasEvent[index] = true;
        }
        //otherwise, generate a supernatural event, since there has been an event and that should still be the same
        else
        {
            generateSupernaturalEvent(index);
            //make sure the road having an event is still set to true
            hasEvent[index] = true;
        }
    }
}