using Internal;
using System.ComponentModel;
using UnityEngine;

namespace Player
{
    public class AttackState : State<PlayerState, PlayerInfo>
    {
        int currentAttackLevel = 0;
        readonly string[] attackString = new string[] { PlayerInfo.Attack1, PlayerInfo.Attack2, PlayerInfo.Attack3};
        
        public override void Activate()
        {
            info.Rgbody.velocity = new();
            currentAttackLevel = -1;
        }

        public override void Update()
        {
            int index = info.Animator.GetLayerIndex("Base Layer");
            bool finished = currentAttackLevel < 0 || info.Animator.GetCurrentAnimatorStateInfo(index).normalizedTime > 1F;
            if (finished)
            {
                if (currentAttackLevel < 2)
                {
                    if (Input.GetKey(KeyCode.Space))
                        NextAttack();
                    else
                        ChangeState(PlayerState.IdleAndMove);
                }
                else
                    ChangeState(PlayerState.IdleAndMove);
            }
        }

        void NextAttack()
        {
            currentAttackLevel++;
            info.Animator.Play(attackString[currentAttackLevel]);
        }
    }
}