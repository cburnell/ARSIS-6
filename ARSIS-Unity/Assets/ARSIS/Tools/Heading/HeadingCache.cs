using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class HeadingCache : MonoBehaviour
{

    public HeadingEvent headingEvent;
    public static HeadingCache HeadingCacheSingleton { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (HeadingCacheSingleton != null && HeadingCacheSingleton != this)
        {
            Destroy(this);
        }
        else
        {
            HeadingCacheSingleton = this;
            EventManager.AddListener<HeadingEvent>(UpdateHeading);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateHeading(HeadingEvent he){
        headingEvent = he;
    }

    public string getHeadingString(){
        return headingEvent.heading.ToString();
    }
}
