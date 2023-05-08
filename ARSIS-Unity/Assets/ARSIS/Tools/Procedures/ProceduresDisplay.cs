using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ARSISEventSystem;

public class ProceduresDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    bool notShown = true;
    GameObject manager;

    void Start(){
        manager = new GameObject();
        manager.name ="ProcedureDisplay";

    }

    // Update is called once per frame
    void Update(){
        if (ProcedureCache.Instance.Count() == 0){
            return;
        }
        if(notShown){
            notShown = false;
            showTask();
        }
    }

    void showTask(){
        ProcedureEvent pe = ProcedureCache.Instance.getProcedure("Mock Procedure");

        manager.AddComponent<Canvas>();
        Canvas displayCanvas = manager.GetComponent<Canvas>();
        displayCanvas.renderMode = RenderMode.WorldSpace;

        manager.AddComponent<CanvasScaler>();
        manager.AddComponent<GraphicRaycaster>();
        manager.transform.parent = this.transform;
        manager.AddComponent<VerticalLayoutGroup>();

        RectTransform rt = displayCanvas.GetComponent (typeof (RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2 (500, 400);
        GameObject textGO = new GameObject();
        textGO.transform.parent = manager.transform;
        TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI>();
        text.text = pe.name;
        ARSISTask task0 = pe.taskList[0];
        foreach(Step s in task0.stepList){
            GameObject stepGO = new GameObject();
            stepGO.transform.SetParent(this.transform);
            stepGO.transform.parent = manager.transform;
            stepGO.name = "wibble";
            if (s.type == "image"){
                Image suits_image = stepGO.AddComponent<Image>();
                string body = s.body;
                string removeIfExists = "base64,";
                int index = body.IndexOf(removeIfExists);
                if (index > 0 && index < 10){
                    body = body.Substring(index + removeIfExists.Length);
                }
                byte[]  imageBytes = Convert.FromBase64String(body);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage( imageBytes );
                Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                suits_image.sprite = sprite;
            }
            if (s.type == "text"){
                TextMeshProUGUI newText = stepGO.AddComponent<TextMeshProUGUI>();
                newText.text = s.body;
            }
        }
    }
}
