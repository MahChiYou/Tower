using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : MonoBehaviour
{
    public GameObject breadHat;
    public GameObject penneHat;
    public GameObject meatballHat;
    public GameObject macaroniHat;

    public Image selection;

    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;

    UIcontroller uiController;

    [Header("Shooting Penne")]
    public float timeBetweenPenne;
    public bool canFirePenne;

    [Header("Shooting macaroni")]
    public float timeBetweenMac;
    public bool canFireMac;

    [Header("Shooting Meatball")]
    public float timeBetweenBall;
    public bool canFireBall;

    [Header("Shooting bread")]
    public float timeBetweenBread;
    public bool canFireBread;

    public int hatEnum;


    private void Awake()
    {
        SwitchHat(1);
        uiController = FindObjectOfType<UIcontroller>();
        selection.rectTransform.position = ability1.transform.position;
    }

    private void Update()
    {
        if (!UIcontroller.isPause)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SwitchHat(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SwitchHat(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SwitchHat(3);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SwitchHat(4);
            }

        }
    }

    public void SwitchHat(int hatNumber)
    {

        switch (hatNumber)
        {
            case 1:
                if (TurretManager.inTurret == false)
                {
                    selection.rectTransform.position = ability1.transform.position;
                }
                penneHat.SetActive(true);
                macaroniHat.SetActive(false);
                meatballHat.SetActive(false);
                breadHat.SetActive(false);
                hatEnum = 1;
                break;

            case 2:
                if (TurretManager.inTurret == false)
                {
                    selection.rectTransform.position = ability2.transform.position;
                }
                macaroniHat.SetActive(true);
                penneHat.SetActive(false);
                meatballHat.SetActive(false);
                breadHat.SetActive(false);
                hatEnum = 2;
                break;

            case 3:
                if (TurretManager.inTurret == false)
                {
                    selection.rectTransform.position = ability3.transform.position;
                }
                meatballHat.SetActive(true);
                penneHat.SetActive(false);
                macaroniHat.SetActive(false);
                breadHat.SetActive(false);
                hatEnum = 3;
                break;

            case 4:
                if (TurretManager.inTurret == false)
                {
                    selection.rectTransform.position = ability4.transform.position;
                }
                breadHat.SetActive(true);
                penneHat.SetActive(false);
                meatballHat.SetActive(false);
                macaroniHat.SetActive(false);
                hatEnum = 4;
                break;


        }
    }
}
