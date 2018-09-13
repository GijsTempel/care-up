using System.Collections.Generic;
using System.Linq;
using System;

namespace MBS
{

    public class MBSEvents<T>
    {
        public Dictionary<T, MBSEventHandler>
        events;

        public void ClearEvents()
        {
            if ( null == events )
                return;
            events.Clear();
        }

        public void AddEvent( T action, MBSEventHandler response )
        {
            if ( null == events )
                events = new Dictionary<T, MBSEventHandler>();

            if ( !events.ContainsKey( action ) )
                events [action] = null;

            if ( null != events [action] && events [action].GetInvocationList().Contains( response ) )
                return;
            events [action] += response;
        }

        public void RemoveEvent( T action, MBSEventHandler response )
        {
            if ( null == events )
                events = new Dictionary<T, MBSEventHandler>();

            if ( !events.ContainsKey( action ) )
                return;

            if ( events [action].GetInvocationList().Contains( response ) )
                events [action] -= response;
        }

        public void TriggerEvent( T action, object source = null, MBSEvent data = null )
        {
            if ( null != events && events.ContainsKey( action ) && null != events [action] )
                events [action]( source, data );
        }
    }

    public class MBSNotices<T>
    {

        public Dictionary<T, Action>
        events;

        public void Clear()
        {
            if ( null == events )
                return;
            events.Clear();
        }

        public void Add( T action, Action response )
        {
            if ( null == events )
                events = new Dictionary<T, Action>();

            if ( !events.ContainsKey( action ) )
                events [action] = null;

            if ( null != events [action] && events [action].GetInvocationList().Contains( response ) )
                return;
            events [action] += response;
        }

        public void Remove( T action, Action response )
        {
            if ( null == events )
                events = new Dictionary<T, Action>();

            if ( !events.ContainsKey( action ) )
                return;

            if ( events [action].GetInvocationList().Contains( response ) )
                events [action] -= response;
        }

        public void Trigger( T action )
        {
            if ( null != events && events.ContainsKey( action ) && null != events [action] )
                events [action]();
        }
    }
}