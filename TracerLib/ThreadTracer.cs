using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace SPP1_Tracer.tracer
{
    public class ThreadTracer:ITracer
    {
        private readonly long _id;
        private MethodTracer _currentMethodTracer;

        private readonly Stopwatch _stopwatch = new();
        private readonly Stack<MethodTracer> _stack = new();
        private readonly List<MethodTracer> _methodTracers = new();

        public ThreadTracer(long id)
        {
            _id = id;
        }
        
        public void StartTrace()
        {
            if (!_stopwatch.IsRunning)
            {
                _stopwatch.Start();
            }

            MethodTracer newMethodTracer = new MethodTracer();

            if (_currentMethodTracer != null)
            {
                _stack.Push(_currentMethodTracer);
                _currentMethodTracer.AddChildMethod(newMethodTracer);
            }
            else
            {
                _methodTracers.Add(newMethodTracer);
            }

            _currentMethodTracer = newMethodTracer;
            _currentMethodTracer.StartTrace();
        }

        public void StopTrace()
        {
            if (_currentMethodTracer != null)
            {
                _currentMethodTracer.StopTrace();
                _currentMethodTracer = _stack.Count != 0 ? _stack.Pop() : null;
            }
        }

        public ITraceResult GetTraceResult()
        {
            StopAllAliveTraces();
            _stopwatch.Stop();
            
            var results =
                    from methodTracer in _methodTracers
                    select (MethodTraceResult)methodTracer.GetTraceResult();

            return new ThreadTraceResult(_id, _stopwatch.ElapsedMilliseconds, results.ToList());
        }

        private void StopAllAliveTraces()
        {
            while (_stack.Count != 0)
            {
                StopTrace();
            }
        }
        

        
    }
    
}