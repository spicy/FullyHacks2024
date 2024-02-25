using UnityEngine;
using System.Collections;

public class ExposionBehavior : MonoBehaviour
{
    public float maxRadius = 5f;
    public float minRadius = 1f;
    public float expandTime = 2f;
    public float shrinkTime = 2f;

    private Vector3 initialScale;
    private Coroutine expandCoroutine;

    private void Start()
    {
        initialScale = transform.localScale;
        expandCoroutine = StartCoroutine(Expand());
    }

    private IEnumerator Expand()
    {
        float timer = 0f;
        while (timer < expandTime)
        {
            float scaleFactor = Mathf.Lerp(0f, maxRadius, timer / expandTime);
            transform.localScale = initialScale * scaleFactor;
            timer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale * maxRadius;

        yield return new WaitForSeconds(1f); 

        float shrinkTimer = 0f;
        while (shrinkTimer < shrinkTime)
        {
            float scaleFactor = Mathf.Lerp(maxRadius, minRadius, shrinkTimer / shrinkTime);
            transform.localScale = initialScale * scaleFactor;
            shrinkTimer += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale * minRadius;

        gameObject.SetActive(false);
    }
}