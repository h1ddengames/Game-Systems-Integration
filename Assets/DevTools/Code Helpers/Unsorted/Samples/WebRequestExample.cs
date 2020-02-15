// Created by h1ddengames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace h1ddengames {
    public class WebRequestExample : MonoBehaviour {
        #region Exposed Variables
        [SerializeField] UnityWebRequest request;
        [SerializeField] string default_URL = "https://cat-fact.herokuapp.com/facts/5b1b411d841d9700146158d9";
        [Multiline(10), SerializeField] string data;
        #endregion

        #region Private Variables
        #endregion

        #region Getters/Setters/Constructors
        #endregion

        #region Unity Methods
        void OnEnable() {
            
        }

        void Start() {
            StartCoroutine(RequestRoutine(default_URL));
            
        }

        void Update() {
            
        }

        void OnDisable() {
            
        }

        private IEnumerator RequestRoutine(string url) {
            request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            data = request.downloadHandler.text;

            JSONObject json = new JSONObject(data);
            Debug.Log(json.GetField("text").str);


            //Debug.Log(JSONFormatter.FormatJSON(data));
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}
