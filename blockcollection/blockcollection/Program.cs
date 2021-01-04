using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Concurrent;

namespace blockcollection
{
    public class Recv
    {
        int threadCount;
        public int ThreadCount { get { return threadCount; } }
        Thread RecvT1;
        Thread RecvT2;
        private BlockingCollection<string> myBC;
        public BlockingCollection<string> RecvBC { get { return myBC; } }

        public Recv()
        {
            myBC = new BlockingCollection<string>();
            RecvT1 = new Thread(new ThreadStart(WorkerMethod));
            RecvT2 = new Thread(new ThreadStart(WorkerMethod));
            threadCount = 2;

            RecvT1.Start();
            RecvT2.Start();
        }

        private void WorkerMethod()
        {
            string s;
            while (true)
            {
                myBC.TryTake(out s, -1);
                if (s == null)
                {
                    Console.WriteLine("Error s is NULL.");
                }
                else if (s != "end")
                {
                    Console.WriteLine
                        ("Thread ID: {0}, Message : {1}", Thread.CurrentThread.ManagedThreadId, s);
                }
                else
                {
                    break;
                }
            }
        }
        public void JoinAll()
        {
            RecvT1.Join();
            RecvT2.Join();
            myBC.Dispose();
        }
    }

    public class Sender
    {
        Recv recv;
        public Sender(Recv r)
        {
            recv = r;
        }
        public void End()
        {
            for (int i=0; i<recv.ThreadCount; i++)
            {
                recv.JoinAll();
            }
        }
        public void SendMessage(string s)
        {
            recv.RecvBC.TryAdd(s, -1);
        }
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Main 메소드에서는 100번의 루프를 돌면서 sender에 string을 넣어주고,
            recv의 Thread는 string을 경쟁적으로 받아들이면서 화면에 출력한다.
            Console.writeLine은 Thread safety하여 화면 출력에 문제없이 동기화가 이루어진다.
            */
            Recv recv = new Recv();
            Sender sender = new Sender(recv);
            for (int i=0; i<100; i++)
            {
                sender.SendMessage(string.Format("Send {0}", i));
            }
            Console.WriteLine("Completed sending message.");
            sender.End();
        }
    }
}
