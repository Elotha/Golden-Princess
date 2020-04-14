using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusMovement : MonoBehaviour
{
    private Vector3 vSin;
    private float fTime = 0f;
    [SerializeField] private float CurveSpeed = 450f;
    [SerializeField] private float verSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        fTime = (fTime + Time.deltaTime * CurveSpeed) % 360f;
        vSin = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * fTime), 0);
        transform.position += vSin * Time.deltaTime * verSpeed;
    }
}
