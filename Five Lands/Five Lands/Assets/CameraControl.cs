using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class CameraControl : CinemachineExtension
{
    private CinemachineFreeLook _vCam;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        _vCam = (CinemachineFreeLook) VirtualCamera;
    }
    private void Update()
    {
        AdjustRotation(Input.GetAxisRaw("Rotation"));
    }

   
    private void AdjustRotation (float delta)
    {
        _vCam.m_XAxis.Value += delta;
    }
}
