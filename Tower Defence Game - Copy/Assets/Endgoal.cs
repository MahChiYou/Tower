using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endgoal : MonoBehaviour
{
    public GameObject tiramisuFull;
    public GameObject tiramisuHalf;
    public GameObject tiramisuSmall;

    UIcontroller uicontroller;

    // Start is called before the first frame update
    void Start()
    {
        uicontroller = FindObjectOfType<UIcontroller>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (uicontroller.lifePoints)
        {
            case 10:
                tiramisuFull.SetActive(true);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(false);
                break;

            case 9:
                tiramisuFull.SetActive(true);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(false);
                break;

            case 8:
                tiramisuFull.SetActive(true);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(false);
                break;

            case 7:
                tiramisuFull.SetActive(true);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(false);
                break;

            case 6:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(true);
                tiramisuSmall.SetActive(false);
                break;

            case 5:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(true);
                tiramisuSmall.SetActive(false);
                break;

            case 4:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(true);
                tiramisuSmall.SetActive(false);
                break;

            case 3:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(true);
                break;

            case 2:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(true);
                break;

            case 1:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(true);
                break;

            case 0:
                tiramisuFull.SetActive(false);
                tiramisuHalf.SetActive(false);
                tiramisuSmall.SetActive(false);
                break;
        }
    }
}
