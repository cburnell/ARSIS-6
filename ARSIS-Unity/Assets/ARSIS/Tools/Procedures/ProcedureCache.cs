using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystem;

public class ProcedureCache : MonoBehaviour
{
    private Dictionary<string, ProcedureEvent> procedureCache;
    private WaitForSeconds procedurePollingDelay = new WaitForSeconds(1.0f);
    // Start is called before the first frame update
    void Start()
    {
        EventManager.AddListener<ProcedureEvent>(proccessProcedureEvent);
        procedureCache = new Dictionary<string, ProcedureEvent>();
        getProcedure("Mock Procedure");
        StartCoroutine(PollProcedureApi());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator PollProcedureApi() {
        while(procedureCache.Count < 1){
            StartCoroutine(getProcedure("Mock Procedure"));
            yield return procedurePollingDelay;
        }
    }

    //TODO this should select an actual procedure
    IEnumerator getProcedure(string procedureName){
        ProcedureGet pg = new ProcedureGet(procedureName);
        EventManager.Trigger(pg);
        yield return 1;
    }

    void proccessProcedureEvent(ProcedureEvent pe){
        procedureCache[pe.procedureName] = pe;
        /* Debug.Log("ppe" + pe.procedureName); */
        Debug.Log(procedureCache.Count);
    }

    /* IEnumerator listProceduresWebRequest(){ */
    /*  */
    /* } */
}
