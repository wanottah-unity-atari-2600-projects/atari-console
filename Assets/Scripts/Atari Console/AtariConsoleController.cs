
using UnityEngine;

//
// Atari Video Game Console Controller v2019.12.17
//
// v2021.12.10
//

public class AtariConsoleController : MonoBehaviour
{
    public static AtariConsoleController atariConsoleController;



    private void Awake()
    {
        atariConsoleController = this;
    }


    public void CoinSlotControl()
    {
        AtariConsoleCPU.atariConsoleCPU.CoinSlotControl();
    }


    // select options control
    public void OptionsControl()
    {
        AtariConsoleCPU.atariConsoleCPU.OptionsControl();
    }


    // set options control
    public void SetOptionsControl()
    {
        AtariConsoleCPU.atariConsoleCPU.SetOptionsControl();
    }


    // select tv type
    public void TvTypeControl()
    {
        AtariConsoleCPU.atariConsoleCPU.TvTypeControl();
    }


    public void LeftDifficultyControl()
    {
        AtariConsoleCPU.atariConsoleCPU.LeftDifficultyControl();
    }


    public void RightDifficultyControl()
    {
        AtariConsoleCPU.atariConsoleCPU.RightDifficultyControl();
    }


    public void MasterVolumeControl()
    {
        AtariConsoleCPU.atariConsoleCPU.MasterVolumeControl();
    }


    public void MusicVolumeControl()
    {
        AtariConsoleCPU.atariConsoleCPU.MusicVolumeControl();
    }


    public void SfxVolumeControl()
    {
        AtariConsoleCPU.atariConsoleCPU.SfxVolumeControl();
    }


    public void GameNumberSelectControl()
    {
        AtariConsoleCPU.atariConsoleCPU.GameNumberControl();
    }


    public void GameResetControl()
    {
        AtariConsoleCPU.atariConsoleCPU.GameResetControl();
    }


    public void GameResumeControl()
    {
        AtariConsoleCPU.atariConsoleCPU.GameResumeControl();
    }


    public void GameStartControl()
    {
        AtariConsoleCPU.atariConsoleCPU.GameStartControl();
    }


} // end of class
