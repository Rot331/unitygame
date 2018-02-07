using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour
{
	public float staringHealth;
	protected float health;
	protected bool dead;

	public event System.Action OnDeath;

	protected virtual void Start ()
	{
		Debug.Log ("LivingEntity.Start");
		health = staringHealth;
	}

	public void TakeHit (float damage, RaycastHit hit)
	{
		// TODO: Do some stuff here with hit.
		TakeDamage (damage);
	}

	public void TakeDamage (float damage)
	{
		health -= damage;

		if (health <= 0 && !dead)
        {
			Die ();
		}
	}

	protected void Die ()
	{
		dead = true;
		if (OnDeath != null)
        {
			OnDeath ();
		}
		Destroy (gameObject);
	}
}
