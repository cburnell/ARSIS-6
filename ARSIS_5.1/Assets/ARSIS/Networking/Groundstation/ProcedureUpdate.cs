using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARSISEventSystem;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;


public class ProcedureUpdate : MonoBehaviour
{
    private static string procedureEndpoint = "http://"+ARSISConstants.ARSISUser.ip+":8181/procedures/";
    private readonly HttpClient httpClient = new HttpClient();
    // Start is called before the first frame update
    TaskFactory taskFactory;
    public static ProcedureUpdate Instance;
    public ProcedureCache ProcedureCacheInstance;
    void Start()
    {
        Debug.Log(System.Environment.Version);
        taskFactory = new TaskFactory();
        ProcedureCacheInstance = ProcedureCache.Instance;
        InvokeRepeating("updateProceduresTrigger", 1, 15);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // TODO: Make this something that deals with generics
    public void updateProceduresTrigger(){
        Debug.Log("update procedures trigger");
        taskFactory.StartNew(() =>
        {
            updateProcedures();
           
        }).Wait();
    }

    public async Task<int> updateProcedures(){
        var response = await httpClient.GetAsync(procedureEndpoint);
        var content = await response.Content.ReadAsStringAsync();
        /* UnityWebRequest www = UnityWebRequest.Get(navigationEndpoint); */
        /* yield return www.SendWebRequest(); */

        if (response.StatusCode != System.Net.HttpStatusCode.OK) {
            Debug.Log(response.StatusCode);
        }
        else {
            // Show results as text
            string resultString = content;
            Debug.Log(resultString);
            Dictionary<string, ProcedureEvent> dictOnly = JsonConvert.DeserializeObject<Dictionary<string, ProcedureEvent>>(resultString);
            ProcedureDictionary newProcedureDictionary = new ProcedureDictionary(dictOnly);
            // EventManager.Trigger(newProcedureDictionary);
            ProcedureCacheInstance.proccessProcedureDictionary(newProcedureDictionary);
        }
        return 0;
    }

    // TODO ASYNC THIS
    void getProcedureTrigger(ProcedureGet pg){
        StartCoroutine(getProcedure(pg).GetEnumerator());
    }

    IEnumerable getProcedure(ProcedureGet procedureToGet){
        string name = procedureToGet.procedureName;
        UnityWebRequest www = UnityWebRequest.Get(procedureEndpoint+name);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            string resultString = www.downloadHandler.text;
            ProcedureEvent newProcedureEvent = JsonConvert.DeserializeObject<ProcedureEvent>(resultString);
            EventManager.Trigger(newProcedureEvent);
        }
    }
}
