using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SphinxQuiz : MonoBehaviour
{
    public GameObject quizUI;             // クイズUI
    public Text questionText;             // クイズのテキスト
    public Button[] answerButtons;        // 答えのボタン
    public Transform startPoint;          // プレイヤーが戻される地点
    public Transform sphinxHand;          // スフィンクスの手

    private string correctAnswer = "Fire"; // 正解

    void Start()
    {
        quizUI.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            quizUI.SetActive(true);
            DisplayQuestion();
        }
    }

    void DisplayQuestion()
    {
        questionText.text = "What burns but has no shadow?"; // 質問
        answerButtons[0].GetComponentInChildren<Text>().text = "Fire"; // 正解
        answerButtons[1].GetComponentInChildren<Text>().text = "Water";
        answerButtons[2].GetComponentInChildren<Text>().text = "Air";

        foreach (var button in answerButtons)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => CheckAnswer(button.GetComponentInChildren<Text>().text));
        }
    }

    void CheckAnswer(string selectedAnswer)
    {
        if (selectedAnswer == correctAnswer)
        {
            quizUI.SetActive(false);
            Debug.Log("Correct Answer! You may proceed.");
            // スフィンクスの手を避けて進める
        }
        else
        {
            Debug.Log("Wrong Answer! Back to start!");
            StartCoroutine(FlickPlayerBack());
        }
    }

    IEnumerator FlickPlayerBack()
    {
        Rigidbody playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        Vector3 flickDirection = (startPoint.position - sphinxHand.position).normalized;
        playerRb.AddForce(flickDirection * 1000f);

        yield return new WaitForSeconds(1f);
        GameObject.FindGameObjectWithTag("Player").transform.position = startPoint.position;
    }
}
