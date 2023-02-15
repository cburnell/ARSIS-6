using System.Collections.Generic;

namespace EventSystem {
    public class ProcedureEvent : BaseArsisEvent
    {
        public readonly string procedureName;
        public readonly List<List<string>> procedureList;

        public ProcedureEvent(string procedureName, List<List<string>> procedureList){
            this.procedureList = procedureList;
            this.procedureName = procedureName;
        }
    }
}
