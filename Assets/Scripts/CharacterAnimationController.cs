using Spine;
using Spine.Unity;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    public static CharacterAnimationController Instance { get; private set; }
    private bool returnToIdle = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.state.Complete += OnAnimationComplete;
        SetIdleAnimation();
    }

    public void SetIdleAnimation()
    {
        skeletonAnimation.AnimationName = "Idle";
    }

    public void SetHoldPlanetAnimation()
    {
        skeletonAnimation.AnimationName = "Hold_Planet";
        returnToIdle = false; // Не возвращаемся к Idle после этой анимации
    }

    public void SetThrowAnimation()
    {
        PlayAnimationOnce("Throw");
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    public void SetSwipeLeftAnimation()
    {
        PlayAnimationOnce("Hands_Left");
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    public void SetSwipeRightAnimation()
    {
        PlayAnimationOnce("Hands_Right");
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    public void SetSwipeUpAnimation()
    {
        PlayAnimationOnce("Hands_Up");
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    public void SetRandomSmileAnimation()
    {
        string[] smiles = { "Smile1", "Smile2" };
        int randomIndex = Random.Range(0, smiles.Length);
        PlayAnimationOnce(smiles[randomIndex]);
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    public void PlayAnimationOnce(string animationName)
    {
        skeletonAnimation.state.SetAnimation(0, animationName, false);
        returnToIdle = true; // Возвращаемся к Idle после этой анимации
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (returnToIdle && trackEntry.Animation.Name != "Idle")
        {
            SetIdleAnimation();
        }
    }
}
