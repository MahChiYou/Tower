using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatBallProjectile : MonoBehaviour
{
    ProjectileUpgradeManager projectileUpgradeManager;
    CameraFollow cam;

    public LayerMask whatIsGround;

    public GameObject hitBox;

    Vector3 currentPosition;
    float heightOffset;
    float distanceTravelled;

    public Vector3 target;

    public float speed;

    float radius = 1.0f;
    float radiusSq;

    public float arcFactor = 0.3f; // Higher number means bigger arc.
    Vector3 origin; // To store where the projectile first spawned.


    // Start is called before the first frame update
    private void OnEnable()
    {
        projectileUpgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
        cam = FindObjectOfType<CameraFollow>();

        radiusSq = radius * radius;

        origin = currentPosition = transform.position;

        RaycastHit hit;

        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(r, out hit, Mathf.Infinity, whatIsGround))
        {
            target = hit.point;
        }
    }
    void Update()
    {
        // Move ourselves towards the target at every frame.
        Vector3 direction = target - transform.position;
        currentPosition += direction.normalized * speed * Time.deltaTime; // Set where we are at.
        distanceTravelled += speed * Time.deltaTime; // Record the distance we are travelling


        float totalDistance = Vector3.Distance(origin, target);
        float heightOffset = arcFactor * totalDistance * Mathf.Sin(distanceTravelled / totalDistance);
        transform.position = currentPosition + new Vector3(0, heightOffset, 0);


        if (direction.sqrMagnitude < radiusSq)
        {
            GameObject collider = Instantiate(hitBox, transform.position, transform.rotation);
            collider.transform.localScale = projectileUpgradeManager.meatballSize;
            cam.StartShake(0.3f, 0.02f);
            Destroy(gameObject);
        }
    }
}
