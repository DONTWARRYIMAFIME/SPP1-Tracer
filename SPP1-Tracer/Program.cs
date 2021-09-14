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

            foo.MyMethod();

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
    
        public void MyMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(10);
            _bar.InnerMethod();
            Thread.Sleep(15);
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
    
        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(200);
            _tracer.StopTrace();
        }

    }
    
    
    
}