using UnityEngine;
using EventSystem;
using UnityEngine.UI;

public class ExampleCounterUpdater : MonoBehaviour {
    /// <summary>
    /// The value of the counter.
    /// </summary>
    public int val = 0;

    /// <summary>
    /// The reference to the button that will increment the counter.
    /// </summary>
    public Button button;

    /// <summary>
    /// Adds the event listener that will increment the counter and trigger an event.
    /// </summary>
    private void Start() {
        button.onClick.AddListener(() => EventManager.Trigger(new ExampleCounterValue(++val)));
    }

    /// <summary>
    /// Adds the event listener that will allow the `val` to be updated and initalized.
    /// **This would not be good if updating the value caused the same event to be triggered.**
    ///
    /// </summary>
    private void Awake() {
        EventManager.AddListener<ExampleCounterValue>(OnCounterUpdate);
    }

    /// <summary>
    /// The method that initalized the value of `val`.
    /// </summary>
    /// <param name="value"></param>
    void OnCounterUpdate(ExampleCounterValue value) {
        val = value;
    }
}