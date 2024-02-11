using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // �������� ����������� ���������

    private Rigidbody playerRigidbody; // ������ �� ��������� Rigidbody

    void Start()
    {
        // �������� ��������� Rigidbody ��� �������
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // �������� ���� �� ������ �� ���� ����������� � ���������
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // ��������� ������ ����������� ��������
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // ��������� ���� � Rigidbody ��� ����������� ���������
        playerRigidbody.AddForce(movement * speed);
    }
}

