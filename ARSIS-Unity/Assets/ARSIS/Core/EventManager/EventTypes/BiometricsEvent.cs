using EventSystem;

public class BiometricsEvent : BaseArsisEvent
{
    public readonly float hr;
    public readonly float o2;

    public BiometricsEvent(float hr, float o2)
    {
        this.hr = hr;
        this.o2 = o2;
    }
}

