using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Foundations.Architecture.EventObserver;

namespace GameSystems.CameraSystem
{
    public class ActivatePlayerCameraController : IEventData { }
    public class DIsActivatePlayerCameraController : IEventData { }
    public class FocusOnPosition_Direct : IEventData
    {
        public Vector2Int FocusPosition;
    }
    public class FocusOnPosition_ToMove : IEventData
    {
        public Vector2Int FocusPosition;
    }
    public class FocusGameObject : IEventData
    {
        public Transform GameObjectTransform;
    }

    public class CameraManagerEventListener : MonoBehaviour, IEventObserverListener
    {
        private IEventObserverLinker EventObserverLinker;

        private void Awake()
        {
            this.EventObserverLinker = new EventObserverLinker(this);
        }

        private void OnEnable()
        {
            this.EventObserverLinker.RegisterSubscriberListener<ActivatePlayerCameraController>();
            this.EventObserverLinker.RegisterSubscriberListener<DIsActivatePlayerCameraController>();
            this.EventObserverLinker.RegisterSubscriberListener<FocusOnPosition_Direct>();
            this.EventObserverLinker.RegisterSubscriberListener<FocusOnPosition_ToMove>();
            this.EventObserverLinker.RegisterSubscriberListener<FocusGameObject>();
        }

        private void OnDisable()
        {
            this.EventObserverLinker.Dispose();
        }

        public void UpdateData(IEventData eventData)
        {
            switch (eventData)
            {
                default:
                    break;
            }
        }
    }

    [Serializable]
    public class CameraControllerData
    {
        [SerializeField] public Camera MainCamera;

        [SerializeField] public float MinX, MaxX;
        [SerializeField] public float MinY, MaxY;

        [SerializeField] public float MinOrthographicSize, MaxOrthographicSize;

        [SerializeField] public float CameraKeyBoardMoveSpeed;
        [SerializeField] public float CameraScrollZoomInOutSpeed;

        public float ClampX(float x) => Mathf.Clamp(x, MinX, MaxX);
        public float ClampY(float y) => Mathf.Clamp(y, MinY, MaxY);
        public float ClampOrthographicSize(float orthographicSize) => Mathf.Clamp(orthographicSize, MinOrthographicSize, MaxOrthographicSize);
    }

    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraZoomInOutController CameraZoomInOutController;
        [SerializeField] private CameraKeyBoardMoveController CameraKeyBoardMoveController;
        [SerializeField] private CameraFocusingController CameraFocusingController;

        [SerializeField] private CameraControllerData CameraControllerData;

        public void ActivateCameraController()
        {
            this.CameraZoomInOutController.enabled = true;
            this.CameraKeyBoardMoveController.enabled = true;
        }

        public void DisActivateCameraController()
        {
            this.CameraZoomInOutController.enabled = false;
            this.CameraKeyBoardMoveController.enabled = false;
        }
    }

    public class CameraZoomInOutController : MonoBehaviour
    {
        [SerializeField] private CameraControllerData CameraControllerData;

        private float scrollInput;
        private float nextOrthographicSize;

        public void InitialSetting(CameraControllerData cameraControllerData)
        {
            this.CameraControllerData = cameraControllerData;
        }

        private void Update()
        {
            this.scrollInput = Input.GetAxis("Mouse ScrollWheel");
            this.nextOrthographicSize = 0;

            if (this.scrollInput != 0)
            {
                // 확대 or 축소
                if (this.scrollInput > 0)
                {
                    this.nextOrthographicSize = this.CameraControllerData.MainCamera.orthographicSize - (this.CameraControllerData.CameraScrollZoomInOutSpeed * Time.deltaTime);
                }
                else
                {
                    this.nextOrthographicSize = this.CameraControllerData.MainCamera.orthographicSize + (this.CameraControllerData.CameraScrollZoomInOutSpeed * Time.deltaTime);
                }

                // 한계치 확인.
                this.nextOrthographicSize = this.CameraControllerData.ClampOrthographicSize(this.nextOrthographicSize);

                // 사이즈 조절
                this.CameraControllerData.MainCamera.orthographicSize = this.nextOrthographicSize;

                this.scrollInput = 0;
            }
        }
    }

    public class CameraKeyBoardMoveController : MonoBehaviour
    {
        [SerializeField] private CameraControllerData CameraControllerData;

        private Vector3 moveDirection = Vector3.zero;
        private Vector3 nextCameraPosition = Vector3.zero;

        public void InitialSetting(CameraControllerData cameraControllerData)
        {
            this.CameraControllerData = cameraControllerData;
        }

        private void Update()
        {
            this.moveDirection = Vector3.zero;

            // 키 입력 감지
            if (Input.GetKey(KeyCode.W)) this.moveDirection.y += 1; // 위
            if (Input.GetKey(KeyCode.S)) this.moveDirection.y -= 1; // 아래
            if (Input.GetKey(KeyCode.A)) this.moveDirection.x -= 1; // 왼쪽
            if (Input.GetKey(KeyCode.D)) this.moveDirection.x += 1; // 오른쪽

            // 이동 방향 정규화 (대각선 이동 속도 일정하게 유지)
            if (this.moveDirection == Vector3.zero)
            {
                return;
            }
            else
            {
                this.moveDirection.Normalize();

                // 다음 카메라 위치 카메라 이동
                this.nextCameraPosition = this.CameraControllerData.MainCamera.transform.position
                                          + this.moveDirection * this.CameraControllerData.CameraKeyBoardMoveSpeed * Time.deltaTime;

                // 이동 범위 제한
                this.nextCameraPosition.x = this.CameraControllerData.ClampX(this.nextCameraPosition.x);
                this.nextCameraPosition.y = this.CameraControllerData.ClampY(this.nextCameraPosition.y);

                // 적용
                this.CameraControllerData.MainCamera.transform.position = this.nextCameraPosition;
            }
        }
    }

    public class CameraFocusingController : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            this.mainCamera = Camera.main;
        }
    }
}

