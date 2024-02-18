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
        // Получаем ввод с клавиатуры
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Получаем направление взгляда камеры
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f; // Устанавливаем y в 0, чтобы игнорировать наклон камеры

        // Нормализуем направление, чтобы избежать изменения скорости при движении по диагонали
        cameraForward.Normalize();

        // Вычисляем направление движения на основе ввода и направления камеры
        Vector3 moveDirection = cameraForward * verticalInput + cameraTransform.right * horizontalInput;

        // Проверяем, было ли введено движение
        if (moveDirection != Vector3.zero)
        {
            // Поворачиваем персонаж в направлении движения
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // Двигаем персонажа
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
