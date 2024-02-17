using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject title;
    [SerializeField] GameObject inputField;
    [SerializeField] GameObject StartBtn;

    [Header("설정")]
    [SerializeField] Transform setBtn;
    [SerializeField] Transform panel;
    [SerializeField] Transform saveSetBtn;

    private string playerName = null;

    private void Start()
    {
    }

    public void NextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void EnterText(string text)
    {
        playerName = text;
    }

    public void OnStart()
    {
        title.SetActive(false);
        inputField.SetActive(true);
        StartBtn.SetActive(false);
    }

    public void SendName()
    {
        //서버에 넘기기 (playerName)
    }

    public void Setting()
    {
        UiManager.Instance.ShowUI(panel, 0, 0);
        UiManager.Instance.UnShowUI(setBtn, -10, setBtn.position.y);
    }

    public void ClosePanel()
    {
        UiManager.Instance.ShowUI(setBtn, saveSetBtn.position.x, saveSetBtn.position.y);
        UiManager.Instance.UnShowUI(panel, 0, 10);
    }
}
