namespace TracerLib
{
    public interface ITracer
    {
        void StartTrace();

        void StopTrace();

        ITraceResult GetTraceResult();
    }
}