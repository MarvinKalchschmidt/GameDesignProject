using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orangutan : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animationName1 = "Orangutan_Idle"; // The name of the first animation.
    [SerializeField] private string _animationName2 = "Orangutan_Thinking";
    private bool isFirstAnimation = true;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }
    private void OnEnable()
    {
       
        if (isFirstAnimation)
        {
            _animator.Play(_animationName1);
        }
        else
        {
            _animator.Play(_animationName2);
        }

        isFirstAnimation = !isFirstAnimation;
    }
}
