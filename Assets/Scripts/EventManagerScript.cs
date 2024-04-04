using UnityEngine;

public class EventManagerScript : MonoBehaviour
{
    public static EventManagerScript instance;

    public Animator m_anim;
    public bool m_isTalking;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_isTalking = false;
    }

    public void PlayAnimation(string _name)
    {
        switch(_name)
        {
            // General Animations //
            case "idle":
                m_anim.SetTrigger("toIdle");
                break;
            case "angry":
                m_isTalking = true;
                m_anim.SetTrigger("toAngry");
                break;
            case "embarrassed":
                m_anim.SetTrigger("toEmbarrassed");
                break;
            case "talk":
                m_isTalking = true;
                m_anim.SetTrigger("toTalk");
                break;

            // Player Animations //
            case "playerNone":
                m_anim.SetTrigger("toPlayerNone");
                break;
            case "playerIdle":
                m_anim.SetTrigger("toPlayerIdle");
                break;
            case "playerHappy":
                m_anim.SetTrigger("toPlayerHappy");
                break;
            case "playerDoubtful":
                m_anim.SetTrigger("toPlayerDoubtful");
                break;
            case "playerEmpathic":
                m_anim.SetTrigger("toPlayerEmpathic");
                break;
            case "playerEmpathic2":
                m_anim.SetTrigger("toPlayerEmpathic2");
                break;
            case "playerAnnoyed":
                m_anim.SetTrigger("toPlayerAnnoyed");
                break;
            case "playerSurprised":
                m_anim.SetTrigger("toPlayerSurprised");
                break;
            case "playerSad":
                m_anim.SetTrigger("toPlayerSad");
                break;
        }
    }

    public void ChangeBackGround(string _name)
    {
        switch (_name)
        {
            default:
                break;
        }
    }

}
