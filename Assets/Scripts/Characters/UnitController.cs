﻿using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace LocationRPG
{
    public abstract class UnitController<T> : MonoBehaviour where T : Unit
    {
        protected T unit;
        
        [SerializeField] protected GameObject unitModel;
        
        [SerializeField] protected AnimationController animationController;

        protected LerpingController _lerpingController;
        protected bool _isMoving;

        public const float MoveTime = 0.5f;

        protected float _attackTime = 3f;
        protected float _beforeAttackTime = 1f;
        protected float _afterAttackTime = 1f;

        public float AttackTime => _attackTime;
        public float BeforeAttackTime => _beforeAttackTime;
        public float AfterAttackTime => _afterAttackTime;

        public float RemainingTime
        {
            get
            {
                float result = _attackTime - _beforeAttackTime - _afterAttackTime;
                return (result < 0f) ? 0f : result;
            }
        }

        public T Unit
        {
            get => unit;
            set => unit = value;
        }

        public AnimationController AnimationController => animationController;

        protected virtual void OnEnable()
        {
            Assert.IsNotNull(unitModel);
        }

        protected virtual void Awake()
        {
            animationController.CurrentAnimation = AnimationConstants.IDLE;
            
            _lerpingController = new LerpingController(MoveTime);
        }

        private void FixedUpdate()
        {
            if (_lerpingController!=null &&_lerpingController.IsLerping)
            {
                gameObject.transform.position = _lerpingController.Lerp();
            }
        }

        public virtual void MoveToEnemy(GameObject enemy)
        {
        }

        public virtual void MoveToPlace()
        {
        }

        protected void MoveToGameObject(GameObject monster, float offset)
        {
            Vector3 endPosition = monster.transform.position;
            endPosition.z += offset;
            _lerpingController.StartLerping(gameObject.transform.position, endPosition);
        }

        protected void MoveToPosition(Vector3 position)
        {
            //Player position in combat scene is (0, 0, 0)
            _lerpingController.StartLerping(gameObject.transform.position, position);
        }
    }
}