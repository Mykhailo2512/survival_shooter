using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Позиція, яку буде виконувати ця камера.
    public float smoothing = 5f; // Швидкість, з якою камера буде слідувати.

    Vector3 offset; // Початкове зміщення від цілі.

    void Start()
    {
        // Обчислити початкове зміщення.
        offset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        // Створіть позицію, яку камера націлює на основі зміщення від цілі.
        Vector3 targetCamPos = target.position + offset;

        // Плавно інтерполюйте поточну позицію камери та цільову позицію.
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}