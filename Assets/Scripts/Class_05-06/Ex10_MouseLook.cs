using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ex10_MouseLook : MonoBehaviour
{
    public Transform turret;


    [Header("Mouse look")]
    public float mouseSensitivity = 1;
    public float turretYawSensitivity = 6;

    [Header("Movement")]
    public float playerAccMagnitude = 400; // meters per second^2
    public float drag = 1.6f;

    // internal state
    float pitchDeg;
    float yawDeg;
    float turretYawOffsetDeg;

    void Awake()
    {
        Vector3 startEuler = transform.eulerAngles;
        pitchDeg = startEuler.x;
        yawDeg = startEuler.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        UpdateMouseLook();
        UpdateTurretYawInput();
        PlaceTurret();
    }



    void UpdateTurretYawInput()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        turretYawOffsetDeg += scrollDelta * turretYawSensitivity;
        turretYawOffsetDeg = Mathf.Clamp(turretYawOffsetDeg, -90, 90);
    }

    void UpdateMouseLook()
    {
        if (Cursor.lockState == CursorLockMode.None)
            return;
        float xDelta = Input.GetAxis("Mouse X");//velociddade do mouse em X
        float yDelta = Input.GetAxis("Mouse Y");//velocidade do mouse em Y
        pitchDeg += -yDelta * mouseSensitivity;//Vertical mouse movemente rotaciona ao longo do eixo X (Pitch), o sinal de menos � para inverter a dire��o
        pitchDeg = Mathf.Clamp(pitchDeg, -90, 90);//Definindo limites
        yawDeg += xDelta * mouseSensitivity;//Horizontal mouse movemente rotaciona ao longo do eixo Y (Yaw)
        transform.rotation = Quaternion.Euler(pitchDeg, yawDeg, 0);//Fazendo rota��o em si
    }

    void PlaceTurret()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            turret.position = hit.point;
            Vector3 yAxis = hit.normal;
            Vector3 zAxis = Vector3.Cross(transform.right, yAxis).normalized;
            Debug.DrawLine(ray.origin, hit.point, new Color(1, 1, 1, 0.4f));
            Quaternion offsetRot = Quaternion.Euler(0, turretYawOffsetDeg, 0);

            //Primeiro faz a rota��o Quaternion.LookRotation(zAxis, yAxis) e depois offsetRot, a ordem aqui � importante, precisa ser no local space no caso
            turret.rotation = Quaternion.LookRotation(zAxis, yAxis) * offsetRot;
        }
    }
}
