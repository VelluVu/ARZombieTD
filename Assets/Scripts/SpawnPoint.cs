using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    public List<GameObject> creatures = new List<GameObject>();
    Transform plane;
    bool spawning;
    bool spawnRdy;
    public float spawnCD;
    float incrementSpawnSpeedTime;

    private void Start()
    {
        plane = GameObject.FindGameObjectWithTag("Ground").transform;
        spawning = false;
        spawnRdy = true;
        transform.SetParent(plane);
        StartCoroutine(IncrementSpawnSpeed());
        incrementSpawnSpeedTime = creatures.Count * 10;
    }

    private void Update()
    {
        SpawnActivate();
    }

    public void SpawnActivate()
    {

        if (spawnRdy)
        {
            spawning = true;
            spawnRdy = false;
            GameObject creature = Instantiate(creatures[Random.Range(0, creatures.Count)], transform.position, Quaternion.identity);
            creature.transform.SetParent(plane);       
            StartCoroutine(SpawnCD());
        }
        
    }

    IEnumerator SpawnCD()
    {
        yield return new WaitForSeconds(spawnCD);
        spawnRdy = true;
    }

    IEnumerator IncrementSpawnSpeed()
    {
        yield return new WaitForSeconds(incrementSpawnSpeedTime);
        if (spawnCD > 0)
        {
            spawnCD -= 1;
        }
    }
}
