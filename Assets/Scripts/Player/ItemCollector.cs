using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    public static int apples;
    public static float timer;

    [SerializeField] private TextMeshProUGUI applesText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private AudioSource collectSoundEffect;

    private void Start()
    {
        apples = 0;
        timer = 0.0f;
    }

    private void Update()
    {
        timer += 1 * Time.deltaTime;
        timerText.text = "Time: " + timer.ToString("0");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Apple"))
        {
            collectSoundEffect.Play();
            Destroy(collision.gameObject);
            apples++;
            applesText.text = "Apples: " + apples;
        }
    }
}
