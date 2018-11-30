using UnityEngine;

static public class MBSUtilities {
	#region Vector2
	static public Vector2 SetX(this Vector2 source, float value)
	{
		source.x = value;
		return source;
	}
	
	static public Vector2 SetY(this Vector2 source, float value)
	{
		source.y = value;
		return source;
	}
	
	static public Vector2 FromString(this Vector2 source, string value)
	{
		value = value.Trim();
		value.Replace(" ", "");
		if (value.Length > 4)
		{ // minimum possible length for Vector3 string
			string[] splitString = value.Substring(1, value.Length - 2).Split(',');
			if (splitString.Length == 2)
			{
				try{
					Vector2 outVector2 = Vector2.zero;
					outVector2.x = float.Parse(splitString[0]);
					outVector2.y = float.Parse(splitString[1]);
					return outVector2;
				} catch(System.Exception e)
				{
					Debug.LogWarning("Invalid source: " + e.Message);
				}
			}
		}
		return Vector2.zero;
	}
	#endregion
	
	#region Vector3
	static public Vector3 SetX(this Vector3 source, float val)
	{
		source.x = val;
		return source;
	}
	
	static public Vector3 SetY(this Vector3 source, float val)
	{
		source.y = val;
		return source;
	}
	
	static public Vector3 SetZ(this Vector3 source, float val)
	{
		source.z = val;
		return source;
	}
	
	static public Vector3 FromString(this Vector3 source, string value)
	{
		value = value.Trim();
		value.Replace(" ", "");
		if (value.Length > 6)
		{ // minimum possible length for Vector3 string
			string[] splitString = value.Substring(1, value.Length - 2).Split(',');
			if (splitString.Length == 3)
			{
				try{
					Vector3 outVector3 = Vector3.zero;
					outVector3.x = float.Parse(splitString[0]);
					outVector3.y = float.Parse(splitString[1]);
					outVector3.z = float.Parse(splitString[2]);
					return outVector3;
				} catch(System.Exception e)
				{
					Debug.LogWarning("Invalid source: " + e.Message);
				}
			}
		}
		return Vector3.zero;
	}
	#endregion
	
	#region Rect
	static public Rect FromString(this Rect source, string value)
	{
		value = value.Trim();
		value.Replace(" ", "");
		if (value.Length > 8)
		{ // minimum possible length for Rect string
			string[] splitString = value.Substring(1, value.Length - 2).Split(',');
			if (splitString.Length == 4)
			{
				try{
					Rect outRect = new Rect(0,0,0,0);
					outRect.x = float.Parse(splitString[0]);
					outRect.y = float.Parse(splitString[1]);
					outRect.width = float.Parse(splitString[2]);
					outRect.height = float.Parse(splitString[3]);
					return outRect;
				} catch(System.Exception e)
				{
					Debug.LogWarning("Invalid source: " + e.Message);
				}
			}
		}
		return new Rect(0,0,0,0);
	}
	#endregion
	
	#region Quaternion
	static public Quaternion FromString(this Quaternion source, string value)
	{
		value = value.Trim();
		value.Replace(" ", "");
		if (value.Length > 8)
		{ // minimum possible length for Vector3 string
			string[] splitString = value.Substring(1, value.Length - 2).Split(',');
			if (splitString.Length == 3)
			{
				try{
					Quaternion outQ = Quaternion.identity;
					outQ.x = float.Parse(splitString[0]);
					outQ.y = float.Parse(splitString[1]);
					outQ.z = float.Parse(splitString[2]);
					outQ.w = float.Parse(splitString[3]);
					return outQ;
				} catch(System.Exception e)
				{
					Debug.LogWarning("Invalid source: " + e.Message);
				}
			}
		}
		return Quaternion.identity;
	}
	#endregion
	
	#region Color
	static public Color SetAlpha(this Color source, float val)
	{
		source.a = val;
		return source;
	}
	
	static public string HexValue(this Color source)
	{
		return string.Format("{0}{1}{2}{3}",((int)(source.r * 255)).ToString("X"), ((int)(source.g * 255)).ToString("X"), ((int)(source.b * 255)).ToString("X"), ((int)(source.a * 255)).ToString("X"));
	}
	
	static public Color FromString(this Color source, string value)
	{
		value = value.Trim();
		value.Replace(" ", "");
		if (value.Length > 8)
		{ // minimum possible length for a Color string
			string[] splitString = value.Substring(1, value.Length - 2).Split(',');
			if (splitString.Length == 4)
			{
				try{
					Color col = Color.black;
					col.r = float.Parse(splitString[0]);
					col.g = float.Parse(splitString[1]);
					col.b = float.Parse(splitString[2]);
					col.a = float.Parse(splitString[3]);
					return col;
				} catch(System.Exception e)
				{
					Debug.LogWarning("Invalid source: " + e.Message);
				}
			}
		}
		return Color.black;
	}
	#endregion
	
	#region string
	static public bool IsValidEmailFormat(this string s)
	{
		string[] invalids = new string[8]{" ", "?", "|", "&", "%", "!", "<", ">"};
		
		s = s.Trim();
		int atIndex	= s.IndexOf('@');
		int lastAt	= s.LastIndexOf('@');
		int dotCom	= s.LastIndexOf('.');
		
		bool result = true;
		foreach(string str in invalids)
			if (s.IndexOf(str, System.StringComparison.CurrentCulture) >= 0)
				result = false;
		
		if (result) result = (atIndex > 0);
		if (result) result = (atIndex == lastAt);
		if (result)	result = (dotCom > atIndex + 1 && dotCom < s.Length - 1);
		
		return result;
	}
	#endregion
	
}
