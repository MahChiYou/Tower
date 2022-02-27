using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPositionCheck : MonoBehaviour
{
    public GameObject ok;
    public GameObject notOk;
    public bool onPath;
    public static bool cannotStart;

    // Start is called before the first frame update
    void Start()
    {
        onPath = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(onPath == true)
        {
            notOk.SetActive(true);
            ok.SetActive(false);

        }
        else if(onPath == false)
        {
            ok.SetActive(true);
            notOk.SetActive(false);

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Path" || other.tag == "Player")
        {
            onPath = true;
            cannotStart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Path" || other.tag == "Player")
        {            
            onPath = false;
            cannotStart = false;
        }
    }
}
