using System.Collections.Generic;
using System.Xml.Serialization;

namespace TracerLib
{
    public class MethodTraceResult:ITraceResult
    {
        [XmlAttribute("name")]
        public string Name { get; init; }
        [XmlAttribute("class")]
        public string ClassName { get; init; }
        [XmlAttribute("time")]
        public long Time { get; init; }
        [XmlElement("method")]
        public List<MethodTraceResult> Methods { get; init; }

        public MethodTraceResult() { }

        public MethodTraceResult(string name, string className, long time, List<MethodTraceResult> methods)
        {
            Name = name;
            ClassName = className;
            Time = time;
            Methods = methods;
        }
    }
}