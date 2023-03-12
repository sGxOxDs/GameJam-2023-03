using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    [SerializeField] GameObject player;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    float originPosX;
    string originText;
    CinemachineBasicMultiChannelPerlin noiseComponent;
    [SerializeField] float currentScore;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || scoreText == null || virtualCamera == null) 
            Init();
        scoreText.text = originText + currentScore.ToString("#0.0");
        if (player == null)
            return;
            
        currentScore = GetScore();
        
        noiseComponent.m_AmplitudeGain = currentScore / 10f;
    }

    public float GetScore()
    {
        return Mathf.Max(player.transform.position.x - originPosX, 0f);
    }

    void Init()
    {
        var GOs = FindObjectsOfType<GameObject>();
        foreach (var go in GOs)
        {
            if (go.CompareTag("Player"))
                player = go;
            else if (go.CompareTag("Score"))
                scoreText = go.GetComponent<TMPro.TextMeshProUGUI>();
            else if (go.CompareTag("Vcam"))
                virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
        }
        if (player != null)
            originPosX = player.transform.position.x;
        originText = "Mile Walked: ";
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
}
