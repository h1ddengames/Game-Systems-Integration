using UnityEngine;

namespace Lean.Transition
{
	/// <summary>This class stores additional base data for transitions that modify a target UnityEngine.Object (most do).</summary>
	public abstract class LeanStateWithTarget<T> : LeanState
		where T : Object
	{
		/// <summary>This is the target of the transition. For most transition methods this will be the component that will be modified.</summary>
		public T Target;

		public override Object GetTarget()
		{
			return Target;
		}

		public override bool CanAutoFill
		{
			get
			{
				return Target != null;
			}
		}

		public override void AutoFill()
		{
			if (Target != null)
			{
				AutoFillWithTarget();
			}
		}

		public virtual void AutoFillWithTarget()
		{
		}

		public override void Begin()
		{
			if (Target != null)
			{
				BeginWithTarget();
			}
		}

		public virtual void BeginWithTarget()
		{
		}

		public override void Update(float progress)
		{
			if (Target != null)
			{
				UpdateWithTarget(progress);
			}
		}

		public virtual void UpdateWithTarget(float progress)
		{
		}
	}
}