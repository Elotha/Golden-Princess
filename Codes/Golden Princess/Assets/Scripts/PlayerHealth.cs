using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHealth 
{
    private static int HealthMax = 100;
    public static int Health = HealthMax;
    public static void TakeDamage()
    {
        if (Health > 0) {
            Health -= 5;
        }
        SpriteRenderer sprRend = CharacterController.sprRenderer;
        Color sprColor = sprRend.color;
        float _color = (Health / (float)HealthMax);
        if (sprColor.b > 0) {
            sprColor = new Color(1f,_color,_color);
        }
        sprRend.color = sprColor;
    }
}
