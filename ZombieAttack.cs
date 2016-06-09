using UnityEngine;
using System.Collections;

public class ZombieAttack : MonoBehaviour {

    public float damage = 50f;

    private Animator anim;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool attacking;
    private int attackFloat;

    void Awake()
    {
        anim = GetComponent<Animator>();
        attackFloat = Animator.StringToHash("Attack");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.gameObject.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        float attack = anim.GetFloat(attackFloat);
        if (attack > 0.5f && !attacking && playerHealth.health > 0)
            AttackPlayer();
        if (attack < 0.5f)
            attacking = false;
    }

    void AttackPlayer()
    {
        attacking = true;
        playerHealth.takeDamage(damage);
    }
}
