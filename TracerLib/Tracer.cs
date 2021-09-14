using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TracerLib
{
    public class Tracer:ITracer
    {
        private readonly List<ITracer> _threadTracers = new();
        private readonly ReaderWriterLockSlim _lock = new ();
        
        private ITracer GetThreadTracer(int id)
        {
            _lock.EnterReadLock();
            try
            {
                return _threadTracers
                    .Cast<ThreadTracer>()
                    .FirstOrDefault(threadTracer => threadTracer.Id == id);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        
        private void AddThreadTracer(ITracer threadTracer)
        {
            _lock.EnterWriteLock();
            try
            {
                _threadTracers.Add(threadTracer);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
        
        public void StartTrace()
        {
            int id = Thread.CurrentThread.ManagedThreadId;

            var currentThreadTracer = GetThreadTracer(id);

            if (currentThreadTracer == null)
            {
                currentThreadTracer = new ThreadTracer(id);
                AddThreadTracer(currentThreadTracer);
            }
            
            currentThreadTracer.StartTrace();
        }

        public void StopTrace()
        {
            int id = Thread.CurrentThread.ManagedThreadId;
            GetThreadTracer(id)?.StopTrace();
        }

        public ITraceResult GetTraceResult()
        {
            _lock.EnterReadLock();
            try
            {
                var results = _threadTracers
                    .Select(threadTracer => (ThreadTraceResult)threadTracer.GetTraceResult())
                    .ToList();

                return new TraceResult(results);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
        
    }
}