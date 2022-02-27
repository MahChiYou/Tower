using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretManager : MonoBehaviour
{
    [Header("References")]
    //Spawner spawner;
    Player player;
    GameObject ghostPlayer;
    public Image slider;
    UIcontroller uiController;
    Ghost ghostHat;
    public BoxCollider moveCollider;
    Vector3 particle;
    CameraFollow cam;

    [Header("Turrets")]
    public GameObject basicTurret;
    public GameObject penneTurret;
    public GameObject macaroniTurret;
    public GameObject meatballTurret;
    public GameObject breadTurret;

    [Header("Turret Moving")]
    Vector3 dist;
    Vector3 startPos;
    float posX;
    float posZ;
    float posY;
    public bool movable;
    TurretPositionCheck positionCheck;
    public GameObject positionBox;

    [Header("Turret Possessing")]
    //Initial Possession
    public bool inRange;
    bool input;
    public bool possessed = false;
    public GameObject possessParticle;
    public MeshRenderer turretBase;

    //During Possession
    public static bool inTurret;
    bool timeOut;
    float currentPossessTime;
    float givenPossessTime = 12;

    [Header("Material Stuff")]
    public Material[] penneMat;
    public MeshRenderer penneAssign;

    public Material[] ballMat;
    public MeshRenderer ballAssign;

    public Material[] breadMat;
    public SkinnedMeshRenderer breadAssign;

    /// <summary>
    /// fixed timer from unpossessing, to prevent player from spamming turret.
    /// </summary>

    float repossessCooldown = 1.5f;
    public Image cooldownPossessUI1, cooldownPossessUI2, cooldownPossessUI3, cooldownPossessUI4;
    bool canRepossess = true;

    // Start is called before the first frame update
    void Start()
    {
        inTurret = false;
        particle = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        currentPossessTime = givenPossessTime;
        //spawner = FindObjectOfType<Spawner>();
        player = FindObjectOfType<Player>();
        ghostPlayer = GameObject.FindGameObjectWithTag("Ghost");
        uiController = FindObjectOfType<UIcontroller>();
        ghostHat = FindObjectOfType<Ghost>();
        positionCheck = GetComponentInChildren<TurretPositionCheck>();
        cam = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIcontroller.isPause)
        {
            movable = true;

            // TO DETECT WHEN PLAYER WANTS TO POSSESS TURRET
            if (inRange)
            {
                if (Input.GetKeyDown(KeyCode.E) && !input && !inTurret && canRepossess) // Lets player possess turret
                {
                    possessed = true;
                    player.enabled = false;
                    ghostPlayer.SetActive(false);
                    Instantiate(possessParticle,particle, transform.rotation);
                    input = true;
                    inTurret = true;
                    cam.turret = this.gameObject;
                    //cam.CamToTurret();
                    DetectHat();
                    DisabledTurret(basicTurret);
                    penneAssign.sharedMaterial = penneMat[ProjectileUpgradeManager.penneLevel];
                    ballAssign.sharedMaterial = ballMat[ProjectileUpgradeManager.ballLevel];
                    breadAssign.sharedMaterial = breadMat[ProjectileUpgradeManager.breadLevel];
                }
                else if (Input.GetKeyDown(KeyCode.E) && input && inTurret || timeOut) //Kicks player out of turret
                {
                    canRepossess = false;
                    StartCoroutine(UnpossessCooldown());
                    possessed = false;
                    player.enabled = true;
                    ghostPlayer.SetActive(true);
                    input = false;
                    inTurret = false;
                    cam.turret = null;
                    turretBase.sharedMaterial = breadMat[0];
                    ActiveTurret(basicTurret);
                    DisabledTurret(penneTurret);
                    DisabledTurret(macaroniTurret);
                    DisabledTurret(meatballTurret);
                    DisabledTurret(breadTurret);
                }
            }

            // TO DETERMINE IF PLAYER CAN MOVE TURRET
            if (!Spawner.prepPhase)
            {
                movable = false;
                positionBox.SetActive(false);
            }
            else if (Spawner.prepPhase)
            {
                if (!possessed)
                {
                    movable = true;
                    positionBox.SetActive(true);
                }
                else if(possessed)
                {
                    movable = false;
                    positionBox.SetActive(false);
                }

            }

            // TO MINUS TIME FROM SLIDER WHEN PLAYER IS POSSESSING TURRET
            if (inTurret)
            {
                currentPossessTime -= 1 * Time.deltaTime;// Minus as long as player is in turret

                if (currentPossessTime <= 0) // If time left reaches 0, kicks player out
                {
                    timeOut = true;
                }
                else if (currentPossessTime >= 0) // If time left is more than 0, allows possession
                {
                    timeOut = false;
                }
            }
            else if (!inTurret)
            {
                currentPossessTime += 1 * Time.deltaTime;
                //Debug.Log("adding");
                // Adds as long as player is out of turret
                timeOut = false;

                if (currentPossessTime >= givenPossessTime) // Cap the time at 12 seconds when maxed
                {
                    currentPossessTime = givenPossessTime;
                }
            }

            slider.fillAmount = 1 / givenPossessTime * currentPossessTime;

            // DETERMINES IF TURRET CAN BE PASSED THROUGH
            if (positionCheck.onPath)
            {
                return;
            }
            else if (!positionCheck.onPath)
            {
                Physics.IgnoreCollision(player.GetComponent<CharacterController>(), moveCollider, false);
            }
        }
        else
        {
            movable = false;
        }
    }

    IEnumerator UnpossessCooldown()
    {
        float i = 0f;
        while (i < repossessCooldown)
        {
            i += Time.deltaTime;
            cooldownPossessUI1.fillAmount = 1 - i / repossessCooldown;
            cooldownPossessUI2.fillAmount = 1 - i / repossessCooldown;
            cooldownPossessUI3.fillAmount = 1 - i / repossessCooldown;
            cooldownPossessUI4.fillAmount = 1 - i / repossessCooldown;
            yield return null;
        }
        canRepossess = true;
    }

    void OnMouseDrag()
    {
        if(movable)
        {
            // We shoot a ray that only hits objects in the Ground layer.
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, uiController.whatIsGround))
            {
                transform.position = hit.point;
                Physics.IgnoreCollision(player.GetComponent<CharacterController>(), moveCollider);
                //Make possess particle in centre of turret not at bottom of turret
                particle = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && player.isGhost)
        {
            inRange = true;
        }
        else if(other.CompareTag("Player") && player.isMage)
        {
            inRange = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    void DetectHat()
    {
        if (ghostHat.hatEnum == 1)
        {
            turretBase.sharedMaterial = breadMat[ProjectileUpgradeManager.penneLevel];
            ActiveTurret(penneTurret);
            DisabledTurret(macaroniTurret);
            DisabledTurret(meatballTurret);
            DisabledTurret(breadTurret);
        }
        else if (ghostHat.hatEnum == 2)
        {
            turretBase.sharedMaterial = breadMat[ProjectileUpgradeManager.macLevel];
            ActiveTurret(macaroniTurret);
            DisabledTurret(penneTurret);
            DisabledTurret(meatballTurret);
            DisabledTurret(breadTurret);
        }
        else if (ghostHat.hatEnum == 3)
        {
            turretBase.sharedMaterial = breadMat[ProjectileUpgradeManager.ballLevel];
            ActiveTurret(meatballTurret);
            DisabledTurret(penneTurret);
            DisabledTurret(macaroniTurret);
            DisabledTurret(breadTurret);
        }
        else if (ghostHat.hatEnum == 4)
        {
            turretBase.sharedMaterial = breadMat[ProjectileUpgradeManager.breadLevel];
            ActiveTurret(breadTurret);
            DisabledTurret(penneTurret);
            DisabledTurret(macaroniTurret);
            DisabledTurret(meatballTurret);
        }
    }

    void ActiveTurret(GameObject activeTurret)
    {
        activeTurret.SetActive(true);
    }

    void DisabledTurret(GameObject unactiveTurrets)
    {
        unactiveTurrets.SetActive(false);
    }
}
