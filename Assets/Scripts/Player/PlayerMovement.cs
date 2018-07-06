using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    public float speed = 6f; //Швидкість, яку буде переміщати гравець.
    Vector3 movement; //Вектор, щоб зберегти напрям руху гравця.
    Animator anim; //Посилання на компонент аніматора.
    Rigidbody playerRigidbody; //Посилання на жорстке тіло гравця.
    int floorMask; //Маска шару так, щоб промінь можна було викинути саме на ігровіоб'єкти на рівні підлоги.
    public float camRayLength = 100f; //Довжина променя з камери на сцену

    void Awake()
    {
        //Створіть шар маски для шару підлоги.
        floorMask = LayerMask.GetMask("Floor");
        // Налаштування посилань.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //Зберігайте вхідні осі.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //еремістіть гравця навколо сцени.
        Move(h, v);

        //  Поверніть гравця до курсора миші.
        Turning();

        //Анімація гравця.
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        // Встановіть вектор руху на вході в осі.
        movement.Set(h, 0f, v);

        // Нормуруйте вектор руху і зробити його пропорційним швидкості в секунду.
        movement = movement.normalized * speed * Time.deltaTime;

        // Перемістіть гравця до поточної позиції плюс рух.
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        // Створіть промінь з курсора миші на екрані в напрямку камери.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Створіть змінну RaycastHit, щоб зберігати інформацію про те, що було зіткнуто променем.
        RaycastHit floorHit;

        // Виконайте raycast, і якщо він ударяє щось на шарі підлоги ...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Створіть вектор з гравця до точки на підлозі, щоб відбити райкаст від миші.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Переконайтеся, що вектор повністю розташований вздовж площини поверху.
            playerToMouse.y = 0f;

            // Створіть кватерніон (обертання) на основі перегляду вектора від гравця до миші.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Встановіть поворот гравця до цього нового повороту.
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        // Створіть логічне значення, яке вірно, якщо будь-яка з вхідних осей не º нулем.
        bool walking = h != 0f || v != 0f;

        // Скажіть аніматору, чи йде програвач.
        anim.SetBool("IsWalking", walking);
    }
}