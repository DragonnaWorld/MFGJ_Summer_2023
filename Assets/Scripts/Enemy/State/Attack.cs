using Internal;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace Enemy
{
    public class AttackState : State<EnemyState, EnemyInfo>
    {
        float countdown = 3F;

        public override void Activate()
        {
            info.Animator.PlayRestart(EnemyInfo.AttackName);
            info.Movement.SetSpeed(0F);
            info.Movement.SetTurn(0F);
            countdown = 3F;
        }

        public override void Update()
        {
            countdown -= Time.deltaTime;
            if (countdown < 0)
            {
                if (info.TargetInSight(out Sensor.HitInfo hitInfo) && 
                    hitInfo.ratio * info.Sensor.Length < info.AcceptedRange)
                        ChangeState(EnemyState.Attack);
                else
                    ChangeState(EnemyState.Idle);
            }
        }
    }
}