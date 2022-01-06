using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private GameObject[] Stages;
    [SerializeField] private PlayerController player;

    [SerializeField] private Image[] UIHealth;
    [SerializeField] private Text UIPoint;
    [SerializeField] private Text UIStage;
    [SerializeField] private GameObject UIStartBtn;
    [SerializeField] private GameObject UIRestartBtn;


    private int totalPoint;
    private int stagePoint;
    private int stageIndex;

    // Start is called before the first frame update
    void Awake()
    {
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;

        UIPoint.text = "0";
        UIStage.text = "STAGE 1";
        
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void UpStagePoint(int point) {
        stagePoint += point;
    }

    public void NextStage() {
        totalPoint += stagePoint;
        stagePoint = 0;

        if (stageIndex < Stages.Length - 1) {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);

            PlayerReposition();
            UIStage.text = "STAGE " + (stageIndex + 1).ToString();
        }
        else {  // clear
            // Control Lock
            Time.timeScale = 0;
            BtnTextSet("Clear!");
        }
    }

    public void HealthDown() {
        health--;
        UIHealth[health].color = new Color(1, 1, 1, 0f);

        if (health <= 0) {
            player.OnDead();
            BtnTextSet("Re?");
        }
    }

    public void PlayerReposition() {
        player.transform.position = new Vector3 (0, 2, -1);
        player.VelocityZero();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            player.OnDamaged(transform.position);
            PlayerReposition();
        }
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void GameStart() {
        Time.timeScale = 1;
        UIStartBtn.SetActive(false);
    }

    private void BtnTextSet(string text) {
        Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
        btnText.text = text;
        UIRestartBtn.SetActive(true);
    }
}
