using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FireBall.OnAnyFireBallExploded += FireBall_OnAnyFireBallExploded; 
    }

    private void FireBall_OnAnyFireBallExploded(object sender, System.EventArgs e)
    {
        ScreenShake.Instance.Shake(5f);
    }
}
