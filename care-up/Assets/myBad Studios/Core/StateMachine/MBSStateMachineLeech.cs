namespace MBS
{
    using System;
    using System.Collections.Generic;

    [Obsolete( "mbsStateMachineLeech is being deprecated. Please use MBSStateMachineLeech instead" )]
    public class mbsStateMachineLeech<T> : MBSStateMachineLeech<T> { public mbsStateMachineLeech( MBSStateMachine<T> _source ) : base( _source ) { } }
    public class MBSStateMachineLeech<T>
    {
        MBSStateMachine<T> source;
        public MBSStateMachine<T> Source => source;
        public Dictionary<T, Action> StateFunctions;

        T _currentState => source.CurrentState;
        public T CurrentState => _currentState;
        Action currentAction;

        public MBSStateMachineLeech( MBSStateMachine<T> _source )
        {
            source = _source;
            StateFunctions = new Dictionary<T, Action>();
        }

        public bool PerformAction()
        {
            if ( StateFunctions.ContainsKey( _currentState ) && null != StateFunctions [_currentState] )
                StateFunctions [_currentState]();

            return true;
        }

        public bool CompareState( T state ) => source.CompareState( state );
        
        public void AddState( T statename, Action function = null )
        {
            if ( null == statename )
                return;

            if ( StateFunctions.ContainsKey( statename ) )
                StateFunctions [statename] = function;
            else
                StateFunctions.Add( statename, function );
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
