using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretControl : MonoBehaviour
{
    [Header("References")]
    TurretManager turretManager;
    public GameObject partToRotate;
    UIcontroller uiController;
    public Ghost ghost;
    CameraFollow cam;


    [Header("Projectiles")]
    public GameObject shootPos;
    public GameObject breadSticks;
    public GameObject pennePasta;
    public GameObject macaroniMac;
    public GameObject meatBallBall;

    [Header("Sounds")]
    AudioSource shootSound;

    public enum Dropdown {penne, macaroni, meatball, breadstick };

    public Dropdown hatType;

    public LayerMask whatIsGround;

    public float breadEffectdelay;
    public float ballEffectdelay;
    public float macEffectdelay;

    public Animation turretAnim;

    private void OnEnable()
    {
        uiController = FindObjectOfType<UIcontroller>();
        turretManager = GetComponentInParent<TurretManager>();
        cam = FindObjectOfType<CameraFollow>();
        shootSound = GetComponent<AudioSource>();


        switch (hatType)
        {
            case Dropdown.penne:
                ghost.canFirePenne = true;
                break;

            case Dropdown.macaroni:
                ghost.canFireMac = true;
                break;

            case Dropdown.meatball:
                ghost.canFireBall = true;
                break;

            case Dropdown.breadstick:
                ghost.canFireBread = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIcontroller.isPause)
        {
            // Manage turret aiming
            if (turretManager.possessed)
            {
                RaycastHit hit;

                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(r, out hit, Mathf.Infinity, whatIsGround))
                {
                    float angle = AngleBetweenPoints(partToRotate.transform.position, hit.point + new Vector3(0, 2, 0));

                    partToRotate.transform.rotation = Quaternion.Euler(new Vector3(0f, -angle - 90, 0f));
                }
                
                Shoot();
            }
            else if (!turretManager.possessed)
            {
                partToRotate.transform.rotation = Quaternion.identity;
                return;
            }
        }
    }

    void Shoot()
    {
        switch (hatType)
        {
            case Dropdown.penne:
                if (Input.GetButtonDown("Fire1") && ghost.canFirePenne)
                {
                    turretAnim.Stop();
                    turretAnim.Play();
                    StartCoroutine(ShootPenne());
                }
                break;
            case Dropdown.macaroni:
                if (Input.GetButtonDown("Fire1") && ghost.canFireMac)
                {
                    turretAnim.Stop();
                    turretAnim.Play();
                    StartCoroutine(ShootMac());
                }
                break;
            case Dropdown.meatball:
                if (Input.GetButtonDown("Fire1") && ghost.canFireBall)
                {
                    turretAnim.Stop();
                    turretAnim.Play();
                    StartCoroutine(ShootBall());
                }
                break;
            case Dropdown.breadstick:
                if (Input.GetButtonDown("Fire1") && ghost.canFireBread)
                {
                    turretAnim.Stop();
                    turretAnim.Play();
                    StartCoroutine(ShootBread());
                }
                break;
        }
    }
    IEnumerator ShootPenne()
    {
        Instantiate(pennePasta, shootPos.transform.position, shootPos.transform.rotation);
        shootSound.Play();
        Debug.Log(shootSound);
        float i = 0f;
        while (i < ghost.timeBetweenPenne)
        {
            i += Time.deltaTime;
            ghost.canFirePenne = false;

            yield return null;
        }
        ghost.canFirePenne = true;
    }
    IEnumerator ShootMac()
    {
        ghost.canFireMac = false;
        yield return new WaitForSeconds(macEffectdelay);
        Instantiate(macaroniMac, shootPos.transform.position, shootPos.transform.rotation);
        shootSound.Play();
        float i = 0f;
        while (i < ghost.timeBetweenMac)
        {
            i += Time.deltaTime;

            yield return null;
        }
        ghost.canFireMac = true;
    }
    IEnumerator ShootBall()
    {
        ghost.canFireBall = false;
        yield return new WaitForSeconds(ballEffectdelay);
        Instantiate(meatBallBall, shootPos.transform.position, shootPos.transform.rotation);
        shootSound.Play();
        float i = 0f;
        while (i < ghost.timeBetweenBall)
        {
            i += Time.deltaTime;

            yield return null;
        }
        ghost.canFireBall = true;
    }
    IEnumerator ShootBread()
    {
        ghost.canFireBread = false;
        yield return new WaitForSeconds(breadEffectdelay);
        GameObject bread = Instantiate(breadSticks, shootPos.transform.position, shootPos.transform.rotation);
        shootSound.Play();
        cam.StartShake(0.3f, 0.04f);
        Debug.Log("Bug");
        //bread.transform.parent = gameObject.transform;
        float i = 0f;
        while (i < ghost.timeBetweenBread)
        {
            i += Time.deltaTime;

            yield return null;
        }
        ghost.canFireBread = true;
    }

    float AngleBetweenPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.z - b.z, a.x - b.x) * Mathf.Rad2Deg;
    }
}
