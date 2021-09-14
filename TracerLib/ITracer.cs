namespace SPP1_Tracer.tracer
{
    public interface ITracer
    {
        void StartTrace();

        void StopTrace();

        ITraceResult GetTraceResult();
    }
}