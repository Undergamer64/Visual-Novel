using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextAsset inkFile;
    public GameObject textBox;
    public GameObject customButton;
    public GameObject optionPanel;
    public bool m_isTalking = false;

    static Story story;
    TextMeshProUGUI nametag;
    TextMeshProUGUI message;
    List<string> tags;
    static Choice choiceSelected;

    [SerializeField]
    private GameObject m_transition;

    [SerializeField]
    private GameObject m_background;

    [SerializeField]
    private List<Sprite> m_backgroundSprite;

    [SerializeField]
    private GameObject m_playerImage;

    [SerializeField]
    private List<Sprite> m_playerSprites = new List<Sprite>();

    [Header("Sprites")]
    [SerializeField]
    private GameObject m_CharacterImage;

    [SerializeField]
    private List<Sprite> m_FrankSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> m_DylanSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> m_MarieSprites = new List<Sprite>();

    [SerializeField]
    private List<Sprite> m_AmberSprites = new List<Sprite>();

    [SerializeField]
    private Animator m_playerAnimation;

    [SerializeField]
    private Animator m_characterAnimation;

    private Sprite m_nextSprite;
    private Sprite m_nextBackground;

    private bool m_canContinueToNextLine = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkFile.text);
        nametag = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tags = new List<string>();
        choiceSelected = null;
        StartCoroutine(SoundsManager.instance.PlayMusicEndlessly(SoundsManager.TypesOfMusics.LEVEL, 5));
        AdvanceDialogue();
    }

    public void TextAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Is there more to the story?
            if (story.canContinue && m_canContinueToNextLine)
            {
                AdvanceDialogue();
            }
        }
    }

    // Advance through the story 
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    // Type out the sentence letter by letter and make character idle if they were talking
    IEnumerator TypeSentence(string sentence)
    {
        m_canContinueToNextLine = false;
        message.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            /*
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                message.text = sentence;
                break;
            }
            */

            message.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        m_canContinueToNextLine = true;

        if (story.currentChoices.Count != 0)
        {
            StartCoroutine(ShowChoices());
        }
    }

    // Create then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {

        List<Choice> _choices = story.currentChoices;
        for (int i = 0; i < _choices.Count; i++)
        {
            Debug.Log(_choices[i]);
            GameObject temp = Instantiate(customButton, optionPanel.transform);
            temp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _choices[i].text;
            temp.GetComponent<Selectable>().element = _choices[i];
            temp.transform.position += Vector3.up * (i*120 - 200);
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    // Tells the story which branch to go to
    public static void SetDecision(Choice element)
    {
        choiceSelected = element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    // After a choice was made, turn off the panel and advance from that choice
    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        choiceSelected = null;
        AdvanceDialogue();
    }

    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string[] tag = t.Split(" : ");

            switch (tag[0].ToLower())
            {
                case "player":
                    SetPlayerAnimation(tag[1]);
                    break;
                case "character":
                    SetCharacterAnimation(tag[1], tag[2]);
                    break;
                case "color":
                    SetTextColor(tag[1]);
                    break;
                case "scene":
                    ChangeBackGround(tag[1]);
                    break;
                case "swap":
                    m_characterAnimation.SetBool("Swap", true);
                    break;
                default:
                    break;
            }
        }
    }

    private void SetPlayerAnimation(string _animation)
    {
        m_playerAnimation.SetBool("PlayerExist", true);
        switch (_animation)
        {
            case "None":
                m_playerAnimation.SetBool("PlayerExist", false);
                break;
            case "Idle":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[0];
                break;
            case "Happy":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[1];
                break;
            case "Doubtful":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[2];
                break;
            case "Embarrassed":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[3];
                break;
            case "Empathic":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[4];
                break;
            case "Annoyed":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[5];
                break;
            case "Surprised":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[6];
                break;
            case "Sad":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[7];
                break;
            case "FacePalm":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[8];
                break;
            default:
                break;
        }
    }

    private void SetCharacterAnimation(string _name, string _animation)
    {
        bool IsSwapping = m_characterAnimation.GetBool("Swap");
        m_characterAnimation.SetBool("CharacterExist", true);
        switch (_name)
        {
            case "Frank":
                switch (_animation)
                {
                    case "Idle":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[0];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[0];
                        }
                        break;
                    case "Happy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[1];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[1];
                        }
                        break;
                    case "Doubtful":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[2];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[2];
                        }
                        break;
                    case "Embarrassed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[3];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[3];
                        }
                        break;
                    case "Embarrassed2":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[4];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[4];
                        }
                        break;
                    case "Empathic":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[5];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[5];
                        }
                        break;
                    case "Annoyed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[6];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[6];
                        }
                        break;
                    case "Surprised":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[7];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[7];
                        }
                        break;
                    case "Sad":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[8];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[8];
                        }
                        break;
                    case "Angry":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[9];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[9];
                        }
                        break;
                    case "VeryHappy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_FrankSprites[10];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_FrankSprites[10];
                        }
                        break;
                    default: 
                        break;
                }
                break;
            case "Dylan":
                switch (_animation)
                {
                    case "Idle":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_DylanSprites[0];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_DylanSprites[0];
                        }
                        break;
                    case "Embarrassed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_DylanSprites[1];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_DylanSprites[1];
                        }
                        break;
                    case "Angry":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_DylanSprites[2];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_DylanSprites[2];
                        }
                        break;
                    default:
                        break;
                }
                break;
            case "Marie":
                switch (_animation)
                {
                    case "Idle":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[0];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[0];
                        }
                        break;
                    case "Happy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[1];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[1];
                        }
                        break;
                    case "Doubtful":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[2];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[2];
                        }
                        break;
                    case "Embarrassed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[3];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[3];
                        }
                        break;
                    case "Embarrassed2":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[4];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[4];
                        }
                        break;
                    case "Empathic":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[5];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[5];
                        }
                        break;
                    case "Empathic2":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[6];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[6];
                        }
                        break;
                    case "Annoyed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[7];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[7];
                        }
                        break;
                    case "Surprised":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[8];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[8];
                        }
                        break;
                    case "Sad":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[9];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[9];
                        }
                        break;
                    case "Angry":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[10];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[10];
                        }
                        break;
                    case "VeryHappy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_MarieSprites[11];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_MarieSprites[11];
                        }
                        break;
                    default:
                        break;
                }
                break;
            case "Amber":
                switch (_animation)
                {
                    case "Idle":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[0];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[0];
                        }
                        break;
                    case "Happy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[1];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[1];
                        }
                        break;
                    case "Doubtful":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[2];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[2];
                        }
                        break;
                    case "Angry":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[3];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[3];
                        }
                        break;
                    case "Annoyed":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[5];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[5];
                        }
                        break;
                    case "VeryHappy":
                        if (IsSwapping)
                        {
                            m_nextSprite = m_AmberSprites[6];
                        }
                        else
                        {
                            m_CharacterImage.GetComponent<Image>().sprite = m_AmberSprites[6];
                        }
                        break;
                    default:
                        break;
                }
                break;
            case "None":
                m_characterAnimation.SetBool("CharacterExist", false);
                break;
            default:
                break;
        }
    }

    public void ChangeBackGround(string _scene)
    {
        switch (_scene)
        {
            case "Server":
                m_nextBackground = m_backgroundSprite[0];
                m_transition.SetActive(true);
                break;
            case "Corridor1":
                m_nextBackground = m_backgroundSprite[1];
                m_transition.SetActive(true);
                break;
            case "Corridor2":
                m_nextBackground = m_backgroundSprite[2];
                m_transition.SetActive(true);
                break;
            case "AmberDesk":
                m_nextBackground = m_backgroundSprite[3];
                m_transition.SetActive(true);
                break;
            case "MeetingRoom":
                m_nextBackground = m_backgroundSprite[4];
                m_transition.SetActive(true);
                break;
            case "DesksRoom":
                m_nextBackground = m_backgroundSprite[5];
                m_transition.SetActive(true);
                break;
            default:
                Debug.Log($"{_scene} is not available as a scene");
                break;
        }
    }

    private void SetTextColor(string _color)
    {
        switch(_color)
        {
            case "red":
                message.color = Color.red;
                break;
            case "blue":
                message.color = Color.cyan;
                break;
            case "green":
                message.color = Color.green;
                break;
            case "white":
                message.color = Color.white;
                break;
            default:
                Debug.Log($"{_color} is not available as a text color");
                message.color = Color.white;
                break;
        }
    }

    public void CharacterSwap()
    {
        m_CharacterImage.GetComponent<Image>().sprite = m_nextSprite;
        m_characterAnimation.SetBool("Swap", false);
    }

    public void BackgroundSwap()
    {
        m_background.GetComponent<Image>().sprite = m_nextBackground;
    }
}
