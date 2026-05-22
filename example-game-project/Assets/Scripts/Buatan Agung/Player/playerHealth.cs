using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHealth : MonoBehaviour
{
    private Animator anim;
    public int maxHealth = 100;
    public int health;
    public GameOverScript gameOver;

    public HealthBar healthBar;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    public void takeDamage(int Damage)
    {
        health -= Damage;
        if(health <=0)
        {
            StartCoroutine(playerDead());
        }
        healthBar.SetHealth(health);
    }

    private IEnumerator playerDead()
    {
        anim.SetBool("dead", true);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
        anim.SetBool("dead", false);
        gameOver.Setup();
    }
}
