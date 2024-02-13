using System.Collections;
using UnityEngine;

public class Bat : Enemy
{
    public Transform attackPos;
    public GameObject BatProjectile;
    public float cooldownTime = 2;
    private float nextFireTime = 0;
    public AudioSource audiosource;
    public AudioClip batHitSound;

    public TextMesh enemyLevel;

    public TextMesh damageDisplay;

    public new bool isTouchingPlayer = false;




    // Start is called before the first frame update
    private void Start()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        enemyLevel.text = "lvl . " + level;
        damage = level * 4;


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
        audiosource.PlayOneShot(batHitSound, 0.7f);
    }

    private IEnumerator DamageDisplay(int damage)
    {
        damageDisplay.text = "" + damage;
        yield return new WaitForSeconds(0.5f);
        damageDisplay.text = "";


    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time > nextFireTime)
        {
            if (Enemy.isAggroed == true)
            {

                _ = Instantiate(BatProjectile, attackPos.position, attackPos.rotation);
                nextFireTime = Time.time + cooldownTime;

            }
        }

    }




}
