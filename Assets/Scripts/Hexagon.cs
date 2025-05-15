using DG.Tweening;
using UnityEngine;

/*public void MoveToLocal(Vector3 targetLocalPosition)
    {
        gameObject.transform.DOKill();

        float delay=transform.GetSiblingIndex() * 0.05f;

        transform.DOLocalMove(targetLocalPosition, 0.3f)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay);

        Vector3 direction = (targetLocalPosition - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(direction, Vector3.up).normalized;

        transform.DOLocalRotateQuaternion(
            Quaternion.AngleAxis(180f, rotationAxis) * transform.localRotation,
            0.2f)
            .SetEase(Ease.InOutSine)
            .SetDelay(delay);


    }

    public void Vanish(float delay)
    {
        gameObject.transform.DOKill();

        gameObject.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .SetDelay(delay)
            .OnComplete(() => Destroy(gameObject));


    }
}*/
public class Hexagon : MonoBehaviour
{
    [Header(" Elements ")] [SerializeField]
    private new Renderer renderer;

    [SerializeField] private new Collider collider;
    public HexStack HexStack { get; private set; }

    public Color Color
    {
        get => renderer.material.color;
        set => renderer.material.color = value;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    public void Configure(HexStack hexStack)
    {
        HexStack = hexStack;
    }

    public void SetParrent(Transform parrent)
    {
        transform.SetParent(parrent);
    }

    
    public void MoveToLocal(Vector3 targetLocalPosition, float duration = 0.3f, float delay = -1f,
        Ease ease = Ease.InOutSine, System.Action onComplete = null)
    {
        gameObject.transform.DOKill();

        if (delay < 0f)
            delay = transform.GetSiblingIndex() * 0.05f;

        var moveTween = transform.DOLocalMove(targetLocalPosition, duration)
            .SetEase(ease)
            .SetDelay(delay);

        Vector3 direction = (targetLocalPosition - transform.localPosition).With(y: 0).normalized;
        Vector3 rotationAxis = Vector3.Cross(direction, Vector3.up).normalized;

        var rotateTween = transform.DOLocalRotateQuaternion(
                Quaternion.AngleAxis(180f, rotationAxis) * transform.localRotation,
                Mathf.Min(duration, 0.2f))
            .SetEase(ease)
            .SetDelay(delay);

        if (onComplete != null)
            moveTween.OnComplete(() => onComplete());

        // Collider animasyon sırasında devre dışı bırakılabilir
        DisableCollider();
    }

    public void Vanish(float delay, bool withFade = false, System.Action onComplete = null)
    {
        gameObject.transform.DOKill();

        var scaleTween = gameObject.transform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.InBack)
            .SetDelay(delay);

        if (withFade && renderer != null)
        {
            var mat = renderer.material;
            mat.DOFade(0f, 0.2f).SetDelay(delay);
        }

        scaleTween.OnComplete(() =>
        {
            onComplete?.Invoke();
            Destroy(gameObject);
        });

        DisableCollider();
    }
    
    public void FadeOut(float duration, float delay)
    {
        var renderer = this.GetComponent<Renderer>();
        if (renderer == null) renderer = this.GetComponentInChildren<Renderer>();
        if (renderer == null) return;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(block);
        float startAlpha = block.GetFloat("_Alpha");
        if (startAlpha == 0) startAlpha = 1f;

        DOTween.To(() => startAlpha, x => {
                block.SetFloat("_Alpha", x);
                renderer.SetPropertyBlock(block);
            }, 0f, duration)
            .SetDelay(delay);
    }
    
    public void PrintNeighbours()
    {
        StackController controller = FindObjectOfType<StackController>();
        var left = controller.GetLeftNeighbour(this.HexStack);
        var right = controller.GetRightNeighbour(this.HexStack);

        var leftTop = left?.GetTopHexagon();
        var rightTop = right?.GetTopHexagon();

        Debug.Log($"{name} - Sol komşu Stack: {left?.name}, Üst Hex: {leftTop?.name}");
        Debug.Log($"{name} - Sağ komşu Stack: {right?.name}, Üst Hex: {rightTop?.name}");
    }
}
