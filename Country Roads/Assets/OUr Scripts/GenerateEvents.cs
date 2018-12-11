using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEvents : MonoBehaviour
{

    System.Random rand = new System.Random();
    public GameObject[] roads;
    public bool[] hasEvent;
    public int eventNumber;
    public bool eventsGenerated;
    public int normalProbability, supernaturalProbability, nothingProbability;
    public GameObject[] normalEvents;
    public GameObject[] supernaturalEvents;
    // public GameObject startingRoad;

    // Use this for initialization
    void Start()
    {
        eventsGenerated = false;
        if (normalProbability == 0)
        {
            normalProbability = 10;
        }
        if (supernaturalProbability == 0)
        {
            supernaturalProbability = 15;
        }
        nothingProbability = 100 - normalProbability - supernaturalProbability;
        //deactivateAllTrees();
        //startingRoad= GameObject.FindWithTag("RoadSpawner").GetComponent<GenerateRoads>().copy_road;
    }

    // Update is called once per frame
    void Update()
    {
        //if (roads.Length == 0)
        //{
        //    roads = GameObject.FindWithTag("RoadSpawner").GetComponent<GenerateRoads>().roads;
        //    hasEvent = new bool[roads.Length];
        //    for (int i = 0; i < hasEvent.Length; i++)
        //    {
        //        hasEvent[i] = false;
        //    }
        //}
        if (roads.Length != 0 && !eventsGenerated)
        {
            hasEvent = new bool[roads.Length];
            for (int i = 0; i < hasEvent.Length; i++)
            {
                hasEvent[i] = false;
            }
            generateEvents();
        }
    }

    void deactivateAllTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("tree");
        foreach(GameObject tree in trees)
        {
            tree.gameObject.GetComponent<MeshRenderer>().enabled = false;
            tree.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        }
    }
    void generateEvents()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if ((i > 0 && enoughRoadDistance(i)) || (i == 0))
            {
                generateSingleEvent(i);
            }
        }
        eventsGenerated = true;
    }

    bool enoughRoadDistance(int index)
    {
        if (index >= 3)
        {
            if (hasEvent[index - 1] || hasEvent[index - 2] || hasEvent[index - 3])
            {
                return false;
            }
            else if (!(hasEvent[index - 1]) && !(hasEvent[index - 2]) && !hasEvent[index - 3])
            {
                return true;
            }
        }
        return false;
    }

    void generateNormalEvent(int index)
    {
        GameObject instance;
        int randomEvent = rand.Next(0, normalEvents.Length);
        instance=Instantiate(normalEvents[randomEvent], roads[index].transform.GetChild(1).transform.position, normalEvents[randomEvent].transform.rotation);
        instanceSetup(instance, index);
    }

    void generateSupernaturalEvent(int index)
    {
        GameObject instance;
        int randomEvent = -1;
        Debug.Log("SL: " + supernaturalEvents.Length);
        if (ghostCarCanSpawnHere(index))
        {
            randomEvent = rand.Next(0, supernaturalEvents.Length);
        }
        else
        {
            randomEvent = rand.Next(0, supernaturalEvents.Length - 1);
        }
        Debug.Log("RE: " + randomEvent);
        if (randomEvent == supernaturalEvents.Length - 1)
        {
            if (roads[index].gameObject.tag == "straightRoad")
            {
                instance=Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x, roads[index].transform.GetChild(0).transform.rotation.y, roads[index].transform.GetChild(0).transform.rotation.z));
                instanceSetup(instance, index);
            }
            else if(roads[index].gameObject.tag == "curvedroad")
            {
                instance=Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x, roads[index].transform.GetChild(0).transform.rotation.y + 45, roads[index].transform.GetChild(0).transform.rotation.z));
                instanceSetup(instance, index);
            }
        }
        else
        {
            instance=Instantiate(supernaturalEvents[randomEvent], roads[index].transform.GetChild(1).transform.position, supernaturalEvents[randomEvent].transform.rotation);
            instanceSetup(instance, index);
        }
    }
    void instanceSetup(GameObject instance,int index)
    {
        if (instance != null)
        {
            instance.GetComponent<ObserveableManager>().index = index;
            instance.GetComponent<ObserveableManager>().eventGenerator = this.gameObject.GetComponent<GenerateEvents>();
        }
    }

    bool ghostCarCanSpawnHere(int index)
    {
        if (index <= supernaturalEvents.Length - 3)
        {
            if ((roads[index + 1].gameObject.tag == "straightRoad") && (roads[index + 2].gameObject.tag == "straightRoad"))
            {
                return true;
            }
        }
        return false;
    }

    public void generateSingleEvent(int index)
    {
        int randomChance = rand.Next(0, 100);
        Debug.Log("Random Chance: " + randomChance);
        if (randomChance <= normalProbability)
        {
            Debug.Log("Generating normal event");
            generateNormalEvent(index);
            hasEvent[index] = true;
        }
        else if (randomChance <= (normalProbability + supernaturalProbability))
        {
            Debug.Log("Generating supernatural event");
            generateSupernaturalEvent(index);
            hasEvent[index] = true;
        }
    }
}
