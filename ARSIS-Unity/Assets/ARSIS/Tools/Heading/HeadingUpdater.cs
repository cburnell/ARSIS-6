using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HeadingUpdater : MonoBehaviour
{
    HeadingCache headingCache;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        headingCache = GetComponentInParent<HeadingCache>();
        text = GetComponent<TMP_Text>();
        Debug.Log(headingCache);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = headingCache.getHeadingString();
    }
}
