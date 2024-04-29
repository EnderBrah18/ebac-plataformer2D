using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class ItemCollectableBase : MonoBehaviour
{
    public string compareTag = "Player";
    public ParticleSystem particlesSystem;
    public GameObject graphicItem;
    public float timeToHide = 3;

    [Header("Sounds")]
    public AudioSource audioSource;


    private void Awake()
    {
        //if (particlesSystem != null) particlesSystem.transform.SetParent(null);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            
            Collect();
        }
    }

    protected virtual void Collect()
    {
        if (graphicItem != null)
        {
            graphicItem.SetActive(false);
        }
        Invoke("HideObject", timeToHide);
        OnCollect();
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void OnCollect()
    {
        if (particlesSystem != null) particlesSystem.Play();
        if (audioSource != null) audioSource.Play();

    }

}
