using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //PLAYER MOVEMENT
    public CharacterController controller;
    public float speed;
    Vector3 velocity;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Animation playerwalk;
    public Animation ghostwalk;

    //PLAYER SWITCHING
    public GameObject mage;
    public GameObject ghost;
    public GameObject mageUI;
    public GameObject ghostUI;
    public bool isGhost;
    public bool isMage;

    // Coin stuff
    public float coinCount;
    public Text coinText;

    // Reference
    UIcontroller uiController;
    Scene scene;
    Shop shop;

    void Start()
    {
        scene = SceneManager.GetActiveScene();
        uiController = FindObjectOfType<UIcontroller>();
        shop = FindObjectOfType<Shop>();
        coinText.text = coinCount.ToString();
        isGhost = true;
        isMage = false;
    }

    void Update()
    {
        if (!UIcontroller.isPause)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            RaycastHit hit;

            coinText.text = coinCount.ToString();

            velocity += Physics.gravity * Time.deltaTime;
            if (Physics.Linecast(groundCheck.position, groundCheck.position + velocity, out hit, whatIsGround))
            {
                velocity = Vector3.zero;
            }

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                controller.Move(direction * speed * Time.deltaTime + velocity);

                if (isMage)
                {
                    playerwalk.Play();
                    Debug.Log("Walking");
                }
                else if (isGhost)
                {
                    ghostwalk.Play();
                }
            }

            if (scene.name != "Tutorial" || TutorialManager.tutorialSwitch == true) // Checks if player is in the tutorial scene. If it is not, will run normally
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    // change to ghost
                    if (isMage)
                    {
                        mage.SetActive(false);
                        ghost.SetActive(true);
                        isMage = false;
                        isGhost = true;
                        ghostUI.SetActive(true);
                        mageUI.SetActive(false);
                        shop.playerStateDrop = Shop.playerState.Ghost;
                    }

                    // change to mage
                    else if (isGhost)
                    {
                        mage.SetActive(true);
                        ghost.SetActive(false);
                        isMage = true;
                        isGhost = false;
                        ghostUI.SetActive(false);
                        mageUI.SetActive(true);
                        shop.playerStateDrop = Shop.playerState.Mage;
                    }
                }
            }
        }
    }
    public void AddCoins(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
    }
}
