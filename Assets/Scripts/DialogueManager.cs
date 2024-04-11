using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
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

    public Animator m_anim;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
        story = new Story(inkFile.text);
        nametag = textBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        message = textBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        tags = new List<string>();
        choiceSelected = null;
        AdvanceDialogue();
    }

    public void TextAction(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
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
        message.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        yield return null;
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
            temp.transform.position += Vector3.up * (i*120);
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
            string[] tag = t.Split(':');

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
                default:
                    break;
            }
        }
    }

    private void SetPlayerAnimation(string _animation)
    {
        m_anim.SetBool("PlayerExist", true);
        switch (_animation)
        {
            case "None":
                m_anim.SetBool("PlayerExist", false);
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
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[4];
                break;
            case "Empathic":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[5];
                break;
            case "Annoyed":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[6];
                break;
            case "Surprised":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[7];
                break;
            case "Sad":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[8];
                break;
            case "FacePalm":
                m_playerImage.GetComponent<Image>().sprite = m_playerSprites[9];
                break;
            default:
                break;
        }
    }

    private void SetCharacterAnimation(string _name, string _animation)
    {
        switch (_name)
        {
            case "Frank":
                switch (_animation)
                {
                    case "idle":
                        m_playerImage.GetComponent<Image>().sprite = m_FrankSprites[0];
                        break;
                    case "angry":
                        m_playerImage.GetComponent<Image>().sprite = m_FrankSprites[1];
                        break;
                    case "embarrassed":
                        m_playerImage.GetComponent<Image>().sprite = m_FrankSprites[2];
                        break;
                    case "talk":
                        m_playerImage.GetComponent<Image>().sprite = m_FrankSprites[3];
                        break;
                }
                break;
            case "Dylan":
                switch (_animation)
                {
                    case "idle":
                        m_anim.SetTrigger("toIdle");
                        break;
                    case "angry":
                        m_anim.SetTrigger("toAngry");
                        break;
                    case "embarrassed":
                        m_anim.SetTrigger("toEmbarrassed");
                        break;
                    case "talk":
                        m_isTalking = true;
                        m_anim.SetTrigger("toTalk");
                        break;
                }
                break;
            case "Marie":
                switch (_animation)
                {
                    case "idle":
                        m_anim.SetTrigger("toIdle");
                        break;
                    case "angry":
                        m_anim.SetTrigger("toAngry");
                        break;
                    case "embarrassed":
                        m_anim.SetTrigger("toEmbarrassed");
                        break;
                    case "talk":
                        m_isTalking = true;
                        m_anim.SetTrigger("toTalk");
                        break;
                }
                break;
            case "Amber":
                switch (_animation)
                {
                    case "idle":
                        m_anim.SetTrigger("toIdle");
                        break;
                    case "angry":
                        m_anim.SetTrigger("toAngry");
                        break;
                    case "embarrassed":
                        m_anim.SetTrigger("toEmbarrassed");
                        break;
                    case "talk":
                        m_isTalking = true;
                        m_anim.SetTrigger("toTalk");
                        break;
                }
                break;
            case "None":

                break;
            default:
                break;
        }
    }

    public void ChangeBackGround(string _scene)
    {
        switch (_scene)
        {
            case "server":
                m_anim.SetTrigger("server");
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

}
