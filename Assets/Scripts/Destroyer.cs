using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private GameObject madeWall;
    [SerializeField] private Animation gateAnimation;
    [SerializeField] private GameObject goalEffect;
    [SerializeField] private AudioSource audioGoal;

    public void OnTriggerEnter(Collider other)
    {
        PuckController collisionPuck = other.gameObject.GetComponent<PuckController>();
        if (collisionPuck)
        {
            Destroy(other.gameObject);
            audioGoal.Play();
            goalEffect.SetActive(true);
            StartCoroutine(WaitDestroy());
            StartCoroutine(AnimationPlaying());
        }
    }


    private IEnumerator AnimationPlaying()
    {
        gateAnimation.Play("Down");

        yield return new WaitUntil(() => !gateAnimation.IsPlaying("Down"));
        madeWall.transform.parent = null;
        transform.parent.gameObject.SetActive(false);
    }
    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(.1f);
        FieldManager.OnPuckChanged?.Invoke();
    }
}

