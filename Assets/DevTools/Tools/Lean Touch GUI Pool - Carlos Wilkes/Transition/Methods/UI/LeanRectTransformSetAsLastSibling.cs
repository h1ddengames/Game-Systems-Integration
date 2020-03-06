using UnityEngine;
using System.Collections.Generic;

namespace Lean.Transition.Method
{
	/// <summary>This component allows you to transition the specified RectTransform.SetAsLastSibling to the target value.</summary>
	[HelpURL(LeanTransition.HelpUrlPrefix + "LeanRectTransformSetAsLastSibling")]
	[AddComponentMenu(LeanTransition.MethodsMenuPrefix + "RectTransform.SetAsLastSibling" + LeanTransition.MethodsMenuSuffix)]
	public class LeanRectTransformSetAsLastSibling : LeanMethodWithStateAndTarget
	{
		public override System.Type GetTargetType()
		{
			return typeof(RectTransform);
		}

		public override void Register()
		{
			PreviousState = Register(GetAliasedTarget(Data.Target), Data.Duration);
		}

		public static LeanState Register(RectTransform target, float duration)
		{
			var data = LeanTransition.RegisterWithTarget(State.Pool, duration, target);

			return data;
		}

		[System.Serializable]
		public class State : LeanStateWithTarget<RectTransform>
		{
			public override bool CanAutoFill
			{
				get
				{
					return false;
				}
			}

			public override void UpdateWithTarget(float progress)
			{
				Target.SetAsLastSibling();
			}

			public static Stack<State> Pool = new Stack<State>(); public override void Despawn() { Pool.Push(this); }
		}

		public State Data;
	}
}