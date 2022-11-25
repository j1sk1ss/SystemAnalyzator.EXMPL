using System.Collections.Generic;
using SystemAnalyzator.EXMPL.OBJECTS;

namespace SystemAnalyzator.EXMPL.DATA {
    public class Data {
        public Data() {
            Processes = new List<Process>();
        }
        public List<Process> Processes { get; set; }
    }
}