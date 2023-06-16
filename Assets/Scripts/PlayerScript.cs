using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private int m_playerMaxHealth;
    private int m_playerHealth;
    private int m_playerDamage;
    private Animator playerAnimation;
    // Start is called before the first frame update
    void Start()
    {
        m_playerMaxHealth = 20;
        m_playerHealth = m_playerMaxHealth;
        m_playerDamage = 10;
        playerAnimation = GetComponent<Animator>();
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetPlayerHealth(){
        return m_playerHealth;
    }
    public void SetPlayerHealth(int playerHealth){
        this.m_playerHealth = playerHealth;
    }
    public int GetPlayerMaxHealth(){
        return m_playerMaxHealth;
    }
    public void SetPlayerMaxHealth(int playerHealth){
        this.m_playerMaxHealth = playerHealth;
    }
    public int GetPlayerDamage(){
        return m_playerDamage;
    }
    public void SetPlayerDamage(int playerDamage){
        this.m_playerDamage = playerDamage;
    }
    public void SetRunningAnimation(bool status){
        playerAnimation.SetBool("isRunning", status);
    }
    public void SetAttackingAnimation(bool status){
        playerAnimation.SetBool("isAttacking", status);
    }
    public void SetDeadAnimation(bool status){
        playerAnimation.SetBool("isDead", status);
    }
}
