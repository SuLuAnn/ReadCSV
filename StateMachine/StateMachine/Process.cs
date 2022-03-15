using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    public enum ProcessState
    {
        Inactive,
        Active,
        Paused,
        Terminated
    }

    public enum Command
    {
        Begin,
        End,
        Pause,
        Resume,
        Exit
    }
    class Process
    {
        Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public Process()
        {
            CurrentState = ProcessState.Inactive;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Inactive, Command.Exit), ProcessState.Terminated },
                { new StateTransition(ProcessState.Inactive, Command.Begin), ProcessState.Active },
                { new StateTransition(ProcessState.Active, Command.End), ProcessState.Inactive },
                { new StateTransition(ProcessState.Active, Command.Pause), ProcessState.Paused },
                { new StateTransition(ProcessState.Paused, Command.End), ProcessState.Inactive },
                { new StateTransition(ProcessState.Paused, Command.Resume), ProcessState.Active }
            };
        }

        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command)
        {
            CurrentState = GetNext(command);
            return CurrentState;
        }

    }
}
