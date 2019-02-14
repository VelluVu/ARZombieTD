using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunTurret : MonoBehaviour {

    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> muzzleEffects = new List<GameObject>();
    public GameObject bulletPrefab;
    public GameObject shootTarget;
    public GameObject sound;
    public GameObject effect;
    public GameObject beam;
    public GameObject levelUpEffect;
    public Transform muzzle;
    public Transform headMuzzle;
    Animator anim;
    public float shootCD;
    public float damage;
    public float rotationSpeed;
    public int damageType;
    public int turretType;
    bool hasTarget;
    bool shotRdy;
    bool isBeaming;
    bool grantedXp;
    public int level;
    public float startXp;
    public float currentXP;
    public float xpToLevel;

    private void Start()
    {
        startXp = 0;
        currentXP = 0;
        xpToLevel = 50;
        level = 1;
        hasTarget = false;
        shotRdy = true;
        isBeaming = false;
        anim = gameObject.GetComponent<Animator>();
        effect = muzzleEffects[turretType];
    }

    private void Update()
    {
        
        Target();

    }

    public void GetXP(float xp)
    {
        currentXP += xp;
        
        if (xpToLevel <= currentXP)
        {
            level++;
            Destroy(Instantiate(levelUpEffect, transform.position, Quaternion.identity), 4f);    

            GameControls.IncrementAchievement(GPGSIds.achievement_tower_upgrade, 1);
            shootCD *= 0.90f;
            damage *= 1.10f;
            rotationSpeed *= 1.10f;
            xpToLevel *= 1.2f;
            currentXP = startXp;
        }
    }

    public void Target()
    {
        
        if(hasTarget && shootTarget != null)
        {
            Quaternion q = Quaternion.LookRotation(shootTarget.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);

            if (!grantedXp && shootTarget != null && shootTarget.GetComponent<EnemyZombie>().isDead == true)
            {
                grantedXp = true;
                GetXP(10f);
                targets.Remove(shootTarget);
                shootTarget = null;
                hasTarget = false;

            }
            if(shootTarget != null)
                ShootTarget();
        }
        else
        {
            
            foreach (var item in targets)
            {
                if(shootTarget == null && item != null)
                {
                    shootTarget = item;                                                                                           
                }
            }
            StartCoroutine(NewTarget());
        }
    }

    IEnumerator NewTarget()
    {
        yield return new WaitForSeconds(0.5f);
        hasTarget = true;
        grantedXp = false;
    }

    void ShootTarget()
    {
        Quaternion lookDir = Quaternion.LookRotation(shootTarget.transform.position - transform.position);
        if (shotRdy && shootTarget != null && lookDir == transform.rotation)
        {
            
            shotRdy = false;
            Destroy(Instantiate(effect, muzzle.transform),2f);          
            Destroy(Instantiate(sound, muzzle.transform), 2f);
            anim.SetTrigger("Shot");
            if (turretType == 2)
            {
                //Beam Gun
                GameObject b = Instantiate(beam) as GameObject;
                LineRenderer line = b.GetComponent<LineRenderer>();
                line.SetPosition(0, muzzle.position);
                line.SetPosition(1, new Vector3(shootTarget.transform.position.x, shootTarget.transform.position.y + 0.1f, shootTarget.transform.position.z));
                Destroy(b, 0.25f);
                shootTarget.GetComponent<EnemyZombie>().TakeDmg(damage, damageType);
            }
            else if (turretType == 0)
            {
                //Basic Gun
                GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
                Rigidbody bRb = bullet.AddComponent<Rigidbody>();
                bRb.AddForce(transform.forward * 3f, ForceMode.Impulse);
                bRb.AddForce(transform.up * 0.75f, ForceMode.Impulse);
                bRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                bullet.GetComponent<GunBullet>().dmg = damage;
                Destroy(bullet, 5f);
            } else if(turretType == 1 || turretType == 3)
            {
                //Missile and nuke
                GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation) as GameObject;
                Rigidbody bRb = bullet.AddComponent<Rigidbody>();
                bRb.AddForce(transform.forward * 2f, ForceMode.Impulse);
                bRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                bRb.useGravity = false;
                bullet.GetComponent<GunBullet>().dmg = damage;
                Destroy(bullet, 5f);
            }

            StartCoroutine(ShootCD());
            
        }
    }
    IEnumerator ShootCD()
    {
        yield return new WaitForSeconds(shootCD);
        shotRdy = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            if (other != null)
            {
                targets.Add(other.gameObject);
                if (!hasTarget)
                {
                    shootTarget = other.gameObject;
                    StartCoroutine(NewTarget());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(shootTarget != null && hasTarget && shootTarget.GetComponent<Collider>() == other)
        {
            shootTarget = null;
            hasTarget = false;
            targets.Remove(shootTarget);
            
        }

        if(other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject != null && shootTarget != other.gameObject)
            {
                targets.Remove(other.gameObject);
                
            }
        }
    }
}
