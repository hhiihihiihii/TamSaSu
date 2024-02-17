using UnityEngine;
using DisgendPattern;
using DG.Tweening;

public class UiManager : SingleTon<UiManager>
{
    public void ShowUI(Transform trm, float x, float y)
    {
        trm.DOMove(new Vector3(x, y, 0), 1);
    }

    public void UnShowUI(Transform trm, float x, float y)
    {
        trm.DOMove(new Vector3(x, y, 0), 1);
    }
}
