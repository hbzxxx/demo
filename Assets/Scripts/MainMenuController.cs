using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private bool _isEnterGame = false;//是否进入了主菜单
    private GameObject _startGame;
    private PlayableDirector _timeline;
    private Button _newGameBtn;
    private Button _continueGameBtn;
    private Button _exitGameBtn;
    private void Awake()
    {
        _startGame = GameObject.Find("StartGame");//启动画面文字
        _timeline = GameObject.Find("Timeline").GetComponent<PlayableDirector>();//过场动画
        _newGameBtn = GameObject.Find("NewGameBtn").GetComponent<Button>();//新游戏
        _continueGameBtn = GameObject.Find("ContinueGameBtn").GetComponent<Button>();//继续游戏
        _exitGameBtn = GameObject.Find("ExitGameBtn").GetComponent<Button>();//退出游戏
    }
    private void Start()
    {
        _newGameBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        _continueGameBtn.onClick.AddListener(() =>
        {
        });
        _exitGameBtn.onClick.AddListener(() =>
        {
        });
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isEnterGame)
        {
            _isEnterGame = true;
            _timeline.Play();
            _startGame.gameObject.SetActive(false);
        }
    }
}
