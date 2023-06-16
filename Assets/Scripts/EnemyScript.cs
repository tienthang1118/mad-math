using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int m_enemyHealth;
    private int m_enemyDamage;
    private int m_enemyMaxHealth;
    private Animator enemyAnimation;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetEnemyHealth(){
        return m_enemyHealth;
    }
    public int GetEnemyMaxHealth(){
        return m_enemyMaxHealth;
    }
    public void SetEnemyHealth(int enemyHealth){
        this.m_enemyHealth = enemyHealth;
    }
    public void SetEnemyMaxHealth(int enemyHealth){
        this.m_enemyMaxHealth = enemyHealth;
    }
    public int GetEnemyDamage(){
        return m_enemyDamage;
    }
    public void SetEnemyDamage(int enemyDamage){
        this.m_enemyDamage = enemyDamage;
    }
    public void SetRunningAnimation(bool status){
        enemyAnimation.SetBool("isRunning", status);
    }
    public void SetAttackingAnimation(bool status){
        enemyAnimation.SetBool("isAttacking", status);
    }
    public void SetTakingHitAnimation(bool status){
        enemyAnimation.SetBool("isTakingHit", status);
    }
}
