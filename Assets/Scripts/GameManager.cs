using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] GameObject[] Stages;
    [SerializeField] PlayerController player;

    private int totalPoint;
    private int stagePoint;
    private int stageIndex;

    // Start is called before the first frame update
    void Start()
    {
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        else {  // clear
            // Control Lock
            Time.timeScale = 0;

            Debug.Log("클리어");
        }
    }

    public void HealthDown() {
        health--;

        if (health <= 0) {
            player.OnDead();
        }
    }

    public void PlayerReposition() {
        player.transform.position = new Vector3 (0, 2, -1);
        player.VelocityZero();
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            HealthDown();
            PlayerReposition();
        }
    }
}
