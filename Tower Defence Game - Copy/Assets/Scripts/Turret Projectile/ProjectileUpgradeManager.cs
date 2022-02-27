using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUpgradeManager : MonoBehaviour
{
    public int pierceAmt;
    public static int penneLevel;

    public Vector3 meatballSize;
    public static int ballLevel;

    public int breadShockDmg;
    public static int breadLevel;
    public Material[] breadMat;

    public static int macaroniAmt;
    public static int macLevel;

    public Vector3 freezeSize;

    public float slowAmount;

    public float coffeeFireRate;

    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        pierceAmt = 2;
        penneLevel = 0;

        breadShockDmg = 2;
        breadLevel = 0;

        meatballSize = new Vector3(1, 1, 1);
        ballLevel = 0;

        macaroniAmt = 0;
        macLevel = 0;

        freezeSize = new Vector3(1, 11, 1);

        slowAmount = 2f;

        coffeeFireRate = 2f;

        damage = 2;
    }

    public void PenneUpgrade()
    {
        pierceAmt += 1;
        penneLevel++;
    }

    public void MeatballUpgrade()
    {
        meatballSize += new Vector3(0.25f, 0, 0.25f);
        ballLevel++;
    }

    public void BreadUpgrade()
    {
        breadShockDmg += 1;
        breadLevel++;
    }

    public void MacaroniUpgrade()
    {
        macaroniAmt += 1;
        macLevel++;
    }

    public void freezeUpgrade()
    {
        freezeSize += new Vector3(0.25f, 0, 0.25f);
    }

    public void SlowUpgrade()
    {
        slowAmount += 0.5f;
    }

    public void CoffeeUpgrade()
    {
        coffeeFireRate += 0.25f;
    }

    public void DamageUpgrade()
    {
        damage += 2;
    }
}

