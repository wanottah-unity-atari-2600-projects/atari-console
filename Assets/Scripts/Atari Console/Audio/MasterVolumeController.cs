
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2020.12.19
//
// Audio Master Volume Controller
//
// created 2019.12.17
//

public class MasterVolumeController : MonoBehaviour
{
    private Text masterVolumeControlText;


    private void Start()
    {
        masterVolumeControlText = GetComponent<Text>();

        GetComponentInParent<Slider>().onValueChanged.AddListener(MasterVolumeControl);
    }


    public void MasterVolumeControl(float masterVolume)
    {
        AudioController.audioController.SetMasterVolume(masterVolume);

        masterVolumeControlText.text = (masterVolume * 10).ToString("0");
    }


} // end of class
