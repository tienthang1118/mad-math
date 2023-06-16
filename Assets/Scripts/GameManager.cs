using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
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
    private bool m_isWinning;
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
    public GameObject optionButton;
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
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        m_endgameScore = 0;
        ConfigLevel();
        m_material = background.GetComponent<Renderer>().material;
        PreparePlayerTurn();
        m_timeOfTurn = 10;
        m_timeOfTurnPassed = 0;
        m_answerText = "0";

        question.HideQuestion();
        m_currentEnemyIndex = 0;
        
        SetEnemyStats();

        if(!PlayerPrefs.HasKey("lv1HighScore"))
        {
            PlayerPrefs.SetInt("lv1HighScore", 0);
        }
        else if(!PlayerPrefs.HasKey("lv2HighScore"))
        {
            PlayerPrefs.SetInt("lv2HighScore", 0);
        }
        else if(!PlayerPrefs.HasKey("lv3HighScore"))
        {
            PlayerPrefs.SetInt("lv3HighScore", 0);
        }
        else if(!PlayerPrefs.HasKey("lv4HighScore"))
        {
            PlayerPrefs.SetInt("lv4HighScore", 0);
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
        if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyMushroom"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(15);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(3);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyMushroomBoss"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(30);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(4);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyEye"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(10);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(4);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyEyeBoss"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(20);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(6);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyGoblin"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(20);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(3);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemyGoblinBoss"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(40);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(4);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemySkeleton"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(25);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(4);
        }
        else if(enemies[m_currentEnemyIndex].gameObject.name == "EnemySkeletonBoss"){
            enemies[m_currentEnemyIndex].SetEnemyMaxHealth(50);
            enemies[m_currentEnemyIndex].SetEnemyHealth(enemies[m_currentEnemyIndex].GetEnemyMaxHealth());
            enemies[m_currentEnemyIndex].SetEnemyDamage(5);
        }

    }
    public void ClickNumberButton(){
        audioManager.PlayButtonSound();
        m_numberText = EventSystem.current.currentSelectedGameObject.name;
        m_answerText += m_numberText;
        if(m_answerText.Length>5){
            m_answerText = "10000";
        }
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
                int playerDamage = (int)((float)player.GetPlayerDamage() /10f * (10f - m_timeOfTurnPassed)) + 3;
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
        optionButton.SetActive(false);
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
            optionButton.SetActive(true);
        }
        else{
            if(m_startOfTurn){
                question.CreateRandomQuestion(m_levelDifficult);
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
                    if(m_currentEnemyIndex == enemies.Length-1){
                        Debug.Log("You win");
                        m_isWinning = true;
                        Time.timeScale = 0;
                        player.gameObject.SetActive(false);
                        enemies[m_currentEnemyIndex].gameObject.SetActive(false);
                        endgameWindow.SetActive(true);
                    }
                    else{
                        m_currentEnemyIndex++;
                        m_isFinishChangingEnemy = false;
                        m_isChangingEnemy = true;
                    }
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
                StartCoroutine(EnemyAttackAnimation());
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
        if(!m_isPlayedSound){
            audioManager.PlayAttackSound();
            m_isPlayedSound = true;
        }
        enemies[m_currentEnemyIndex].SetAttackingAnimation(true);
        yield return new WaitForSeconds(0.5f);
        enemies[m_currentEnemyIndex].SetAttackingAnimation(false);
        m_isEnemyFinishAttack = true;
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
            m_isWinning = false; 
            Time.timeScale = 0;
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
    void ConfigLevel(){
        if(SceneManager.GetActiveScene().name == "Level1"){
            m_levelDifficult = 1;
        }
        else if(SceneManager.GetActiveScene().name == "Level2"){
            Debug.Log(SceneManager.GetActiveScene().name);
            m_levelDifficult = 2;
        }
        else if(SceneManager.GetActiveScene().name == "Level3"){
            Debug.Log(SceneManager.GetActiveScene().name);
            m_levelDifficult = 3;
        }
        else if(SceneManager.GetActiveScene().name == "Level4"){
            Debug.Log(SceneManager.GetActiveScene().name);
            m_levelDifficult = 4;
        }
    }

    public void ClickNextButton(){
        audioManager.PlayButtonSound();
        if(SceneManager.GetActiveScene().buildIndex <4){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
        }
    }
    public void ClickReloadButton(){
        audioManager.PlayButtonSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void ClickHomeButton(){
        audioManager.PlayButtonSound();
        SceneManager.LoadScene("MainMenu");
    }
    public void ClickBackButton(){
        audioManager.PlayButtonSound();
        optionWindow.SetActive(false);
        Time.timeScale = 1;
    }
    public void ClickSettingButton(){
        audioManager.PlayButtonSound();
        Time.timeScale = 0;
        optionWindow.SetActive(true);
    }
    void ConfigEndgameWindow(){
        if(m_isWinning){
            endgameTitle.text = "YOU WIN";
            if(SceneManager.GetActiveScene().buildIndex <4){
                nextButton.SetActive(true);
            }
            Debug.Log(SceneManager.GetActiveScene().buildIndex+1);
            Debug.Log(PlayerPrefs.GetInt("LevelCap"));
            if(PlayerPrefs.GetInt("LevelCap") < SceneManager.GetActiveScene().buildIndex+1){
                PlayerPrefs.SetInt("LevelCap", SceneManager.GetActiveScene().buildIndex+1);
                
            }
        }
        else{
            endgameTitle.text = "YOU LOSE";
        }
        if(SceneManager.GetActiveScene().buildIndex == 1){
            if(m_endgameScore>PlayerPrefs.GetInt("lv1HighScore")){
                PlayerPrefs.SetInt("lv1HighScore", m_endgameScore);
            }
            endgameHighScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("lv1HighScore").ToString();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2){
            if(m_endgameScore>PlayerPrefs.GetInt("lv2HighScore")){
                PlayerPrefs.SetInt("lv2HighScore", m_endgameScore);
            }
            endgameHighScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("lv2HighScore").ToString();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3){
            if(m_endgameScore>PlayerPrefs.GetInt("lv3HighScore")){
                PlayerPrefs.SetInt("lv3HighScore", m_endgameScore);
            }
            endgameHighScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("lv3HighScore").ToString();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 4){
            if(m_endgameScore>PlayerPrefs.GetInt("lv4HighScore")){
                PlayerPrefs.SetInt("lv4HighScore", m_endgameScore);
            }
            endgameHighScoreText.text = "HIGHSCORE " + PlayerPrefs.GetInt("lv4HighScore").ToString();
        }
        endgameScoreText.text = "SCORE " + m_endgameScore.ToString(); 
        
    }
}
