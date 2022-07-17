using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class HealthUIComponent : MonoBehaviour
    {
        HealthComponent healthComponent;
        List<GameObject> healthBar = new List<GameObject>();

        public GameObject healthPrefab;

        void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
        }

        // Use this for initialization
        void Start()
        {
            GameObject UILayer = GameObject.FindGameObjectWithTag("UI");

            for (int i =0; i < healthComponent.health; ++i)
            {
                GameObject newHealth = Instantiate(healthPrefab, UILayer.transform);
                Vector2 UIPosition = new Vector2(70 + ((float)i * (125.0f + 15.0f)), -170);
                RectTransform UITransform = newHealth.GetComponent<RectTransform>();
                UITransform.anchoredPosition = UIPosition;
                healthBar.Add(newHealth);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (healthBar.Count > healthComponent.health && healthBar.Count > 0)
            {
                GameObject lastHealth = healthBar[healthBar.Count - 1];
                healthBar.RemoveAt(healthBar.Count - 1);
                Destroy(lastHealth);
            }
        }
    }
}