using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    private int m_firstNumber;
    private int m_secondNumber;
    private string m_operation;
    private int m_answer;

    public TextMeshProUGUI questionText;
    // Start is called before the first frame update
    void Start()
    {
        CreateRandomQuestion(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRandomQuestion(int level){
        int operationCode;
        if(level == 1){
            m_firstNumber = Random.Range(1,21);
            m_secondNumber = Random.Range(1,21);
            m_operation = "+";
            m_answer = m_firstNumber + m_secondNumber;
        }
        else if(level == 2){
            m_firstNumber = Random.Range(10,50);
            m_secondNumber = Random.Range(1,m_firstNumber);
            operationCode = Random.Range(1,3);
            if(operationCode == 1){
                m_operation = "+";
                m_answer = m_firstNumber + m_secondNumber;
            }
            else if(operationCode == 2){
                m_operation = "-";
                m_answer = m_firstNumber - m_secondNumber;
            }
        }
        else if(level == 3){
            operationCode = Random.Range(1,4);
            if(operationCode == 1){
                m_firstNumber = Random.Range(10, 101);
                m_secondNumber = Random.Range(10, 101);
                m_operation = "+";
                m_answer = m_firstNumber + m_secondNumber;
            }
            else if(operationCode == 2){
                m_firstNumber = Random.Range(30,101);
                m_secondNumber = Random.Range(10, m_secondNumber);
                m_operation = "-";
                m_answer = m_firstNumber - m_secondNumber;
            }
            else if(operationCode == 3){
                m_operation = "x";
                m_firstNumber = Random.Range(2,21);
                m_secondNumber = Random.Range(2, 10);
                m_answer = m_firstNumber * m_secondNumber;
            }
        }
        else if(level == 4){
            operationCode = Random.Range(1,5);
            if(operationCode == 1){
                m_firstNumber = Random.Range(50, 101);
                m_secondNumber = Random.Range(50, 101);
                m_operation = "+";
                m_answer = m_firstNumber + m_secondNumber;
            }
            else if(operationCode == 2){
                m_firstNumber = Random.Range(50,101);
                m_secondNumber = Random.Range(10, m_secondNumber);
                m_operation = "-";
                m_answer = m_firstNumber - m_secondNumber;
            }
            else if(operationCode == 3){
                m_operation = "x";
                m_firstNumber = Random.Range(2,51);
                m_secondNumber = Random.Range(2, 12);
                m_answer = m_firstNumber * m_secondNumber;
            }
            else if(operationCode == 4){
                m_operation = ":";
                m_firstNumber = Random.Range(50, 201);
                m_secondNumber = RandomDivisible(m_firstNumber);
                m_answer = m_firstNumber / m_secondNumber;
            }
        }
    }
    public bool CheckAnswer(int answer){
        if(m_answer == answer){
            return true;
        }
        else{
            return false;
        }
    }
    public int GetFirstNumber(){
        return m_firstNumber;
    }
    public int GetSecondNumber(){
        return m_secondNumber;
    }
    public string GetOperation(){
        return m_operation;
    }
    public int GetAnswer(){
        return m_answer;
    }
    public void DisplayQuestion(int tmpAnswer){
        if(tmpAnswer == 0){
            questionText.text = m_firstNumber + " " + m_operation + " " + m_secondNumber + " = ";
        }
        else{
            questionText.text = m_firstNumber + " " + m_operation + " " + m_secondNumber + " = " + tmpAnswer;
        }
    }
    public void HideQuestion(){
        questionText.text = "";
    }
    private int RandomDivisible(int firstNumber)
    {
        int randomResult = Random.Range(2, firstNumber); 
        if (firstNumber % randomResult == 0) 
        {
            return randomResult;
        }
        else
        {
            return RandomDivisible(firstNumber); 
        }
    }
}
