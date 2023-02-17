using EventSystem;

public class ExampleCounterValue : BaseArsisEvent {
    /// <summary>
    /// This is the value to set the counter to.
    /// </summary>
    public int value;

    /// <summary>
    /// This constructor is used to set the value of the counter.
    /// </summary>
    /// <param name="value">the value of the counter for the event</param>
    public ExampleCounterValue(int value) {
        this.value = value;
    }

    /// <summary>
    /// This method declares an implicit cast allowing the event to be used as an int without having to use the .value property.
    /// </summary>
    /// <param name="e"></param>
    public static implicit operator int(ExampleCounterValue e) {
        return e.value;
    }

    public override string ToString() {
        return value.ToString();
    }
}