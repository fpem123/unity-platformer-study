using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private int point;

    [SerializeField] private GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {

    }

    public void DeactiveItem() {
        gameObject.SetActive(false);

        gameManager.UpStagePoint(point);
    }
}
