using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public bool isProjectile;
    public bool isRotatingPivot;

    public GameObject macaroni1;
    public GameObject macaroni2;
    public GameObject macaroni3;

    Vector3 rotateAround;

    GameObject rotationObject;

    public Vector3 axis;

    public float angle;

    private void Start()
    {
        if (isProjectile)
        {
            rotationObject = GameObject.FindGameObjectWithTag("rotater");
            rotateAround = rotationObject.transform.position;

            Destroy(gameObject, 1.5f);
        }


    }

    private void OnEnable()
    {
        if (!isProjectile)
        {
            rotationObject = GameObject.FindGameObjectWithTag("TurretRotate");
            rotateAround = rotationObject.transform.position;
        }

        if (isRotatingPivot)
        {
            switch (ProjectileUpgradeManager.macaroniAmt)
            {
                case 0:
                    macaroni1.SetActive(false);
                    macaroni2.SetActive(false);
                    macaroni3.SetActive(false);
                    break;

                case 1:
                    macaroni1.SetActive(true);
                    macaroni2.SetActive(false);
                    macaroni3.SetActive(false);
                    break;

                case 2:
                    macaroni1.SetActive(true);
                    macaroni2.SetActive(true);
                    macaroni3.SetActive(false);
                    break;

                case 3:
                    macaroni1.SetActive(true);
                    macaroni2.SetActive(true);
                    macaroni3.SetActive(true);
                    break;
            }
        }
    }

    private void Update()
    {
        transform.RotateAround(rotateAround, axis, angle * Time.deltaTime);
    }

}
