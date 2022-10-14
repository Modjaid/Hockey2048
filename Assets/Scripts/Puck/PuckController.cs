using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PuckController : MonoBehaviour
{

    [SerializeField] public int point;

    public int Id;

    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private Collider collider;
    [SerializeField] private Collider eaterCollider;
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private TextMeshProUGUI testUI_id;
    [SerializeField] private AudioSource audioMerge;
    [SerializeField] private AudioSource audioPush;
    [SerializeField] private Animation ownAnim;


    private Coroutine ColorChangeThread;


    private void Start()
    {
        Id = UnityEngine.Random.Range(1, 999999);
        pointText.text = point.ToString();

        testUI_id.text = Id.ToString();
    }

    public void ChangeIdWithDelay(float timer)
    {
        Id = -1;
        testUI_id.text = Id.ToString();
        StartCoroutine(delay());
        IEnumerator delay()
        {
            yield return new WaitForSeconds(timer);
            Id = UnityEngine.Random.Range(0, 999999);
            testUI_id.text = Id.ToString();
        }
    }


    public void MergePuck(PuckController targetPuck,float mergeSpeed)
    {
        targetPuck.point *= 2;
        this.transform.parent = null;
        eaterCollider.enabled = false;
        collider.enabled = false;
        rigidBody.isKinematic = true;
        // rigidBody.velocity = Vector3.zero;
        audioMerge.Play();
        StartCoroutine(TranslateToTarget());
        
        IEnumerator TranslateToTarget()
        {
            yield return new WaitForSeconds(0.2f);
            var distance = Vector3.Distance(transform.position, targetPuck.transform.position);
            while (distance > 0.5f)
            {
                transform.position = Vector3.Lerp(this.transform.position, targetPuck.transform.position, Time.deltaTime * mergeSpeed);
                distance = Vector3.Distance(transform.position, targetPuck.transform.position);
                yield return null;
            }
            targetPuck.pointText.text = targetPuck.point.ToString();
            targetPuck.PlayInflatingAnim();
            Destroy(this.gameObject);
        }
    }

    public void PushPuck()
    {
        Vector3 randomVector = UnityEngine.Random.insideUnitSphere;
        rigidBody.AddForce(randomVector, ForceMode.Impulse);
    }

    public void SetSuitColor(Color suitColor)
    {
         if(ColorChangeThread != null)
         {
             StopCoroutine(ColorChangeThread);
         }
        ColorChangeThread = StartCoroutine(ColorLerp());

        IEnumerator ColorLerp()
        {
            float maxColor = (meshRenderer.material.color - suitColor).maxColorComponent;
            while (maxColor > 0.01f)
            {
                meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, suitColor, 1.5f * Time.deltaTime);
                maxColor = (meshRenderer.material.color - suitColor).maxColorComponent;
                yield return null;
            }
            // Debug.Log("Finish change Color");
         //   ColorChangeThread = null;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        audioPush.pitch = UnityEngine.Random.Range(.5f, 1f);
        audioPush.Play();
    }

    /// <summary>
    /// ВКЛ/ВЫКЛ звуки и коллайдер для работы предсказания траектории
    /// </summary>
    /// <param name="isActive"></param>
    public void SwitchObjectActive(bool isActive)
    {
        audioMerge.mute = !isActive;
        audioPush.mute = !isActive;
        eaterCollider.enabled = isActive;
    }

    public void PlayInflatingAnim()
    {
        ownAnim.Play("Inflate");
    }

}
