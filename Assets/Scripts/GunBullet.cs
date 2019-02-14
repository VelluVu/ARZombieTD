using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBullet : MonoBehaviour
{

    public float dmg;
    public bool isMissile;
    public bool isNuke;
    public GameObject radioActive;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other != null && !isMissile && !isNuke)
            {
                other.gameObject.GetComponent<EnemyZombie>().TakeDmg(dmg, 0);
                Destroy(gameObject);
            }
            else if (other != null && isMissile && !isNuke)
            {
                Collider[] cols = Physics.OverlapSphere(other.transform.position, 0.15f);
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(15f, other.transform.position, 0.15f);
                other.gameObject.GetComponent<EnemyZombie>().TakeDmg(dmg, 1);

                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i] != null && cols[i].CompareTag("Enemy") && cols[i] != other)
                    {
                        cols[i].gameObject.GetComponent<EnemyZombie>().TakeDmg(dmg * 0.35f, 0);

                    }
                }

                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                MeshRenderer[] arr = gameObject.GetComponentsInChildren<MeshRenderer>();

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i].enabled = false;
                }
                Destroy(gameObject, 5f);
            }
            else if (other != null && isNuke && !isMissile)
            {
                Collider[] cols = Physics.OverlapSphere(other.transform.position, 0.3f);
                other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(25f, other.transform.position, 0.3f);
                other.gameObject.GetComponent<EnemyZombie>().TakeDmg(dmg, 3);
                GameObject radioactivefield = Instantiate(radioActive, other.gameObject.transform.position, Quaternion.identity) as GameObject;
                Destroy(radioactivefield, 5f);

                for (int i = 0; i < cols.Length; i++)
                {
                    if (cols[i] != null && cols[i].CompareTag("Enemy") && cols[i] != other)
                    {
                        cols[i].gameObject.GetComponent<EnemyZombie>().TakeDmg(dmg * 0.50f, 0);

                    }
                }
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
                MeshRenderer[] arr = gameObject.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i].enabled = false;
                }
                Destroy(gameObject, 5f);
            }
        }      
    }
}
