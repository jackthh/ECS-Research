using Unity.Entities;
using UnityEngine;

namespace _ECS_Research.Scripts.Survival_Game.Mobs
{
    public struct MobStats : IComponentData
    {
        public float movementSpeed;
        public int attackDmg;
        public float attackTime;
        public float attackInterval;
        public float attackRange;
    }



    public struct MobRuntimeData : IComponentData
    {
        public int hp;
        public bool chasing;
        public float currentAttackTime;
        public double nextAttackCDTime;
    }


    public class MobAuthoring : MonoBehaviour
    {
        public int hp;
        public int attackDmg;
        public float movementSpeed;
        public float attackTime;
        public float attackInterval;
        public float attackRange;
    }



    public class MobAuthoringBaker : Baker<MobAuthoring>
    {
        public override void Bake(MobAuthoring _authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new MobStats
            {
                movementSpeed = _authoring.movementSpeed,
                attackDmg = _authoring.attackDmg,
                attackTime = _authoring.attackTime,
                attackInterval = _authoring.attackInterval,
                attackRange =  _authoring.attackRange
            });
            
            AddComponent(entity, new MobRuntimeData
            {
                hp = _authoring.hp,
                chasing = true,
                currentAttackTime = 0f
            });
        }
    }
}