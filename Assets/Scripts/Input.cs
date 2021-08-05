using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Input : MonoBehaviour
{
    public IObservable<Vector2> Movement { get; private set; }

    private void Awake()
    {
        Movement = this.FixedUpdateAsObservable().Select((unit =>
        {
            var x = UnityEngine.Input.GetAxis("Horizontal");
            var y = UnityEngine.Input.GetAxis("Vertical");
            return new Vector2(x, y);
        }));
    }
}
