using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    TargetPoint target;

    [SerializeField]
    Transform turret = default, laserBeam = default;

    [SerializeField, Range(1f, 100f)]
    float damagePerSecond = 10f;

    Vector3 laserBeamScale;

    //public override TowerType TowerType => throw new System.NotImplementedException();

    public override TowerType TowerType => TowerType.Laser;

    void Awake()
    {
        laserBeamScale = transform.localScale;
    }

    public override void GameUpdate()
    {
        if (TrackTarget(ref target) || AcquireTarget(out target))
        {
            Shoot();
        }
        else
        {
            laserBeam.localScale = Vector3.zero;
        }
    }

    void Shoot()
    {
        Vector3 point = target.Position;
        turret.LookAt(point);
        laserBeam.localRotation = turret.localRotation;

        float d = Vector3.Distance(turret.position, point);
        laserBeamScale.z = d;
        laserBeam.localScale = laserBeamScale;
        laserBeam.localPosition =
            turret.localPosition + 0.5f * d * laserBeam.forward;
        target.Enemy.ApplyDamage(damagePerSecond * Time.deltaTime);
    }

}
