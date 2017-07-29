using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityTools.DesignPatern;

// ReSharper disable once CheckNamespace
namespace UnityTools
{
    public abstract class AGameManager<T, TE> : Singleton<T>
        where T : MonoBehaviour
        where TE : struct, IConvertible
    {
        // ReSharper disable once EmptyConstructor
        protected AGameManager() {}

        protected delegate void GameAction();
        protected readonly Dictionary<TE, GameAction> GameStatesUpdateActions = new Dictionary<TE, GameAction>();
        protected readonly Dictionary<TE, GameAction> GameStatesFixedUpdateActions = new Dictionary<TE, GameAction>();

        protected override void Awake()
        {
            base.Awake();
            bool init = false;
            foreach (TE state in Enum.GetValues(typeof(TE)))
            {
                if (!init)
                {
                    init = true;
                }
                Type type = typeof(T);
                MethodInfo method = type.GetMethod("On" + state + "Game", BindingFlags.NonPublic);
                if (method != null)
                {
                    GameStatesUpdateActions[state] = (GameAction)Delegate.CreateDelegate(typeof(GameAction), this, method.Name);
                }
                method = type.GetMethod("OnFixed" + state + "Game");
                if (method != null)
                {
                    GameStatesFixedUpdateActions[state] = (GameAction)Delegate.CreateDelegate(typeof(GameAction), this, method.Name);
                }
            }
        }
        
        public TE CurrentState;

        public T GetInstance()
        {
            return Instance;
        }

        protected virtual void Update()
        {
            if (GameStatesUpdateActions.ContainsKey(CurrentState))
                GameStatesUpdateActions[CurrentState]();
        }

        protected virtual void FixedUpdate()
        {
            if (GameStatesFixedUpdateActions.ContainsKey(CurrentState))
                GameStatesFixedUpdateActions[CurrentState]();
        }
    }
}
