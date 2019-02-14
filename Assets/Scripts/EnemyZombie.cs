using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyZombie : MonoBehaviour {

    Vector3 move;
    public float moveSpeed;
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> bloodSpills = new List<GameObject>();
    public List<GameObject> dmgTypeEffects = new List<GameObject>();
    public List<GameObject> sounds = new List<GameObject>();
    public AudioClip freezeSound;
    public Transform target;
    public Transform plane;
    public GameObject dmgTypeEffect;
    public float enemyHealth;
    public bool gamePause;
    Animator eAnim;
    Rigidbody rb;
    bool attackRdy;
    bool spilledBlood;
    public bool isDead;

    private void Start()
    {
        isDead = false;
        enemyHealth = 100;
        attackRdy = true;
        eAnim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        moveSpeed = Random.Range(0.01f,0.03f);
        move.Set(0, 0, moveSpeed * Time.deltaTime);
        
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < arr.Length; i++)
        {
            targets.Add(arr[i]);
        }

        target = targets[Random.Range(0, targets.Count)].transform;
    }

    private void Update()
    {
        if(gamePause)
        {
            gameObject.GetComponent<AudioSource>().Pause();
        }
        
        transform.position.Set(transform.position.x, plane.position.y , transform.position.z);
        if(transform.position.y <= -1.0f)
        {
            Destroy(gameObject);
        }
        if (!gamePause)
        {
            gameObject.GetComponent<AudioSource>().UnPause();
            EnemyMovement();
        }
       
    }

    public void GamePaused()
    {
        gamePause = true;
    }

    public void GameContinue()
    {
        gamePause = false;
    }

    public void EnemyMovement()
    {

        if (Vector3.Distance(transform.position, target.position) < 0.15f)
        {
            eAnim.SetBool("Walk", false);
            if (attackRdy)
            {
                gameObject.GetComponent<AudioSource>().Stop();
                attackRdy = false;
                StartCoroutine(EnemyAttack());
                StartCoroutine(AttackCD());
            }

        }
        else
        {
            eAnim.SetBool("Walk", true);
            transform.LookAt(target.transform);         
            rb.transform.Translate(move);
            
        }
    }

    public void TakeDmg(float dmg, int dmgType)
    {
        
        int type = dmgType;
        dmgTypeEffect = dmgTypeEffects[type];
        enemyHealth -= dmg;

        if (type == 4)
        {
            SkinnedMeshRenderer[] arr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int j = 0; j < arr.Length; j++)
            {
                arr[j].material.color = Color.green;
            }
        }
        else if(type == 2)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 0.1f);
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].gameObject.CompareTag("Enemy"))
                {
                    SkinnedMeshRenderer[] arr = cols[i].gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
                    for (int j = 0; j < arr.Length; j++)
                    {
                        arr[j].material.color = Color.blue;
                    }
                    if (cols[i].gameObject.GetComponent<EnemyZombie>().move.z >= 0.00005f)
                    {
                        cols[i].gameObject.GetComponent<EnemyZombie>().move.z *= 0.95f;
                        cols[i].gameObject.GetComponent<AudioSource>().clip = freezeSound;
                        cols[i].gameObject.GetComponent<AudioSource>().spatialBlend = 1.0f;
                        cols[i].gameObject.GetComponent<AudioSource>().maxDistance = 100f;
                        cols[i].gameObject.GetComponent<AudioSource>().Play();


                    }
                }
            }
        }
        else
        {
            Destroy(Instantiate(bloodSpills[Random.Range(0, bloodSpills.Count)], transform.position, Quaternion.identity), 4f);
            Destroy(Instantiate(sounds[1], transform.position, Quaternion.identity), 1f);
            Destroy(Instantiate(sounds[Random.Range(5, 7)], transform.position, Quaternion.identity), 1f);
            Destroy(Instantiate(dmgTypeEffect, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.identity), 2f);
        }
       
        
        
        if(enemyHealth <= 0 && !isDead)
        {
            isDead = true;
            GameControls.euros += 5;
            if(GameControls.euros == 500)
            {
                GameControls.UnlockAchievement(GPGSIds.achievement_new_mr_drumb);
            }
            eAnim.SetBool("Walk", false);
            Destroy(Instantiate(sounds[0], transform.position, Quaternion.identity), 2f);
            eAnim.SetTrigger("Die");
            gameObject.GetComponent<EnemyZombie>().move.z = 0;
            GameControls.IncrementAchievement(GPGSIds.achievement_zombies_slain, 1);
            GameControls.IncrementAchievement(GPGSIds.achievement_50_zombies_slain, 1);
            GameControls.IncrementAchievement(GPGSIds.achievement_100_zombies_slain, 1);
            GameControls.IncrementAchievement(GPGSIds.achievement_200_zombies_slain, 1);
            GameControls.IncrementAchievement(GPGSIds.achievement_500_zombies_slain, 1);
            GameControls.IncrementAchievement(GPGSIds.achievement_over_9000, 1);
            GameControls.zombieCount++;
            Destroy(gameObject, 1f);
        }
    }
    
    IEnumerator AttackCD()
    {
        
        yield return new WaitForSeconds(2f);
        attackRdy = true;

    }

    IEnumerator EnemyAttack()
    {
        eAnim.SetTrigger("Attack");
        Destroy(Instantiate(sounds[Random.Range(2, 4)], transform.position, Quaternion.identity), 1f);
        target.GetComponentInParent<PlayerBase>().TakeDmg(1f);
        yield return new WaitForSeconds(2f);
        
    }

}
