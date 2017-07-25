## SceneCollection

A scene collection can store and load multiple scene easily

### Overview
![SceneCollection panel](https://github.com/Nicolas-Constanty/UnityTools/blob/master/images/SceneCollectionPanel.png)

### Create

Open SceneManagement window
```
Window -> SceneManagement -> Create SceneCollection
```

Rename it and move it to an appropriate folder

### Load in editor

Once your collection is created, if you select it you should obtain something like this :

![Capture load collection](https://github.com/Nicolas-Constanty/UnityTools/blob/master/images/SceneCollection-LoadCollection.png)

(Clic on green zone to load your scene collection)

### Load additive in editor

Once your collection is created, if you select it you should obtain something like this :

![Capture load additive collection](https://github.com/Nicolas-Constanty/UnityTools/blob/master/images/SceneCollection-LoadAdditiveCollection.png)

(Clic on green zone to load your scene collection with additive parameter, allow to combine SceneCollection)

### Transition

You can chose a Transition scene for each level, you can customize you scene by using one of these tree mode :

#### Mode
![Capture mode](https://github.com/Nicolas-Constanty/UnityTools/blob/master/images/SceneCollection-Mode.png)

* Auto (Default parameter)
* Curve (Drive your transition scene with curve)
* Constant (Drive your transition scene with constant value)
* None (No transition value)

### Access to the transitions info

#### In/Out

You can access to the transition like in this example of FadeIn/Out :

```csharp
    //Call before loading your next level
    SceneManager.Instance.TransitionInEvent.AddListener('your function with float parameter');
    
    //Call after loading your next level
    SceneManager.Instance.TransitionOutEvent.AddListener('your function with float parameter');
```

```csharp
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityTools;

[RequireComponent(typeof(Image))]
public class Fade : MonoBehaviour
{
    private Image m_Img;

    private void Awake()
    {
        m_Img = GetComponent<Image>();
        //Makes color transparent before fadeIn/Out
        m_Img.color = new Color(m_Img.color.r, m_Img.color.g, m_Img.color.b, 0);

        //AddListener for transition In
        SceneManager.Instance.TransitionInEvent.AddListener(FadeTransition);

        //AddListener for transition Out
        SceneManager.Instance.TransitionOutEvent.AddListener(FadeTransition);
    }

    //Value v is directly inject by SceneManager once you have register these events
    private void FadeTransition(float v)
    {
        Color c = m_Img.color;
        c.a = v;
        m_Img.color = c;
    }
}
```

#### Loading Percentage

The sceneManager automatically calculates the loading percentage of your level you can simply access with :

```csharp
    //Call when your level is loading (each frame)
    SceneManager.Instance.LoadingEvent.AddListener('your function with float parameter');
```

```csharp
using UnityEngine;
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
        
        //Register LoadingEvent
        SceneManager.Instance.LoadingEvent.AddListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        //Assign value inject by the SceneManager event
        m_T.text = string.Format(Format, value * 100);
    }

    private void OnDestroy()
    {
        SceneManager.Instance.LoadingEvent.RemoveListener(UpdateText);
    }
}

```
