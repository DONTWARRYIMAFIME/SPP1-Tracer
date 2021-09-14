using System.Collections.Generic;

namespace TracerLib
{
    public record MethodTraceResult(string Name, string ClassName, long Time, List<MethodTraceResult> Methods):ITraceResult
    {

    }
}