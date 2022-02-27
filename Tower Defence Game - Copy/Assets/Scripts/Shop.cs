using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public Player player;
    public Ghost ghost;
    ProjectileUpgradeManager upgradeManager;
    public AudioSource buyAudio;

    public enum playerState { Ghost, Mage};
    public playerState playerStateDrop;
    
    public enum borderState { border1, border2 , border3 , border4 };
    public borderState borderStateDrop;

    #region Borders
    public Image[] firstBorder;
    public Image[] secondBorder;
    public Image[] thirdBorder;
    public Image[] fourthBorder;
    #endregion

    #region Ghost Inspactor
    // Penne
    [Header("Penne")]
    public int penneCost;
    public Text penneCostText;
    public GameObject maxPenneLevel;

    [Header("Meatball")]
    public int ballCost;
    public Text ballCostText;
    public GameObject maxMeatballLevel;
    
    [Header("Bread")]
    public int breadCost;
    public Text breadCostText;
    public GameObject maxBreadLevel;
    
    [Header("Macaroni")]
    public int macCost;
    public Text macCostText;
    public GameObject maxMacaroniLevel;
    #endregion

    #region Mage Inspactor
    [Header("Mage")]
    public Mage mage;


    // Freeze
    [Header("Freeze")]
    public int freezeCost;
    public Text freezeCostText;
    public GameObject maxFreezeLevel;
    
    // Slow
    [Header("Slow")]
    public int slowCost;
    public Text slowCostText;
    public GameObject maxSlowLevel;
    
    // Attack Speed
    [Header("Attack Speed")]
    public int atkSpeedCost;
    public Text atkSpeedCostText;
    public GameObject maxAtkSpdLevel;

    // Damage
    [Header("Damage Speed")]
    public int dmgCost;
    public Text dmgCostText;
    public GameObject maxDmgLevel;
    #endregion

    public GameObject penneHat;
    public GameObject macaroniHat;
    public GameObject meatballHat;
    public GameObject breadHat;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
        upgradeManager = FindObjectOfType<ProjectileUpgradeManager>();
    }

    private void Update()
    {
        switch (playerStateDrop)
        {
            case playerState.Ghost:
                for (int i = 0; i < firstBorder.Length; i++)
                {
                    firstBorder[i].gameObject.SetActive(false);
                    firstBorder[ProjectileUpgradeManager.penneLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < secondBorder.Length; i++)
                {
                    secondBorder[i].gameObject.SetActive(false);
                    secondBorder[ProjectileUpgradeManager.macLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < thirdBorder.Length; i++)
                {
                    thirdBorder[i].gameObject.SetActive(false);
                    thirdBorder[ProjectileUpgradeManager.ballLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < fourthBorder.Length; i++)
                {
                    fourthBorder[i].gameObject.SetActive(false);
                    fourthBorder[ProjectileUpgradeManager.breadLevel].gameObject.SetActive(true);
                }
                break;

            case playerState.Mage:
                for (int i = 0; i < firstBorder.Length; i++)
                {
                    firstBorder[i].gameObject.SetActive(false);
                    firstBorder[Mage.freezeLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < secondBorder.Length; i++)
                {
                    secondBorder[i].gameObject.SetActive(false);
                    secondBorder[Mage.slowLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < thirdBorder.Length; i++)
                {
                    thirdBorder[i].gameObject.SetActive(false);
                    thirdBorder[Mage.atkSpeedLevel].gameObject.SetActive(true);
                }
                for (int i = 0; i < fourthBorder.Length; i++)
                {
                    fourthBorder[i].gameObject.SetActive(false);
                    fourthBorder[Mage.dmgLevel].gameObject.SetActive(true);
                }
                break;
        }
    }

    #region Ghost
    public void BuyPenne()
    {
        if (player.coinCount >= penneCost)
        {
            upgradeManager.PenneUpgrade();
            player.coinCount -= penneCost;

            penneCost = 20 + (10 * ProjectileUpgradeManager.penneLevel);
            penneCostText.text = penneCost.ToString();
            borderStateDrop = borderState.border1;

            buyAudio.Play();

            if (ProjectileUpgradeManager.penneLevel >= 3)
            {
                maxPenneLevel.SetActive(true);
                Debug.Log("Penne is already level 3");
                return;
            }
        }
    }

    public void BuyMac()
    {
        if (player.coinCount >= macCost)
        {
            upgradeManager.MacaroniUpgrade();
            //Debug.Log("Mac Level " + ProjectileUpgradeManager.macLevel);
            player.coinCount -= macCost;

            macCost = 20 + (10 * ProjectileUpgradeManager.macLevel);
            macCostText.text = macCost.ToString();
            borderStateDrop = borderState.border2;

            buyAudio.Play();

            if (ProjectileUpgradeManager.macLevel >= 3)
            {
                maxMacaroniLevel.SetActive(true);
                Debug.Log("Mac is already level 3");
                return;
            }
        }
    }

    public void BuyBread()
    {
        if (player.coinCount >= breadCost)
        {
            upgradeManager.BreadUpgrade();
            //Debug.Log("bread Level " + ProjectileUpgradeManager.breadLevel);
            player.coinCount -= breadCost;

            breadCost = 20 + (10 * ProjectileUpgradeManager.breadLevel);
            breadCostText.text = breadCost.ToString();
            borderStateDrop = borderState.border4;

            buyAudio.Play();

            if (ProjectileUpgradeManager.breadLevel >= 3)
            {
                maxBreadLevel.SetActive(true);
                Debug.Log("Bread is already level 3");
                return;
            }
        }
    }

    public void BuyBall()
    {

        if (player.coinCount >= ballCost)
        {
            upgradeManager.MeatballUpgrade();
            //Debug.Log("ball Level " + ProjectileUpgradeManager.ballLevel);
            player.coinCount -= ballCost;

            ballCost = 20 + (10 * ProjectileUpgradeManager.ballLevel);
            ballCostText.text = ballCost.ToString();
            borderStateDrop = borderState.border3;

            buyAudio.Play();

            if (ProjectileUpgradeManager.ballLevel >= 3)
            {
                maxMeatballLevel.SetActive(true);
                Debug.Log("Meatball is already level 3");
                return;
            }
        }
    }

    public void SwitchHats(int hatnumber)
    {
        switch (hatnumber)
        {
            case 1:
                penneHat.SetActive(true);
                macaroniHat.SetActive(false);
                meatballHat.SetActive(false);
                breadHat.SetActive(false);
                break;

            case 2:
                penneHat.SetActive(false);
                macaroniHat.SetActive(true);
                meatballHat.SetActive(false);
                breadHat.SetActive(false);
                break;

            case 3:
                penneHat.SetActive(false);
                macaroniHat.SetActive(false);
                meatballHat.SetActive(true);
                breadHat.SetActive(false);
                break;

            case 4:
                penneHat.SetActive(false);
                macaroniHat.SetActive(false);
                meatballHat.SetActive(false);
                breadHat.SetActive(true);
                break;

            default:
                penneHat.SetActive(false);
                macaroniHat.SetActive(false);
                meatballHat.SetActive(false);
                breadHat.SetActive(false);
                break;
        }
    }

    #endregion

    #region Mage
    public void BuyFreeze()
    {

        if (player.coinCount >= freezeCost)
        {
            //mage.freezeCooldown -= 0.5f;
            upgradeManager.freezeUpgrade();
            Mage.freezeLevel++;
            //Debug.Log("Freeze Level " + Mage.freezeLevel);
            player.coinCount -= freezeCost;

            freezeCost = 20 + (10 * Mage.freezeLevel);
            freezeCostText.text = freezeCost.ToString();
            borderStateDrop = borderState.border1;

            buyAudio.Play();

            if (Mage.freezeLevel >= 3)
            {
                maxFreezeLevel.SetActive(true);
                Debug.Log("Freeze is already level 3");
                return;
            }
        }
    }
    public void BuySlow()
    {

        if (player.coinCount >= slowCost)
        {
            upgradeManager.SlowUpgrade();
            Mage.slowLevel++;
            //Debug.Log("Slow Level " + Mage.slowLevel);
            player.coinCount -= slowCost;

            slowCost = 20 + (10 * Mage.slowLevel);
            slowCostText.text = slowCost.ToString();
            borderStateDrop = borderState.border2;

            buyAudio.Play();

            if (Mage.slowLevel >= 3)
            {
                maxSlowLevel.SetActive(true);
                Debug.Log("Slow is already level 3");
                return;
            }
        }
    }
    public void BuyAtkSpeed()
    {

        if (player.coinCount >= atkSpeedCost)
        {
            upgradeManager.CoffeeUpgrade();
            Mage.atkSpeedLevel++;
            //Debug.Log("atkSpeed Level " + Mage.atkSpeedLevel);
            player.coinCount -= atkSpeedCost;

            atkSpeedCost = 20 + (10 * Mage.atkSpeedLevel);
            atkSpeedCostText.text = atkSpeedCost.ToString();
            borderStateDrop = borderState.border3;

            buyAudio.Play();

            if (Mage.atkSpeedLevel >= 3)
            {
                maxAtkSpdLevel.SetActive(true);
                Debug.Log("Attak Speed is already level 3");
                return;
            }
        }
    }
    public void BuyDmg()
    {

        if (player.coinCount >= dmgCost)
        {
            upgradeManager.DamageUpgrade();
            Mage.dmgLevel++;
            //Debug.Log("dmg Level " + Mage.dmgLevel);
            player.coinCount -= dmgCost;

            dmgCost = 20 + (10 * Mage.dmgLevel);
            dmgCostText.text = dmgCost.ToString();
            borderStateDrop = borderState.border4;

            buyAudio.Play();

            if (Mage.dmgLevel >= 3)
            {
                maxDmgLevel.SetActive(true);
                Debug.Log("Damage is already level 3");
                return;
            }
        }
    }
    #endregion
}

