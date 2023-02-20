using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class TelemetryClient : MonoBehaviour
{
    private static string telemetryServerUrl = "http://localhost:8080";
    private static string telemetryServerLocation = telemetryServerUrl + "/location/";
    private static string telemetryServerBiometrics = telemetryServerUrl + "/biometrics/";
    private static string telemetryServerHeading = telemetryServerUrl + "/heading/";
    // We create a timer outside of the loop to make sure we're not creating and then GCing constantly
    private WaitForSeconds telemetryPollingDelay = new WaitForSeconds(1.0f);

    // Start is called before the first frame update
    void Start()
    {
        // Start all of our polling when this starts
        StartCoroutine(StartPollingTelemetryApi());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator StartPollingTelemetryApi() {
        //Start each one as its own coroutine
        StartCoroutine(StartPollingEvent<HeadingEvent>(telemetryServerHeading, telemetryPollingDelay));
        StartCoroutine(StartPollingEvent<LocationEvent>(telemetryServerLocation, telemetryPollingDelay));
        StartCoroutine(StartPollingEvent<BiometricsEvent>(telemetryServerBiometrics, telemetryPollingDelay));
        yield break;
    }

    //Generic way to creating a polling event using the type of event to be triggered when it gets a sucessful request.
    //TODO: Add more robust handling of faults.
    IEnumerator StartPollingEvent<E>(string url, WaitForSeconds tpd){
        while(true){
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                string resultString = www.downloadHandler.text;
                /* Debug.Log(resultString); */
                E newEvent = JsonConvert.DeserializeObject<E>(resultString);
                EventManager.Trigger(newEvent);

            }
            yield return telemetryPollingDelay;
        }
    }
}
