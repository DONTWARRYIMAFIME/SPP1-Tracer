using System.Collections.Generic;

namespace TracerLib
{
    public record TraceResult(List<ThreadTraceResult> Root):ITraceResult
    {

    }
}