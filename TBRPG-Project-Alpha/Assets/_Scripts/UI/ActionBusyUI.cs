using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{

    private void Start()
    {
        gameObject.SetActive(false);
        UnitActionSystem.Instance.OnBusyChanged += Instance_OnBusyChanged;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="isBusy"></param>
    private void Instance_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
