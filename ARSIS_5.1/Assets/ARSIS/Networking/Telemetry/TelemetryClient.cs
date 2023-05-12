using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ARSISEventSystem;
using ARSISConstants;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Net.Http;
using TMPro;
using System.Threading.Tasks;
public class UserToRegister
{
    public string name;
    public UserToRegister(string name)
    {
        this.name = name;
    }
}

public class RegisteredUser
{
    public int id;
    public string name;
    public string createdAt;
    public RegisteredUser(int id, string name, string createdAt)
    {
        this.id = id;
        this.name = name;
        this.createdAt = createdAt;
    }
}

public class RegisteredUsersDict
{
    public List<RegisteredUser> users;
    public RegisteredUsersDict(List<RegisteredUser> users)
    {
        this.users = users;
    }
}

public class TelemetryClient : MonoBehaviour
{
    public static string ip = ARSISUser.ip;
    private static string userMockName = ARSISConstants.ARSISUser.username;
    private static string telemetryServerUrl = "http://" + ip + ":8080";
    private static string telemetryServerUser = telemetryServerUrl + "/user/";
    private string telemetryServerLocation = telemetryServerUrl + "/location/";
    private string telemetryServerBiometrics = telemetryServerUrl + "/biometrics/";

    public enum Endpoint { LOCATION, BIOMETRICS, USER };
    private Dictionary<Endpoint, string> serverEndpointDict = new Dictionary<Endpoint, string>();

    private WaitForSeconds telemetryPollingDelay = new WaitForSeconds(1.0f);

    public RegisteredUser registeredUser;
    public TaskFactory taskFactory;
    private readonly HttpClient httpClient = new HttpClient();
    

    BiometricsCache bioCache;
    LocationCache locCache;

    void Start()
    {
        updateServerEndpointDict();
        StartCoroutine(PopulateRegisterdUser());
        taskFactory = new TaskFactory();
        InvokeRepeating("PollingTelemetryApi", 1, 1);
        bioCache = BiometricsCache.BiometricsCacheSingleton;
        locCache = LocationCache.LocationCacheSingleton;
    }

    void Update()
    {
    }

    void updateServerEndpointDict()
    {
        string userId = "";
        if (registeredUser != null)
        {
            userId = registeredUser.id.ToString();
        }
        serverEndpointDict[Endpoint.USER] = telemetryServerUser;
        serverEndpointDict[Endpoint.LOCATION] = telemetryServerLocation + userId;
        serverEndpointDict[Endpoint.BIOMETRICS] = telemetryServerBiometrics + userId;

    }
    IEnumerator PopulateRegisterdUser()
    {
        StartCoroutine(CheckIfUsernameIsUsed());
        yield return null;
    }

    IEnumerator CheckIfUsernameIsUsed()
    {
        UnityWebRequest www = UnityWebRequest.Get(telemetryServerUser);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string resultString = www.downloadHandler.text;
            RegisteredUsersDict registeredUsersDict = JsonConvert.DeserializeObject<RegisteredUsersDict>(resultString);
            foreach (RegisteredUser r in registeredUsersDict.users)
            {
                if (r.name == userMockName)
                {
                    registeredUser = r;
                    updateServerEndpointDict();
                }
            }
        }
        StartCoroutine(RegisterWithApi());
        yield return null;
    }
    IEnumerator RegisterWithApi()
    {
        while (registeredUser == null)
        {
            UserToRegister user = new UserToRegister(userMockName);
            string userData = JsonConvert.SerializeObject(user);
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(userData);
            using (UnityWebRequest www = UnityWebRequest.Put(telemetryServerUser, myData))
            {
                www.SetRequestHeader("accept", "application/json");
                www.SetRequestHeader("Content-Type", "application/json");
                yield return www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    string resultString = www.downloadHandler.text;
                    registeredUser = JsonConvert.DeserializeObject<RegisteredUser>(resultString);
                    updateServerEndpointDict();
                }
            }
            yield return telemetryPollingDelay;
        }
    }

    void PollingTelemetryApi()
    {
        taskFactory.StartNew(() =>
        {
            StartPollingEndpointBio(Endpoint.BIOMETRICS);
            StartPollingEndpointLoc(Endpoint.LOCATION);
        }).Wait();
        return;
    }


    public async void StartPollingEndpointBio(Endpoint endpoint)
    {
        string endpointUrl = serverEndpointDict[endpoint];
        var response = await httpClient.GetAsync(endpointUrl);
        var content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Debug.Log(response.StatusCode);
        }
        else
        {
            string resultString = content;
            
            BiometricsEvent eventToFire = JsonConvert.DeserializeObject<BiometricsEvent>(resultString);

            bioCache.UpdateBiometrics(eventToFire);
            
        }
        return;
    }


    public async void StartPollingEndpointLoc(Endpoint endpoint)
    {
        string endpointUrl = serverEndpointDict[endpoint];
        var response = await httpClient.GetAsync(endpointUrl);
        var content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            Debug.Log(response.StatusCode);
        }
        else
        {
            string resultString = content;
            
            LocationEvent eventToFire = JsonConvert.DeserializeObject<LocationEvent>(resultString);

            locCache.UpdateLocation(eventToFire);
            
        }
        return;
    }
}

