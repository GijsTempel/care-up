using UnityEngine;
using UnityEngine.UI;

public class UMP_DialogUI : MonoBehaviour
{
    [Header("Settings")]
    public string AnimationParamenter = "show";
    [Header("References")]
    [SerializeField]private Text mText = default(Text);
    [SerializeField]private Animator m_Animator = default(Animator);

    void OnEnable()
    {
        if (m_Animator)
        {
            m_Animator.SetBool(AnimationParamenter, true);
        }
    }

    public void Close()
    {
        if (m_Animator)
        {
            m_Animator.SetBool(AnimationParamenter, false);
            Invoke("Desactive", m_Animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void InstantClose () => Desactive ();

    public void SetText(string infoText)
    {
        mText.text = infoText;
    }

    void Desactive()
    {
        gameObject.SetActive(false);
    }
}