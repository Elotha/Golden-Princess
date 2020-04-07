using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraEffect
{
    private static Camera _cam;
    private static bool CamShaking = false;

    public static IEnumerator ShakingEffect(int faceDir)
    {
        if (!CamShaking) {
            Vector3 dir = new Vector3(Mathf.Cos(Mathf.Deg2Rad * faceDir * 90),Mathf.Sin(Mathf.Deg2Rad * faceDir * 90));
            _cam = Camera.main;
            CamShaking = true;
            _cam.transform.position += (Vector3) dir / 10f;
            yield return new WaitForSeconds(0.1f);
            _cam.transform.position -= (Vector3) dir * 2 / 10f;;
            yield return new WaitForSeconds(0.1f);
            _cam.transform.position += (Vector3) dir / 10f;;
            CamShaking = false;
        }

    }
}
