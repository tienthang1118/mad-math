using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NumpadScript : MonoBehaviour
{
    // [SerializeField]
    // private string m_numberText;
    // [SerializeField]
    // private string m_answerText;
    // [SerializeField]
    // private int m_answerNumber;
    // [SerializeField]
    // private bool m_isClickAnswerButton;
    
    // // Start is called before the first frame update
    // void Start()
    // {
    //     m_answerText = "0"; 
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
    // public void ClickNumberButton(){
    //     m_numberText = EventSystem.current.currentSelectedGameObject.name;
    //     m_answerText += m_numberText;
    //     m_answerNumber = int.Parse(m_answerText);
    // }
    // public void ClickDellButton(){
    //     m_answerText = "0";
    // }
    // public void ClickAnswerButton(){
    //     m_answerNumber = int.Parse(m_answerText);
    //     m_answerText = "0";
    //     if(Question.GetComponent<QuestionScirpt>().CheckAnswer(m_answerNumber)){
    //         Debug.Log("Correct");
    //     }
    //     else{
    //         Debug.Log("Wrong");
    //     }
    // }
    // public int GetInputAnswer(){
    //     return m_answerNumber;
    // }
    // public bool IsClickAnswerButton(){
    //     return m_isClickAnswerButton;
    // }
}
