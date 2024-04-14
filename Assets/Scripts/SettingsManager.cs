using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public bool m_inMenu = false;

    [Header("Slider references :")]
    [SerializeField] Slider m_mainSoundVolumeSlider;

    [Header("Graphism references :")]
    [SerializeField] Toggle m_fullScreenToggle;

    // Sound variables
    float m_mainVolume = 0.5f;

    // Graphic variables
    bool m_isOnFullScreen = false;

    public float m_MainVolume { get { return m_mainVolume; }}

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        // Link volume sliders to their respective functions
        m_mainSoundVolumeSlider.onValueChanged.AddListener(delegate {OnMainSoundVolumeChanged();} );

        // Link graphic's UI to their respective functions
        m_fullScreenToggle.onValueChanged.AddListener(delegate {OnFullScreenChanged();} );
    }

    private void Start()
    {
        this.gameObject.SetActive(false);
        m_inMenu = false;

        // Set sound's value to be equal to our variables
        m_mainSoundVolumeSlider.value = m_mainVolume;

        // Set graphic's value to be equal to our variables
        if (Screen.fullScreen)
        {
            m_fullScreenToggle.isOn = true;
            m_isOnFullScreen = true;
        }
        else
        {
            m_fullScreenToggle.isOn = false;
            m_isOnFullScreen = false;
        }
    }

    // -- Sound part -- //

    void OnMainSoundVolumeChanged()
    {
        m_mainVolume = m_mainSoundVolumeSlider.value;
        if (SoundsManager.instance != null)
        {
            SoundsManager.instance.m_musicsPlayerAudioSource.volume = m_mainVolume;
        }
    }

    // -- Graphic part -- //

    void OnFullScreenChanged()
    {
        if (m_isOnFullScreen)
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed);
            m_isOnFullScreen = false;
        }
        else
        {
            Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
            m_isOnFullScreen = true;
        }
    }
}