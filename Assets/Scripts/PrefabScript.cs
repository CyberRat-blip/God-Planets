using UnityEngine;
using System.Collections;

[System.Serializable]
public class PlanetData
{
    public int prefabIndex;
    public Vector3 position;

    public PlanetData(int index, Vector3 pos)
    {
        prefabIndex = index;
        position = pos;
    }
}

public enum PrefabState
{
    Ready,
    Collision,
    Bounciness
}

public class PrefabScript : MonoBehaviour
{
    public int prefabIndex;
    private float creationTime;
    public PrefabState prefabState = PrefabState.Ready;
    private Rigidbody2D rb;
//<<<<<<< HEAD
    public Vector3 originalScale;
    public PhysicsMaterial2D newMaterial;
    public bool isInSafeZone = false;
//=======
    //public Vector3 originalScale; // Оригинальный scale
    //public PhysicsMaterial2D newMaterial; // Новый физический материал
//>>>>>>> parent of b0a21ad (Ads Update v.0.3)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        creationTime = Time.time;
        originalScale = transform.localScale;
        SetState(PrefabState.Ready);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
        HandleCollisionWithOtherPrefab(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bounciness"))
        {
            ApplyNewMaterial();
        }
        else if (collision.gameObject.CompareTag("GameOver"))
        {
            GameManager.Instance.EndGame();
            GameDataController.ClearGameData();
        }
        else if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("PrefabTag"))
        {
            SetState(PrefabState.Collision);
            StopRotation();
        }
    }

    private void HandleCollisionWithOtherPrefab(Collision2D collision)
    {
        PrefabScript otherPrefab = collision.gameObject.GetComponent<PrefabScript>();
        if (otherPrefab != null && otherPrefab.prefabIndex == this.prefabIndex)
        {
            if (this.creationTime < otherPrefab.creationTime)
            {
                StartCoroutine(CreateAndDestroySequence(otherPrefab));
            }
        }
    }

    private IEnumerator CreateAndDestroySequence(PrefabScript otherPrefab)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject newPrefab = PrefabManager.Instance.SpawnNextPrefab(transform.position, prefabIndex + 1);
        if (newPrefab != null)
        {
            InitializeNewPrefab(newPrefab);
        }

        Destroy(otherPrefab.gameObject);
        Destroy(gameObject);
    }

    private void InitializeNewPrefab(GameObject newPrefab)
    {
        var newPrefabScript = newPrefab.GetComponent<PrefabScript>();
        if (newPrefabScript != null)
        {
            newPrefabScript.SetInitialScale();
            newPrefabScript.StartScaleAnimation(0.5f, newPrefabScript.originalScale, 0.05f);

            int pointsToAdd = PrefabManager.Instance.pointsForPrefabs[prefabIndex];
            ScoreManager.Instance.AddScore(pointsToAdd);
        }

        AudioSource audioSource = newPrefab.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    private void ApplyNewMaterial()
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null && newMaterial != null)
        {
            collider.sharedMaterial = newMaterial;
        }
    }

    public void StartScaleAnimation(float startScaleFactor, Vector3 endScale, float duration)
    {
        StartCoroutine(ScaleOverTime(startScaleFactor, endScale, duration));
    }

    private IEnumerator ScaleOverTime(float startScaleFactor, Vector3 endScale, float duration)
    {
        Vector3 startScaleVec = new Vector3(originalScale.x * startScaleFactor, originalScale.y * startScaleFactor, originalScale.z * startScaleFactor);
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            transform.localScale = Vector3.Lerp(startScaleVec, endScale, t);
            yield return null;
        }
        transform.localScale = endScale;
    }

//<<<<<<< HEAD
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NonGameOver"))
        {
            isInSafeZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NonGameOver"))
        {
            isInSafeZone = false;
        }
    }

//=======
//>>>>>>> parent of b0a21ad (Ads Update v.0.3)
    public void SetInitialScale()
    {
        transform.localScale = originalScale;
    }
//<<<<<<< HEAD

    public bool IsInSafeZone()
    {
        return isInSafeZone;
    }
//=======
//>>>>>>> parent of b0a21ad (Ads Update v.0.3)

    private void StopRotation()
    {
        // Добавьте здесь логику, если необходимо остановить вращение
    }

    public void SetState(PrefabState newState)
    {
        prefabState = newState;
        switch (newState)
        {
            case PrefabState.Ready:
                rb.mass = 1f;
                rb.gravityScale = 10f;
                rb.drag = 0f;
                break;
            case PrefabState.Collision:
                rb.mass = 1f;
                rb.gravityScale = 2f;
                rb.drag = 2f;
                break;
        }
    }
}
