using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Transform tf = transform;
        speed = Mathf.Max(speed, 0.01f);
        tf.DORotate(new Vector3(0f, 360f, 0f), 360f / speed, RotateMode.LocalAxisAdd)
                 .SetEase(Ease.Linear)
                 .SetLoops(-1);
    }
}
