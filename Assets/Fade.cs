using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityTools;

[RequireComponent(typeof(Image))]
public class Fade : MonoBehaviour
{

    private Image m_Img;
    private enum FadeMode
    {
        In,
        Out
    }

    private void Awake()
    {
        m_Img = GetComponent<Image>();
        m_Img.color = new Color(m_Img.color.r, m_Img.color.g, m_Img.color.b, 0);
        SceneManager.Instance.TransitionInEvent.AddListener(FadeTransition);
        SceneManager.Instance.TransitionOutEvent.AddListener(FadeTransition);
    }

    private void FadeTransition(float v)
    {
        Color c = m_Img.color;
        c.a = v;
        m_Img.color = c;
    }

    public void FadeIn()
    {
        StartCoroutine("AsyncFade", FadeMode.In);
    }

    public void FadeOut()
    {
        StartCoroutine("AsyncFade", FadeMode.Out);
    }

    IEnumerator AsyncFade(FadeMode value)
    {
        Color c = m_Img.color;
        switch (value)
        {
            case FadeMode.In:
                while (m_Img.color.a < 1)
                {
                    c.a += .01f;
                    m_Img.color = c;
                    yield return new WaitForEndOfFrame();
                }
                break;
            case FadeMode.Out:
                while (m_Img.color.a > 0)
                {
                    c.a -= .01f;
                    m_Img.color = c;
                    yield return new WaitForEndOfFrame();
                }
                break;
        }
    }
}
