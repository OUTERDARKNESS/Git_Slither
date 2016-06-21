using System;
using UnityEngine;

namespace Worm
{
	public class InputController : MonoBehaviour
	{
		public float input_Horizontal = 0;
		public float InputSpeed = 0;

		public void Update_UserInput()
		{
			input_Horizontal = Input.GetAxis ("Horizontal");
			InputSpeed = Input.GetAxis ("Boost");
		}

		void OnGUI () 
		{
			GUILayout.Box ("InputSpeed: " + InputSpeed);
		}
	}
}

