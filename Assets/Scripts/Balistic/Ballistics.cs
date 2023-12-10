using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballistics : MonoBehaviour
{

    public Transform SpawnTransform;
    public Transform TargetTransform;

    public float AngleInDegrees;

    float g = Physics.gravity.y;

    public GameObject[] Bullets; // Массив объектов для броска

    private GameObject currentBullet; // Текущий шарик, готовящийся к броску
    private bool canPrepareShot = true; // Флаг, позволяющий подготовить новый объект для броска

    void Update()
    {
        SpawnTransform.localEulerAngles = new Vector3(-AngleInDegrees, 0f, 0f);

        if (Input.GetMouseButtonDown(0) && canPrepareShot && !IsPointerOverNoTrackZone())
        {
            PrepareShot();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Shot();
        }
    }

    // Метод для включения/выключения подготовки броска
    public void SetCanPrepareShot(bool canPrepare)
    {
        canPrepareShot = canPrepare;
    }

    // Метод для подготовки шарика к броску
    private void PrepareShot()
    {
        if (Bullets.Length == 0) return;

        GameObject selectedBullet = Bullets[Random.Range(0, Bullets.Length)];
        currentBullet = Instantiate(selectedBullet, SpawnTransform.position, Quaternion.identity);
        Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Отключаем физику
        }
    }

    public void Shot()
    {
        if (currentBullet == null) return;

        Rigidbody2D rb = currentBullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = false; // Включаем физику

            Vector3 fromTo = TargetTransform.position - transform.position;
            Vector3 fromToXZ = new Vector3(fromTo.x, 0f, fromTo.z);

            transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);

            float x = fromToXZ.magnitude;
            float y = fromTo.y;

            float AngleInRadians = AngleInDegrees * Mathf.PI / 180;

            float v2 = (g * x * x) / (2 * (y - Mathf.Tan(AngleInRadians) * x) * Mathf.Pow(Mathf.Cos(AngleInRadians), 2));
            float v = Mathf.Sqrt(Mathf.Abs(v2));

            rb.velocity = ((SpawnTransform.forward * v) * 3.1f);
        }

        currentBullet = null; // Сброс текущего шарика
    }
    private bool IsPointerOverNoTrackZone()
    {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("NoTrack"))
        {
            return true;
        }

        return false;
    }
}
