using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;

    public GameObject leftX;
    public GameObject rightX;
    public GameObject top;
    public GameObject bottom;

    public Vector3 offset;

    int speed = 4;

    int moveSpeed = 6;

    public GameObject turret;

    Rigidbody rb;

    float rightMax;
    float leftMax;
    float upMax;
    float downMax;

    float shakeTimeRemaining, shakePower, shakeFadeTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rightMax = rightX.transform.position.x - 5;
        leftMax = leftX.transform.position.x + 5;
        upMax = top.transform.position.z - 10;
        downMax = bottom.transform.position.z - 5;
    }

    // Update is called once per frame
    void Update()
    {

        if (turret)
        {
            float xDirection = Input.GetAxis("Horizontal");
            float zDirection = Input.GetAxis("Vertical");

            if(transform.position.x > rightMax)
            {
                transform.position = new Vector3(rightMax, transform.position.y, transform.position.z);
                //Debug.Log("rightmax");
            }
            else if(transform.position.x < leftMax)
            {
                transform.position = new Vector3(leftMax, transform.position.y, transform.position.z);
                //Debug.Log("leftmax");
            }

            if(transform.position.z > upMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, upMax);
                //Debug.Log("upmax");
            }
            else if (transform.position.z < downMax)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, downMax);
                //Debug.Log("downmax");
            }


            rb.velocity = new Vector3(xDirection, 0, zDirection) * moveSpeed;

        }   
        else
        {
            rb.velocity = new Vector3(0, 0, 0);

            float xPos = Mathf.Clamp(Player.transform.position.x, leftMax, rightMax);

            float topdown = Mathf.Clamp(Player.transform.position.z - 6, downMax, upMax);

            Vector3 targetPos = new Vector3(xPos, transform.position.y, topdown);

            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        } 
    }

    private void LateUpdate()
    {
        if(shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakePower;
            float z = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(x, 0, z);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
    }

    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;
    }


}
