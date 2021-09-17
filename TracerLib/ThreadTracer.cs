using System.Collections.Generic;
using System.Diagnostics;

namespace TracerLib
{
    public class ThreadTracer:ITracer
    {
        public long Id { get; init; }
        private MethodTracer _currentMethodTracer;

        private readonly Stopwatch _stopwatch = new();
        private readonly Stack<MethodTracer> _stack = new();
        private readonly List<MethodTracer> _methodTracers = new();
        
        public ThreadTracer(long id)
        {
            Id = id;
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
            long executionTime = 0;
            
            List<MethodTraceResult> results = new List<MethodTraceResult>();
            foreach (var methodTracer in _methodTracers)
            {
                var result = (MethodTraceResult)methodTracer.GetTraceResult();
                executionTime += result.Time;
                results.Add(result);
            }

            return new ThreadTraceResult(Id, executionTime, results);
        }
        
    }
    
}