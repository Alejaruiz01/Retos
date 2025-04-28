using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrow : MonoBehaviour
{
    public Vector3 bigSize = new Vector3(2f, 2f, 2f); // tamaño grande
    public Vector3 normalSize = new Vector3(1f, 1f, 1f); // tamaño normal
    public float growDuration = 20f; // cuánto dura agrandado

    private bool isBig = false;
    private Coroutine growCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mushroom"))
        {
            if (growCoroutine != null)
            {
                StopCoroutine(growCoroutine);
            }
            growCoroutine = StartCoroutine(GrowTemporarily());
            Destroy(other.gameObject); // eliminar el hongo después de tocarlo
        }
    }

    private IEnumerator GrowTemporarily()
    {
        transform.localScale = bigSize;
        isBig = true;

        yield return new WaitForSeconds(growDuration);

        transform.localScale = normalSize;
        isBig = false;
    }
}
