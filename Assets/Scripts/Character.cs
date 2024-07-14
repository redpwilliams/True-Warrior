using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Rigidbody2D _rb2d;
    [SerializeField] private float _finalPosition = 0f;
    [SerializeField] private float _speed = 1f;
    private bool isMoving = true;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        // Move the character towards the final position
        Vector2 currentPosition = _rb2d.position;
        Vector2 targetPosition = new Vector2(_finalPosition, currentPosition.y);
        _rb2d.MovePosition(Vector2.MoveTowards(currentPosition, targetPosition,
            _speed * Time.fixedDeltaTime));

        // Check if the character has reached or passed the final position
        if (_rb2d.position.x < _finalPosition) return;
        
        // Stop the movement (set velocity to zero)
        _rb2d.velocity = Vector2.zero;
        isMoving = false;
    }
}