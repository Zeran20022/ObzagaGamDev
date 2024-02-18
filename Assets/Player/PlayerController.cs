using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        // �������� ���� � ����������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // �������� ����������� ������� ������
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // ������������� y � 0, ����� ������������ ������ ������

        // ����������� �����������, ����� �������� ��������� �������� ��� �������� �� ���������
        cameraForward.Normalize();

        // ��������� ����������� �������� �� ������ ����� � ����������� ������
        Vector3 moveDirection = cameraForward * verticalInput + cameraTransform.right * horizontalInput;

        // ���������, ���� �� ������� ��������
        if (moveDirection != Vector3.zero)
        {
            // ������������ �������� � ����������� ��������
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // ������� ���������
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
