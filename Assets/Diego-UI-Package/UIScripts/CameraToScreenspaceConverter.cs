using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
namespace Diego
{
    public class CameraToScreenspaceConverter
    {

        private static CameraToScreenspaceConverter _instance;

        public static CameraToScreenspaceConverter SingletonInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CameraToScreenspaceConverter();
                }
                return _instance;
            }
        } 

        private const string _renderTextureTagName = "BaseRender";
        private const string _uiTagName = "UIInclude";
        private RectTransform _renderTextureBounds;
        private GameCamera _gameCamera;
        private Camera _uiCamera;
        private Canvas _canvas;
        private RectTransform GetRenderTextureBounds()
        {
            var renderTexture = GameObject.FindGameObjectWithTag(_renderTextureTagName);
            var result = renderTexture.GetComponent<RectTransform>();
            _canvas = result.gameObject.GetComponentInParent<Canvas>();        
            return result;
        }

        private GameCamera GetGameCamera()
        {
            var gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
            return gameCamera.GetComponent<GameCamera>();
        }

        private Camera GetUICamera()
        {
            var uiInclude = GameObject.FindGameObjectWithTag(_uiTagName);
            return uiInclude.GetComponent<Camera>();
        }

        public Transform GetFromScreenSpace(Vector2 pos)
        {
            Vector3 gameSpace = new Vector3(pos.x / Screen.width - 0.5f, pos.y / Screen.height - 0.5f, 0);
            gameSpace.x *= 16;
            gameSpace.y *= 10;
            gameSpace += new Vector3(1.2f, 0.75f, 0);
            gameSpace.x *= 16 / 13.6f;
            gameSpace.y *= 10 / 8.5f;
            RaycastHit2D hit = Physics2D.GetRayIntersection(new Ray(GameObject.FindWithTag("MainCamera").transform.position + gameSpace, GameObject.FindWithTag("MainCamera").transform.forward));
            return hit.transform;
        }

        public Vector2 ViewportToRenderTextureViewport(Vector2 generalCoords)
        {
            if (_renderTextureBounds == null)
            {
                _renderTextureBounds = GetRenderTextureBounds();
            }
            // pretend like generalCoords is local coords for canvas.
            var intermediary = _canvas.transform.localToWorldMatrix * generalCoords;
            var transformed = _renderTextureBounds.transform.worldToLocalMatrix * intermediary;
            return transformed;

        }
        public Vector2 ViewportToWorld(Vector2 whereViewport)
        {
            if (_renderTextureBounds == null)
            {
                _renderTextureBounds = GetRenderTextureBounds();
            }
            if (_gameCamera == null)
            {
                _gameCamera = GetGameCamera();
            }
            if (_uiCamera == null)
            {
                _uiCamera = GetUICamera();
            }
            var viewportToUICamerasGlobal = _uiCamera.ViewportToWorldPoint(whereViewport);
            // return downward
            var diff = _uiCamera.transform.position - _gameCamera.transform.position;
            var ratio = Screen.currentResolution.width / Screen.currentResolution.height;

            viewportToUICamerasGlobal -= diff;

            Debug.Log("transformed_test: " + ViewportToRenderTextureViewport(whereViewport));
            // DEBUG
            var DEBUG_refloc_g = GameObject.Find("REFLOC_GREEN");
            var DEBUG_refloc_b = GameObject.Find("REFLOC_BLU");
            var DEBUG_refloc_r = GameObject.Find("REFLOC_RED");

            var death = _uiCamera.ScreenToWorldPoint(Input.mousePosition) - diff;

            // scale off of camera origin instead.
            // fake-up coordinates of the camera's local space
            var fakeUpToGlobalG = _gameCamera.transform.TransformPoint(whereViewport - (Vector2.one / 2f));
            var fakeUpToGlobalR = _gameCamera.transform.TransformPoint((whereViewport - (Vector2.one / 2f)) * 15f);
            var fakeUpToGlobalB = _gameCamera.transform.TransformPoint((whereViewport - (Vector2.one / 2f)) * 10f);
            // multiply by.. what?


            DEBUG_refloc_g.transform.position = _gameCamera.transform.TransformPoint(Vector2.zero);
            DEBUG_refloc_r.transform.position = fakeUpToGlobalR;
            DEBUG_refloc_b.transform.position = fakeUpToGlobalB;

            // multiply result by an unknown factor.
            
            return _uiCamera.ScreenToWorldPoint(Input.mousePosition) - diff;


            // var halfExtents = _renderTextureBounds.sizeDelta / 2f;

            // var boundsMin =  _renderTextureBounds.localToWorldMatrix * 
            //                 new Vector2(
            //                 _renderTextureBounds.anchoredPosition.x - halfExtents.x,
            //                 _renderTextureBounds.anchoredPosition.y - halfExtents.y
            //                 );
            
            // var boundsMax = _renderTextureBounds.localToWorldMatrix * 
            //                 new Vector2(
            //                 _renderTextureBounds.anchoredPosition.x + halfExtents.y,
            //                 _renderTextureBounds.rect.yMax + halfExtents.y
            //                 );
            
            // var viewportBoundsMin = Camera.main.WorldToViewportPoint(boundsMin);
            // var viewportBoundsMax = Camera.main.WorldToViewportPoint(boundsMax);

            // Debug.Log("vpBoundMin: " + viewportBoundsMin);
            // Debug.Log("vpBoundMax: " + viewportBoundsMax);

            // var viewportToRenderTextureLocal = new Vector2(
            //      Mathf.InverseLerp(viewportBoundsMin.x, viewportBoundsMax.x, whereViewport.x),
            //      Mathf.InverseLerp(viewportBoundsMin.y, viewportBoundsMax.y, whereViewport.y)
            // );

            // Debug.Log("local final coords: " + viewportToRenderTextureLocal);

            // return Camera.main.ViewportToWorldPoint(viewportToRenderTextureLocal);
        }
    }
}
