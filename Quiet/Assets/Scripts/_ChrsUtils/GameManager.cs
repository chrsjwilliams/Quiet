using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.SceneManagement;
using BeatManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private KeyCode RESTART_GAME = KeyCode.Backspace;

    private const string RELOAD_GAME = "_Main";
    [SerializeField] private bool timedRestart;

    [SerializeField]
    private Clock clock;
    public double levelBPM;

    private float inactivityTimer;
    private const float inactivityBeforeReset = 180f;

    private bool soundEffectsEnabled = true;
    public bool SoundEffectsEnabled
    {
        get { return soundEffectsEnabled; }
        set
        {
            soundEffectsEnabled = value;
            UpdateSoundEffectPlayerPrefs();
        }
    }
    private bool musicEnabled = true;

    public bool MusicEnabled
    {
        get { return musicEnabled; }
        set
        {
            musicEnabled = value;
            UpdateMusicPlayerPrefs();
        }
    }

    private readonly string SOUNDEFFECTSENABLED = "soundEffectsEnabledKey";
    private readonly string MUSICENABLED = "musicEnabledKey";

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    [SerializeField] private int _numPlayers;
    public int NumPlayers
    {
        get { return _numPlayers; }
        private set
        {
            if (_numPlayers <= 0)
            {
                _numPlayers = 1;
            }
            else
            {
                _numPlayers = value;
            }
        }
    }

    public float duration { get; private set; }


    [SerializeField] private Camera _mainCamera;
    public Camera MainCamera
    {
        get { return _mainCamera; }
    }

    private void Awake()
    {
        Assert.raiseExceptions = true;
        InitializeServices();
        Services.GlobalEventManager.Register<Reset>(Reset);
        Services.GlobalEventManager.Register<TouchDown>(ResetInactivity);
        Services.GlobalEventManager.Register<MouseDown>(ResetInactivity);

    }

    public void Init()
    {
        NumPlayers = 1;
        _mainCamera = Camera.main;
        Services.GameEventManager.Register<KeyPressedEvent>(OnKeyPressed);
        Services.GlobalEventManager.Register<Reset>(Reset);
        Services.GlobalEventManager.Register<TouchDown>(ResetInactivity);
        Services.GlobalEventManager.Register<MouseDown>(ResetInactivity);
        Input.simulateMouseWithTouches = false;
    }

    private void InitializeServices()
    {
        Services.GameEventManager = new GameEventsManager();
        Services.GlobalEventManager = new GameEventsManager();
        Services.GameManager = this;
        Services.GameData = GetComponent<GameData>();
        Init();

        Services.Clips = Resources.Load<ClipLibrary>("Audio/ClipLibrary");
        Services.AudioManager = new GameObject("Audio Manager").AddComponent<AudioManager>();

        Services.GeneralTaskManager = new TaskManager();
        Services.Prefabs = Resources.Load<PrefabDB>("Prefabs/PrefabDB");


        Services.InputManager = new InputManager();
        Services.Scenes = new GameSceneManager<TransitionData>(gameObject, Services.Prefabs.Scenes);
        Services.CameraController = MainCamera.GetComponent<CameraController>();
        Services.Clock = clock;
        Services.Clock.Init(levelBPM);
    }

    private void OnKeyPressed(KeyPressedEvent e)
    {
        if (e.key == RESTART_GAME) ReloadGame();
    }

    public void ShowInstructions()
    {
        Services.Scenes.PushScene<InstructionSceneScript>();
    }

    public void PopScene()
    {
        Services.Scenes.PopScene();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(RELOAD_GAME);
    }

    public void RefreshButton()
    {
        Debug.Assert(Services.Scenes.CurrentScene is GameSceneScript);

        RefreshGame();
    }

    public void RefreshGame()
    {
        Services.Scenes.PopScene();
        Services.Scenes.PushScene<GameSceneScript>();    
    }

	// Use this for initialization
	public void Init (int players)
    {
        NumPlayers = players;
        _mainCamera = Camera.main;
	}
	
    public void ChangeCameraTo(Camera camera)
    {
        _mainCamera = camera;
    }

    public void SetDuration(float t) { duration = t; }

    // Update is called once per frame
    void Update()
    {
        Services.InputManager.Update();
        Services.GeneralTaskManager.Update();
        if (timedRestart)
        {
            InactivityCheck();
        }
    }

    private void ResetInactivity(MouseDown e)
    {
        inactivityTimer = 0;
    }

    private void ResetInactivity(TouchDown e)
    {
        inactivityTimer = 0;
    }

    private void InactivityCheck()
    {
        inactivityTimer += Time.deltaTime;
        if (inactivityTimer >= inactivityBeforeReset)
        {
            Services.AudioManager.FadeOutLevelMusic();
            Reset(new Reset());
        }
    }
    public void Reset(Reset e)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void UpdateSoundEffectPlayerPrefs()
    {
        PlayerPrefs.SetInt(SOUNDEFFECTSENABLED, SoundEffectsEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateMusicPlayerPrefs()
    {
        PlayerPrefs.SetInt(MUSICENABLED, MusicEnabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static Color float_array_to_Color(float[] arr)
    {
        Debug.Assert(arr.Length == 3);

        return new Color(arr[0], arr[1], arr[2]);
    }

    public static float[] color_to_float_array(Color color)
    {
        return new float[] { color.r, color.g, color.b };
    }

    private Color vector3_to_color(Vector3 vec)
    {
        return new Color(vec.x, vec.y, vec.z);
    }

    private Vector3 color_to_vector3(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }
}
