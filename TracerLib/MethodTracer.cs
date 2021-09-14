using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SPP1_Tracer.tracer
{
    public class MethodTracer:ITracer
    {
        private readonly string _name;
        private readonly string _className;
        
        private readonly Stopwatch _stopwatch = new();
        private readonly List<MethodTracer> _methodTracers = new();

        public MethodTracer()
        {
            MethodBase method = new StackFrame(2, false).GetMethod();
            _name = method?.Name;
            _className = method?.DeclaringType?.FullName;
        }

        public void AddChildMethod(MethodTracer methodTracer)
        {
            _methodTracers.Add(methodTracer);
        }

        public void StartTrace()
        {
            _stopwatch.Start();
        }

        public void StopTrace()
        {
            _stopwatch.Stop();
        }
        
        public ITraceResult GetTraceResult()
        {
            var results =
                    from methodTracer in _methodTracers 
                    select (MethodTraceResult)methodTracer.GetTraceResult();
            
            return new MethodTraceResult(_name, _className, _stopwatch.ElapsedMilliseconds, results.ToList());
        }
        
    }
}