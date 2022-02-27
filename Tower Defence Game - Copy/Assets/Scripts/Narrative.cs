using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Narrative : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences; // public array to type in sentences
    public Image[] talkingCharacter;
    public GameObject[] characterNames;
    public int index;
    public float typingSpeed;
    public GameObject continueButton;
    public GameObject loadingScreen;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(TypeEffect());
        characterNames[index].SetActive(true);
        talkingCharacter[index].color = hexToColor("FFFFFF");
        continueButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplay.text == sentences[index]) // If sentence is done/finished, allows player to click in order to continue to next sentence
        {
            continueButton.SetActive(true);
        }
    }

    IEnumerator TypeEffect()
    {
        foreach (char letter in sentences[index].ToCharArray()) // Displays each letter of the sentence in the array after a set timing
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);

        }

    }

    public void NextLine()
    {
        continueButton.SetActive(false);

        if (index < sentences.Length - 1) // If there are more sentences, show next sentence in array
        {
            characterNames[index].SetActive(false);
            talkingCharacter[index].color = hexToColor("9F9F9F");
            index++;
            textDisplay.text = "";
            characterNames[index].SetActive(true);
            talkingCharacter[index].color = hexToColor("FFFFFF");
            StartCoroutine(TypeEffect());
        }
        else // If no more sentences in array, deactivate evrything
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
            LoadLevel();
        }
    }

    public Color hexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public void Skip()
    {
        LoadLevel();
    }


    public void LoadLevel()
    {
        StartCoroutine(LoadingScreen());
    }

    IEnumerator LoadingScreen()
    {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(4.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
