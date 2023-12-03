
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2021.06.07
//
// 2019.12.17
//

public class CoinSlotController : MonoBehaviour
{
    [SerializeField] private Text coinsInsertedText;

    [SerializeField] private Text insertCoinsText;
    [SerializeField] private Text pressGameSelectText;
    [SerializeField] private Text pressStartText;

    // game credits
    private const int INSERT_COINS = 0;
    public const int ONE_PLAYER_COINS = 1;
    private const int MAXIMUM_COINS = 1;

    // game credits
    public int gameCredits;

    public bool canPlay;



    private void Start()
    {
        InitialiseCoinSlot();
    }


    private void InitialiseCoinSlot()
    {
        gameCredits = INSERT_COINS;

        UpdateGameCreditsText();
    }


    public void CoinSlotControl()
    {
        gameCredits += 1;

        if (gameCredits > MAXIMUM_COINS)
        {
            gameCredits = MAXIMUM_COINS;
        }

        AudioController.audioController.PlayAudioClip("Coin Inserted");

        AudioController.audioController.PlayAudioClip("1UP Credit");

        UpdateGameCreditsText();

        insertCoinsText.gameObject.SetActive(false);

        canPlay = true;
    }


    private void UpdateGameCreditsText()
    {
        coinsInsertedText.text = gameCredits.ToString("00");
    }


} // end of class
