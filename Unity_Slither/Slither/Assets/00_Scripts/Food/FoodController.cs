using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Food
{
	public class FoodController : MonoBehaviour
	{
		public Vector2 SceneScale = new Vector2(100, 100);
		public Vector2 FoodScale = new Vector2(0.2f, 0.5f);
		public GameObject FoodPrefab;
		public int StartFoodCount = 200;
		List<GameObject> FoodGOs = new List<GameObject>();
		List<FoodParticle> FoodParticles = new List<FoodParticle>();
		List<FoodParticle> DisabledFoodParticles = new List<FoodParticle>();

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
			for (int i = 0; i<StartFoodCount; i++) 
			{
				AddOneFood ();
			}
		}

		void AddOneFood () 
		{
			GameObject GO = GameObject.Instantiate (FoodPrefab);
			FoodParticle FP = GO.GetComponent<FoodParticle> ();

			FP.ThisColor = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), 1);;
			FP.CreateParticle (SceneScale, FoodScale);

			FoodGOs.Add (GO);
			FoodParticles.Add(FP);		
		}

		void Food_EatenParticle_Handler (object val) 
		{
			FoodParticle FP = (FoodParticle)val;
			FP.gameObject.SetActive (false);

			DisabledFoodParticles.Add (FP);
			if (DisabledFoodParticles.Count > 10) 
			{
				DisabledFoodParticles[0].CreateParticle (SceneScale, FoodScale);
				DisabledFoodParticles[0].gameObject.SetActive(true);
				DisabledFoodParticles.RemoveAt (0);
			}
		}
	}
}