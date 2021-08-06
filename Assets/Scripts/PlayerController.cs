using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializedVariables

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [SerializeField] [Range(-90, 0)] private float minViewAngle = -60f;
    [SerializeField] [Range(0, 90)] private float maxViewAngle = 60f;

    #endregion

    private CharacterController _character;
    private Camera _view;

    private void Awake()
    {
        _character = GetComponent<CharacterController>();
        _view = GetComponentInChildren<Camera>();
    }

    private void Start()
    {
        InputManager.Instance.Movement.Where(v => v != Vector2.zero).Subscribe((inputMovement =>
        {
            Vector2 inputVelocity = inputMovement * (InputManager.Instance.Run.Value ? runSpeed : walkSpeed);

            Vector3 playerVelocity =
                inputVelocity.x * transform.right +
                inputVelocity.y * transform.forward;

            Vector3 distance = playerVelocity * Time.fixedDeltaTime;
            _character.Move(distance);
        })).AddTo(this);


        InputManager.Instance.Mouselook
            .Where(v => v != Vector2.zero)
            .Subscribe(inputLook =>
            {
                Vector3 horizonLook = inputLook.x * Time.deltaTime * Vector3.up;
                transform.localRotation *= Quaternion.Euler(horizonLook);

                Vector3 verticalLook = inputLook.y * Time.deltaTime * Vector3.left;
                Quaternion newQ = _view.transform.localRotation * Quaternion.Euler(verticalLook);

                _view.transform.localRotation = ClampRotationAroundXAxis(newQ, -maxViewAngle, -minViewAngle);
            }).AddTo(this);
    }

    private static Quaternion ClampRotationAroundXAxis(Quaternion q, float minAngle, float maxAngle)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, minAngle, maxAngle);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}