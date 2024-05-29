using UnityEngine;
using System.Collections;
using TowerType = Core.GameEnums.TowerType;
using Troop;
using System.Collections.Generic;
using System.Net;

namespace Tower
{
    public class RocketLauncherTower : TowerBase
    {
        public bool IsActivelyShooting { get; private set; }

        [SerializeField] private Transform startingPoint;
        [SerializeField] private Transform target;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float fireInterval = 5f;

        private List<Vector3> points = new List<Vector3>();

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Rocket;
            CanCreateConnections = false;
            UsedPaths = MaxPaths = 0;
        }

        protected override void Start()
        {
            startingPoint = this.transform;
            base.Start();
            GenerateParabolicPoints(startingPoint.position, target.position, 16);
            StartCoroutine(FireBulletRoutine());

            for (int i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;

                Debug.DrawLine(points[i], points[i + 1], Color.red, 10f);
            }
        }

        private void GenerateParabolicPoints(Vector3 start, Vector3 end, int numberOfPoints)
        {
            for (int i = 0; i <= numberOfPoints; i++)
            {
                float t = i / (float)numberOfPoints;
                Vector3 parabolicPoint = CalculateParabolicPoint(start, end, t);
                points.Add(parabolicPoint);
            }
        }

        private Vector3 CalculateParabolicPoint(Vector3 start, Vector3 end, float t)
        {
            float height = curve.Evaluate(t);
            Vector3 midPoint = Vector3.Lerp(start, end, t);
            Vector3 point = new Vector3(midPoint.x, Mathf.Lerp(start.y, end.y, t) + height, midPoint.z);
            return point;
        }

        private IEnumerator FireBulletRoutine()
        {
            while (true)
            {
                FireBullet();
                yield return new WaitForSeconds(fireRate);
            }
        }

        private void FireBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, startingPoint.position, Quaternion.identity);
            StartCoroutine(MoveBullet(bullet));
        }

        private IEnumerator MoveBullet(GameObject bullet)
        {
            float duration = 1f; // Total duration to move the bullet
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                Vector3 position = CalculateParabolicPoint(startingPoint.position, target.position, t);
                bullet.transform.position = position;
                yield return null;
            }

            Destroy(bullet); // Destroy the bullet after it reaches the target
        }

        public void Retarget(Vector3 pos)
        {
        }

        protected override void Spawn()
        {
        }

        protected override void Update()
        {
        }
    }
}
