using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARSISEventSystem;
using TMPro;

public class BiometricsCache : MonoBehaviour
{
    public BiometricsEvent biometricsEvent { get; private set; }
    private static int MINUTES = 5;
    private static int MAX_ENTRYS = 60*MINUTES;
    private List<BiometricsEvent> BiometricsList;
    public static BiometricsCache BiometricsCacheSingleton { get; private set; }
    public int Count;
    public int OutOfRangeCount;
    public HashSet<string> outOfRangeBiometrics;
    

    
    private void Awake()
    {
        BiometricsList = new List<BiometricsEvent>();
        if (BiometricsCacheSingleton != null && BiometricsCacheSingleton != this)
        {
            Destroy(this);
            //EventManager.RemoveListener<BiometricsEvent>(UpdateBiometrics);
        }
        else
        {
            BiometricsCacheSingleton = this;
            //EventManager.AddListener<BiometricsEvent>(UpdateBiometrics);
            outOfRangeBiometrics = new HashSet<string>();
        }
    }
    public void UpdateBiometrics(BiometricsEvent be){
        biometricsEvent = be;
        CheckBiometrics(biometricsEvent);
        if (BiometricsList.Count > MAX_ENTRYS){
            BiometricsList.RemoveAt(0);
        }
        BiometricsList.Add(be);
        Count = BiometricsList.Count;
        OutOfRangeCount = outOfRangeBiometrics.Count;
    }

    public void CheckBiometrics(BiometricsEvent be){
        foreach((int rate, int[] limits, string name) in be.intCheckList){
            if(rate < limits[0] || rate > limits[1]){
                outOfRangeBiometrics.Add(name);
            }
        }
        foreach((bool status, bool nominal, string name) in be.boolCheckList){
            if(status != nominal){
                outOfRangeBiometrics.Add(name);
            }
        }
    }
    public void acknowledgeOutOfRange(string name){
        outOfRangeBiometrics.Remove(name);
    }
}
