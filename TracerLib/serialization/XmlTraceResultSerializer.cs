using System.IO;
using System.Xml.Serialization;

namespace TracerLib.serialization
{
    public class XmlTraceResultSerializer:ISerializer
    {

        public string Serialize(object obj)
        {
            XmlSerializer xmlSerializer = new(obj.GetType());

            StringWriter textWriter = new();
            xmlSerializer.Serialize(textWriter, obj);
            return textWriter.ToString();
        }
    }
}