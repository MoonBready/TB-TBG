using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    public MapParty party;
    public List<Encounter> encounters = new List<Encounter>();
    public MapData data;

    public static MapManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        UpdateEncounterStates();

        party.transform.position = encounters[data.curEncounter].transform.position;
    }

    void UpdateEncounterStates()
    {
        for(int i = 0; i < encounters.Count; i++) 
        { 
            if(i <= data.curEncounter)
            {
                encounters[i].SetState(Encounter.State.Visited);
            }
            else if(i == data.curEncounter + 1) 
            {
                encounters[i].SetState(Encounter.State.CanVisit);
            }
            else if (i > data.curEncounter + 1)
            {
                encounters[i].SetState(Encounter.State.Locked);
            }
        }
    }

    public void MoveParty (Encounter encounter)
    {
        party.MoveToEncounter(encounter, OnPartyArriveAtEncounter);
    }

    private void OnPartyArriveAtEncounter(Encounter encounter)
    {
        data.curEncounter = encounters.IndexOf(encounter);
        GameManager.curEnemySet = encounter.enemySet;
        SceneManager.LoadScene("Battle");
    }
}
