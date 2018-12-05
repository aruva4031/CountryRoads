using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEvents : MonoBehaviour
{

    System.Random rand = new System.Random();
    public GameObject[] roads;
    public bool[] hasEvent;
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
    void generateEvents()
    {
        for (int i = 0; i < roads.Length; i++)
        {
            if ((i > 0 && enoughRoadDistance(i)) || (i == 0))
            {
                int randomChance = rand.Next(0, 100);
                Debug.Log("Random Chance: " + randomChance);
                if (randomChance <= normalProbability)
                {
                    Debug.Log("Generating normal event");
                    generateNormalEvent(i);
                    hasEvent[i] = true;
                }
                else if (randomChance <= (normalProbability + supernaturalProbability))
                {
                    Debug.Log("Generating supernatural event");
                    generateSupernaturalEvent(i);
                    hasEvent[i] = true;
                }
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
        int randomEvent = rand.Next(0, normalEvents.Length);
        Instantiate(normalEvents[randomEvent], roads[index].transform.GetChild(1).transform.position, normalEvents[randomEvent].transform.rotation);
    }

    void generateSupernaturalEvent(int index)
    {
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
            Instantiate(supernaturalEvents[randomEvent], new Vector3(roads[index].transform.GetChild(0).transform.position.x, supernaturalEvents[randomEvent].transform.position.y, roads[index].transform.GetChild(0).transform.position.z), Quaternion.Euler(roads[index].transform.GetChild(0).transform.rotation.x, roads[index].transform.GetChild(0).transform.rotation.y, roads[index].transform.GetChild(0).transform.rotation.z));
        }
        else
        {
            Instantiate(supernaturalEvents[randomEvent], roads[index].transform.GetChild(1).transform.position, supernaturalEvents[randomEvent].transform.rotation);
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
}
