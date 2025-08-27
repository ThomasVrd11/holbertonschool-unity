using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera blendCamera;
    public Camera camA;
    public Camera camB;

    public float blendDuration = 1f;

    private float blendTime = 0f;
    private bool isBlending = false;
    private Transform from;
    private Transform to;
    private Camera targetCam;

    public void SwitchCam(bool top)
    {
        if (top)
        {
            BlendTo(camB);
        }
        else
        {
            BlendTo(camA);
        }
    }
    private void BlendTo(Camera destination)
    {
        from = blendCamera.transform;
        to = destination.transform;
        targetCam = destination;

        Camera currentCam = camA.enabled ? camA : camB;
        blendCamera.transform.position = currentCam.transform.position;
        blendCamera.transform.rotation = currentCam.transform.rotation;
        blendCamera.fieldOfView = currentCam.fieldOfView;

        camA.enabled = false;
        camB.enabled = false;
        blendCamera.enabled = true;

        blendTime = 0f;
        isBlending = true;
    }

    void Update()
    {
        if (!isBlending) return;

        blendTime += Time.deltaTime;
        float t = Mathf.Clamp01(blendTime / blendDuration);

        blendCamera.transform.position = Vector3.Lerp(from.position, to.position, t);
        blendCamera.transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);

        if (t >= 1f)
        {
            targetCam.transform.position = blendCamera.transform.position;
            targetCam.transform.rotation = blendCamera.transform.rotation;
            targetCam.enabled = true;
            blendCamera.enabled = false;

            isBlending = false;
        }
    }
}
