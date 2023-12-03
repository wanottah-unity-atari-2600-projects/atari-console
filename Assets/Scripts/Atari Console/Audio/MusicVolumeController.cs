
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2020.12.19
//
// Audio Music Volume Controller
//
// created 2019.12.17
//

public class MusicVolumeController : MonoBehaviour
{
    private Text musicVolumeControlText;


    private void Start()
    {
        musicVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(MusicVolumeControl);
    }


    public void MusicVolumeControl(float musicVolume)
    {
        AudioController.audioController.SetMusicVolume(musicVolume);

        musicVolumeControlText.text = (musicVolume * 10).ToString("0");
    }


} // end of class
