using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DartHazard : MonoBehaviour
{
    public Transform dartSpawnPoint;
    public GameObject dartPrefab;
    public DartValues dartValues;

    public void Shoot()
    {
        Dart d = Instantiate(dartPrefab, dartSpawnPoint.position, dartSpawnPoint.rotation).AddComponent<Dart>();

        d.direction = dartValues.direction;
        d.speed = dartValues.speed;
        d.lifetime = dartValues.lifetime;
    }

    [System.Serializable]
    public class DartValues
    {
        [Tooltip("true is right, false is left")]
        public bool direction = true;
        public float speed = 8;
        public float lifetime = 12;
    }

    [RequireComponent(typeof(Collider2D))]
    public class Dart : Hazard
    {
        [Tooltip("true is right, false is left")]
        public bool direction = true;
        public float speed = 8;
        public float lifetime = 12;

        private void Update()
        {
            if (lifetime <= 0)
                Destroy(gameObject);

            if (direction)
                transform.position += speed * Time.deltaTime * transform.right;
            else
                transform.position -= speed * Time.deltaTime * transform.right;
            lifetime -= Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger)
                return;

            OldPlayerController p = collision.GetComponent<OldPlayerController>();
            if (p != null)
                p.Kill(this);
        }
    }
}