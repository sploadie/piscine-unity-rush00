using UnityEngine;
using System.Collections;

[RequireComponent (typeof (RectTransform))]
public class ammoBar : MonoBehaviour {

	private RectTransform rect;

	// Use this for initialization
	void Start () {
		rect = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		rect.offsetMax = new Vector2 (-60f, rect.offsetMax.y);
		Weapon weapon = playerHandler.instance.character.weapon;
		if (weapon)
			rect.offsetMax = new Vector2 (rect.offsetMax.x - (((float)weapon.ammoCount / (float)weapon.ammo_count) * -60f), rect.offsetMax.y);
	}
}
