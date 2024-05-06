
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//
// Atari Video Game Console Controller v2019.12.17
//
// v2021.12.10
//

public class AtariConsoleCPU : MonoBehaviour
{
    public static AtariConsoleCPU atariConsoleCPU;

    public Transform gameCartridge;

    public Animator consoleAnimator;

    private CoinSlotController coinSlotController;

    #region ATARI CONSOLE CONTROLS
    [Header("Console Controls")]
    [SerializeField] private Button coinSlotButton;

    [SerializeField] private Button optionsButton;
    [SerializeField] private Button setOptionsButton;

    [SerializeField] private Button tvTypeButton;

    [SerializeField] private Button leftDifficultyButton;
    [SerializeField] private Button rightDifficultyButton;

    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    [SerializeField] private Button masterVolumeButton;
    [SerializeField] private Button musicVolumeButton;
    [SerializeField] private Button sfxVolumeButton;

    [SerializeField] private Button gameNumberSelectButton;
    [SerializeField] private Button gameResetButton;

    [SerializeField] private Button gameStartButton;
    [SerializeField] private Button gameResumeButton;

    // console control text components
    [Header("Console Controls Text")]
    [SerializeField] private Text tvTypeControlText;
    [SerializeField] private Text audioControlText;

    [SerializeField] private Text leftDifficultyControlText;
    [SerializeField] private Text rightDifficultyControlText;

    [SerializeField] private Text audioSelectControlText;
    [SerializeField] private Text masterVolumeControlText;
    [SerializeField] private Text musicVolumeControlText;
    [SerializeField] private Text sfxVolumeControlText;

    [SerializeField] private Text gameNumberSelectControlText;
    [SerializeField] private Text gameResetControlText;

    [SerializeField] private Text gameResumeControlText;
    [SerializeField] private Text gameStartControlText;

    [Header("Console Display Text")]
    [SerializeField] private Text noCartridgeInsertedText;
    [SerializeField] private Text gameTitleText;
    [SerializeField] private Text gameNumberSelectedText;
    [SerializeField] private Text numberOfPlayersText;
    #endregion

    #region CONTROL LABELS
    private const string COLOUR_TV_TEXT = "COLOUR";
    private const string BW_TV_TEXT = "B/W";
    private const string AUDIO_TEXT = "AUDIO";

    private const string DIFFICULTY_A_TEXT = "A";
    private const string DIFFICULTY_B_TEXT = "B";

    private const string MASTER_VOLUME_TEXT = "MASTER";
    private const string MUSIC_VOLUME_TEXT = "MUSIC";
    private const string SFX_VOLUME_TEXT = "SFX";

    private const string GAME_SELECT_TEXT = "GAME" + "\n" + "SELECT";
    private const string RESET_TEXT = "RESET";

    private const string START_TEXT = "START";
    private const string RESUME_TEXT = "RESUME";
    #endregion

    #region ATARI CONSOLE CONSTANTS
    // console state
    private const int CONSOLE_ACTIVE = 1;
    private const int CONSOLE_INACTIVE = -1;

    // control state
    private const bool CONTROL_ACTIVE = true;
    private const bool CONTROL_INACTIVE = false;

    // tv type control
    private const int NUMBER_OF_TV_TYPES = 2;
    private const int COLOUR_TV = 1;
    private const int BW_TV = 2;

    // audio control
    private const int NUMBER_OF_AUDIO_CONTROLS = 3;
    private const int MASTER_VOLUME = 1;
    private const int MUSIC_VOLUME = 2;
    private const int SFX_VOLUME = 3;

    // difficulty switches
    private const int DIFFICULTY_A = 1;
    private const int DIFFICULTY_B = -1;
    private const bool LEFT_DIFFICULTY_SWITCH = true;
    private const bool RIGHT_DIFFICULTY_SWITCH = false;

    private const string NO_GAME_TITLE = "";
    private const int NO_GAME_SELECTED = 0;

    private const int NUMBER_OF_PLAYERS = 0;
    private const int ONE_PLAYER = 1;
    private const int TWO_PLAYERS = 2;
    private const int THREE_PLAYERS = 3;
    private const int FOUR_PLAYERS = 4;
    private const int DOUBLES = 2;
    #endregion

    // console control modes
    [HideInInspector] public int consoleState;

    [HideInInspector] public int tvType;
    //[HideInInspector] public int audioType;

    [HideInInspector] public int leftDifficulty;
    [HideInInspector] public int rightDifficulty;

    //[HideInInspector] public int audioControlSelected;
    [HideInInspector] public float masterVolume;
    [HideInInspector] public float musicVolume;
    [HideInInspector] public float sfxVolume;

