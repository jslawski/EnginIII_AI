using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float moveSpeed = 15f;
    private float lifespanInSeconds = 3.0f;

    public int attackPower = 5;

    [SerializeField]
    private Rigidbody projectileRb;

    private Creature owningCreature;

    public void Launch(Creature creature, Vector3 direction)
    {
        this.owningCreature = creature;
        StartCoroutine(this.MoveProjectile(direction.normalized));
    }

    private IEnumerator MoveProjectile(Vector3 direction)
    {
        float currentLifespan = 0.0f;
        while (currentLifespan < this.lifespanInSeconds)
        {
            currentLifespan += Time.fixedDeltaTime;
            
            float moveSpeedThisFrame = this.moveSpeed * Time.fixedDeltaTime;
            Vector3 targetDestination = this.projectileRb.position + (direction * moveSpeedThisFrame);            
            this.projectileRb.MovePosition(targetDestination);

            yield return new WaitForFixedUpdate();
        }

        Destroy(this.gameObject);
    }

    //Based on layers, projectiles can ONLY collide with the Player right now
    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        other.gameObject.GetComponent<Creature>().TakeDamage(this.owningCreature, this.attackPower, false);
        Destroy(this.gameObject);
    }
}
