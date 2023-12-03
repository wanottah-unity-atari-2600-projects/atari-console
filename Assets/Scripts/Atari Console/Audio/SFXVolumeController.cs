
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2020.12.19
//
// Audio Sound Effects Volume Controller
//
// created 2019.12.17
//

public class SFXVolumeController : MonoBehaviour
{
    private Text sfxVolumeControlText;


    
    private void Start()
    {
        sfxVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(SFXVolumeControl);
    }


    public void SFXVolumeControl(float sfxVolume)
    {
        AudioController.audioController.SetSFXVolume(sfxVolume);

        sfxVolumeControlText.text = (sfxVolume * 10).ToString("0");
    }


} // end of class
