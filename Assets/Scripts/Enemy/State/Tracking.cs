using UnityEngine;

namespace Enemy
{
    public class SightTrackState : Internal.State<EnemyState, EnemyInfo>
    {
        Vector3 lastSeenPosition;

        public override void Activate()
        {
            lastSeenPosition = info.TargetOfInterest.transform.position;
            info.Movement.SetTurn(0F);
            info.Movement.SetSpeed(0F);
        }

        public override void Update()
        {
            bool stillSeeTarget = info.TargetInSight(out Sensor.HitInfo hit);
            bool inAcceptedRange;
            if (stillSeeTarget)
            {
                lastSeenPosition = hit.visibleObject.transform.position;
                inAcceptedRange = info.AcceptedRange >= Vector3.Distance(info.Movement.transform.position, hit.visibleObject.transform.position);
                if (inAcceptedRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else
                    AdvanceToTarget(info.TargetOfInterest.transform.position);
            }
            else
            {
                inAcceptedRange = info.AcceptedRange >= Vector3.Distance(lastSeenPosition, info.Movement.transform.position);
                if (inAcceptedRange)
                {
                    info.Movement.SetSpeed(0F);
                    info.Movement.SetTurn(1F);
                }
                else
                    AdvanceToTarget(lastSeenPosition);
            }

            if (inAcceptedRange)
                info.Animator.PlayContinue(EnemyInfo.IdleName);
            else
                info.Animator.PlayContinue(EnemyInfo.MoveName);
        }

        void AdvanceToTarget(Vector3 position)
        {
            var direction = position - info.Movement.transform.position;
            float targetAngle = Mathf.Abs(direction.z) <= 1e-5F ? (direction.x >= 0 ? 90 : -90) : Mathf.Rad2Deg * Mathf.Acos(direction.z / direction.magnitude) * (direction.x >= 0 ? 1 : -1);
            float delta = AngleNormalizer.ShortestDifference360(info.Movement.CurrentAngle, targetAngle);
            info.Movement.SetTurn(2F * delta / 180F);
            info.Movement.SetSpeed(1F);
        }
    }
}