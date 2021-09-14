using System.Text.Json;

namespace TracerLib.serialization
{
    public class JsonTraceResultSerializer:ISerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
            { 
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

        public string Serialize(object obj)
        {
            return JsonSerializer.Serialize(obj, _options);
        }
        
    }
}