    [HideInInspector] public int gameNumberSelected;
    [HideInInspector] public int numberOfPlayers;

    // options flag
    [HideInInspector] public bool inSetOptionsMode;

    // console initialisation flag
    [HideInInspector] public bool initialisingConsoleSystem;

    // game cartridge
    private bool gameCartridgeInserted;
    private int numberOfGames;
    private int numberOfGameVariations;
    private int[,] game;

    [Header("Game State Flags")]
    // system state
    public bool inStartupMode;



    private void Awake()
    {
        atariConsoleCPU = this;

        coinSlotController = GetComponentInChildren<CoinSlotController>();
    }


    void Start()
    {
        InitialiseConsole();

        // show atari console
        SetConsoleState(CONSOLE_ACTIVE);

        // check a game cartridge has been inserted
        gameCartridgeInserted = GameCartridgeInserted();
        //Debug.Log(gameCartridgeInserted);

        // read the game cartridge data
        ReadGameCartridge();
    }


    public void InitialiseConsole()
    {
        coinSlotController.canPlay = false;

        initialisingConsoleSystem = true;

        // tv type - b/w = 1, colour = 2
        tvType = COLOUR_TV;
        SetTvType();

        // audio mode - master = 1, volume = 2, sfx = 3
        setOptionsButton.gameObject.SetActive(CONTROL_INACTIVE);

        leftDifficulty = DIFFICULTY_A;
        SetDifficulty(leftDifficulty, true);

        rightDifficulty = DIFFICULTY_A;
        SetDifficulty(rightDifficulty, false);

        SetVolumeControlButtonState(CONTROL_INACTIVE);

        gameNumberSelectButton.gameObject.SetActive(CONTROL_ACTIVE);

        gameResetButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameResumeButton.gameObject.SetActive(CONTROL_INACTIVE);
 
        gameStartButton.gameObject.SetActive(CONTROL_ACTIVE);

        // set game title
        gameTitleText.text = NO_GAME_TITLE;

        // initial game number selection
        gameNumberSelected = NO_GAME_SELECTED;

        SetGameSelection();

        // initialise game options
        //InitialiseGameCartridge();

        //insertCoinsText.gameObject.SetActive(true);

        // console initialisation complete
        initialisingConsoleSystem = false;
    }


    private bool GameCartridgeInserted()
    {
        if (gameCartridge != null)
        {
            return true;
        }

        return false;
    }


    private void ReadGameCartridge()
    {
        if (gameCartridgeInserted)
        {
            numberOfGames = 9;
            numberOfGameVariations = numberOfGames;

            game = new int[numberOfGames, numberOfGameVariations];

            // game 1
            //game[NUMBER_OF_PLAYERS] = TWO_PLAYERS;

            //SetGameSelection();
        }

        else
        {
            Debug.Log("no game cartridge");
        }
    }


