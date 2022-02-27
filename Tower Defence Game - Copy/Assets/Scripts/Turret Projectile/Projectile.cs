using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    EnemyAI enemyCS;
    public Transform target;
    public float projectileSpeed;


    private void Start()
    {
        enemyCS = FindObjectOfType<EnemyAI>();
    }
    void Update()
    {
        // Makes bullet go fast
        if (target == null)
        {
            Destroy(this.gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = projectileSpeed * Time.deltaTime;

        //if (dir.magnitude <= distanceThisFrame)
        //{
        //    HitTarget();
        //    return;
        //}

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

    }

    public void Seek(Transform _target) 
    {
        target = _target;
    }

    public void HitTarget()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HitTarget();
        }
    }
}
