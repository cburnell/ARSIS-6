using EventSystem;

public class HeadingEvent : BaseArsisEvent
{
    public readonly float heading;

    public HeadingEvent(float heading)
    {
        this.heading = heading;
    }
}

