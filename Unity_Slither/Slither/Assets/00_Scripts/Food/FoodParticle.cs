using UnityEngine;
using System.Collections;

namespace Food
{
	public class FoodParticle : MonoBehaviour
	{
		public int Calories = 1;
		public Color ThisColor;

		public void CreateParticle (Vector2 SceneScale, Vector2 FoodScale) 
		{
			Vector3 ThisPos = new Vector3 (Random.Range (SceneScale.x * 0.5f, SceneScale.x * -0.5f), 0.2f, Random.Range (SceneScale.y * 0.5f, SceneScale.y * -0.5f));
			gameObject.transform.position = ThisPos;

			Vector3 ThisScale = Vector3.one * Random.Range (FoodScale.x, FoodScale.y);
			gameObject.transform.localScale = ThisScale;

			gameObject.transform.parent = transform;

			Renderer[] Rends = gameObject.GetComponentsInChildren<Renderer> ();
			for (int i = 0; i < Rends.Length; i++) 
			{
				Rends [i].material.color = ThisColor;
				Rends [i].material.SetColor ("_TintColor", ThisColor);
			}
		}
	}
}