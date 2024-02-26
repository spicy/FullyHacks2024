using System.Collections;
using UnityEngine;

public class DeactivateAfterTime : MonoBehaviour
{
    private float lifetime = 10f;

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterTimePassed());
    }

    private IEnumerator DeactivateAfterTimePassed()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }
}