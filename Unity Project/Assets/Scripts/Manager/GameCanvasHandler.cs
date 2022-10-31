using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameCanvasHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI healthText;

    InputAssets.PlayerController playerController;

    void Awake()
    {
        playerController = GameObject.Find("Player").GetComponentInChildren<InputAssets.PlayerController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerController.health.ToString();
    }
}
