using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Spawner : MonoBehaviour
{
    // Reference.
    UIcontroller uiController;
    
    // Wave Assignments
    public Wave[] waves;
    public int waveNumber;

    // Wave UI
    public bool waveEnded;
    public TextMeshProUGUI startWaveText;
    public GameObject startWave;
    
    public TextMeshProUGUI endWaveText;
    public GameObject endWave;

    public GameObject failToStartWaveText;
    
    // Others
    public static bool prepPhase;
    public static bool waveLock; // Prevents coroutine from firing during prep phase
    public static int enemiesAlive;
    public GameObject WinScreen;
    Scene scene;

    // For multiple spawners.
    public static Spawner primary;
    public static List<Spawner> secondaries;

    void Start()
    {
        enemiesAlive = 0;

        if (!primary)
        {
            primary = this; // Sets the first/only Spawner GameObject as the "primary".
            this.name = "Spawner (Primary)";
        }
        // Gets all Spawner GameObject to put in the list, and removes the one that had been assigned as "primary"
        // -------------------- NOTE! the newest instance of this will be the primary. --------------------
        secondaries = new List<Spawner>(FindObjectsOfType<Spawner>());
        secondaries.Remove(primary);
        if (secondaries == null) {
            print("secondary is empty"); 
            return; 
        }
        else
        {
            foreach (Spawner secspawn in secondaries)
            {
                secspawn.name = "Spawner (Secondary)";
            }
        }
        //else if (secondaries[0]) secondaries[0].name = "Spawner (Secondary)";
        //if (secondaries[0] == null) return;
        //else secondaries[0].name = "Spawner (Secondary)";

        scene = SceneManager.GetActiveScene();
        WinScreen.SetActive(false);
        uiController = FindObjectOfType<UIcontroller>();
        
        // Sets the first wave text.
        //startWaveText.text = ("" + waveNumber + 1);
        prepPhase = true;
    }

    // Constantly Churns enemy out
    void Update()
    {
        if (!UIcontroller.isPause)
        {
            if(scene.name != "Tutorial" || TutorialManager.tutorialWaveStart == true)
            {
                if (prepPhase && !TurretPositionCheck.cannotStart)
                {
                    if (this == primary && Input.GetKeyDown(KeyCode.Space))
                    {
                        StartCoroutine(WaveUI());

                        foreach (Spawner s in secondaries)
                        {
                            s.Initialise();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) && TurretPositionCheck.cannotStart && this == primary)
            {
                failToStartWaveText.SetActive(true);
                Invoke("CannotStartWave", 2.0f);
            }

            if (enemiesAlive <= 0 && waveLock == true && this == primary)
            {
                StartCoroutine(WaveEndUI());
                prepPhase = true;
            }
        }
    }

    //For secondary spawners only
    void Initialise()
    {
        StartCoroutine(SpawnWave());
        Debug.Log(gameObject.name + " Started");
    }

    //Calls once in primary spawner
    IEnumerator WaveUI()
    {
        if (primary == this)
        {
            prepPhase = false;
            startWave.SetActive(true);
            startWaveText.text = ((waveNumber+1).ToString());
        }
        yield return new WaitForSeconds(3.5f);
        startWave.SetActive(false);
        StartCoroutine(SpawnWave());
        waveLock = true;
        waveEnded = false;
    }

    // Calls once in primary spawner when enemies alive = 0
    IEnumerator WaveEndUI()
    {
        waveNumber++;
        
        if (waveNumber == waves.Length)
        {
            WinScreen.SetActive(true);
            Time.timeScale = 0f;
            yield return null;
        }

        enemiesAlive = 0;
        waveLock = false;
        endWaveText.text = ("" + waveNumber);
        print(endWave);
        endWave.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        endWave.SetActive(false);
    }


    // Spawns Enemy based on wave number.     
    IEnumerator SpawnWave()
    {
        Wave wave = waves[waveNumber];
        for (int z = 0; z < wave.enemies.Length; z++) // Amount of types of enemy
        {
            for (int i = 0; i < wave.enemies[z].count; i++) // Amount of enemies
            {
                if (primary == this)
                {
                    wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints = GameObject.Find("Waypoint (Primary)").GetComponent<Waypoints>();
                }
                else if(primary != this)
                {
                    wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints = GameObject.Find("Waypoint (Secondary)").GetComponent<Waypoints>();
                }

                SpawnEnemy(wave.enemies[z].enemy); // Spawns the enemy
                print(enemiesAlive);
                //wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin = gameObject;

 

                //if (wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin.name == "Spawner (Primary)")
                //{
                //    wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints = GameObject.Find("Waypoint (Primary)").GetComponent<Waypoints>();
                //    //print(name + " is following " + wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints.name);
                //    //print(name + " came from " + wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin.name);
                //}
                //else if (wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin.name == "Spawner (Secondary)")
                //{
                //    wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints = GameObject.Find("Waypoint (Secondary)").GetComponent<Waypoints>();
                //    //print(name + " is following " + wave.enemies[z].enemy.GetComponent<EnemyAI>().waypoints.name + " from secondary");
                //    //print(name + " came from " + wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin.name + " from secondary");
                //}
                //Debug.Log(wave.enemies[z].enemy.GetComponent<EnemyAI>().spawnOrigin.name);
                yield return new WaitForSeconds(1f / wave.spawnRate);
            }
        }
    }

    //Spawns enemies
    public void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, transform.position, transform.rotation);

        enemiesAlive++;
    }

    void CannotStartWave()
    {
        failToStartWaveText.SetActive(false);
    }
}
