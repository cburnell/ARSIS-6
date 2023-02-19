using System.Collections.Generic;
using UnityEngine;

namespace EventSystem {
    public class ProcedureEvent : BaseArsisEvent
    {
        public readonly string procedureName;
        public readonly List<List<byte>> taskList;

        public ProcedureEvent(string procedureName, List<List<byte>> taskList){
            this.taskList = taskList;
            this.procedureName = procedureName;
        }
    }
}
