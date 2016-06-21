using UnityEngine;
//using System.Collections;

namespace Worm
{
	public class MovementController : MonoBehaviour
	{
		public Transform SteerTargGO;
		public float Speed = 5;
		public float InputSpeedMultiplier = 10;
		public float RotationMultiplier = 300;
		Vector3 CurrentPosition;
		Vector3 MovementDirection;

		public void Update_Movement(float input_Horizontal, float InputSpeed)
		{
			CurrentPosition = transform.position;

			MovementDirection = transform.localEulerAngles;
			MovementDirection.y += Time.deltaTime * input_Horizontal * RotationMultiplier;
			transform.localEulerAngles = MovementDirection;

			CurrentPosition = Vector3.MoveTowards(CurrentPosition, SteerTargGO.position, (Time.deltaTime * Speed) + (InputSpeed * InputSpeedMultiplier * Time.deltaTime));
			transform.position = CurrentPosition;
		}
	}
}