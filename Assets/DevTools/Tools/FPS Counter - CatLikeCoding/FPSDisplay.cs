using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace h1ddengames {
	[RequireComponent(typeof(FPSCounter))]
	public class FPSDisplay : MonoBehaviour {

		[System.Serializable]
		private struct FPSColor {
			public Color color;
			public int minimumFPS;
		}

		public TextMeshProUGUI highestFPSLabel, averageFPSLabel, lowestFPSLabel;

		[NaughtyAttributes.ReorderableList, SerializeField] private FPSColor[] coloring;

		FPSCounter fpsCounter;

		void Awake() {
			fpsCounter = GetComponent<FPSCounter>();
		}

		void Update() {
			Display(highestFPSLabel, fpsCounter.HighestFPS, "HIGH");
			Display(averageFPSLabel, fpsCounter.AverageFPS, "AVG");
			Display(lowestFPSLabel, fpsCounter.LowestFPS, "LOW");
		}

		void Display(TextMeshProUGUI label, int fps, string name) {
			label.text = fps.ToString() + " " + name + " FPS"; //stringsFrom00To99[Mathf.Clamp(fps, 0, 99)];
			for (int i = 0; i < coloring.Length; i++) {
				if (fps >= coloring[i].minimumFPS) {
					label.color = coloring[i].color;
					break;
				}
			}
		}
	}
}
