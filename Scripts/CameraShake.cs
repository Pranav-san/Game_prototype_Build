using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsedtime = 0;

        while(elapsedtime < duration)
        {
            float x = Random.Range(-1,1) * magnitude;
            float y = Random.Range(-1,1) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsedtime += Time.deltaTime;

            yield return null;


        }

        transform.localPosition = originalPos;
    }
}
