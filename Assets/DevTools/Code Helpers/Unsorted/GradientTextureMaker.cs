// Original Version Created by IronWarrior:
// https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Texture/GradientTextureMaker.cs
// Updated/Current Version by h1ddengames

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace h1ddengames {
    [RequireComponent(typeof(RawImage))]
    public class GradientTextureMaker : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private int width = 256;
        [SerializeField] private int height = 1;
        [NaughtyAttributes.ReorderableList, SerializeField] private Color[] colors;
        [SerializeField] private TextureWrapMode textureWrapMode;
        [SerializeField] private FilterMode filterMode;
        [SerializeField] private bool isLinear;
        #endregion

        #region Private Fields
        private RawImage rawImage;
        private Texture2D texture;
        #endregion

        #region Getters/Setters/Constructors
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Color[] Colors { get => colors; set => colors = value; }
        public TextureWrapMode TextureWrapMode { get => textureWrapMode; set => textureWrapMode = value; }
        public FilterMode FilterMode { get => filterMode; set => filterMode = value; }
        public bool IsLinear { get => isLinear; set => isLinear = value; }
        #endregion

        #region Unity Methods
        private void Start() {
            if (rawImage == null) {
                rawImage = GetComponent<RawImage>();
            }

            texture = Create(Colors, TextureWrapMode, FilterMode, IsLinear, false);
            rawImage.texture = texture;
        }

        private void OnValidate() {
            if (rawImage == null) {
                rawImage = GetComponent<RawImage>();
            }

            texture = Create(Colors, TextureWrapMode, FilterMode, IsLinear, false);
            rawImage.texture = texture;
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

            // build gradient from colors
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

            // create gradient
            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);

            // create texture
            Texture2D outputTex = new Texture2D(Width, Height, TextureFormat.ARGB32, false, isLinear);
            outputTex.wrapMode = textureWrapMode;
            outputTex.filterMode = filterMode;

            // draw texture
            for (int i = 0; i < Width; i++) {
                outputTex.SetPixel(i, 0, gradient.Evaluate((float) i / (float) Width));
            }
            outputTex.Apply(false);

            return outputTex;
        }
    }
}
