using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SurvivalScript : MonoBehaviour
{
    public AudioManager audioManager;
    
    [SerializeField]private bool m_isPlayerAttacking;
    [SerializeField]private bool m_isPlayerFinishAttack;
    [SerializeField]private bool m_isEnemyAttacking;
    [SerializeField]private bool m_isEnemyFinishAttack;
    [SerializeField]private bool m_isPlayerHPCaculate;
    [SerializeField]private bool m_playerTurn;
    [SerializeField]private bool m_chooseAction;
    [SerializeField]private bool m_startOfTurn;
    [SerializeField]private bool m_isChangingEnemy;
    private bool m_isFinishChangingEnemy;
    // private bool m_isWinning;
    private bool m_isPlayedSound;

    public enum ActionType {attack, regenerate}
    public ActionType m_actionType;
    
    public TextMeshProUGUI m_playerTurnText;
    public TextMeshProUGUI m_timeOfTurnText;
    private float m_timeOfTurn;
    private float m_timeOfTurnPassed;
    public PlayerScript player;
    public GameObject numpad;
    public GameObject actionButtons;
    public GameObject playerStartPos;
    public GameObject playerGoalPos;
    public GameObject enemyStartPos;
    public GameObject enemyGoalPos;
    public GameObject background;
    public GameObject regenerateAnim;
    public GameObject wrongShout;
    public GameObject endgameWindow;
    public GameObject optionWindow;
    public TextMeshProUGUI endgameTitle;
    public TextMeshProUGUI endgameScoreText;
    public TextMeshProUGUI endgameHighScoreText;
    public GameObject nextButton;
    
    private int m_endgameScore;
    [SerializeField] private int m_levelDifficult;

    Material m_material;
    // public GameObject Player;
    
    //numpad
    [SerializeField]
    private string m_numberText;
    [SerializeField]
    private string m_answerText;
    [SerializeField]
    private int m_answerNumber;
    [SerializeField]
    private bool m_isClickAnswerButton;
    
    //player and enemy info
    public Slider playerHealthBar;
    public Slider enemyHealthBar;
    public EnemyScript[] enemies;
    public QuestionScript question;
    private int m_currentEnemyIndex;
    float m_backgroundOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        m_endgameScore = 0;

        m_material = background.GetComponent<Renderer>().material;
        PreparePlayerTurn();
        m_timeOfTurn = 10;
        m_timeOfTurnPassed = 0;
        m_answerText = "0";

        question.HideQuestion();
        m_currentEnemyIndex = 0;
        
        SetEnemyStats();

        if(!PlayerPrefs.HasKey("survivalHighScore"))
        {
            PlayerPrefs.SetInt("survivalHighScore", 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        ChangeTurn();
        if(m_isPlayerAttacking){
            PlayerAttacking();
        }
        if(m_isEnemyAttacking){
            EnemyAttacking();
        }
        if(m_isChangingEnemy){
            ChangeEnemy();
        }
        ConfigEndgameWindow();
    }
    public void SetEnemyStats(){
        
        enemies[0].SetEnemyMaxHealth(15);
        enemies[0].SetEnemyHealth(enemies[0].GetEnemyMaxHealth());
        enemies[0].SetEnemyDamage(3);
    
    
        enemies[1].SetEnemyMaxHealth(30);
        enemies[1].SetEnemyHealth(enemies[1].GetEnemyMaxHealth());
        enemies[1].SetEnemyDamage(5);
    
    
        enemies[2].SetEnemyMaxHealth(10);
        enemies[2].SetEnemyHealth(enemies[2].GetEnemyMaxHealth());
        enemies[2].SetEnemyDamage(4);
    
    
        enemies[3].SetEnemyMaxHealth(20);
        enemies[3].SetEnemyHealth(enemies[3].GetEnemyMaxHealth());
        enemies[3].SetEnemyDamage(6);
    
    
        enemies[4].SetEnemyMaxHealth(20);
        enemies[4].SetEnemyHealth(enemies[4].GetEnemyMaxHealth());
        enemies[4].SetEnemyDamage(3);
    
    
        enemies[5].SetEnemyMaxHealth(40);
        enemies[5].SetEnemyHealth(enemies[5].GetEnemyMaxHealth());
        enemies[5].SetEnemyDamage(4);
    
    
        enemies[6].SetEnemyMaxHealth(25);
        enemies[6].SetEnemyHealth(enemies[6].GetEnemyMaxHealth());
        enemies[6].SetEnemyDamage(4);
    
        enemies[7].SetEnemyMaxHealth(50);
        enemies[7].SetEnemyHealth(enemies[7].GetEnemyMaxHealth());
        enemies[7].SetEnemyDamage(5);
        

    }
    public void ClickNumberButton(){
        audioManager.PlayButtonSound();
        m_numberText = EventSystem.current.currentSelectedGameObject.name;
        m_answerText += m_numberText;
        m_answerNumber = int.Parse(m_answerText);
        question.DisplayQuestion(m_answerNumber);
    }
    public void ClickDellButton(){
        audioManager.PlayButtonSound();
        m_answerText = "0";
        m_answerNumber = int.Parse(m_answerText);
        question.DisplayQuestion(m_answerNumber);
    }
    public void ClickAnswerButton(){
        audioManager.PlayButtonSound();
        question.HideQuestion();
        numpad.gameObject.SetActive(false);
        
        
        m_answerNumber = int.Parse(m_answerText);
        m_answerText = "0";
        
        if(question.CheckAnswer(m_answerNumber)){
            if(m_actionType == ActionType.attack){
                m_endgameScore += (int)(10f - m_timeOfTurnPassed);

                m_isPlayerAttacking = true;
                int playerDamage = (int)((float)player.GetPlayerDamage() /10f * (10f - m_timeOfTurnPassed));
                int enemyHealth = enemies[m_currentEnemyIndex].GetEnemyHealth() - playerDamage;
                Debug.Log("Correct");
                enemies[m_currentEnemyIndex].SetEnemyHealth(enemyHealth);
            }
            else if(m_actionType == ActionType.regenerate){
                StartCoroutine(RegenerateAnimation());
            }
        }
        else{
            Debug.Log("Wrong");
            StartCoroutine(WrongShoutAnimation());
        }
    }
    public void ClickAttackButton(){
        audioManager.PlayButtonSound();
        m_actionType = ActionType.attack;
        DisplayNumpad();
    }
    public void ClickRegenerateButton(){
        audioManager.PlayButtonSound();
        m_actionType = ActionType.regenerate;
        DisplayNumpad();
    }
    public void DisplayNumpad(){
        m_chooseAction = false;
        actionButtons.gameObject.SetActive(false);
        numpad.gameObject.SetActive(true);
    }
    void PlayerHPChanged()
    {
        playerHealthBar.value = (float)player.GetPlayerHealth()/20;    
    }
    void EnemyHPChanged()
    {
        if(enemies[m_currentEnemyIndex].GetEnemyHealth()>0){
            enemyHealthBar.value = (float)enemies[m_currentEnemyIndex].GetEnemyHealth()/(float)enemies[m_currentEnemyIndex].GetEnemyMaxHealth();
        }
        else{
            enemyHealthBar.gameObject.SetActive(false);
        }
    }

    void ChangeTurn(){
        if(m_playerTurn){
            PlayerTurn();
        }
        else{
            EnemyTurn();
        }
    }
    void PlayerTurn(){
        if(m_chooseAction){
            m_timeOfTurnText.text = "";
            actionButtons.gameObject.SetActive(true);
        }
        else{
            if(m_startOfTurn){
                question.CreateRandomQuestion(4);
                question.DisplayQuestion(0);
                m_startOfTurn = false;
            }
            if(numpad.activeSelf){
                m_timeOfTurnText.text = (10-m_timeOfTurnPassed).ToString("00");
                m_timeOfTurnPassed += Time.deltaTime;
            }
            else{
                m_timeOfTurnText.text = "";
            }
            if(m_timeOfTurnPassed>=m_timeOfTurn){
                PrepareEnemyTurn();
                numpad.gameObject.SetActive(false);
                question.HideQuestion();
            }
            
        }
        
        m_playerTurnText.text = "YOUR TURN";
    }
    void PlayerAttacking(){
        if(!m_isPlayerFinishAttack){
            if(player.transform.position.x <= playerGoalPos.transform.position.x){
                player.SetRunningAnimation(true);
                player.gameObject.transform.position += Vector3.right * 5 * Time.deltaTime;
            }
            else{
                if(!m_isPlayerFinishAttack){
                    StartCoroutine(PlayerAttackAnimation());
                }
            }
        }
        else{
            if(player.transform.position.x >= playerStartPos.transform.position.x){
                if(player.gameObject.transform.localScale.x >0){
                    player.gameObject.transform.localScale = new Vector3(player.gameObject.transform.localScale.x*-1, player.gameObject.transform.localScale.y, player.gameObject.transform.localScale.z);
                }
                player.SetRunningAnimation(true);
                player.gameObject.transform.position += Vector3.left * 5 * Time.deltaTime;
            }
            else{
                player.SetRunningAnimation(false);
                player.gameObject.transform.localScale = new Vector3(player.gameObject.transform.localScale.x*-1, player.gameObject.transform.localScale.y, player.gameObject.transform.localScale.z);

                if(enemies[m_currentEnemyIndex].GetEnemyHealth()<=0){
                    m_currentEnemyIndex = Random.Range(0, 8);
                    m_isFinishChangingEnemy = false;
                    m_isChangingEnemy = true;
                }
                else{
                    PrepareEnemyTurn();
                }
                m_isPlayerAttacking = false;
                m_isPlayerFinishAttack = false;
            }
        }
    }
    void EnemyAttacking(){
        if(!m_isEnemyFinishAttack){
            if(enemies[m_currentEnemyIndex].transform.position.x >= enemyGoalPos.transform.position.x){
                enemies[m_currentEnemyIndex].SetRunningAnimation(true);
                enemies[m_currentEnemyIndex].gameObject.transform.position += Vector3.left * 5 * Time.deltaTime;
            }
            else{
                if(!m_isEnemyFinishAttack){
                    StartCoroutine(EnemyAttackAnimation());
                }
            }
        }
        else{
            if(enemies[m_currentEnemyIndex].transform.position.x <= enemyStartPos.transform.position.x){
                if(enemies[m_currentEnemyIndex].gameObject.transform.localScale.x >0){
                    enemies[m_currentEnemyIndex].gameObject.transform.localScale = new Vector3(enemies[m_currentEnemyIndex].gameObject.transform.localScale.x*-1, enemies[m_currentEnemyIndex].gameObject.transform.localScale.y, enemies[m_currentEnemyIndex].gameObject.transform.localScale.z);
                }
                enemies[m_currentEnemyIndex].SetRunningAnimation(true);
                enemies[m_currentEnemyIndex].gameObject.transform.position += Vector3.right * 5 * Time.deltaTime;
            }
            else{
                enemies[m_currentEnemyIndex].SetRunningAnimation(false);
                enemies[m_currentEnemyIndex].gameObject.transform.localScale = new Vector3(enemies[m_currentEnemyIndex].gameObject.transform.localScale.x*-1, enemies[m_currentEnemyIndex].gameObject.transform.localScale.y, enemies[m_currentEnemyIndex].gameObject.transform.localScale.z);
                m_isEnemyAttacking = false;
                PreparePlayerTurn();
            }
        }
    }
    void EnemyTurn(){
        m_playerTurnText.text = "ENEMY TURN";
        m_timeOfTurnText.text = "";

        
    }
    void ChangeEnemy(){
        if(!m_isFinishChangingEnemy){
            player.SetRunningAnimation(true);
            m_backgroundOffset += (Time.deltaTime*(float)0.3);
            m_material.SetTextureOffset("_MainTex", new Vector2(m_backgroundOffset, 0));

            m_playerTurnText.text = "";
            m_timeOfTurnText.text = "";
            
            StartCoroutine(Waiter());
        }
        else{
            
            m_isChangingEnemy = false;
            PreparePlayerTurn();
        }

    }
    IEnumerator PlayerAttackAnimation()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        enemies[m_currentEnemyIndex].SetTakingHitAnimation(true);
        player.SetAttackingAnimation(true);
        if(!m_isPlayedSound){
            audioManager.PlayAttackSound();
            m_isPlayedSound = true;
        }
        yield return new WaitForSeconds(0.5f);
        EnemyHPChanged();
        // yield return new WaitForSeconds(0.3f);
        enemies[m_currentEnemyIndex].SetTakingHitAnimation(false);
        player.SetAttackingAnimation(false);
        m_isPlayerFinishAttack = true;
        if(enemies[m_currentEnemyIndex].GetEnemyHealth()<=0){
            enemies[m_currentEnemyIndex].gameObject.SetActive(false);
        }
    }
    IEnumerator EnemyAttackAnimation()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        enemies[m_currentEnemyIndex].SetAttackingAnimation(true);
        if(!m_isPlayedSound){
            audioManager.PlayAttackSound();
            m_isPlayedSound = true;
        }
        yield return new WaitForSeconds(0.3f);
        m_isEnemyFinishAttack = true;
        enemies[m_currentEnemyIndex].SetAttackingAnimation(false);
        if(!m_isPlayerHPCaculate){
            int playerHealth = player.GetPlayerHealth();
            int enemyDamage = enemies[m_currentEnemyIndex].GetEnemyDamage();
            player.SetPlayerHealth(playerHealth - enemyDamage);
            
            m_isPlayerHPCaculate = true;
        }

        PlayerHPChanged();
        
        yield return new WaitForSeconds(0.3f);
        

        if(player.GetPlayerHealth()<0){
            player.SetDeadAnimation(true);
            yield return new WaitForSeconds(0.5f);
            
            Time.timeScale = 0;
            player.gameObject.SetActive(false);
            enemies[m_currentEnemyIndex].gameObject.SetActive(false);
            endgameWindow.SetActive(true);
        }
    }
    IEnumerator Waiter()
    {
        
        yield return new WaitForSeconds(3);
        player.SetRunningAnimation(false);

        enemies[m_currentEnemyIndex].gameObject.SetActive(true);
        SetEnemyStats();
        enemyHealthBar.gameObject.SetActive(true);
        EnemyHPChanged();
        
        m_isFinishChangingEnemy = true;

    }
    IEnumerator RegenerateAnimation()
    {
        regenerateAnim.SetActive(true);
        audioManager.PlayHealingSound();
        yield return new WaitForSeconds(1.5f);
        regenerateAnim.SetActive(false);
        int playerRegenerate = player.GetPlayerHealth() + (int)((float)player.GetPlayerDamage() /10f * (10f - m_timeOfTurnPassed)) + 3;
        if((player.GetPlayerHealth() + playerRegenerate)>player.GetPlayerMaxHealth()){
            playerRegenerate = player.GetPlayerMaxHealth();
        }
        player.SetPlayerHealth(playerRegenerate);
        PlayerHPChanged();
        yield return new WaitForSeconds(0.5f);
        PrepareEnemyTurn();

    }
    IEnumerator WrongShoutAnimation()
    {
        wrongShout.SetActive(true);
        audioManager.PlayWrongSound();
        yield return new WaitForSeconds(1);
        wrongShout.SetActive(false);
        PrepareEnemyTurn();
    }
    void PreparePlayerTurn(){
        m_timeOfTurnPassed = 0;
        m_isPlayerAttacking = false;
        m_isPlayerFinishAttack = false;
        m_chooseAction = true;
        m_startOfTurn = true;
        m_playerTurn = true;
        m_isEnemyFinishAttack = false;
        m_isPlayerHPCaculate = false;
        m_isPlayedSound = false;
    }
    void PrepareEnemyTurn(){
        m_isEnemyFinishAttack = false;
        m_isPlayerHPCaculate = false;
        m_isEnemyFinishAttack = false;
        m_isEnemyAttacking = true;
        m_playerTurn = false;
        m_isPlayedSound = false;
    }

    public void ClickReloadButton(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void ClickHomeButton(){
        SceneManager.LoadScene("MainMenu");
    }
    public void ClickBackButton(){
        optionWindow.SetActive(false);
        Time.timeScale = 1;
    }
    public void ClickSettingButton(){
        Time.timeScale = 0;
        optionWindow.SetActive(true);
    }
    void ConfigEndgameWindow(){
        endgameTitle.text = "YOU LOSE";
        if(m_endgameScore>PlayerPrefs.GetInt("survivalHighScore")){
            PlayerPrefs.SetInt("survivalHighScore", m_endgameScore);
        }
        endgameHighScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("survivalHighScore").ToString();
        endgameScoreText.text = "SCORE " + m_endgameScore.ToString(); 
        
    }
}
