using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioActiveArea : MonoBehaviour {

    public float dmg;
    bool tick;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if (!tick)
            {
                tick = true;
                other.GetComponent<EnemyZombie>().TakeDmg(dmg, 4);
                StartCoroutine(ReTick());
            }
        }
    }
    IEnumerator ReTick()
    {
        yield return new WaitForSeconds(0.2f);
        tick = false;
    }
}
