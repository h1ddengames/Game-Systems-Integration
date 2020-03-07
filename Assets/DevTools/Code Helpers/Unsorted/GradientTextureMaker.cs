// Original Version Created by IronWarrior:
// https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Texture/GradientTextureMaker.cs
// Updated/Current Version by h1ddengames

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
    public class GradientTextureMaker : MonoBehaviour {
        #region Exposed Fields
        [BoxGroup("References"), SerializeField] private SpriteRenderer sprite;
        [BoxGroup("References"), SerializeField] private RawImage rawImage;

        [BoxGroup("Configuration"), SerializeField] private string fileName;
        [BoxGroup("Configuration"), SerializeField] private int width = 256;
        [BoxGroup("Configuration"), SerializeField] private int height = 1;
        [BoxGroup("Configuration"), SerializeField] private TextureWrapMode textureWrapMode;
        [BoxGroup("Configuration"), SerializeField] private FilterMode filterMode;
        [BoxGroup("Configuration"), SerializeField] private bool isLinear;

        [BoxGroup("Configuration"), NaughtyAttributes.ReorderableList, SerializeField] private Color[] colors;
        #endregion

        #region Private Fields
        private Texture2D texture;
        #endregion

        #region Getters/Setters/Constructors
        public SpriteRenderer Sprite { get => sprite; set => sprite = value; }
        public RawImage RawImage { get => rawImage; set => rawImage = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Color[] Colors { get => colors; set => colors = value; }
        public TextureWrapMode TextureWrapMode { get => textureWrapMode; set => textureWrapMode = value; }
        public FilterMode FilterMode { get => filterMode; set => filterMode = value; }
        public bool IsLinear { get => isLinear; set => isLinear = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        #endregion

        [ExecuteAlways]
        private void UpdateDisplay() {
            Texture = Create(Colors, TextureWrapMode, FilterMode, IsLinear, false);

            if (RawImage != null) {
                RawImage.texture = Texture;
            } else if (Sprite != null) {
                Sprite.sprite = ConvertToSprite(Texture);
            }
        }

        #region Unity Methods
        private void Start() {
            Texture = Create(Colors, TextureWrapMode, FilterMode, IsLinear, false);

            if (RawImage != null) {
                RawImage.texture = Texture;
            } else if(Sprite != null) {
                Sprite.sprite = ConvertToSprite(Texture);
            } 
        }
        #endregion

        public Texture2D Create(Color[] colors, TextureWrapMode textureWrapMode = TextureWrapMode.Clamp, FilterMode filterMode = FilterMode.Point, bool isLinear = false, bool hasMipMap = false) {
            if (colors == null || colors.Length == 0) {
                Debug.LogError("No colors assigned");
                return null;
            }

            int length = colors.Length;
            if (colors.Length > 8) {
                Debug.LogWarning("Too many colors! maximum is 8, assigned: " + colors.Length);
                length = 8;
            }

            // Build gradient from colors.
            var colorKeys = new GradientColorKey[length];
            var alphaKeys = new GradientAlphaKey[length];

            float steps = length - 1f;
            for (int i = 0; i < length; i++) {
                float step = i / steps;
                colorKeys[i].color = colors[i];
                colorKeys[i].time = step;
                alphaKeys[i].alpha = colors[i].a;
                alphaKeys[i].time = step;
            }

            // Create gradient.
            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);

            // Create texture.
            Texture2D outputTex = new Texture2D(Width, Height, TextureFormat.ARGB32, false, isLinear);
            outputTex.wrapMode = textureWrapMode;
            outputTex.filterMode = filterMode;

            // Draw texture.
            for (int i = 0; i < Width; i++) {
                outputTex.SetPixel(i, 0, gradient.Evaluate((float) i / (float) Width));
            }
            outputTex.Apply(false);

            return outputTex;
        }

        public Sprite ConvertToSprite(Texture2D texture) {
            return UnityEngine.Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        [NaughtyAttributes.Button("Save Image as PNG")]
        public void SaveImage() {
            if (!string.IsNullOrWhiteSpace(fileName)) {
                File.WriteAllBytes(Application.dataPath + "/Generated PNGS/" + fileName + ".png", Create(Colors, TextureWrapMode, FilterMode, IsLinear, false).EncodeToPNG());
            } else {
                Debug.LogError("Requires filename to not be null or empty.");
            }
        }
    }
}
