using EventSystem;
using UnityEngine;

public class ExampleCounterCache : MonoBehaviour {
    /// <summary>
    /// This sets the cached value whenever the value is updated.
    /// </summary>
    /// <param name="value"></param>
    void OnCounterUpdate(ExampleCounterValue value) {
        IntStore.value = value;
    }

    /// <summary>
    /// Sets the event listener and triggers the counter value initalization
    /// </summary>
    private void Start() {
        EventManager.AddListener<ExampleCounterValue>(OnCounterUpdate);
        EventManager.Trigger(new ExampleCounterValue(IntStore.value));
    }
}