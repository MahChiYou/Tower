using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyovertime : MonoBehaviour
{
    public float time;

    public bool deactivateCollider;

    // Start is called before the first frame update
    void Start()
    {
       
        if (deactivateCollider)
        {
            StartCoroutine(Deactivate());
        }
        else
        {
            GameObject.Destroy(gameObject, time);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator Deactivate()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        yield return new WaitForSeconds(time);
        collider.enabled = false;
        yield return new WaitForSeconds(5);
        GameObject.Destroy(gameObject);
    }
}
