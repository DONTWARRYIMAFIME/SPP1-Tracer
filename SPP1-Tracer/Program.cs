using System;
using System.IO;
using System.Threading;
using TracerLib;
using TracerLib.serialization;

namespace SPP1_Tracer
{
    class Program
    {
        static void Main(string[] args)
        {
            ITracer tracer = new Tracer();
            Foo foo = new Foo(tracer);

            Thread thread1 = new Thread(foo.MyMethod1);
            thread1.Start();

            Thread thread2 = new Thread(foo.MyMethod2);
            thread2.Start();

            thread1.Join();
            thread2.Join();

            ITraceResult result = tracer.GetTraceResult();
            
            ISerializer jsonSerializer = new JsonTraceResultSerializer();
            ISerializer xmlSerializer = new XmlTraceResultSerializer();

            string json = jsonSerializer.Serialize(result);
            string xml = xmlSerializer.Serialize(result);
            
            Console.WriteLine(json);
            Console.WriteLine(xml);
            
            File.WriteAllText("out.json", json);
            File.WriteAllText("out.xml", xml);
        }
        
    }
    
    public class Foo
    {
        private readonly Bar _bar;
        private readonly ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(tracer);
        }
    
        public void MyMethod1()
        {
            _tracer.StartTrace();
            MyMethod2();
            Thread.Sleep(105);
            _bar.InnerMethod1();
            Thread.Sleep(15);
            MyMethod3();
            _tracer.StopTrace();
        }
        
        public void MyMethod2()
        {
            _tracer.StartTrace();
            Thread.Sleep(110);
            _bar.InnerMethod1();
            Thread.Sleep(50);
            _bar.InnerMethod2();
            Thread.Sleep(90);
            _tracer.StopTrace();
        }
        
        public void MyMethod3()
        {
            _tracer.StartTrace();
            Thread.Sleep(150);
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private readonly ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }
    
        public void InnerMethod1()
        {
            _tracer.StartTrace();
            Thread.Sleep(120);
            _tracer.StopTrace();
        }
        
        public void InnerMethod2()
        {
            _tracer.StartTrace();
            InnerMethod3();
            Thread.Sleep(40);
            _tracer.StopTrace();
        }
        
        public void InnerMethod3()
        {
            _tracer.StartTrace();
            Thread.Sleep(15);
            InnerMethod1();
            Thread.Sleep(30);
            _tracer.StopTrace();
        }

    }

}