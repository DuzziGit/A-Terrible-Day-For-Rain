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

    public bool isTouchingPlayer = false;

    void Start()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        enemyLevel.text = "lvl . " + level;
        EnemyDamage = level * 2;


        if (level > 0 && level < 10)
        {
            enemyLevel.color = tutEnemy;
        }
        else
        if (level > 10 && level < 20)
        {
            enemyLevel.color = smallEnemy;
        }
        else
        if (level > 20 && level < 30)
        {
            enemyLevel.color = medEnemy;
        }
        else
        if (level > 30 && level < 40)
        {
            enemyLevel.color = bigEnemy;
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Damage Taken" + damage);
        StartCoroutine(DamageDisplay(damage));
        Debug.Log("Current Health" + health);
        audiosource.PlayOneShot(slimeHitSound, 0.7f);
    }


    IEnumerator DamageDisplay(int damage)
    {
        damageDisplay.text = "" + damage;
        yield return new WaitForSeconds(0.5f);
        damageDisplay.text = "";


    }
}
