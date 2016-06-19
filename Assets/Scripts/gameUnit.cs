using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class gameUnit : MonoBehaviour {

	public float speed = 3.0f;
	public bool dead = false;
	public UnityEvent Death { get; private set; }
	
	public Weapon weapon { get; private set; }

	public Rigidbody2D body { get; private set; }
	public CircleCollider2D hitbox { get; private set; }
	public SpriteRenderer sprite { get; private set; }
	
	private float deadTime = 0f;

	void Awake () {
		Death = new UnityEvent ();
		Death.AddListener (isDead);
		body = GetComponent<Rigidbody2D> ();
		hitbox = GetComponent<CircleCollider2D> ();
		sprite = GetComponent<SpriteRenderer> ();
		weapon = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (dead && !dying) {
		if (dead) {
			deadTime += Time.deltaTime;
			sprite.color = Color.Lerp(Color.white, new Color(1,0,0,0), deadTime);
		}
	}

	private void isDead() {
		if (!dead) {
			dead = true;
			drop ();
			gameManager.instance.sounds.Play ("Dead");
			deadTime -= Time.deltaTime;
			Destroy (this.gameObject, 1.0f);
		}
	}

	public void equip (Weapon wpn) {
		if (!weapon) {
			weapon = wpn.equip(this);
			if (gameManager.instance.sounds)
				gameManager.instance.sounds.Play("Selected");
		} else {
			Debug.Log ("Unit "+this+" not holding a weapon!");
		}
	}

	public void drop () {
		if (weapon) {
			weapon.drop ();
			weapon = null;
		} else {
			Debug.Log ("Unit "+this+" not holding a weapon!");
		}
	}
}
