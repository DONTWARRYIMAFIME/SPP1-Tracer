using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;

namespace TracerLib
{
    public class ThreadTracer:ITracer
    {
        public long Id { get; init; }
        private MethodTracer _currentMethodTracer;

        private readonly Stopwatch _stopwatch = new();
        private readonly Stack<MethodTracer> _stack = new();
        private readonly List<MethodTracer> _methodTracers = new();

        private long _time;

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

                if (_stack.Count == 0)
                {
                    _time += _stopwatch.ElapsedMilliseconds;
                    _stopwatch.Restart();
                }
            }
        }

        public ITraceResult GetTraceResult()
        {
            var results = _methodTracers
                .Select(threadTracer => (MethodTraceResult)threadTracer.GetTraceResult())
                .ToList();

            return new ThreadTraceResult(Id, _time, results);
        }
        
    }
    
}