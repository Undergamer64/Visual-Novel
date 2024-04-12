using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;

    // References
    [Header("References")]
    [SerializeField] private AudioSource m_musicsPlayerAudioSource;
    [SerializeField] private AudioSource m_sfxPlayerAudioSource;


    // Getters Setters
    public AudioSource m_MusicsPlayerAudioSource { get { return m_musicsPlayerAudioSource; } }
    public AudioSource m_SFXPlayerAudioSource { get { return m_sfxPlayerAudioSource; } }

    [Header("Sounds")]
    [SerializeField] private AllMusics m_allMusics;
    [SerializeField] private AllSFX m_allSFX;

    public enum TypesOfMusics
    {
        // Menu
        MAIN_MENU,

        // In-Game
        LEVEL,
    }

    public enum TypesOfSFX
    {
        // Players's actions
        ANT_SELECTED,
        ANT_MOVED,
        DICE_THROW,

        // Ant Damaged
        ANT_HITTEN,
        ANT_DESTROYED,

        // Enemy anthill's actions
        // TO DO : Add more sound

        // Enemy anthill's moves
        ENEMY_ANTHILL_SHOOTING,
        ENEMY_ANTHILL_BEING_UPGRADED,

        // Enemy anthill damaged
        ENEMY_ANTHILL_HITTEN,
        ENEMY_ANTHILL_DESTOYRED,

        // Environment
        FLUORESCENT_LIGHT_BUZZ,
        FLUORESCENT_LIGHT_CLICK,

        // UIs
        HOVER_BUTTON,
        BUTTON_PRESSED,
    }

    // Structs
    [Serializable]
    struct AllMusics
    {
        [Header("Menus :")]
        public List<AudioClip> m_MainMenu;

        [Header("In-game")]
        public List<AudioClip> m_Level;
    }

    [Serializable]
    struct AllSFX
    {
        [Header("Player actions :")]
        public AudioClip[] m_AntSelected;
        public AudioClip[] m_AntMoved;
        public AudioClip[] m_DiceThrow;

        [Header("Ant damaged :")]
        public AudioClip[] m_AntHitten;
        public AudioClip[] m_AntDestroyed;

        //[Header("Enemy anthill's actions :")]
        // TO DO : Add more sound

        [Header("Enemy anthill's moves :")]
        public AudioClip[] m_EnemyAnthillShotting;
        public AudioClip[] m_EnemyAnthillBeingUpgraded;

        [Header("Enemy anthill damaged :")]
        public AudioClip[] m_EnemyAnthillHitten;
        public AudioClip[] m_EnemyAnthillDestroyed;

        [Header("Environment :")]
        public AudioClip[] m_FluorescentLightBuzz;
        public AudioClip[] m_FluorescentLightClick;

        [Header("UIs :")]
        public AudioClip[] m_HoverButton;
        public AudioClip[] m_ButtonPressed;
    }

    // -- Methods -- //
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    /// <summary> Return a random AudioClip of the type of Music wanted </summary>
    public List<AudioClip> ReturnMusic(TypesOfMusics _typesOfMusics)
    {
        // NOTE : If you are here because of a Unity error (not in default) that's surely because :
        // The music is not define. Go in SoundManager GameObject to define it in the Inspector. 

        switch (_typesOfMusics)
        {
            // Menus
            case TypesOfMusics.MAIN_MENU:
                return m_allMusics.m_MainMenu;

            // In-game
            case TypesOfMusics.LEVEL:
                return m_allMusics.m_Level;

            default:
                Debug.LogError($"The type of SFX {_typesOfMusics} is not planned in the switch statement.");
                return null;
        }
    }

    /// <summary> Randomize the music list given and play it endlessly /!\ It's a Coroutine /!\ </summary>
    public IEnumerator PlayMusicEndlessly(TypesOfMusics _typesOfMusics, float _musicVolume = 1)
    {
        List<AudioClip> musicList = ReturnMusic(_typesOfMusics);

        while (true)
        {
            // Generation of a random number
            System.Random _randomNumber = new();

            // Shuffling the musicList
            for (int i = musicList.Count - 1; i > 0; i--)
            {
                // Get a random emplacement in the list
                int randomIndex = _randomNumber.Next(0, i + 1);

                // Change the position of the music into a random one in the list without making a temporary variable
                (musicList[randomIndex], musicList[i]) = (musicList[i], musicList[randomIndex]);
            }

            // Playing the music list entierely
            for (int i = 0; i < musicList.Count; i++)
            {
                m_musicsPlayerAudioSource.clip = musicList[i];

                m_musicsPlayerAudioSource.Play();

                m_musicsPlayerAudioSource.volume = _musicVolume * 1;

                yield return new WaitForSecondsRealtime(musicList[i].length);
            }
        }
    }

    public void StopMusic()
    {
        m_musicsPlayerAudioSource.Stop();
    }

    /// <summary> Return a random AudioClip of the type of SFX wanted </summary>
    public AudioClip ReturnSFX(TypesOfSFX _typeOfSFX)
    {
        // NOTE : If you are here because of a Unity error (not in default) that's surely because :
        // The SFX is not define. Go in SoundManager GameObject to define it in the Inspector. 

        switch (_typeOfSFX)
        {
            // Players's actions
            case TypesOfSFX.ANT_SELECTED:
                return m_allSFX.m_AntSelected[Random.Range(0, m_allSFX.m_AntSelected.Length)];

            case TypesOfSFX.ANT_MOVED:
                return m_allSFX.m_AntMoved[Random.Range(0, m_allSFX.m_AntMoved.Length)];

            case TypesOfSFX.DICE_THROW:
                return m_allSFX.m_DiceThrow[Random.Range(0, m_allSFX.m_DiceThrow.Length)];

            // Ant damaged
            case TypesOfSFX.ANT_HITTEN:
                return m_allSFX.m_AntHitten[Random.Range(0, m_allSFX.m_AntHitten.Length)];

            case TypesOfSFX.ANT_DESTROYED:
                return m_allSFX.m_AntDestroyed[Random.Range(0, m_allSFX.m_AntDestroyed.Length)];

            // Enemy anthill's actions
            // TO DO : Add more sound

            // EnemyAnthill's moves
            case TypesOfSFX.ENEMY_ANTHILL_SHOOTING:
                return m_allSFX.m_EnemyAnthillShotting[Random.Range(0, m_allSFX.m_EnemyAnthillShotting.Length)];

            case TypesOfSFX.ENEMY_ANTHILL_BEING_UPGRADED:
                return m_allSFX.m_EnemyAnthillBeingUpgraded[Random.Range(0, m_allSFX.m_EnemyAnthillBeingUpgraded.Length)];

            // Enemy anthill damaged
            case TypesOfSFX.ENEMY_ANTHILL_HITTEN:
                return m_allSFX.m_EnemyAnthillHitten[Random.Range(0, m_allSFX.m_EnemyAnthillHitten.Length)];

            case TypesOfSFX.ENEMY_ANTHILL_DESTOYRED:
                return m_allSFX.m_EnemyAnthillDestroyed[Random.Range(0, m_allSFX.m_EnemyAnthillDestroyed.Length)];

            // Environment
            case TypesOfSFX.FLUORESCENT_LIGHT_BUZZ:
                return m_allSFX.m_FluorescentLightBuzz[Random.Range(0, m_allSFX.m_FluorescentLightBuzz.Length)];

            case TypesOfSFX.FLUORESCENT_LIGHT_CLICK:
                return m_allSFX.m_FluorescentLightClick[Random.Range(0, m_allSFX.m_FluorescentLightClick.Length)];

            // UIs
            case TypesOfSFX.HOVER_BUTTON:
                return m_allSFX.m_HoverButton[Random.Range(0, m_allSFX.m_HoverButton.Length)];

            case TypesOfSFX.BUTTON_PRESSED:
                return m_allSFX.m_ButtonPressed[Random.Range(0, m_allSFX.m_ButtonPressed.Length)];

            default:
                Debug.LogError($"The type of SFX {_typeOfSFX} is not planned in the switch statement.");
                return null;
        }
    }

    /// <summary> Play a random SFX of the type of SFX you wanted </summary>
    public void PlaySFX(TypesOfSFX _typesOfSFX, float _SFXvolume = 1)
    {
        m_sfxPlayerAudioSource.PlayOneShot(ReturnSFX(_typesOfSFX), _SFXvolume * 1);
    }
}