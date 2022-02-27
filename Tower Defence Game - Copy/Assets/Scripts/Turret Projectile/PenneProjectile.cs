using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenneProjectile : MonoBehaviour
{
    ProjectileUpgradeManager projectileUpgradeManager;

    public float speed;
    public float range;

    int piercingPower;

    // Start is called before the first frame update
    void Start()
    {
        projectileUpgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
        piercingPower = projectileUpgradeManager.pierceAmt;
        Destroy(gameObject, range);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(piercingPower <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            piercingPower -= 1;
        }
    }
}
