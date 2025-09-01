using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveAndRotateObjectInteractable : Interactable
{

    [Header("Move Or Rotate")]
    [SerializeField] bool moveObj = false;
    [SerializeField] bool rotateObj = false;

    [Header("Move Settings")]
    [SerializeField] bool hasMoved = false;
    [SerializeField] Vector3 defaultPos;
    [SerializeField] Vector3 movPos;
    private Transform moveobjTransform;

    protected override void Start()
    {
        moveobjTransform = transform;
        defaultPos = transform.localPosition;
        
    }

    public override void Interact(playerManager player)
    {
        if (moveObj)
        {
            if (!hasMoved)
            {
                StartCoroutine(MoveToPosition(movPos, 0.5f));
                hasMoved = true;
            }
            else
            {
                StartCoroutine(MoveToPosition(defaultPos, 0.5f));
                hasMoved = false;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.localPosition = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localPosition = target;
    }


}
