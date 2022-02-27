using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialManager : MonoBehaviour
{
    int previousNumber;
    public int tutorialNumber;

    public GameObject[] box;

    public static bool tutorialSwitch;

    public static bool tutorialWaveStart;

    Spawner spawner;
    bool afterWave = true;
    bool firstStart = true;

    public GameObject enemyPatataIntro;
    public GameObject enemyBorgorIntro;

    bool tutorialLock = false;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        tutorialSwitch = false;
        tutorialWaveStart = false;
        box[tutorialNumber].SetActive(true);
        Debug.Log(Spawner.enemiesAlive);
    }

    // Update is called once per frame
    void Update()
    {
        if(tutorialWaveStart == true && !TurretPositionCheck.cannotStart && firstStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                firstStart = false;
                NextTutorial();
                tutorialWaveStart = false;
            }
        }


        if(spawner.waveNumber == 1 && afterWave)
        {
            afterWave = false;
            NextTutorial();
        }

        if(spawner.waveNumber == 1 && !Spawner.prepPhase && !tutorialLock)
        {
            tutorialLock = true;
            enemyPatataIntro.SetActive(true);
            Time.timeScale = 0f;
        }

        if (spawner.waveNumber == 2 && !Spawner.prepPhase && tutorialLock)
        {
            tutorialLock = false;
            enemyBorgorIntro.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void NextTutorial()
    {
        if(Spawner.enemiesAlive <= 0)
        {
            previousNumber = tutorialNumber;
            tutorialNumber += 1;

            Debug.Log(tutorialNumber);

            if (tutorialNumber == 10)
            {
                tutorialSwitch = true;
            }

            if (tutorialNumber == 8)
            {
                tutorialWaveStart = true;
            }

            if (tutorialNumber >= box.Length)
            {
                box[previousNumber].SetActive(false);

                tutorialWaveStart = true;
            }
            else
            {
                box[tutorialNumber].SetActive(true);
                
                box[previousNumber].SetActive(false);
            }
        }

    }

    public void EnemyTutorial()
    {
        Debug.Log("Pressed");
        Time.timeScale = 1f;
        enemyPatataIntro.SetActive(false);
        enemyBorgorIntro.SetActive(false);
    }

}
