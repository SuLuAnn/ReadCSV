using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Process p = new Process();
            Console.WriteLine("Current State = " + p.CurrentState);
            Console.WriteLine("Command.Begin: Current State = " + p.MoveNext(Command.Begin));
            Console.WriteLine("Command.Pause: Current State = " + p.MoveNext(Command.Pause));
            Console.WriteLine("Command.End: Current State = " + p.MoveNext(Command.End));
            Console.WriteLine("Command.Exit: Current State = " + p.MoveNext(Command.Exit));
            Console.ReadLine();
        }
    }
}
