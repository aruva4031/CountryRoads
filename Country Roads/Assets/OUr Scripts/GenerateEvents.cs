using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEvents : MonoBehaviour {

    public GameObject[] roads;
    public bool[] hasEvent;
    public bool eventsGenerated;
    public int normalProbability, supernaturalProbability, nothingProbability;
    public GameObject[] normalEvents;
    public GameObject[] supernaturalEvents;
    public GameObject startingRoad;

    // Use this for initialization
    void Start () {
        eventsGenerated = false;
        normalProbability = 10;
        supernaturalProbability = 15;
        nothingProbability = 100 - normalProbability - supernaturalProbability;
        startingRoad= GameObject.FindWithTag("RoadSpawner").GetComponent<GenerateRoads>().copy_road;
    }
	
	// Update is called once per frame
	void Update () {
        if (roads.Length == 0)
        {
            roads = GameObject.FindWithTag("RoadSpawner").GetComponent<GenerateRoads>().roads;
            hasEvent = new bool[roads.Length];
            for(int i = 0; i < hasEvent.Length; i++)
            {
                hasEvent[i] = false;
            }
        }
        if (!eventsGenerated)
        {
            generateEvents();
        }
	}
    void generateEvents()
    {
        for(int i = 0; i < roads.Length; i++)
        {
            if (enoughRoadDistance(i))
            {
                int randomChance = Random.Range(0, 100);
                if (randomChance <= normalProbability)
                {
                    generateNormalEvent(i);
                    hasEvent[i] = true;
                }
                else if (randomChance <= supernaturalProbability)
                {
                    generateSupernaturalEvent(i);
                    hasEvent[i] = true;
                }
            }
        }
    }

    bool enoughRoadDistance(int index)
    {
        if (hasEvent[index - 1] || hasEvent[index - 2] || hasEvent[index - 3])
        {
            return false;
        }
        else if (!(hasEvent[index - 1] )&&!( hasEvent[index - 2] )&&! hasEvent[index - 3])
        {
            return true;
        }
        return false;
    }

    void generateNormalEvent(int index)
    {
        int randomEvent = Random.Range(0, normalEvents.Length);
        Instantiate(normalEvents[randomEvent], roads[index].transform.position, roads[index].transform.rotation);
    }

    void generateSupernaturalEvent(int index)
    {
        int randomEvent = -1;
        if (ghostCarCanSpawnHere(index))
        {
            randomEvent = Random.Range(0, supernaturalEvents.Length);
        }
        else
        {
            randomEvent = Random.Range(0, supernaturalEvents.Length-1);
        }
        Instantiate(supernaturalEvents[randomEvent], roads[index].transform.position, roads[index].transform.rotation);
    }

    bool ghostCarCanSpawnHere(int index)
    {
        if((roads[index+1].gameObject.tag=="straightRoad")&& (roads[index + 2].gameObject.tag == "straightRoad"))
        {
            return true;
        }
        return false;
    }
}
