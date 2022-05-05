using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShaker : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    private bool isMyMove;
    private Vector3 startingPos;

    public float speed;
    public float amount;

    private void Awake()
    {
        startingPos.x = transform.localPosition.x;
        startingPos.y = transform.localPosition.y;
        startingPos.z = 0f;
    }
    
    private void FixedUpdate()
    {
        if (TurnControllerOnline.Instance.GetCurrentPlayer() == player)
            isMyMove = true;
        else
        {
            if(isMyMove)
                StartCoroutine(LerpPosition(new Vector3(startingPos.x, startingPos.y, 0), .4f));

            isMyMove = false;
        }
    }

    private void Update()
    {
        if (isMyMove && GameControllerOnline.Instance.GameInProgress)
        {
            transform.localPosition = new Vector3(
                startingPos.x + (Mathf.Sin(Time.time * speed) * amount),
                startingPos.y + (Mathf.Sin(Time.time * speed) * amount),
                0f
                );
        }

    }
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.localPosition;

        while (time < duration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = targetPosition;
    }
}
