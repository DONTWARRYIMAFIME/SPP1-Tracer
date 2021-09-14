using System.Collections.Generic;

namespace SPP1_Tracer.tracer
{
    public record ThreadTraceResult(long Id, long Time, List<MethodTraceResult> Methods):ITraceResult
    {
        
    }
}