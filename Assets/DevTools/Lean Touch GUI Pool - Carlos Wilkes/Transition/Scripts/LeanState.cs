using UnityEngine;
using System.Collections.Generic;

namespace Lean.Transition
{
	/// <summary>This is the base class for all transition states. When you register a transition (e.g. LeanTransformLocalPosition), it will return an instance of this class, allowing it to be updated by the transition manager.</summary>
	public abstract class LeanState
	{
		public enum ConflictType
		{
			None,
			Ignore,
			Complete
		}

		/// <summary>The transition will complete after this many seconds.</summary>
		public float Duration = 1.0f;

		/// <summary>The current amount of seconds this transition has been running (-1 for pending Begin call).</summary>
		[System.NonSerialized]
		public float Age;

		/// <summary>If this transition is chained to another, then this tells you which must finish before this can begin.</summary>
		[System.NonSerialized]
		public List<LeanState> Prev = new List<LeanState>();

		/// <summary>If this transition is chained to another, then this tells you which will begin after this finishes.</summary>
		[System.NonSerialized]
		public List<LeanState> Next = new List<LeanState>();

		/// <summary>If a transition is marked as skipped, then it won't call Begin or Update any more, but will otherwise act as normal and not be removed. This is so any transition chain can still work as expected.</summary>
		[System.NonSerialized]
		public bool Skip;

		/// <summary>If you want this transition to begin after another completes, then call this method.</summary>
		public void BeginAfter(LeanState previousState)
		{
			Prev.Add(previousState);
			previousState.Next.Add(this);
		}

		public virtual Object GetTarget()
		{
			return default(Object);
		}

		public virtual ConflictType Conflict
		{
			get
			{
				return ConflictType.Ignore;
			}
		}

		public virtual bool CanAutoFill
		{
			get
			{
				return false;
			}
		}

		public virtual void AutoFill()
		{
		}

		public abstract void Begin();

		public abstract void Update(float progress);

		public virtual void Despawn()
		{
		}
	}
}