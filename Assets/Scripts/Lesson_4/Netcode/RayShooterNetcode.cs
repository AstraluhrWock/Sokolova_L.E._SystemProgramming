﻿using System.Collections;
using UnityEngine;
public class RayShooterNetcode : FireActionNetcode
{
    private Camera _camera;
    protected override void Start()
    {
        base.Start();
        _camera = GetComponentInChildren<Camera>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shooting();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reloading();
        }
        if (Input.anyKey && !Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    protected override void Shooting()
    {
        base.Shooting();
        if (bullets.Count > 0)
        {
            StartCoroutine(Shoot());
        }
    }
    private IEnumerator Shoot()
    {
        if (isReloading)
        {
            yield break;
        }
        var point = new Vector3(Camera.main.pixelWidth / 2,
        Camera.main.pixelHeight / 2, 0);
        var ray = _camera.ScreenPointToRay(point);
        if (!Physics.Raycast(ray, out var hit))
        {
            yield break;
        }
        var shoot = bullets.Dequeue();
        var bulletCount = bullets.Count.ToString();
        ammunition.Enqueue(shoot);
        shoot.SetActive(true);
        shoot.transform.position = hit.point;
        shoot.transform.parent = hit.transform;
        yield return new WaitForSeconds(2.0f);
        shoot.SetActive(false);
    }
}