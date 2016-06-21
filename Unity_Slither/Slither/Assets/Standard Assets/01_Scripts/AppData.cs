#region File Notes
/// <summary>
/// Version - 1.0
/// Created - 2012-05-07, JAC
/// LastMod - 2012-05-14, JAC
/// 
/// USAGE:
/// Make sure this script is in "Standard Assets" somewhere, and make static method calls.
/// When binding method calls from JavaScript, be sure to do the data binding from the
/// JS file directly, not from a different JS file. For example:
/// 
/// AppData.bindToData("OrthoZoomChange", MapZoomValueHandler); // Method local to the current file
/// 
/// 
/// DESCRIPTION:
/// This stores Key/Value style data, and keeps track of who wants to be notified when
/// the value changes. This can also be set up to synchronize all changes across
/// the network.
/// 
/// Valid types that can be synchronized are:
/// . int, System.Int32
/// . float, System.Single
/// . string, System.String
/// . NetworkPlayer, UnityEngine.NetworkPlayer
/// . NetworkViewID, UnityEngine.NetworkViewID
/// . Vector3, UnityEngine.Vector3
/// . Quaternion, UnityEngine.Quaternion
/// . Enumerations, System.Enum based objects
/// . bool, System.Boolean
/// 
/// CHANGE LOG:
/// 1.0
/// Included System.Boolean in the allowed RPC list.
/// 
/// Beta 1.5
/// Converted back to pure static class for ease of use, and split out the networking component to AppDataNetwork.
/// 
/// Beta 1.4
/// Converted to a MonoBehaviour in order to allow it to be used in a networking situation.
/// </summary>
#endregion
using UnityEngine;
using System.Collections.Generic;

public delegate void AppDataUpdated(object NewValue);

public static class AppData
{
    #region Fields
    static public Dictionary<string, object> StoredValues = new Dictionary<string, object>();
    static private Dictionary<string, AppDataUpdated> StoredBindings = new Dictionary<string, AppDataUpdated>();
	
	static public List<string> Errors = new List<string>();
	//static public AppDataNetwork SynchToNetwork = null;
	
	static public bool ReturnNullOnNotFound = false;
	#endregion

	#region Methods
	static private object GetData(string Key)
	{
		if (StoredValues.ContainsKey(Key)) { return StoredValues[Key]; }
		else if (ReturnNullOnNotFound) { return null; }

		throw new System.Exception("Key data not found: " + Key);
	}
	
    /// <summary>
    /// Convenience method for initial set up.
    /// </summary>
    /// <param name='Key'>Unique name for the entry</param>
    /// <param name='Value'>String value to be stored</param>
    /// <param name='Binding'>Callback function to notify when the value changes</param>
    static private void SetData(string Key, object Value, AppDataUpdated Binding)
    {
        SetData(Key, Value, false);
        BindData(Key, Binding);
    }

    /// <summary>
    /// Convenience method for assigning a value.
    /// </summary>
    /// <param name='Key'>Unique name for the entry</param>
    /// <param name='Value'>String value to be stored</param>
    static private void SetData(string Key, object Value)
    {
        SetData(Key, Value, true);
    }

	/// <summary>
	/// Stores the provided data.
	/// </summary>
	/// <param name='Key'>Unique name for the entry</param>
	/// <param name='Value'>String value to be stored</param>
    /// <param name='TriggerEvent'>Flag to control activating the event</param>
	static private void SetData(string Key, object Value, bool TriggerEvent)
    {
        SetData(Key, Value, TriggerEvent, true);
    }

    /// <summary>
    /// Stores the provided data.
    /// </summary>
    /// <param name='Key'>Unique name for the entry</param>
    /// <param name='Value'>String value to be stored</param>
    /// <param name='TriggerEvent'>Flag to control activating the event</param>
    /// <param name='TriggerNetworkUpdate'>Controls updating the network server</param>
    static public void SetData(string Key, object Value, bool TriggerEvent, bool TriggerNetworkUpdate)
    {
        if (StoredValues.ContainsKey(Key) == false || StoredValues[Key] != Value)
        {
            StoredValues[Key] = Value;

            if (TriggerEvent && StoredBindings.ContainsKey(Key) && StoredBindings[Key] != null)
            {
                StoredBindings[Key](Value);
            }
			
//            if (SynchToNetwork != null && 
//				TriggerNetworkUpdate &&
//				Network.peerType != NetworkPeerType.Disconnected && 
//				GoodRPCType(Value.GetType()))
//            {
//                SynchToNetwork.SendToNetwork(Key, Value);
//            }
        }
	}
	
	/// <summary>
	/// Checks to see if this type of object can be synchronized across a network connection.
	/// </summary>
	/// <returns>true if the type is recognized</returns>
	/// <param name='TheType'>Type of the object being checked</param>
	public static bool GoodRPCType(System.Type TheType)
	{
		if (TheType == typeof(int) ||
			TheType == typeof(float) ||
			TheType == typeof(string) || 
//			TheType == typeof(NetworkPlayer) || 
//			TheType == typeof(NetworkViewID) || 
			TheType == typeof(Vector3) || 
			TheType == typeof(Quaternion) ||
			TheType.BaseType == typeof(System.Enum) || 
			TheType == typeof(bool))
		{
					return true;
		}

		return false;
	}
	
