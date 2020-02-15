using UnityEngine;
using System.Collections.Generic;

namespace Lean.Transition.Method
{
	/// <summary>This component allows you to transition the specified GameObject.SetActive to the target value.</summary>
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanGameObjectSetActive")]
	[AddComponentMenu(LeanTransition.MethodsMenuPrefix + "GameObject.SetActive" + LeanTransition.MethodsMenuSuffix)]
	public class LeanGameObjectSetActive : LeanMethodWithStateAndTarget
	{
		public override System.Type GetTargetType()
		{
			return typeof(GameObject);
		}

		public override void Register()
		{
			PreviousState = Register(GetAliasedTarget(Data.Target), Data.Active, Data.Duration);
		}

		public static LeanState Register(GameObject target, bool active, float duration)
		{
			var data = LeanTransition.RegisterWithTarget(State.Pool, duration, target);

			data.Active = active;

			return data;
		}

		[System.Serializable]
		public class State : LeanStateWithTarget<GameObject>
		{
			[Tooltip("The state we will transition to.")]
			public bool Active;

			public override bool CanAutoFill
			{
				get
				{
					return Target != null && Target.activeSelf != Active;
				}
			}

			public override void AutoFillWithTarget()
			{
				Active = Target.activeSelf;
			}

			public override void BeginWithTarget()
			{
			}

			public override void UpdateWithTarget(float progress)
			{
				if (progress == 1.0f)
				{
					Target.SetActive(Active);
				}
			}

			public static Stack<State> Pool = new Stack<State>(); public override void Despawn() { Pool.Push(this); }
		}

		public State Data;
	}
}

namespace Lean.Transition
{
	public static partial class LeanExtensions
	{
		public static GameObject SetActiveTransition(this GameObject target, bool active, float duration)
		{
			Method.LeanGameObjectSetActive.Register(target, active, duration); return target;
		}
	}
}