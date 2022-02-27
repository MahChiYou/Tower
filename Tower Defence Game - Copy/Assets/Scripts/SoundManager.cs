using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource soundSetting;
    //public AudioSource clickSound;
    public AudioMixer audioMixerBMG;
    public AudioMixer audioMixerEFFECTS;

    public float bmgValue;
    public float effectValue;

    public Slider bmgSlider, effectSlider;


    private void Start()
    {
        // Sets slider value to previous scene.
        bmgValue = PlayerPrefs.GetFloat("SliderBMGLevel", bmgValue);
        bmgSlider.value = bmgValue;

        // Sets slider value to previous scene.
        effectValue = PlayerPrefs.GetFloat("SliderEffectLevel", effectValue);
        effectSlider.value = effectValue;
    }

    public void BMGsound(float SetBMG)
    {
        // Sets Music volume to slider
        audioMixerBMG.SetFloat("BMG", SetBMG);
        // Saves Music volume for next scenes.
        bmgValue = SetBMG;
        PlayerPrefs.SetFloat("SliderBMGLevel", bmgValue);
        //print(SetBMG);
    }

    public void EffectSounds(float SetEffect)
    {
        // Sets Effects volume to slider
        audioMixerEFFECTS.SetFloat("Effect", SetEffect);
        // Saves Music volume for next scenes.
        effectValue = SetEffect;
        PlayerPrefs.SetFloat("SliderEffectLevel", effectValue);
        //print(SetEffect);
    }

    public void sound()
    {
        // For player to hear how loud the sound is.
        soundSetting.PlayOneShot(soundSetting.clip, 0.5f);
    }
    //public void ClickSound()
    //{
    //    // For press feedback
    //    soundSetting.PlayOneShot(clickSound.clip, 0.5f);
    //}

}
