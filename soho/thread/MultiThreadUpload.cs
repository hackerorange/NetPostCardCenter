//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.Remoting.Messaging;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace soho.thread
//{
//   public  class MultiThreadUpload
//    {
//        public delegate void uploadCallBack();





//    }

//    class Program
//    {
//        public class Person
//        {
//            public string Name;
//            public int Age;
//        }

//        delegate string MyDelegate(string name);

//        static void Main(string[] args)
//        {
//            ThreadMessage("Main Thread");

//            //建立委托
//            MyDelegate myDelegate = new MyDelegate(Hello);

//            //建立Person对象
//            Person person = new Person
//            {
//                Name = "Elva",
//                Age = 27
//            };

//            //异步调用委托，输入参数对象person, 获取计算结果
//            myDelegate.BeginInvoke("Leslie", new AsyncCallback(Completed), person);

//            //在启动异步线程后，主线程可以继续工作而不需要等待
//            for (int n = 0; n < 6; n++)
//                Console.WriteLine("  Main thread do work!");
//            Console.WriteLine("");

//            Console.ReadKey();
//        }

//        static string Hello(string name)
//        {
//            ThreadMessage("Async Thread");
//            Thread.Sleep(2000);
//            return "\nHello " + name;
//        }

//        static void Completed(IAsyncResult result)
//        {
//            ThreadMessage("Async Completed");

//            //获取委托对象，调用EndInvoke方法获取运行结果
//            AsyncResult _result = (AsyncResult)result;
//            MyDelegate myDelegate = (MyDelegate)_result.AsyncDelegate;
//            string data = myDelegate.EndInvoke(_result);
//            //获取Person对象
//            Person person = (Person)result.AsyncState;
//            string message = person.Name + "'s age is " + person.Age.ToString();

//            Console.WriteLine(data + "\n" + message);
//        }

//        static void ThreadMessage(string data)
//        {
//            string message = string.Format("{0}\n  ThreadId is:{1}",
//                   data, Thread.CurrentThread.ManagedThreadId);
//            Console.WriteLine(message);
//        }
//    }


//}
