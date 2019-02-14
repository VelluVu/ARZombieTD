using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{

    public GameObject buildSpot;
    public List<GameObject> cubes = new List<GameObject>();
    public Transform plane;
    Vector3 start;


    private void Start()
    {     
        start = transform.position;
        for (int x = 0; x < 12; x++)
        {
            for (int z = 0; z < 22; z++)
            {

                GameObject cube = Instantiate(buildSpot, start + new Vector3(x * 0.6f , transform.position.y, z * 0.6f), Quaternion.identity) as GameObject;           
                cubes.Add(cube);
            }
        }

        foreach (var item in cubes)
        {
            item.transform.SetParent(plane);          
        }
    }
    
}