    #region ESCAPE KEY
    public void EscapeKey()
    {
        //if (inDemoMode)
        //{
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                consoleState = -consoleState;

                SetConsoleState(consoleState);
            }
        //}
    }


    public void SetConsoleState(int consoleMode)
    {
        switch (consoleMode)
        {
            case CONSOLE_ACTIVE:

                consoleAnimator.SetBool("consoleMode", true);

                if (KnightRiderController.runSensorSweep)
                {
                    StartCoroutine(SensorSweepDelay());
                }

                break;

            case CONSOLE_INACTIVE:

                consoleAnimator.SetBool("consoleMode", false);

                KnightRiderController.runSensorSweep = true;

                break;
        }
    }


    private IEnumerator SensorSweepDelay()
    {
        yield return new WaitForSeconds(1f);

        AudioController.audioController.PlayAudioClip("Knight Rider");
    }
    #endregion


    #region COIN SLOT CONTROL
    public void CoinSlotControl()
    {
        if (gameCartridgeInserted)
        {
            coinSlotController.CoinSlotControl();
        }
    }
    #endregion


    #region OPTIONS CONTROL
    // activate tv type and volume controls
    public void OptionsControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        leftDifficultyButton.gameObject.SetActive(CONTROL_INACTIVE);

        rightDifficultyButton.gameObject.SetActive(CONTROL_INACTIVE);

        tvTypeButton.gameObject.SetActive(CONTROL_ACTIVE);

        gameNumberSelectButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameResetButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameResumeButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameStartButton.gameObject.SetActive(CONTROL_INACTIVE);

        SetVolumeControlButtonState(CONTROL_ACTIVE);

        optionsButton.gameObject.SetActive(CONTROL_INACTIVE);

        setOptionsButton.gameObject.SetActive(CONTROL_ACTIVE);

        inSetOptionsMode = true;
    }


    public void SetOptionsControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        tvTypeButton.gameObject.SetActive(CONTROL_INACTIVE);

        SetVolumeControlSliderState(CONTROL_INACTIVE);

        SetVolumeControlButtonState(CONTROL_INACTIVE);

        leftDifficultyButton.gameObject.SetActive(CONTROL_ACTIVE);

        rightDifficultyButton.gameObject.SetActive(CONTROL_ACTIVE);

        gameResetButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameResumeButton.gameObject.SetActive(CONTROL_INACTIVE);

        gameNumberSelectButton.gameObject.SetActive(CONTROL_ACTIVE);

        gameStartButton.gameObject.SetActive(CONTROL_ACTIVE);

        setOptionsButton.gameObject.SetActive(CONTROL_INACTIVE);

        optionsButton.gameObject.SetActive(CONTROL_ACTIVE);

        inSetOptionsMode = false;
    }
    #endregion


    #region TV TYPE CONTROL
    // select tv type
    public void TvTypeControl()
    {
        tvType += 1;

        AudioController.audioController.PlayAudioClip("Button Press");

        if (tvType > NUMBER_OF_TV_TYPES)
        {
            tvType = COLOUR_TV;
        }

        SetTvType();
    }


    // set tv type
    private void SetTvType()
    {
        switch (tvType)
        {
            case COLOUR_TV:

                tvTypeControlText.text = COLOUR_TV_TEXT;

                // change display to colour
                //SetColourType();

                break;

            case BW_TV:

                tvTypeControlText.text = BW_TV_TEXT;

                // change display to black and white
                //SetClassicType();

                break;
        }
    }


    private void SetClassicType()
    {
        //PostProcessVolume volume = greyScalePostProcessing.gameObject.GetComponent<PostProcessVolume>();

        //volume.isGlobal = true;
    }


    private void SetColourType()
    {
        //PostProcessVolume volume = greyScalePostProcessing.gameObject.GetComponent<PostProcessVolume>();

        //volume.isGlobal = false;
    }
    #endregion


    #region AUDIO CONTROL
    public void MasterVolumeControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        InitialiseSelectedAudioControls(MASTER_VOLUME);
    }


    public void MusicVolumeControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        InitialiseSelectedAudioControls(MUSIC_VOLUME);
    }


    public void SfxVolumeControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        InitialiseSelectedAudioControls(SFX_VOLUME);
    }


    private void InitialiseSelectedAudioControls(int audioControlSelected)
    {
        switch (audioControlSelected)
        {
            case MASTER_VOLUME:

                musicVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                sfxVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                masterVolumeSlider.gameObject.SetActive(CONTROL_ACTIVE);

                masterVolumeControlText.text = (masterVolumeSlider.value * 10).ToString("0");

                break;

            case MUSIC_VOLUME:

                masterVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                sfxVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                musicVolumeSlider.gameObject.SetActive(CONTROL_ACTIVE);

                musicVolumeControlText.text = (musicVolumeSlider.value * 10).ToString("0");

                break;

            case SFX_VOLUME:

                masterVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                musicVolumeSlider.gameObject.SetActive(CONTROL_INACTIVE);

                sfxVolumeSlider.gameObject.SetActive(CONTROL_ACTIVE);

                sfxVolumeControlText.text = (sfxVolumeSlider.value * 10).ToString("0");

                break;
        }
    }


    private void SetVolumeControlButtonState(bool controlState)
    {
        masterVolumeButton.gameObject.SetActive(controlState);

        musicVolumeButton.gameObject.SetActive(controlState);

        sfxVolumeButton.gameObject.SetActive(controlState);
    }


    private void SetVolumeControlSliderState(bool controlState)
    {
        masterVolumeSlider.gameObject.SetActive(controlState);

        musicVolumeSlider.gameObject.SetActive(controlState);

        sfxVolumeSlider.gameObject.SetActive(controlState);
    }
    #endregion


    #region DIFFICULTY CONTROLS
    public void LeftDifficultyControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        leftDifficulty = -leftDifficulty;

        SetDifficulty(leftDifficulty, LEFT_DIFFICULTY_SWITCH);
    }


    public void RightDifficultyControl()
    {
        AudioController.audioController.PlayAudioClip("Button Press");

        rightDifficulty = -rightDifficulty;

        SetDifficulty(rightDifficulty, RIGHT_DIFFICULTY_SWITCH);
    }


    private void SetDifficulty(int difficultySetting, bool leftSwitch)
    {
        switch (difficultySetting)
        {
            case DIFFICULTY_A:

                if (leftSwitch)
                {
                    leftDifficultyControlText.text = DIFFICULTY_A_TEXT;

                    SetLeftDifficultyA();
                }


                else
                {
                    rightDifficultyControlText.text = DIFFICULTY_A_TEXT;

                    SetRightDifficultyA();
                }

                break;

            case DIFFICULTY_B:

                if (leftSwitch)
                {
                    leftDifficultyControlText.text = DIFFICULTY_B_TEXT;

                    SetLeftDifficultyB();
                }

                else
                {
                    rightDifficultyControlText.text = DIFFICULTY_B_TEXT;

                    SetRightDifficultyB();
                }

                break;
        }
    }


    private void SetLeftDifficultyA()
    {

    }


    private void SetRightDifficultyA()
    {

    }


    private void SetLeftDifficultyB()
    {

    }


    private void SetRightDifficultyB()
    {

    }
    #endregion


    #region GAME NUMBER CONTROL
    public void GameNumberControl()
    {
        if (gameCartridgeInserted && coinSlotController.canPlay) // && !inPawzMode)
        {
            gameNumberSelected += 1;

            AudioController.audioController.PlayAudioClip("Button Press");

            if (coinSlotController.gameCredits == CoinSlotController.ONE_PLAYER_COINS || gameNumberSelected > numberOfGames)
            {
                // game 1
                gameNumberSelected = ONE_PLAYER;
            }

            SetGameSelection();
        }

        else
        {
            AudioController.audioController.PlayAudioClip("Button Disabled");
        }
    }


    private void SetGameSelection()
    {
        gameNumberSelectedText.text = gameNumberSelected.ToString();

        if (gameNumberSelected != NO_GAME_SELECTED)
        {
            //numberOfPlayersText.text = game[gameNumberSelected - 1].ToString();

            //gameOverText.gameObject.SetActive(false);

            //pressStartText.gameObject.SetActive(true);
        }

        else
        {
            numberOfPlayersText.text = "0";
        }
    }
    #endregion


    public void GameResetControl()
    {

    }


    public void GameResumeControl()
    {
        AudioController.audioController.PlayAudioClip("Start Button");

        //inPawzMode = false;

        //inGameMode = true;

        SetPawzModeSwitches();

        SetPlayMode();
    }


    public void GameStartControl()
    {
        if (gameNumberSelected > NO_GAME_SELECTED)
        {
            AudioController.audioController.PlayAudioClip("Start Button");

            switch (gameNumberSelected)
            {
                case ONE_PLAYER:

                    //AtariGameController.atariGameController.StartOnePlayerGame();

                    break;

                case TWO_PLAYERS:

                    //AtariGameController.atariGameController.StartTwoPlayerGame();

                    break;

                case THREE_PLAYERS:

                    //AtariGameController.atariGameController.StartThreePlayerGame();

                    break;

                case FOUR_PLAYERS:

                    //AtariGameController.atariGameController.StartFourPlayerGame();

                    break;
            }
        }

        else
        {
            AudioController.audioController.PlayAudioClip("Button Disabled");
        }
    }


    public void GamePawzSwitch()
    {
        //GameController.gameController.inPawzMode = true;

        //GameController.gameController.inGameMode = false;

        SetPawzModeSwitches();

        //GameController.gameController.SetPawzMode();
    }


    // =============================================================================
    // sets the pawz, reset and resume switches depending on the game state
    // =============================================================================
    public void SetPawzModeSwitches()
    {
        if (initialisingConsoleSystem)
        {
            //gamePawzSwitch.gameObject.SetActive(false);
        }


        //if (GameController.gameController.inPawzMode)
        //{
        //gamePawzSwitch.gameObject.SetActive(false);

        //gameStartSwitch.gameObject.SetActive(false);

        //gameResumeSwitch.gameObject.SetActive(true);
        //}


        //if (GameController.gameController.inDemoMode)
        //{
        //gamePawzSwitch.gameObject.SetActive(false);

        //gameResumeSwitch.gameObject.SetActive(false);

        //gameStartSwitch.gameObject.SetActive(true);
        //}


        //if (gameController.inPlayMode)
        //{
        //gamePawzSwitch.gameObject.SetActive(true);
        //}

    }


    public void SetPawzMode()
    {
        //mainBall1Controller.FreezeBall();

        //SetAtariConsoleMode(AtariConsole.CONSOLE_VISIBLE);
    }


    public void SetPlayMode()
    {
        //SetAtariConsoleMode(AtariConsole.CONSOLE_HIDDEN);

        //mainBall1Controller.ResumeBall();
    }


} // end of class
