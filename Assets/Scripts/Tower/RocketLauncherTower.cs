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

        private List<Vector3> points;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Rocket;
            CanCreateConnections = false;
            UsedPaths = MaxPaths = 0;
        }

        protected override void Start()
        {
            base.Start();
            GenerateParabolicPoints(startingPoint.position, target.position, 16);
            for (int i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;

                Debug.DrawLine(points[i], points[i + 1], Color.red, 10f);
            }
        }

        private void GenerateParabolicPoints(Vector3 start, Vector3 end, int numberOfPoints)
        {
            Vector3 midpoint = (start + end) / 2f;

            Vector3 vertex = new Vector3(midpoint.x, Mathf.Max(start.y, end.y) + 2f, midpoint.z);
            for (int i = 0; i <= numberOfPoints; i++)
            {
                float t = i / (float)numberOfPoints;
                Vector3 parabolicPoint = CalculateParabolicPoint(start, vertex, end, t);
                points.Add(parabolicPoint);
            }
        }

        private Vector3 CalculateParabolicPoint(Vector3 start, Vector3 vertex, Vector3 end, float t)
        {
            float oneMinusT = 1f - t;
            Vector3 point = oneMinusT * oneMinusT * start + 2f * oneMinusT * t * vertex + t * t * end;
            return point;
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