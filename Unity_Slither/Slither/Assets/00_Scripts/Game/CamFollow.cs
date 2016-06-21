using UnityEngine;
using System.Collections;

namespace Game
{
	public class CamFollow : MonoBehaviour
	{
		public Transform WormTransform;
		Vector3 CurrentPosition;
		float Elevation = 0;

		float Input_ZoomInput = 0;

		public float ZoomMult = 10f;
		float MaxZoom = 60;
		float MinZoom = 5;

		void Start () 
		{
			Elevation = transform.position.y - WormTransform.position.y;
		}

		void Update () 
		{
			CurrentPosition = WormTransform.position;
			CurrentPosition.y += Elevation;
			transform.position = CurrentPosition;

			Update_UserInput ();
		}

		void Update_UserInput()
		{
			Input_ZoomInput = Input.GetAxis ("RtStick_Vert");

			Elevation += Input_ZoomInput * Time.deltaTime * ZoomMult;
			Elevation = Mathf.Clamp (Elevation, MinZoom, MaxZoom);
		}

		void OnGUI()
		{
			GUILayout.Space (300);
			GUILayout.Box ("Input_ZoomInput: " + Input_ZoomInput);
		}
	}
}