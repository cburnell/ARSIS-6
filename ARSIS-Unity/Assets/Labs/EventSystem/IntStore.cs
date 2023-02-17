using UnityEditor;
using UnityEngine;
public static class IntStore {
    public static int value { 
        get { 
            return PlayerPrefs.GetInt("IntStore.value", 0);
        } 
        set {
            PlayerPrefs.SetInt("IntStore.value", value);
        } 
    }
}

public class IntStoreMenu {
    [MenuItem("DemoCache/Clear")]
    public static void DeletePlayerPrefs() {
        PlayerPrefs.DeleteKey("IntStore.value");
    }
}