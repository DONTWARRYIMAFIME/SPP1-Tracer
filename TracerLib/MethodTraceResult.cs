using System.Collections.Generic;

namespace SPP1_Tracer.tracer
{
    public record MethodTraceResult(string Name, string ClassName, long Time, List<MethodTraceResult> Methods):ITraceResult
    {

    }
}