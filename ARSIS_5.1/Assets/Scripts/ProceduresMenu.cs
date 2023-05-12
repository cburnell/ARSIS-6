using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;
using System;

public class ProceduresMenu : MonoBehaviour
{
    public List<GameObject> toClear;
    private int taskNum = 0;
    Procedure selectedProcedure;
    ListManager list;
    void Start()
    {
        taskNum = 0;
        toClear = new List<GameObject>();
        list = this.GetComponent<ListManager>();
    }

    void killAll(){
        foreach(GameObject go in toClear){
            Destroy(go);
        }
    }
    void OnEnable(){
        killAll();
        ListManager list = this.GetComponent<ListManager>(); 
        List<Procedure> procedures = TaskManager.S.allProcedures;
        foreach (Procedure p in procedures)
        {
            GameObject listItem = list.addListItem(p.procedure_title);
            toClear.Add(listItem);
            Interactable interact = listItem.GetComponent<Interactable>();
            interact.OnClick.AddListener(()=>
            {
                taskNum = 0;
                selectedProcedure = p;
                ShowTask();
            });
        }
    }

    public void ShowTask(){
        killAll();
        Task t = selectedProcedure.GetTask(taskNum);
        for(int i = 0; i < t.numSubTasks; i++){
            SubTask st = t.GetSubTask(i);
            if(st.type ==  "text"){
                GameObject listItem = list.addListItem(st.text);
                toClear.Add(listItem);
            }
            
        }
    }

    public void next(){
        Debug.Log(selectedProcedure.num_steps);
        Debug.Log(taskNum);
        if(selectedProcedure.num_steps > taskNum + 1){
            taskNum++;
            ShowTask();
        }
    }
}
