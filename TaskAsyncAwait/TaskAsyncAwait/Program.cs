using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskAsyncAwait
{
    public class Program
    {
        static void Main(string[] args)
        {
            //new Bank().Run();                   
            new BankWithLock().Run();
            Console.ReadLine();
            Console.ReadKey();
        }

        private class Bank
        {
            private int Property;

            public void Run()
            {
                Property = 500;
                Thread t1 = new Thread(Save);
                Thread t2 = new Thread(Pickup);

                t1.Start(2);
                t2.Start(1);
            }

            private void Save(object delay)
            {
                Property += 100;
                Thread.Sleep((int)delay);
                Console.WriteLine("目前剩餘存款: {0}", Property);
            }

            private void Pickup(object delay)
            {
                Property -= 100;
                Thread.Sleep((int)delay);
                Console.WriteLine("目前剩餘存款: {0}", Property);
            }
        }

        private class BankWithLock
        {
            private int Property;

            private object Locker;

            public void Run()
            {
                Locker = new object();
                Property = 500;
                Thread t1 = new Thread(Save);
                Thread t2 = new Thread(Pickup);

                t1.Start(300);
                t2.Start(100);
            }

            private void Save(object delay)
            {
                lock (Locker)
                {
                    Property += 100;
                    Thread.Sleep((int)delay);
                    Console.WriteLine("目前剩餘存款: {0}", Property);
                }             
            }

            private void Pickup(object delay)
            {
                lock (Locker)
                {
                    Property -= 100;
                    Thread.Sleep((int)delay);
                    Console.WriteLine("目前剩餘存款: {0}", Property);
                }              
            }
        }
    }
}
