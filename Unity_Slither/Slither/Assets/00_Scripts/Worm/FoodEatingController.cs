using UnityEngine;
using System.Collections;
using Food;

namespace Worm
{
	public class FoodEatingController : MonoBehaviour
	{
		void OnTriggerEnter (Collider other) 
		{
			FoodParticle FP = other.gameObject.GetComponent<FoodParticle> ();
			if (FP == null) { return; }
			 
			AppData.setData (AppDataKeys.Food_Eaten, FP);
		}
	}
}