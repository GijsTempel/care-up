using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace MBS {
	public enum WUTActions	{
		Dummy, 
		FetchAllStats, 
		FetchStat, 
		GetStatStatus, 
		SpendPoints, 
		GetPoints, 
		UpdateMaxPoints, 
		UpdateMaxTimer, 
		SetMaxPoints, 
		SetMaxTimer,
		DeleteTimer,
		Count }

	public class WUTimer : MonoBehaviour {
		
		int 
			points = 0,
			points_max = 0,
			timer = 0,
			timer_max = 0,
			
			previous_value = 0;
		
		string
			formatted_timer;
		
		static readonly string timer_filepath = "wuss_timer/unity_functions.php";
		static readonly string TIMERConstant = "TIMER";
		
		public System.Action<CMLData>
			onContactedServer;

		public System.Action<int>
			onTimerEvent;
		
		public string
			field_name = "Energy";		//this value will be stored in the database so make sure to mind the spelling 
		
		public int
			initial_starting_value = 5, //ideally keep these two values the same...
			initial_max_value = 5,		//these values configure the database with initial values
			initial_max_timer = 60;
		
		public bool
			count_down = false;
		
		public int Value				{ get { return points; } }
		public int ValueBounds			{ get { return points_max; } }
		public int Timer				{ get { return count_down ? timer : TimerBounds - timer; } }
		public int TimerBounds			{ get { return timer_max; } } 
		public int PreviousValue		{ get { return previous_value; } }
		public string FormattedTimer	{ get { return formatted_timer; } }
		
		System.Action<CML> _onContactedServer;
		
		// Use this for initialization
		void Start () {
			_onContactedServer = __onContactedServer;
			
			points = initial_starting_value;
			points_max = initial_max_value;
			timer_max = initial_max_timer;
			
			FetchStat();
		}

		void OnDestroy()
		{
			CancelInvoke ();
			points = points_max = timer = timer_max = 0;
			formatted_timer = ConvertToTimer (0);
		}
		
		//fetch all stat details...
		public void FetchStat()
		{
			CMLData data = new CMLData();
			data.Set ("fid", field_name);
			data.Seti("tx" , initial_max_timer);
			data.Seti("vx" , initial_max_value);
			data.Seti("v"  , initial_starting_value);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.FetchStat.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		//same as FetchStat() but only returns the timer and points, not the maximum values also
		public void UpdateStat()
		{
			CMLData data = new CMLData();
			data.Set ("fid", field_name);
			
			//you could copy the fields from the FetchStat() function but that would be
			//a total waste as you are sending configuration data each time your timer updates...
			//here you can specify that you don't want to send the eXtra info along
			//by doing this, you are eXplicitly declaring that you are 100% sure this field
			//has already been created at some point in the past... Set X to anything other than 0
			data.Seti("x", 1); 
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.GetStatStatus.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		//when force_to_0 is true, you can spend more points than you have and the results will be 0
		//when it is false, if you do not have enough points, the total is not updated,
		//the response is never called and the WUTServer.onServerContactFailed callback is triggered
		public void SpendPoints(int amount)
		{
			SpendPoints(amount, false);
		}
		
		public void SpendPoints(int amount, bool force_to_0)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			data.Seti ("force", force_to_0 ? 1 : 0);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.SpendPoints.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void GivePoints(int amount)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.GetPoints.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void UpdateMaxPoints(int amount)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.UpdateMaxPoints.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void SetMaxPoints(int amount)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.SetMaxPoints.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void UpdateMaxTimer(int amount)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.UpdateMaxTimer.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void SetMaxTimer(int amount)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);
			data.Seti ("amt"  , amount);
			
			CancelInvoke ();
			WPServer.ContactServer(WUTActions.SetMaxTimer.ToString(), timer_filepath, TIMERConstant, data, _onContactedServer);
		}
		
		public void DeleteTimer(Action<CML> onSuccess, Action<CMLData> onError = null)
		{
			CMLData data = new CMLData();
			data.Set  ("fid"  , field_name);

			CancelInvoke ();
			WPServer.ContactServer(WUTActions.DeleteTimer.ToString(), timer_filepath, TIMERConstant, data, onSuccess, onError);
		}

		void __onContactedServer(CML response)
		{
			CMLData _timer = response.GetFirstNodeOfType("timer");
			if (null != _timer)
			{
				int 
					max			= _timer.Int ("px"),
					max_timer	= _timer.Int ("tx"),
					pts			= _timer.Int ("p"),
					ttu			= _timer.Int ("t");
				
				
				if (max > 0) points_max = max;
				if (max_timer > 0 ) timer_max = max_timer;
				
				if (points != pts)
					previous_value = points;
				
				points = pts;
				timer = ttu;
				
				if (points > points_max)
				{
					points = points_max;
					timer = -1;
				}
				
				//format output text...
				FormatOutPutText();
				
				//allow external kits to plug into this action...
				if (null != onContactedServer)
					onContactedServer(_timer);
				
				if (points != PreviousValue && null != onTimerEvent)
					onTimerEvent(points);
				
				if (timer > 0)
					Invoke("UpdateTimer", 1);
			}
		}
		
		static public string ConvertToTimer(int amount)
		{
			int seconds = amount % 60;
			amount -= seconds;
			
			int minutes = amount / 60;
			int hours	= 0;
			int days	= 0;
			
			if (minutes > 59)
			{
				minutes %= 60;
				hours = ((amount / 60) - minutes) / 24;
			}
			
			if (hours > 23)
			{
				int h = hours % 24;
				days = (hours - h) / 24;
				hours = h;
			}
			
			if (days > 0)
				return $"{days:D2}:{hours:D2}:{minutes:D2}:{seconds:D2}";
			if (hours > 0)
                return $"{hours:D2}:{minutes:D2}:{seconds:D2}";

            return $"{minutes:D2}:{seconds:D2}";
		}
		
		void FormatOutPutText()
		{
			//the server returns how many seconds are left and we keep taking seconds off that value
			//so let's see how many more seconds are left on the timer
			int difference = timer_max - timer;
			if (difference > timer_max)
				difference = timer_max;
			
			//difference will go from 0 to timer_max but to count down we want it to go the other way
			//so subtract the difference from timer_max again
			if (count_down)
				difference = timer_max - difference;

			//now set the string value
			formatted_timer = ConvertToTimer(difference);
		}
		
		void UpdateTimer () {
			if (timer >= 0)
				timer--;
			
			FormatOutPutText();
			
			switch(timer)
			{
			case 0:
				UpdateStat();
				break;
			case -1:
				Invoke("UpdateStat", timer_max);
				break;
			default:
				Invoke("UpdateTimer", 1);
				break;
			}
		}
	}
}