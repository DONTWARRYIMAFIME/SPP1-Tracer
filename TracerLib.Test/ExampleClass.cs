using System.Threading;
using TracerLib;

namespace TracerLibTest
{
    public class ExampleClass
    {
        private readonly ITracer _tracer;
        private readonly Foo _foo;

        public ExampleClass(ITracer tracer)
        {
            _tracer = tracer;
            _foo = new Foo(tracer);
        }

        public void SimpleMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(20);
            _tracer.StopTrace();
        }
        public void SimpleMethod(int sleepTime)
        {
            _tracer.StartTrace();
            Thread.Sleep(sleepTime);
            _tracer.StopTrace();
        }
        
        public void MediumMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(60);
            _foo.Method1();
            Thread.Sleep(120);
            _tracer.StopTrace();
        }

        public void DifficultMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(60);
            _foo.Method1();
            _foo.Method2();
            Thread.Sleep(60);
            _tracer.StopTrace();
        }

        private class Foo
        {
            private readonly ITracer _tracer;

            public Foo(ITracer tracer)
            {
                _tracer = tracer;
            }

            public void Method1()
            {
                _tracer.StartTrace();
                Thread.Sleep(30);
                _tracer.StopTrace();
            }
            
            public void Method2()
            {
                _tracer.StartTrace();
                Thread.Sleep(50);
                Method3();
                Thread.Sleep(50);
                _tracer.StopTrace();
            }
            
            public void Method3()
            {
                _tracer.StartTrace();
                Thread.Sleep(120);
                _tracer.StopTrace();
            }
            
            
        }
        
        
        
    }
}