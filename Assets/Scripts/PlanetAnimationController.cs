using System.Collections;
using UnityEngine;

public class PlanetAnimationController : MonoBehaviour
{
    private Animator animator;
    private float timer;
    private bool isInitialAnimation = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRandomInitialAnimation();
    }

    private void SetRandomInitialAnimation()
    {
        int initialAnimationIndex = Random.Range(0, 2); // Рандом между 0 и 1
        animator.SetInteger("AnimationIndex", initialAnimationIndex);
        timer = 10.0f; // Таймер на 3 секунды
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                if (isInitialAnimation)
                {
                    // Выбираем между Wink (2), Astonishment (3), Angry (4)
                    int nextAnimationIndex = Random.Range(2, 5);
                    animator.SetInteger("AnimationIndex", nextAnimationIndex);
                    isInitialAnimation = false; // Переключаем флаг
                    timer = 10.0f; // Сброс таймера для следующей анимации
                }
                else
                {
                    SetRandomInitialAnimation(); // Возвращаемся к Smile или Sadness
                    isInitialAnimation = true; // Сброс флага
                }
            }
        }
    }
}
