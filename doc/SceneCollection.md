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

You can chose a Transition scene for each level, you can customize you scene buy using one of these tree mode :

#### Mode
![Capture mode](https://github.com/Nicolas-Constanty/UnityTools/blob/master/images/SceneCollection-Mode.png)

* Auto --------------------------------- Default parameter
* Curve -------------------------------- Drive your transition scene with curve
* Constant ----------------------------- Drive your transition scene with constant value
* None --------------------------------- No transition value (0 - 1)
