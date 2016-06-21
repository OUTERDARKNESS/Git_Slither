using UnityEngine;
using System.Collections;
using Food;

namespace Game
{
	public class Score : MonoBehaviour
	{
		public int CurScore;

		void Awake () 
		{
			AppData.bindToData (AppDataKeys.Food_Eaten, Food_EatenParticle_Handler);
		}

		void OnDestroy () 
		{
			AppData.unbindToData (AppDataKeys.Food_Eaten, Food_EatenParticle_Handler);
		}

		void Start () 
		{
			CurScore = 0;
		}	

		void Food_EatenParticle_Handler (object val) 
		{
			FoodParticle FP = (FoodParticle)val;
			CurScore += FP.Calories;
		}

		void OnGUI () 
		{
			GUILayout.Space (50);
			GUILayout.Box ("Score: ", GUILayout.Width(100));
			GUILayout.Box ("" + CurScore, GUILayout.Width(100));
		}
	}
}