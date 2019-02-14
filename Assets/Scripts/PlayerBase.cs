using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour {

    float health;
    public Image healthBar;
    public GameObject smokePrefab;
    public GameObject explosionPrefab;
    bool isSmoke;
    public bool baseDestroyed;

    void Start () {
        health = 100;
	}
	
    public void TakeDmg(float dmg)
    {
        health -= dmg;
        gameObject.GetComponentInChildren<Text>().text = "Health: " + health;
        healthBar.fillAmount -= dmg * 0.01f;
        if(health <= 75 && !isSmoke)
        {
            isSmoke = true;
            GameObject smoke = Instantiate(smokePrefab, transform.position, smokePrefab.transform.rotation);
            smoke.transform.SetParent(gameObject.transform);
            if(health <= 0)
            {
                Destroy(smoke);
            }
        }
        LoseGame();
    }

    public void LoseGame()
    {
        if (health <= 0 && !baseDestroyed)
        {           
            Destroy(Instantiate(explosionPrefab, transform.position, Quaternion.identity),5f);
            Debug.Log("LOSE GAME");
            FindObjectOfType<GameControls>().GameIsOver();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            baseDestroyed = true;
        }
    }
}
