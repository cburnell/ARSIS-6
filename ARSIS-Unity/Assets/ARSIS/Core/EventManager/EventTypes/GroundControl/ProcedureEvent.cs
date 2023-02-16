using System.Collections.Generic;
using UnityEngine;

namespace EventSystem {
    public class ProcedureEvent : BaseArsisEvent
    {
        public readonly string procedureName;
        public readonly List<List<string>> taskList;

        public ProcedureEvent(string procedureName, List<List<string>> taskList){
            this.taskList = taskList;
            this.procedureName = procedureName;
        }
    }
}
