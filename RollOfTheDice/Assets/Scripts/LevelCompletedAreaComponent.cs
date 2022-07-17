using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class LevelCompletedAreaComponent : MonoBehaviour
    {
        GameFlow flow;

        // Use this for initialization
        void Start()
        {
            GameObject gameLogic = GameObject.FindGameObjectWithTag("GameController");
            flow = gameLogic.GetComponent<GameFlow>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                flow.OnLevelCompleted();
            }
        }
    }
}