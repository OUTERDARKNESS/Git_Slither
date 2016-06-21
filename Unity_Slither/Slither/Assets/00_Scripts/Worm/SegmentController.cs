using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Food;

namespace Worm
{
	public class SegmentController : MonoBehaviour
	{
		public float TotalCalorie;
		public Vector3 LastPos;
		public GameObject SegPrefab;
		public int SegCount = 5;
		public float SpeedPenaltyMult = 2;
		public List<Transform> SegList = new List<Transform>();

		public int StartSegCount = 5;
		public List<Vector3> HeadPosList = new List<Vector3>();

		void Awake () 
		{
			AppData.bindToData (AppDataKeys.Food_Eaten, Food_Eaten_Handler);
		}

		void OnDestroy () 
		{
			AppData.unbindToData (AppDataKeys.Food_Eaten, Food_Eaten_Handler);
		}

		void Start () 
		{
			StartSegCount = SegCount;
			TotalCalorie = (float)SegCount;
			LastPos = transform.position;
			Update_SegCount ();

			for (int i = 0; i < SegCount + 2; i++) 
			{
				HeadPosList.Add (Vector3.zero);
			}
		}

		void Food_Eaten_Handler (object val) 
		{
			FoodParticle FP = (FoodParticle)val;
			SegCount += FP.Calories;

			TotalCalorie = (float)SegCount;
			Update_SegCount ();
		}

		public void Update_SpeedPenalty(float InputSpeed)
		{
			if (InputSpeed > 0 && SegCount > StartSegCount) 
			{
				TotalCalorie -= Time.deltaTime * SpeedPenaltyMult;
				if (Mathf.CeilToInt (TotalCalorie) < SegCount) 
				{
					SegCount = Mathf.CeilToInt (TotalCalorie);
					Update_SegCount ();
				}
			} else {
				InputSpeed = 0;
			}
		}

		public void Update_HeadPos()
		{
			float segmentDistanceThreshold = 0.3333f;
			float distance = Vector3.Distance (transform.position, LastPos);

			if (distance > segmentDistanceThreshold) 
			{
				HeadPosList.Add (transform.position);
				if (HeadPosList.Count > (SegCount + 500)) 
				{
					HeadPosList.RemoveAt (0);
				}
				LastPos = transform.position;
			}

			Update_SegPos ();
		}

		public void Update_SegPos()
		{
			for (int i = 0; i < SegList.Count; i++) 
			{
				SegList [i].transform.position = HeadPosList [HeadPosList.Count - 1 - i];
			}
		}

		void Update_SegCount()
		{
			while(SegList.Count < SegCount) 
			{
				GameObject GO = GameObject.Instantiate (SegPrefab);
				GO.name = GO.name + SegCount;
				SegList.Add (GO.transform);
			}

			for (int i = 0; i < SegList.Count; i++) 
			{
				SegList [i].gameObject.SetActive( (SegCount > i));
			}
		}
	}
}