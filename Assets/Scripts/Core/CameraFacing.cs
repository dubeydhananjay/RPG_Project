using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        private Cinemachine.CinemachineVirtualCamera cinemachineVirtualCamera;
        private void Start()
        {
            cinemachineVirtualCamera = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        }
        private void LateUpdate()
        {
            transform.forward = cinemachineVirtualCamera.transform.forward;
        }
    }
}
