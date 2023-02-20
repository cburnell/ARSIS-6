using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class HeadingCache : MonoBehaviour
{
    public HeadingEvent headingEvent;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<HeadingEvent>(UpdateHeading);
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
