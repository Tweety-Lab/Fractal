using UnityEngine;
using System.Collections;

public class screen_shake_master : MonoBehaviour
{
    public AnimationCurve curve;
    public float duration = 1f;
    public void ShakeScreen(float dur) 
    {
        duration = dur;
        StartCoroutine(Shaking());
    }
    IEnumerator Shaking()
    {
        Vector3 startPos = transform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.localPosition = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }
        transform.localPosition = startPos;
    }
}