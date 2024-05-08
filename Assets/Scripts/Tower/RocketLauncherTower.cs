using UnityEngine;
using System.Collections;
using TowerType = Core.GameEnums.TowerType;
using Troop;

namespace Tower
{
    public class RocketLauncherTower : TowerBase
    {
        [SerializeField] AnimationCurve curve; // Animation curve to define the fixed path
        [SerializeField] Transform targetPos; // Target position to shoot the rocket
        [SerializeField] GameObject rocketPrefab; // Prefab of the rocket
        [SerializeField] float rocketSpeed = 5f; // Fixed speed of the rocket
        [SerializeField] float curveDuration = 1f; // Duration of the animation
        [SerializeField] float attackInterval; // Attacking Interval

        private float currentAttackInterval;

        protected override void Awake()
        {
            base.Awake();
            TowerType = TowerType.Rocket;
            CanCreateConnections = false;
            UsedPaths = MaxPaths = 0;
        }
        protected override void Update()
        {
            base.Update();
            if (currentAttackInterval < 0)
            {
                Spawn();
                currentAttackInterval = attackInterval;
            }
            else
            {
                currentAttackInterval -= Time.deltaTime;
            }
        }
        protected override void Spawn()
        {
            GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity); // Instantiating a rocket prefab at the launcher tower
            StartCoroutine(FollowCurve(rocket)); 
        }

        IEnumerator FollowCurve(GameObject rocket)  // Follows a parabolic path
        {
            float startTime = Time.time;
            float elapsedTime = 0f;

            while (elapsedTime < curveDuration)
            {
                float t = elapsedTime / curveDuration; // calculate the current progress

                float curveValue = curve.Evaluate(t); // Reads the animation curve

                Vector3 newPosition = Vector3.Lerp(transform.position, targetPos.position, curveValue); // Following animation curve

                rocket.transform.position = newPosition; // Moving Rocket towards target

                elapsedTime = Time.time - startTime;
                yield return null;
            }

            rocket.transform.position = targetPos.position; // Setting Rocket position to target position
            StopCoroutine("FollowCurve");
        }
    }
}