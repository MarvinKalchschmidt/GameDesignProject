using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeOfLife : MonoBehaviour
{
    [SerializeField] private float maxLife;
    [SerializeField] public float currentLife { get; private set; }
}
