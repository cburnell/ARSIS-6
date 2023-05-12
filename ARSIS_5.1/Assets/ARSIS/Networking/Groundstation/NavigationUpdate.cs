using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARSISEventSystem;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

public class NavigationUpdate : MonoBehaviour
{
    
    private static string navigationEndpoint = "http://"+ARSISConstants.ARSISUser.ip+":8181/navigation/";
    private readonly HttpClient httpClient = new HttpClient();
    // Start is called before the first frame update
    TaskFactory taskFactory;
    public static NavigationCache NavigationCacheInstance;
    void Start()
    {
        //EventManager.AddListener<NavigationGet>(getNavigationTrigger);
        //EventManager.AddListener<UpdateNavigationEvent>(updateNavigationTrigger);
        taskFactory = new TaskFactory();
        NavigationCacheInstance = NavigationCache.Instance;
        InvokeRepeating("updateNavigationTrigger", 1, 15);
    }

    // TODO: Make this something that deals with generics
    void updateNavigationTrigger(){
        Debug.Log("updateNavigationTrigger");
        taskFactory.StartNew(() =>
        {
            updateNavigation();
           
        }).Wait();
    }

    public async Task<int> updateNavigation(){
        var response = await httpClient.GetAsync(navigationEndpoint);
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
            Dictionary<string, NavigationEvent> dictOnly = JsonConvert.DeserializeObject<Dictionary<string, NavigationEvent>>(resultString);
            NavigationDictionary newNavigationDictionary = new NavigationDictionary(dictOnly);
            // EventManager.Trigger(newNavigationDictionary);
            NavigationCacheInstance.proccessNavigationDictionary(newNavigationDictionary);
        }
        return 0;
    }

    // TODO ASYNC THIS
    void getNavigationTrigger(NavigationGet pg){
        StartCoroutine(getNavigation(pg).GetEnumerator());
    }
    IEnumerable getNavigation(NavigationGet navigationToGet){
        string name = navigationToGet.navigationPathName;
        UnityWebRequest www = UnityWebRequest.Get(navigationEndpoint+name);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            // Show results as text
            string resultString = www.downloadHandler.text;
            NavigationEvent newNavigationEvent = JsonConvert.DeserializeObject<NavigationEvent>(resultString);
            EventManager.Trigger(newNavigationEvent);
        }
    }
}
