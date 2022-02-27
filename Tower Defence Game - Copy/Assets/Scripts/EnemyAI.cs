using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    // Basic Enemy Script (Soda), no need change anything.
    [Header("Movement")]
    public float startSpeed;
    public float speed;
    public GameObject enemyModel;
    public GameObject ice;
    

    [Header("For Abilities")]
    [Header("Ability 1 Freeze")]
    public float freezeDuration;
    public bool freezed;
    //public GameObject freezeProjectile;

    [Header("Ability 2 Slow")]
    public float slowedDuration;
    public float startSlowedDuration;
    public bool slowed;

    // Check which spawner this spawned from, to determine which waypoint to take.
    private Transform target;
    public int waypointIndex = 0;

    //public GameObject spawnOrigin;

    [Header("HEALTH")]
    public float health;
    public float maxHealth;
    public Image healthBar;

    public bool dead;
    public GameObject sodatoCoinDeath;
    public GameObject patataCoinDeath;
    public GameObject borgorCoinDeath;

    UIcontroller castleHealth;

    public GameObject hitEffect;

    // References
    private Player player;
    ProjectileUpgradeManager projectileUpgradeManager;
    public Waypoints waypoints;

    [Header("Enemy type")]

    [Header("Fries")] // Split up into 3 other enemy, Is Basic Enemy Script.
    public GameObject smallerFry;
    public GameObject smallerFrySpawn;
    public GameObject smallerFry1Spawn;
    public GameObject smallerFry2Spawn;

    public enum Enemy { Sodato, Fries, Borgor, SmallerFries}
    public Enemy enemyDropdown;

    public static EnemyAI primary;
    public static List<EnemyAI> secondaries;

    private void Start()
    { 
        projectileUpgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
        
        // Make primary & secondary system for waypoints as well.
        target = waypoints.WPoint[waypointIndex];
        startSpeed = speed;
        castleHealth = FindObjectOfType<UIcontroller>();
        player = FindObjectOfType<Player>();

        health = maxHealth;

        if(enemyDropdown == Enemy.SmallerFries)
        {
            StartCoroutine(SmallerFryBegin());
        }
    }

    IEnumerator Fries()
    {
        Spawner.enemiesAlive += 3;
        print("spawned");
        GameObject temp = Instantiate(smallerFry, smallerFrySpawn.transform.position, smallerFrySpawn.transform.rotation);
        GameObject temp1 = Instantiate(smallerFry, smallerFry1Spawn.transform.position, smallerFry1Spawn.transform.rotation);
        GameObject temp2 = Instantiate(smallerFry, smallerFry2Spawn.transform.position, smallerFry2Spawn.transform.rotation);

        temp.GetComponent<EnemyAI>().waypoints = this.waypoints;
        temp.GetComponent<EnemyAI>().waypointIndex = this.waypointIndex;
        temp1.GetComponent<EnemyAI>().waypoints = this.waypoints;
        temp1.GetComponent<EnemyAI>().waypointIndex = this.waypointIndex;
        temp2.GetComponent<EnemyAI>().waypoints = this.waypoints;
        temp2.GetComponent<EnemyAI>().waypointIndex = this.waypointIndex;
        Destroy(gameObject);
        yield return null;
    }

    // Universal stuff
    #region
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float rotationAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        enemyModel.transform.rotation = Quaternion.Euler(0f, rotationAngle + 180, 0f);
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            GetNextPoint();
        }

        if (slowed)
        {
            slowedDuration = startSlowedDuration;
            slowedDuration -= Time.deltaTime;
            if (slowedDuration <= 0f)
            {
                speed = startSpeed;
            }
        }

        HEALTHBAR();        

        if (dead)
        {
            // To check what enemy gives how many coins.
            switch (enemyDropdown)
            {
                case Enemy.Sodato:
                    // invoke 2x
                    player.AddCoins(2);
                    Instantiate(sodatoCoinDeath, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case Enemy.Fries:
                    // invoke 3x
                    player.AddCoins(3);
                    Instantiate(patataCoinDeath, transform.position, Quaternion.identity);
                    StartCoroutine(Fries());
                    break;
                case Enemy.Borgor:
                    // invoke 5x
                    player.AddCoins(5);
                    Instantiate(borgorCoinDeath, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                    break;
                case Enemy.SmallerFries:
                    Destroy(gameObject);
                    break;
            }
        }
    }

    void GetNextPoint()
    {
        if (waypointIndex >= waypoints.WPoint.Length - 1)
        {
            EndPath();
            Spawner.enemiesAlive--;
            return;
        }
        waypointIndex++;
        target = waypoints.WPoint[waypointIndex];        
    }

    void EndPath()
    {
        castleHealth.MinusHealth();
        Destroy(gameObject);
    }

    public void HEALTHBAR()
    {
        healthBar.fillAmount = health / maxHealth;
        
        if (health <= 0)
        {
            Spawner.enemiesAlive--;
            dead = true;
        }
    }

    public IEnumerator Freeze()
    {
        float i = freezeDuration;
            Animation anim = GetComponentInChildren<Animation>();
        speed = 0f;
        while (i > 0)
        {
            i -= Time.deltaTime;
            ice.SetActive(true);
            if (anim)
                anim.Stop();
            Debug.Log("Freeze Timer: " + i);
            yield return null;
        }
        freezed = false;
        speed = startSpeed;
        if (anim)
            anim.Play();
        ice.SetActive(false);
    }
    #endregion

    // Ask about json file and how to read it to put in here.
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Projectile"))
        {
            health -= 1;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Penne"))
        {
            health -= 3;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Macaroni"))
        {
            health -= 4;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Meatball"))
        {
            health -= 3;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Bread"))
        {
            health -= 6;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("BreadShock"))
        {
            health -= projectileUpgradeManager.breadShockDmg;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("MacaroniRotate"))
        {
            health -= 2;
            Instantiate(hitEffect, transform.position, transform.rotation);
        }
        else if (other.gameObject.CompareTag("Freeze"))
        {
            print("hit");
            StartCoroutine(Freeze());
        }
        else if (other.gameObject.CompareTag("Slow"))
        {
            if (!freezed)
            {
                speed /= projectileUpgradeManager.slowAmount;
                slowed = true;
            }
        }
        else if (other.gameObject.CompareTag("Lagsana"))
        {
            health -= projectileUpgradeManager.damage;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Slow"))
        {
            speed = startSpeed;
            slowed = false;
        }
    }

    public IEnumerator SmallerFryBegin()
    {
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;
        yield return new WaitForSeconds(0.2f);
        capsule.enabled = true;
        yield return null;
    }
}
