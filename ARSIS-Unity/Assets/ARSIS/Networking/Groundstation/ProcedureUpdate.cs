using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;
using UnityEngine.Networking;
using Newtonsoft.Json;


public class ProcedureUpdate : MonoBehaviour
{
    private static string procedureEndpoint = "http://0.0.0.0:8181/procedure/";
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<ProcedureGet>(getProcedureTrigger);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void getProcedureTrigger(ProcedureGet pg){
        StartCoroutine(getProcedure(pg));
    }
    IEnumerator getProcedure(ProcedureGet procedureToGet){
        string name = procedureToGet.procedureName;
        UnityWebRequest www = UnityWebRequest.Get(procedureEndpoint+name);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            string resultString = www.downloadHandler.text;
            Debug.Log(resultString);
            ProcedureEvent newProcedureEvent = JsonConvert.DeserializeObject<ProcedureEvent>(resultString);
            /* var thingToTest = JsonConvert.DeserializeObject<ProcedureEvent>(resultString); */
            /* Debug.Log(thingToTest.procedureName); */
            Debug.Log("before trigger" + newProcedureEvent.procedureName);
            EventManager.Trigger(newProcedureEvent);
        }
    }
}
