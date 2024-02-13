using System.Collections;
using TMPro;
using UnityEngine;
public class Slime : Enemy
{
    public AudioSource audiosource;
    public AudioClip slimeHitSound;

    public int EnemyDamage;


    public TextMeshPro damageDisplay;


    public TextMesh enemyLevel;

    public new bool isTouchingPlayer = false;

    private void Start()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        enemyLevel.text = "lvl . " + level;
        EnemyDamage = level * 2;


        if (level is > 0 and < 10)
        {
            enemyLevel.color = tutEnemy;
        }
        else
        if (level is > 10 and < 20)
        {
            enemyLevel.color = smallEnemy;
        }
        else
        if (level is > 20 and < 30)
        {
            enemyLevel.color = medEnemy;
        }
        else
        if (level is > 30 and < 40)
        {
            enemyLevel.color = bigEnemy;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Damage Taken" + damage);
        _ = StartCoroutine(DamageDisplay(damage));
        Debug.Log("Current Health" + health);
        audiosource.PlayOneShot(slimeHitSound, 0.7f);
    }

    private IEnumerator DamageDisplay(int damage)
    {
        damageDisplay.text = "" + damage;
        yield return new WaitForSeconds(0.5f);
        damageDisplay.text = "";


    }
}
