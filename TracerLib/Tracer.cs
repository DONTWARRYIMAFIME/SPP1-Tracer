using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SPP1_Tracer.tracer
{
    public class Tracer:ITracer
    {
        private readonly List<ITracer> _threadTracers = new();
        private readonly ReaderWriterLockSlim _lock = new ();

        private ITracer _currentThreadTracer;
        
        public void StartTrace()
        {
            /*
             * TODO: add lock
             */
            if (_currentThreadTracer == null)
            {
                int id = Thread.CurrentThread.ManagedThreadId;
                _currentThreadTracer = new ThreadTracer(id);
                _threadTracers.Add(_currentThreadTracer);
            }
            _currentThreadTracer.StartTrace();
        }

        public void StopTrace()
        {
            _currentThreadTracer.StopTrace();
        }

        public ITraceResult GetTraceResult()
        {
            var results =
                from threadTracer in _threadTracers
                select (ThreadTraceResult)threadTracer.GetTraceResult();


            return new TraceResult(results.ToList());
        }
        
    }
}