using System.Collections.Generic;

namespace TracerLib
{
    public record ThreadTraceResult(long Id, long Time, List<MethodTraceResult> Methods):ITraceResult
    {
        
    }
}