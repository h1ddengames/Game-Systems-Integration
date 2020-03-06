/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CodeMonkey.Utils {
    /*
     * Various assorted utilities functions
     * 
     */
    public static class UtilsClass {
        // Get Mouse Position in World with Z = 0f
        public static Vector3 GetMouseWorldPosition() {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
        
        // Is Mouse over a UI Element? Used for ignoring World clicks through UI
        public static bool IsPointerOverUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            } else {
                PointerEventData pe = new PointerEventData(EventSystem.current);
                pe.position =  Input.mousePosition;
                List<RaycastResult> hits = new List<RaycastResult>();
                EventSystem.current.RaycastAll( pe, hits );
                return hits.Count > 0;
            }
        }

		// Returns 00-FF, value 0->255
	    public static string Dec_to_Hex(int value) {
		    return value.ToString("X2");
	    }

		// Returns 0-255
	    public static int Hex_to_Dec(string hex) {
		    return Convert.ToInt32(hex, 16);
	    }
        
		// Returns a hex string based on a number between 0->1
	    public static string Dec01_to_Hex(float value) {
		    return Dec_to_Hex((int)Mathf.Round(value*255f));
	    }

		// Returns a float between 0->1
	    public static float Hex_to_Dec01(string hex) {
		    return Hex_to_Dec(hex)/255f;
	    }

        // Get Hex Color FF00FF
	    public static string GetStringFromColor(Color color) {
		    string red = Dec01_to_Hex(color.r);
		    string green = Dec01_to_Hex(color.g);
		    string blue = Dec01_to_Hex(color.b);
		    return red+green+blue;
	    }
        
        // Get Hex Color FF00FFAA
	    public static string GetStringFromColorWithAlpha(Color color) {
		    string alpha = Dec01_to_Hex(color.a);
		    return GetStringFromColor(color)+alpha;
	    }

        // Sets out values to Hex String 'FF'
	    public static void GetStringFromColor(Color color, out string red, out string green, out string blue, out string alpha) {
		    red = Dec01_to_Hex(color.r);
		    green = Dec01_to_Hex(color.g);
		    blue = Dec01_to_Hex(color.b);
		    alpha = Dec01_to_Hex(color.a);
	    }
        
        // Get Hex Color FF00FF
	    public static string GetStringFromColor(float r, float g, float b) {
		    string red = Dec01_to_Hex(r);
		    string green = Dec01_to_Hex(g);
		    string blue = Dec01_to_Hex(b);
		    return red+green+blue;
	    }
        
        // Get Hex Color FF00FFAA
	    public static string GetStringFromColor(float r, float g, float b, float a) {
		    string alpha = Dec01_to_Hex(a);
		    return GetStringFromColor(r,g,b)+alpha;
	    }
        
        // Get Color from Hex string FF00FFAA
	    public static Color GetColorFromString(string color) {
		    float red = Hex_to_Dec01(color.Substring(0,2));
		    float green = Hex_to_Dec01(color.Substring(2,2));
		    float blue = Hex_to_Dec01(color.Substring(4,2));
            float alpha = 1f;
            if (color.Length >= 8) {
                // Color string contains alpha
                alpha = Hex_to_Dec01(color.Substring(6,2));
            }
		    return new Color(red, green, blue, alpha);
	    }

        // Generate random normalized direction
        public static Vector3 GetRandomDir() {
            return new Vector3(UnityEngine.Random.Range(-1f,1f), UnityEngine.Random.Range(-1f,1f)).normalized;
        }
        
        public static Vector3 GetVectorFromAngle(int angle) {
            // angle = 0 -> 360
            float angleRad = angle * (Mathf.PI/180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        public static float GetAngleFromVectorFloat(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        public static int GetAngleFromVector(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static int GetAngleFromVector180(Vector3 dir) {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, Vector3 vecRotation) {
            return ApplyRotationToVector(vec, GetAngleFromVectorFloat(vecRotation));
        }

        public static Vector3 ApplyRotationToVector(Vector3 vec, float angle) {
            return Quaternion.Euler(0,0,angle) * vec;
        }

        public static FunctionUpdater CreateMouseDraggingAction(Action<Vector3> onMouseDragging) {
            return CreateMouseDraggingAction(0, onMouseDragging);
        }

        public static FunctionUpdater CreateMouseDraggingAction(int mouseButton, Action<Vector3> onMouseDragging) {
            bool dragging = false;
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    dragging = true;
                }
                if (Input.GetMouseButtonUp(mouseButton)) {
                    dragging = false;
                }
                if (dragging) {
                    onMouseDragging(UtilsClass.GetMouseWorldPosition());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickFromToAction(Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            return CreateMouseClickFromToAction(0, 1, onMouseClickFromTo, onWaitingForToPosition);
        }

        public static FunctionUpdater CreateMouseClickFromToAction(int mouseButton, int cancelMouseButton, Action<Vector3, Vector3> onMouseClickFromTo, Action<Vector3, Vector3> onWaitingForToPosition) {
            int state = 0;
            Vector3 from = Vector3.zero;
            return FunctionUpdater.Create(() => {
                if (state == 1) {
                    if (onWaitingForToPosition != null) onWaitingForToPosition(from, UtilsClass.GetMouseWorldPosition());
                }
                if (state == 1 && Input.GetMouseButtonDown(cancelMouseButton)) {
                    // Cancel
                    state = 0;
                }
                if (Input.GetMouseButtonDown(mouseButton) && !UtilsClass.IsPointerOverUI()) {
                    if (state == 0) {
                        state = 1;
                        from = UtilsClass.GetMouseWorldPosition();
                    } else {
                        state = 0;
                        onMouseClickFromTo(from, UtilsClass.GetMouseWorldPosition());
                    }
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickAction(Action<Vector3> onMouseClick) {
            return CreateMouseClickAction(0, onMouseClick);
        }

        public static FunctionUpdater CreateMouseClickAction(int mouseButton, Action<Vector3> onMouseClick) {
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    onMouseClick(GetWorldPositionFromUI());
                }
                return false; 
            });
        }

        public static FunctionUpdater CreateMouseClickAction(int mouseButton, Action onMouseClick) {
            return FunctionUpdater.Create(() => {
                if (Input.GetMouseButtonDown(mouseButton)) {
                    onMouseClick();
                }
                return false;
            });
        }

        public static FunctionUpdater CreateKeyCodeAction(KeyCode keyCode, Action onKeyDown) {
            return FunctionUpdater.Create(() => {
                if (Input.GetKeyDown(keyCode)) {
                    onKeyDown();
                }
                return false; 
            });
        }

        // Get UI Position from World Position
        public static Vector2 GetWorldUIPosition(Vector3 worldPosition, Transform parent, Camera uiCamera, Camera worldCamera) {
            Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldPosition);
            Vector3 uiCameraWorldPosition = uiCamera.ScreenToWorldPoint(screenPosition);
            Vector3 localPos = parent.InverseTransformPoint(uiCameraWorldPosition);
            return new Vector2(localPos.x, localPos.y);
        }

        public static Vector3 GetWorldPositionFromUIZeroZ() {
            Vector3 vec = GetWorldPositionFromUI(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }

        // Get World Position from UI Position
        public static Vector3 GetWorldPositionFromUI() {
            return GetWorldPositionFromUI(Input.mousePosition, Camera.main);
        }

        public static Vector3 GetWorldPositionFromUI(Camera worldCamera) {
            return GetWorldPositionFromUI(Input.mousePosition, worldCamera);
        }

        public static Vector3 GetWorldPositionFromUI(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}