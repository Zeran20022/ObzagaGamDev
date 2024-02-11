using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Скорость перемещения персонажа

    private Rigidbody playerRigidbody; // Ссылка на компонент Rigidbody

    void Start()
    {
        // Получаем компонент Rigidbody при запуске
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Получаем ввод от игрока по осям горизонтали и вертикали
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Вычисляем вектор направления движения
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Применяем силу к Rigidbody для перемещения персонажа
        playerRigidbody.AddForce(movement * speed);
    }
}

