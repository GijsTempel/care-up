namespace MBS
{
    using System;
    using System.Collections.Generic;
    using UnityEngine.Assertions;

    [Obsolete( "mbsStateMachine is being deprecated. Please use MBSStateMachine instead" )]
    public class mbsStateMachine<T> : MBSStateMachine<T> { public mbsStateMachine() : base() { } }
    public class MBSStateMachine<T>
    {
        T currentState;
        Action _currentAction;

        public Dictionary<T, Action> StateFunctions;

        public MBSStateMachine()
        {
            StateFunctions = new Dictionary<T, Action>();
        }

        public T CurrentState => currentState;
        public bool PerformAction() => PerformAction( currentState );
        public bool PerformAction( T action )
        {
            int a = (int)Convert.ChangeType( action, typeof( int ) );
            int c = (int)Convert.ChangeType( currentState, typeof( int ) );

            if ( a != c )
                SetState( action );

            _currentAction?.Invoke();

            return true;
        }

        public bool CompareState( T state )
        {
            int s = (int)Convert.ChangeType( state, typeof( int ) );
            int c = (int)Convert.ChangeType( currentState, typeof( int ) );
            return s == c;
        }

        virtual public bool SetState( T to )
        {
            if ( StateFunctions.ContainsKey( to ) )
            {
                currentState = to;
                _currentAction = StateFunctions [currentState];
                return true;
            }
            Assert.IsTrue( false, $"Cannot find state {to}" );
            return false;
        }

        public void AddState( T statename, Action function = null )
        {
            if ( null == statename )            
                return;
            

            if ( StateFunctions.ContainsKey( statename ) )
            {
                StateFunctions [statename] = function;
            }
            else
            {
                StateFunctions.Add( statename, function );

                if ( null == _currentAction )
                    SetState( statename );
            }
        }

        public bool RemoveState( T statename )
        {
            if ( StateFunctions.ContainsKey( statename ) )
            {
                StateFunctions.Remove( statename );
                return true;
            }
            return false;
        }
    }
}
