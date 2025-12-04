using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Sprite fullHeart; 
    public Sprite emptyHeart;

    public GameObject heartPrefab; 
    public int heartSize = 50;

    private List<Image> hearts = new List<Image>();

    public void UpdateHearts(int currentHealth, int maxHealth)
    {
        // Si el número de corazones cambió, reconstruir UI
        if (hearts.Count != maxHealth)
        {
            RebuildHearts(maxHealth);
        }

        // Mostrar corazones llenos o vacíos
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = (i < currentHealth) ? fullHeart : emptyHeart;
        }
    }

    void RebuildHearts(int maxHealth)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        hearts.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject h = Instantiate(heartPrefab, transform);
            Image img = h.GetComponent<Image>();
            RectTransform rt = h.GetComponent<RectTransform>();

            rt.sizeDelta = new Vector2(heartSize, heartSize);

            hearts.Add(img);
        }
    }
}