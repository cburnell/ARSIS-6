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
    private WaitForSeconds telemetryPollingDelay = new WaitForSeconds(1.0f);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartPollingTelemetryApi());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator StartPollingTelemetryApi() {
        StartCoroutine(StartPollingEvent<HeadingEvent>(telemetryServerHeading, telemetryPollingDelay));
        StartCoroutine(StartPollingEvent<LocationEvent>(telemetryServerLocation, telemetryPollingDelay));
        StartCoroutine(StartPollingEvent<BiometricsEvent>(telemetryServerBiometrics, telemetryPollingDelay));
        yield break;
    }
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
