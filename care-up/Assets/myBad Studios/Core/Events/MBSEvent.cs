namespace MBS {
    using System;

    [Obsolete( "mbsEventHandler is being deprecated. Please use MBSEventHandler instead" )]
    public delegate void mbsEventHandler( object source, MBSEvent e );
    public delegate void MBSEventHandler( object source, MBSEvent e );

    [Obsolete("mbsEvent is being deprecated. Please use MBSEvent instead")]
    public class mbsEvent : MBSEvent { public mbsEvent( CML data = null, object obj = null ) : base(data, obj) { } }
	public class MBSEvent : EventArgs
	{
		public CML details;
		public object event_object;
		
		public MBSEvent(CML data = null, object obj = null)
		{
			if (null == data)
			{
				data = new CML();
				data.AddNode("details");
			}
			details = data;
			event_object = obj;
		}
		
		public MBSEvent(int value, string field="value", string type = "details", object obj = null)
		{
			details = new CML();
			details.AddNode(type, $"{field}={value}");
			event_object = obj;
		}	
	}	
}