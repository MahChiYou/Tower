using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorUI : MonoBehaviour
{
    public Image selection;

    public GameObject ability1;
    public GameObject ability2;
    public GameObject ability3;
    public GameObject ability4;

    // Start is called before the first frame update
    void Start()
    {
        selection.rectTransform.position = ability1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (TurretManager.inTurret == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selection.rectTransform.position = ability1.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                selection.rectTransform.position = ability2.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                selection.rectTransform.position = ability3.transform.position;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                selection.rectTransform.position = ability4.transform.position;
            }
        }

    }
}
