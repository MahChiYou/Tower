using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    // References
    Shop shop;
    Player player;
    public Ghost ghost;
    public Mage mage;
    public AudioSource clickSound;

    // For pause
    public static bool isPause;

    // UI stuff
    public GameObject mainScreen;
    public GameObject pauseMenu;
    public GameObject shopBorder;
    public GameObject shopGhost;
    public GameObject shopMage;
    public GameObject Settings;
    public GameObject Help;
    public Text mageCoin;
    public Text ghostCoin;


    //Health Stuff
    public float lifePoints;
    public GameObject loseScreen;
    public Text showHealth;
    public GameObject healthLossEffect;

    public LayerMask whatIsGround;

    private void Start()
    {
        Time.timeScale = 1f;
        player = FindObjectOfType<Player>();
        shop = FindObjectOfType<Shop>();
        lifePoints = 10;
        loseScreen.SetActive(false);
        ClosePause();
    }
    private void Update()
    {
        // Sets the text on the respective shop UI to how much the player has.
        ghostCoin.text = player.coinCount.ToString();
        mageCoin.text = player.coinCount.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenPause();
        }
    }

    public void OpenShop()
    {
        if (player.isGhost)
        {
            clickSound.Play();
            isPause = true;
            shopGhost.SetActive(true);
            shopBorder.SetActive(true);
            shop.playerStateDrop = Shop.playerState.Ghost;
        }
        else if (player.isMage)
        {
            clickSound.Play();
            isPause = true;
            shopMage.SetActive(true);
            shopBorder.SetActive(true);
            shop.playerStateDrop = Shop.playerState.Mage;
        }
    }

    public void CloseShop()
    {
        Time.timeScale = 1f;
        isPause = false;
        clickSound.Play();
        shopGhost.SetActive(false);
        shopMage.SetActive(false);
        shopBorder.SetActive(false);
    }

    public void SwitchShops()
    {
        if (shopMage.activeInHierarchy)
        {
            clickSound.Play();
            shopMage.SetActive(false);
            shopGhost.SetActive(true);
            shop.playerStateDrop = Shop.playerState.Ghost;
        }
        else if (shopGhost.activeInHierarchy)
        {
            clickSound.Play();
            shopGhost.SetActive(false);
            shopMage.SetActive(true);
            shop.playerStateDrop = Shop.playerState.Mage;
        }
    }

    // Menus
    #region Menus
    public void OpenPause()
    {
        clickSound.Play();
        isPause = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    
    public void ClosePause()
    {
        isPause = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void OpenSettings()
    {
        clickSound.Play();
        Settings.SetActive(true);
    }
    public void CloseSettings()
    {
        clickSound.Play();
        Settings.SetActive(false);
    }
    public void OpenHelp()
    {
        clickSound.Play();
        Help.SetActive(true);
    }
    public void CloseHelp()
    {
        clickSound.Play();
        Help.SetActive(false);
    }

    // Win Screen
    public void GoMenu()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainMenu");
    }

    // Next Level
    public void NextLevel()
    {
        clickSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        clickSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    public void MinusHealth()
    {
        lifePoints -= 1;
        showHealth.text = lifePoints.ToString();
        healthLossEffect.SetActive(true);
        Invoke("DeactivateLossEffect", 1.0f);

        if (lifePoints <= 0)
        {
            Time.timeScale = 0f;
            loseScreen.SetActive(true);
        }       
    }

    void DeactivateLossEffect()
    {
        healthLossEffect.SetActive(false);
    }
}