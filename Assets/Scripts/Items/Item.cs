// Created by h1ddengames

using System;
using UnityEngine;
using NaughtyAttributes;

namespace h1ddengames {
    public abstract class Item : ScriptableObject {
        #region Exposed Fields
        [BoxGroup("Item Details"), SerializeField] private string defaultItemName;
        [BoxGroup("Item Details"), SerializeField] private string currentItemName;
        [BoxGroup("Item Details"), SerializeField] private Sprite defaultItemSprite;
        [BoxGroup("Item Details"), SerializeField] private Sprite currentItemSprite;

        [BoxGroup("Item Details"), SerializeField] private int maxStackAmount;

        [BoxGroup("Item Details"), SerializeField] private bool isDroppable;
        [BoxGroup("Item Details"), SerializeField] private bool isSellable;
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        public string DefaultItemName { get => defaultItemName; }
        public string CurrentItemName { get => currentItemName; set => currentItemName = value; }

        public Sprite DefaultItemSprite { get => defaultItemSprite; }
        public Sprite CurrentItemSprite { get => currentItemSprite; set => currentItemSprite = value; }

        public int MaxStackAmount { get => maxStackAmount; }

        public bool IsDroppable { get => isDroppable; }
        public bool IsSellable { get => isSellable; }
        #endregion

        #region My Methods
        public virtual void Use(int amount) => throw new NotImplementedException();
        public virtual void Drop(int amount) => throw new NotImplementedException();
        #endregion

        #region Helper Methods
        // int or float defaultValue
        // Vector2 minMaxValue
        // int or float currentValue
        #endregion
    }
}