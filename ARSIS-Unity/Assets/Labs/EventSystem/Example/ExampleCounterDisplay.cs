using EventSystem;
using UnityEngine;
using UnityEngine.UI;

public class ExampleCounterDisplay : MonoBehaviour {
    /// <summary>
    /// A reference to the text object that will display the counter value.
    /// </summary>
    public Text counterText;

    /// <summary>
    /// This method is called whenever the counter value is updated. 
    /// It updates the text object to display the new value.
    /// </summary>
    /// <param name="value"></param>
    void UpdateCounterValue(ExampleCounterValue value)
    {
        counterText.text = value.ToString();
    }

    /// <summary>
    /// This adds the event listener to the event manager when the object/script is enabled.
    /// </summary>
    void OnEnable()
    {
        EventManager.AddListener<ExampleCounterValue>(UpdateCounterValue);
    }

    /// <summary>
    /// This removes the event listener from the event manager when the object/script is disabled.
    /// </summary>
    void OnDisable()
    {
        EventManager.RemoveListener<ExampleCounterValue>(UpdateCounterValue);
    }
}