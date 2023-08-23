using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IdleState : Internal.State<EnemyState, EnemyInfo>
    {
        public override void Activate()
        {
            info.Animator.Play(EnemyInfo.IdleName);
        }

        public override void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                ChangeState(EnemyState.Track);
            }
        }

        public override void Deactivate()
        {
        }
    }
}