using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityTools;

[RequireComponent(typeof(Text))]
public class UpdateLoading : MonoBehaviour
{

    public string Format = "Loading {0}%";
    private Text m_T;

    private void Start()
    {
        m_T = GetComponent<Text>();
        SceneManager.Instance.LoadingEvent.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        m_T.text = string.Format(Format, value * 100);
    }

    private void OnDestroy()
    {
        SceneManager.Instance.LoadingEvent.RemoveListener(UpdateText);
    }
}
