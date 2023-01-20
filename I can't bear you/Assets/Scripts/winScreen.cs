using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class winScreen : MonoBehaviour
{
    [SerializeField] private GameObject winSkull;
    [SerializeField] private GameObject[] winNpc;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Transform positionToSpawnSkull;
    [SerializeField] private WinNpc tempNpc;
    [SerializeField] private int skullToSpawn;
    [SerializeField] private int npcWhoFleeToSpawn;
    [SerializeField] private TextMeshProUGUI killText;
    [SerializeField] private TextMeshProUGUI fleeText;
    private int textKillCount;
    private int textFleeCount;

    private void OnEnable()
    { 
        skullToSpawn = NpcManager.instance.npcCountkilled;
        npcWhoFleeToSpawn = NpcManager.instance.npcCountfleed;
        StartCoroutine(SpawnSkulls());
        StartCoroutine(SpawnNpc());
        killText.gameObject.SetActive(true);
        fleeText.gameObject.SetActive(true);
    }
    
    IEnumerator SpawnSkulls()
    {
        if (skullToSpawn > 0)
        {
            Instantiate(winSkull, positionToSpawnSkull.position, Random.rotation);
            skullToSpawn--;
            yield return new WaitForSeconds(0.1f);
            UpdateTextSkull();
            StartCoroutine(SpawnSkulls());
        }
    }

    IEnumerator SpawnNpc()
    {
        if (npcWhoFleeToSpawn > 0)
        {
            tempNpc = Instantiate(winNpc[Random.Range(0,winNpc.Length)], wayPoints[0].position, Quaternion.identity).GetComponent<WinNpc>();
            tempNpc.enabled = true;
            tempNpc.LoadWaypoints(wayPoints);
            npcWhoFleeToSpawn--;
            yield return new WaitForSeconds(0.3f);
            UpdateFleeText();
            StartCoroutine(SpawnNpc());
        }
    }

    private void UpdateTextSkull()
    {
        textKillCount++;
        killText.text = "Killed Npc: " + textKillCount;
    }

    private void UpdateFleeText()
    {
        textFleeCount++;
        fleeText.text = "Flee Npc: " + textFleeCount;
    }
}
