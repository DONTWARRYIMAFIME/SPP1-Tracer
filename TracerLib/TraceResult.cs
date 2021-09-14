using System.Collections.Generic;

namespace SPP1_Tracer.tracer
{
    public record TraceResult(List<ThreadTraceResult> Root):ITraceResult
    {

    }
}