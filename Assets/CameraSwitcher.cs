using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase _tpsCam;
    [SerializeField] CinemachineVirtualCameraBase _fpsCam;
    CinemachineBrain _brain;

    void Start()
    {
        _brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public void OnSwitchCamera(InputValue value)
    {
        if (value.isPressed)
        {
            _fpsCam.MoveToTopOfPrioritySubqueue();
            StartCoroutine(LookForward());
        }
        else
        {
            _tpsCam.MoveToTopOfPrioritySubqueue();
        }
    }

    /// <summary>
    /// カメラをFPSに切り替える時に呼ぶコルーチン
    /// カメラが撮影している方向を向く
    /// </summary>
    /// <returns></returns>
    IEnumerator LookForward()
    {
        float duration;

        if (_brain.ActiveBlend != null)
            duration = _brain.ActiveBlend.Duration;
        else
            duration = _brain.m_DefaultBlend.BlendTime;

        var dir = Camera.main.transform.forward;
        dir.y = 0;
        var targetRotation = Quaternion.LookRotation(dir);

        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / (duration * 0.3f));   // duration の3割の時間で方向を変える
            yield return new WaitForEndOfFrame();
        }
    }
}
