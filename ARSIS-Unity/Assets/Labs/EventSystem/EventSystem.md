
# Event System Lab
- [Event System Lab](#event-system-lab)
- [Summary](#summary)
- [Setup](#setup)
- [Part 1: Creating A Basic System](#part-1-creating-a-basic-system)
  - [UI](#ui)
  - [Creating The Event](#creating-the-event)
  - [Listening For The Event](#listening-for-the-event)
  - [Sending The Event](#sending-the-event)
  - [Testing The Event](#testing-the-event)
- [Part 2: Adding A Cache](#part-2-adding-a-cache)
- [Conclusion](#conclusion)

# Summary

Welcome to this lab on creating and calling basic events using the Arsis Event System.
In this lab, you will learn about the fundamental design principles and data flow of events in Arsis.
By the end of the lab, you will have created a simple button that increments a 
counter when pressed, and a cache that stores the counter value between restarts.

This lab should take about `30` minutes to complete.

# Setup
Before you begin, make sure that the branch of the repository you are working on has the lab files. 
If you are not sure, there should be the following files in the `Assets/Labs/EventSystem` folder:
```
- Example
    - ...
- EventSystem.md
- IntStore.cs
```

# Part 1: Creating A Basic System
## UI
Before we start setting up the backend events, let's create a simple user interface (UI) to display the 
outputs so we can see them in action.
1. Create a new empty scene for this lab by selecting File > New Scene from the Unity Editor menu.
2. Add a text display and a button to the scene by following these steps. 
   - To create the text, add a Text gameobject to the scene by selecting `GameObject > UI > Text` from the unity editor menu.
   - To create the button, right click the Canvas object in the hirarchy window select `UI > Button`
     - You can change the text on the button by changing the text on the child Text gameobject

Now the scene should look like this:
![UI](./.images/EventSystemUI.png)


*Note: The new input system may cause errors when the scene is run. If that happens select the event system and click the `Replace with InputSystemUIInputModule` button.*

## Creating The Event
Now that we have a UI to display the output, lets create the event that will increment the counter when the button is pressed.
To create a new event type, we need to create a new class that inherits from `BaseArsisEvent`. Follow these steps
to create the new event sctipt.
1. In the Unity Editor, navigate to the Assets/ARSIS/Core/EventManager/EventTypes folder.
2. Right-click on the folder and select `Create > C# Script` from the context menu. Name the script CounterValue.cs and open it in your code editor.
3. Import the `EventSystem` namespace to use the `BaseArsisEvent` class.
4. Define a class with the properties we want that inherits from `BaseArsisEvent`. 

In this case we might write:
```csharp
using EventSystem;

public class CounterValue : BaseArsisEvent {
    public int value;
}
```
Here, CounterValue is the name of our event and it has a single public property called value that stores an integer. Events are named after the values they contain because it helps to maintain readability and consistency.

We can also add some quality of life features to our event to make it easier to use. In this case:
 - A constructor that sets the `value` peoperty.
 - An implicit conversion operator that allows us to use a `CounterValue` as an int.
 - A `ToString` method that lets us easily display the value of the event.
  
The updated class might look like this:
```csharp
using EventSystem;

public class CounterValue : BaseArsisEvent {
    public int value;

    public CounterValue(int value) {
        this.value = value;
    }

    public static implicit operator int(CounterValue e) {
        return e.value;
    }

    public override string ToString() {
        return value.ToString();
    }
}
```
Now we have a basic event that can be used to set the counter value. Next, we need to create the scripts that will listen for the event and update the UI, and the script that will send the event when the button is pressed.

## Listening For The Event
1. Create a new MonoBeheiviour and open it in your editor.
2. Import the `EventSystem` and `UnityEngine.UI` namespaces.
3. Define a class with a method that sets the text of the UI to the value of a `CounterValue` event. This method might look like this:
```csharp
void UpdateCounterValue(CounterValue v)
{
    // counterText is a reference to the Text component of the UI Text GameObject
    counterText.text = v.ToString(); 
}
```
4. Add the `OnEnable` and `OnDisable` methods to the class. These methods are called when the script is enabled and disabled. We will use them to register and unregister the event listener.
```csharp
void OnEnable()
{
    EventManager.AddListener<CounterValue>(UpdateCounterValue);
}
void OnDisable()
{
    EventManager.RemoveListener<CounterValue>(UpdateCounterValue);
}
```
5. Add the script to the Text GameObject in the scene and set the `counterText` variable to the Text component.

## Sending The Event
Now that we've created the event, we need to create a script that listens for the event and updates the UI.

1. Create a new script
2. Open the script in your editor.
3. Import the `EventSystem` and `UnityEngine.UI` namespaces.
4. Add a event listener to the button's `onClick` event that increments the value of an int and sends the new value in a `CounterValue` event. 

The complete script might look like this:
```csharp
using UnityEngine;
using EventSystem;
using UnityEngine.UI;

public class CounterUpdater : MonoBehaviour {
    public int val = 0;
    public Button button;
    private void Start() {
        button.onClick.AddListener(() => EventManager.Trigger(new CounterValue(++val)));
    }
}
```

Lastly, add the script to the button GameObject in the scene and set the `button` variable to the Button component.

## Testing The Event
Now we can try out the event that we've just created and see if it works as expected. Here's how to test it:

- Make sure that the scene is running by clicking the Play button in the Unity Editor.
- Press the button that we added to the UI. You should see the number on the screen increase by one each time you press the button.
- Congratulations, you've successfully tested the event!

Although this might seem like a lot of work just to increment a number in a text field, it's worth it because we can now easily add more buttons and displays without having to write additional code to update them and we can easily expand on the system by adding features like a Cache.

# Part 2: Adding A Cache
By using events, we can easily add a cache to persist the counter value between restarts. To achieve this, we need to add a script that listens for the CounterValue event and stores the value in a cache. Then, the same script can emit a CounterValue event when the scene is loaded to set the counter to the cached value. The code for this might look like:
```csharp
using EventSystem;
using UnityEngine;

public class CounterCache : MonoBehaviour {
    void OnCounterUpdate(CounterValue value) {
        IntStore.value = value;
    }

    private void Start() {
        EventManager.Trigger(new CounterValue(IntStore.value));
    }
}
```
Once we have added this script, we need to update the CounterUpdater script to allow initialization of the counter value from the cache. We can do this by adding an event listener for the CounterValue event in the Awake method. The updated code might look like:
```csharp
using UnityEngine;
using EventSystem;
using UnityEngine.UI;

public class CounterUpdater : MonoBehaviour {
    public int val = 0;
    public Button button;
    private void Start() {
        button.onClick.AddListener(() => EventManager.Trigger(new CounterValue(++val)));
    }

    private void Awake() {
        EventManager.AddListener<CounterValue>(OnCounterUpdate);
    }

    void OnCounterUpdate(CounterValue value) {
        val = value;
    }
}
```

Now, when the scene is run, the counter increments and retains its value between restarts. If you want to clear the cache, you can use the `DemoCache/Clear` menu item in the Unity Editor.

# Conclusion
I hope this lab demonstrates how to use events to manage the flow of data between different parts of a Unity project. By decoupling the components and using events, we can avoid tightly coupled code that's difficult to modify and maintain. The event system provides an efficient and flexible way to manage communication between objects, which is especially useful when building complex systems. In this lab, we have built a simple counter system that illustrates how data is passed between different parts of ARSIS.
