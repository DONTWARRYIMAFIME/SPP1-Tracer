using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using TracerLib;

namespace TracerLibTest
{
    public class TracerTests
    {
        
        private TraceResult SimpleMethodTrace(int sleepTime = 20)
        {
            ITracer tracer = new Tracer();
            ExampleClass exampleClass = new ExampleClass(tracer);
            exampleClass.SimpleMethod(sleepTime);

            return (TraceResult)tracer.GetTraceResult();
        }
        
        private TraceResult MediumMethodTrace()
        {
            ITracer tracer = new Tracer();
            ExampleClass exampleClass = new ExampleClass(tracer);
            exampleClass.MediumMethod();

            return (TraceResult)tracer.GetTraceResult();
        }
        
        private TraceResult DifficultMethodTrace()
        {
            ITracer tracer = new Tracer();
            ExampleClass exampleClass = new ExampleClass(tracer);
            exampleClass.DifficultMethod();
            
            return (TraceResult)tracer.GetTraceResult();
        }

        [Test]
        public void TestThreadId()
        {
            long expected = Thread.CurrentThread.ManagedThreadId;
            var result = SimpleMethodTrace();
            
            Assert.AreEqual(1, result.Threads.Count);
            Assert.AreEqual(expected, result.Threads[0].Id);
        }
        
        [Test]
        public void TestClassName()
        {
            string expected = typeof(ExampleClass).FullName;
            var result = SimpleMethodTrace();

            Assert.AreEqual(expected, result.Threads[0].Methods[0].ClassName);
        }
        
        [Test]
        public void TestMethodName()
        {
            const string expected = "SimpleMethod";
            var result = SimpleMethodTrace();
            Assert.AreEqual(1, result.Threads[0].Methods.Count);
            Assert.AreEqual(expected, result.Threads[0].Methods[0].Name);
        }
        
        [Test]
        public void TestMethodExecutionTime()
        {
            const int baseTime = 150;
            
            var result = SimpleMethodTrace(150);

            Assert.True(result.Threads[0].Time == baseTime);
            Assert.True(result.Threads[0].Methods[0].Time >= baseTime);
            Assert.True(result.Threads[0].Time >= result.Threads[0].Methods[0].Time);
        }
        
        [Test]
        public void TestSimpleMethod()
        {
            var result = SimpleMethodTrace();

            Assert.AreEqual(1, result.Threads.Count);
            Assert.AreEqual(1, result.Threads[0].Methods.Count);
            Assert.AreEqual("SimpleMethod",result.Threads[0].Methods[0].Name);
        }
        
        [Test]
        public void TestMediumMethod()
        {
            var result = MediumMethodTrace();

            Assert.AreEqual(1, result.Threads.Count);
            Assert.AreEqual(1, result.Threads[0].Methods.Count);
            Assert.AreEqual(1, result.Threads[0].Methods[0].Methods.Count);
            Assert.AreEqual("MediumMethod",result.Threads[0].Methods[0].Name);
            Assert.AreEqual("Method1",result.Threads[0].Methods[0].Methods[0].Name);
        }
        
        [Test]
        public void TestDifficultMethod()
        {
            var result = DifficultMethodTrace();

            Assert.AreEqual(1, result.Threads.Count);
            Assert.AreEqual(1, result.Threads[0].Methods.Count);
            Assert.AreEqual(2, result.Threads[0].Methods[0].Methods.Count);
            Assert.AreEqual("DifficultMethod",result.Threads[0].Methods[0].Name);
            Assert.AreEqual("Method1",result.Threads[0].Methods[0].Methods[0].Name);
            Assert.AreEqual("Method2",result.Threads[0].Methods[0].Methods[1].Name);
        }
        
        [Test]
        public void TestThreadsCount()
        {
            ITracer tracer = new Tracer();
            ExampleClass exampleClass = new ExampleClass(tracer);
            List<Thread> threads = new List<Thread>()
            {
                new(exampleClass.SimpleMethod),
                new(exampleClass.SimpleMethod),
                new(exampleClass.SimpleMethod),
                new(exampleClass.SimpleMethod),
                new(exampleClass.SimpleMethod),
            };
            
            foreach (var thread in threads)
            {
                thread.Start();
                thread.Join();
            }

            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(threads.Count, result.Threads.Count);
        }
        
        [Test]
        public void TestThreadsExecutionTime()
        {
            ITracer tracer = new Tracer();
            ExampleClass exampleClass = new ExampleClass(tracer);
            List<Thread> threads = new List<Thread>()
            {
                new(exampleClass.SimpleMethod),
                new(exampleClass.MediumMethod),
                new(exampleClass.DifficultMethod),
            };

            foreach (var thread in threads)
            {
                thread.Start();
                thread.Join();
            }

            TraceResult result = (TraceResult)tracer.GetTraceResult();
            Assert.AreEqual(threads.Count, result.Threads.Count);
            
            Assert.True(result.Threads[1].Time > result.Threads[0].Time);
            Assert.True(result.Threads[2].Time > result.Threads[1].Time);
        }
        
        
    }
}