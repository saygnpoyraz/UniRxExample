using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public IObservable<Vector2> Movement { get; private set; }
    public IObservable<Vector2> Mouselook { get; private set; }

    public ReadOnlyReactiveProperty<bool> Run { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        Run = this.UpdateAsObservable()
            .Select(_ => Input.GetButton("Fire3"))
            .ToReadOnlyReactiveProperty();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Movement = this.FixedUpdateAsObservable().Select((unit =>
        {
            var x = Input.GetAxis("Horizontal");
            var y = Input.GetAxis("Vertical");
            return new Vector2(x, y);
        }));

        Mouselook = this.UpdateAsObservable()
            .Select(_ =>
            {
                var x = Input.GetAxis("Mouse X");
                var y = Input.GetAxis("Mouse Y");
                return new Vector2(x, y);
            });
    }
}