	/// <summary>
	/// Binds the provided method to the event delegate.
	/// </summary>
	/// <param name='Key'>Unique name for the entry</param>
	/// <param name='Binding'>Callback function to notify when the value changes</param>
	static private void BindData(string Key, AppDataUpdated Binding)
	{
		if (Binding != null)
		{
			if (StoredBindings.ContainsKey(Key) == false) { StoredBindings.Add(Key, null); }
			StoredBindings[Key] += Binding;
		}
	}
	
	/// <summary>
	/// Unbinds the provided method from the event delegate.
	/// </summary>
	/// <param name='Key'>Unique name for the entry</param>
	/// <param name='Binding'>Callback function to no longer notify when the value changes</param>
	static private void UnbindData(string Key, AppDataUpdated Binding)
	{
		if (Binding != null && Key != null && StoredBindings.ContainsKey(Key))
		{
			StoredBindings[Key] -= Binding;
		}
	}

    /// <summary>
    /// Simple method to record any errors that happen. Mostly useful for networking
    /// </summary>
    /// <param name="NewError"></param>
    static public void RecordError(string NewError)
    {
        string TmpError = string.Format("{0:yyyy-MM-dd HH:mm:ss}: {1}", System.DateTime.Now, NewError);
        Errors.Add(TmpError);
        Debug.LogError(TmpError);
    }
	#endregion
	
	#region Static Helper/Convenience Methods
	static public object getData(string Key) { return GetData(Key); }

    static public string getDataString(string Key)
    {
        string Ret = null;
        object RetObj = GetData(Key);
        if (RetObj != null) { Ret = RetObj.ToString(); }
        return Ret;
    }
	
	static public int getDataInt(string Key)
	{
		int Ret = 0;
		if (StoredValues.ContainsKey(Key) && StoredValues[Key].GetType() == typeof(int)) { Ret = (int)StoredValues[Key]; }
		else
		{
			int TmpInt;
			if (int.TryParse(getDataString(Key), out TmpInt)) { Ret = TmpInt; }
		}
		return Ret;
	}
	
	static public float getDataFloat(string Key)
	{
		float Ret = 0f;
		if (StoredValues.ContainsKey(Key) && StoredValues[Key].GetType() == typeof(float)) { Ret = (float)StoredValues[Key]; }
		else
		{
			float TmpFloat;
			if (float.TryParse(getDataString(Key), out TmpFloat)) { Ret = TmpFloat; }
		}
		return Ret;
	}
	
	static public bool getDataBool(string Key)
	{
		bool Ret = false;
		if (StoredValues.ContainsKey(Key) && StoredValues[Key].GetType() == typeof(bool)) { Ret = (bool)StoredValues[Key]; }
		else
		{
			bool TmpBool;
			if (bool.TryParse(getDataString(Key), out TmpBool)) { Ret = TmpBool; }
		}
		return Ret;
	}

//	static public U getDataType<U>(string Key)
//	{
//		U Ret;
//		if (StoredValues.ContainsKey(Key) && StoredValues[Key].GetType() == typeof(U)) { Ret = (U)StoredValues[Key]; }
//		else
//		{
//			//U TmpU;
//			//if (U.TryParse(getDataString(Key), out TmpU)) { Ret = TmpU; }
//			try
//			{
//				Ret = (U)System.TypeDescriptor.GetConvertor(typeof(U)).ConvertFromString(getDataString(Key));
//			}
//			catch { }
//		}
//		return Ret;
//	}
	
	static public void setData(string Key, object Value) { SetData(Key, Value); }

    static public void setData(string Key, object Value, bool TriggerEvent) { SetData(Key, Value, TriggerEvent); }
	
	static public void setData(string Key, object Value, AppDataUpdated Binding) { SetData(Key, Value, Binding); }
	
	static public void createIfNotExists(string Key, object Value)
	{
		if (!StoredValues.ContainsKey(Key))
		{
			StoredValues.Add(Key, Value);
		}
	}
	
	static public void bindToData(string Key, AppDataUpdated Binding) { BindData(Key, Binding); }
	
	static public AppDataUpdated bindingForData(string Key)
	{
		if (StoredBindings.ContainsKey(Key) == false) { StoredBindings.Add(Key, null); }
		return StoredBindings[Key];
	}
	
	static public void unbindToData(string Key, AppDataUpdated Binding) { UnbindData(Key, Binding); }
	#endregion
	
	#region Debugging
	static public void Log(string Message)
	{
		Debug.Log(string.Format("{0:yyyy-MM-dd HH:mm:ss} ({1:0.0000}): {2}", System.DateTime.Now, Time.time, Message));
	}
	
	static public void Log(string FormattedMessage, params object[] ListForFormat)
	{
		Log(string.Format(FormattedMessage, ListForFormat));
	}
	
	static public string printDataBase()
	{
		System.Text.StringBuilder Output = new System.Text.StringBuilder();
		Output.AppendFormat("[AppData: Value Count={0}", StoredValues.Count);
		foreach (KeyValuePair<string, object> kvp in StoredValues)
		{
			Output.AppendLine();
			Output.AppendFormat("  Key='{0}', {1}", kvp.Key, kvp.Value.ToString());
		}
		Output.Append("]");
		Debug.Log(Output.ToString());
		return Output.ToString();
	}
	#endregion
}
