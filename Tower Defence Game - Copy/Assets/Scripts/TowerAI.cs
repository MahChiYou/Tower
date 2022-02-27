using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour
{
    [Header("Required to work")]
    private Transform Target; // Finds target enemy
    public Transform partToRotate; // Rotating gameobject
    bool targetBurger;
    public GameObject burgerRef;
    
    public Transform shootingPos;
    public GameObject ProjectilePref;
    public GameObject coffeeAffected;

    UIcontroller uiController;
    ProjectileUpgradeManager projectileUpgradeManager;

    [Header("Can Be Upgraded")]
    public float fireRate;
    private float initialFireRate;
    private float fireCountdown = 0f;
    public float range = 15f;

    // Animation
    public Animation anim;

    void Start()
    {
        projectileUpgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
        uiController = FindObjectOfType<UIcontroller>();
        initialFireRate = fireRate;
    }

    private void OnDisable()
    {
        Target = null; // Ensure turret will untarget enemy, prevent bug
    }

    void Update()
    {
        UpdateTarget();

        if (Target == null)
            return;

        Vector3 dir = Target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, 10f * Time.deltaTime).eulerAngles;

        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
       
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); //Find all enemies on map
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        // Checks for how many enemies there are
        foreach (GameObject enemy in enemies)
        {
            // Checks if Enemy is a burger. If it is, It will target the closest burger.
            if (GameObject.Find("burger(Clone)"))
            {
                float distanceToBurger = Vector3.Distance(transform.position, GameObject.Find("burger(Clone)").transform.position);

                if(distanceToBurger < range)
                {
                    print("burger found");
                    targetBurger = true;
                    burgerRef = GameObject.Find("burger(Clone)");
                    nearestEnemy = burgerRef;
                }
            }
            
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); // Detects distance from all enemies

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy; // Finds shortest distance from enemy
                nearestEnemy = enemy; 
            }
        }
        
        if (nearestEnemy != null && shortestDistance <= range)
        {
            Target = nearestEnemy.transform; // Sets closest enemy to target enemy
        }
        else
        {
            Target = null; // Voids target if there are no enemies in range
        }
    }

    void Shoot()
    {
        anim.Play();

        GameObject bulletGO =  (GameObject)Instantiate(ProjectilePref, shootingPos.position, shootingPos.rotation);
        Projectile bullet = bulletGO.GetComponent<Projectile>();
        
        if (bullet != null)
        {
            bullet.Seek(Target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coffee"))
        {
            fireRate = projectileUpgradeManager.coffeeFireRate;
            coffeeAffected.SetActive(true);
            Invoke("DisableCoffee", 5.0f);
        }
    }

    void DisableCoffee()
    {
        // Attack Speed Boost
        fireRate = initialFireRate;
        coffeeAffected.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (Target == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(shootingPos.transform.position, Target.transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
