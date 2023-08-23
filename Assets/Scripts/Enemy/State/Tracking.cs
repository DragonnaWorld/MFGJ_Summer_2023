using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class TrackState : Internal.State<EnemyState, EnemyInfo>
    {
        public override void Activate()
        {
            info.Animator.Play(EnemyInfo.MoveName);
        }

        public override void Update()
        {
        }

        public override void Deactivate()
        {
        }
    }
}