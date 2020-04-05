using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contro : MonoBehaviour
{
    public Camera[] _cameras;
  public AnimationCurve _fov;
    public AnimationCurve _timeScale;
   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(SwapAsync());
        }
    }
    

    IEnumerator SwapAsync()
    {

        for (float t = 0; t < 1.0f; t += Time.unscaledDeltaTime * 1.2f)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].fieldOfView = _fov.Evaluate(t);
            }
            Time.timeScale = _timeScale.Evaluate(t);
            yield return null;
        }

        for (int i = 0; i < _cameras.Length; i++)
        {
            _cameras[i].fieldOfView = _fov.Evaluate(1.0f);
        }


        Time.timeScale = 1.0f;

    }

}