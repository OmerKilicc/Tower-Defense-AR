using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Singleton

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion



    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private GameObject _gameOverCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideMainMenu(GameObject canvasMainMenu) 
    {
        canvasMainMenu.SetActive(false);
    }

    public void TogglePauseMenu(bool isOpen) 
    {
        _pauseCanvas.SetActive(isOpen);
    }

    public void HandleGameOver() 
    {
        _gameOverCanvas.SetActive(true);
    }

    public void TryAgainButton() 
    {
        GameManager.Instance.RestartGame(); 
    }
}
