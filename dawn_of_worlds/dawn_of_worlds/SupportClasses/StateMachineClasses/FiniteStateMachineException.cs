using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.StateMachineClasses
{
    class FiniteStateMachineException : Exception
    {


        public override string Message
        {
            get
            {
                return "The FSM encountered an invalid transition";
            }
        }
    }
}
