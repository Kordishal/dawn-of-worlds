using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dawn_of_worlds.StateMachineClasses
{
    class FiniteStateMachine<S>
    {
        public delegate void StateTransitionCall();

        protected S currentState { get; set; }
        protected S previousState { get; set; }


        protected Dictionary<StateTransition<S>, Delegate> TransitionTable { get; set; }


        public FiniteStateMachine()
        {
            TransitionTable = new Dictionary<StateTransition<S>, Delegate>();
        }

        public void Initialise(S state)
        {
            currentState = state;
        }

        virtual public IEnumerable<S> getPossibleStates()
        {
            foreach (StateTransition<S> s in TransitionTable.Keys)
            {
                if (s.InitialState.Equals(currentState))
                    yield return s.EndState;
            }
        } 

        virtual public void Advance(S next_state)
        {
            StateTransition<S> temp_state_transition = new StateTransition<S>(currentState, next_state);

            System.Delegate temp_delegate;
            if (TransitionTable.TryGetValue(temp_state_transition, out temp_delegate))
            {
                if (temp_delegate != null)
                {
                    StateTransitionCall call = temp_delegate as StateTransitionCall;
                    call();
                }

                previousState = currentState;
                currentState = next_state;
            }
            else
            {
                throw new FiniteStateMachineException();
            }
        }

        public void AddTransition(S initial_state, S end_state, StateTransitionCall call)
        {
            StateTransition<S> temp_transition = new StateTransition<S>(initial_state, end_state);

            if (TransitionTable.ContainsKey(temp_transition))
            {
                return;
            }

            TransitionTable.Add(temp_transition, call);
        }
    }
}
