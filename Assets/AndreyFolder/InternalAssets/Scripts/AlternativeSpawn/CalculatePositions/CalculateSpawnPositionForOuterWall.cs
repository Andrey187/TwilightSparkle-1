using System.Collections;
using UnityEngine;

public class CalculateSpawnPositionForOuterWall : ParamsForCalculateSpawnPositions
{
    public Transform[] _listOuterWall { get; set; }

    public void InitCollider()
    {
        foreach (Transform obj in _listOuterWall)
        {
            _colliderOuterWall = obj.GetComponent<BoxCollider>();
        }
        _colliderGround = _ground.GetComponent<BoxCollider>();
    }

    public IEnumerator PositionOnSide(Transform[] _wall)
    {
        yield return new WaitForSeconds(0f);
        if (_wall[0])
        {
            var fromCenterOfGroundUpPos = _ground.transform.TransformPoint(new Vector3(0f,
                _centerGround.y + zSizeGround / 2,
                _centerGround.z + zSizeGround / 2));

            _wall[0].transform.position = new Vector3(fromCenterOfGroundUpPos.x,
                fromCenterOfGroundUpPos.y + (_wall[0].transform.localScale.y / 2),
                fromCenterOfGroundUpPos.z - (_wall[0].transform.localScale.z / 2));
        }
        if (_wall[1])
        {
            var fromCenterOfGroundDownPos = _ground.transform.TransformPoint(new Vector3(0f,
                _centerGround.y + zSizeGround / 2,
                _centerGround.z - zSizeGround / 2));

            _wall[1].transform.position = new Vector3(fromCenterOfGroundDownPos.x,
                fromCenterOfGroundDownPos.y + (_wall[1].transform.localScale.y / 2),
                fromCenterOfGroundDownPos.z + (_wall[1].transform.localScale.z / 2));
        }
        if (_wall[2])
        {
            var fromCenterOfGroundLeftPos = _ground.transform.TransformPoint(new Vector3(
                _centerGround.x - xSizeGround / 2,
                _centerGround.y + zSizeGround / 2,
                0f));

            _wall[2].transform.position = new Vector3(fromCenterOfGroundLeftPos.x + (_wall[2].transform.localScale.y / 2),
                fromCenterOfGroundLeftPos.y + (_wall[2].transform.localScale.y / 2),
                0f);
            _wall[2].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        if (_wall[3])
        {
            var fromCenterOfGroundRightPos = _ground.transform.TransformPoint(new Vector3(_centerGround.x + xSizeGround / 2,
                _centerGround.y + zSizeGround / 2,
                0f));

            _wall[3].transform.position = new Vector3(fromCenterOfGroundRightPos.x - (_wall[3].transform.localScale.y / 2),
                fromCenterOfGroundRightPos.y + (_wall[3].transform.localScale.y / 2),
                0f);
            _wall[3].transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

    }

    public void ResizeWall()
    {
        for (int i = 0; i < _listOuterWall.Length; i++)
        {
            _listOuterWall[i].transform.localScale = new Vector3(xScaleOuterWall.x * _groundXSize, 1f, 1f);
        }
    }
}
