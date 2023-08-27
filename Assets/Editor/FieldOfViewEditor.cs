using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        

        Vector3[] viewAngles = new Vector3[] { DirectionFromAngle(fov.transform.eulerAngles.y, -fov.GetAngle()/2),
                                               DirectionFromAngle(fov.transform.eulerAngles.y,  fov.GetAngle()/2)};

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, viewAngles[0], fov.GetAngle(), fov.GetRadius());

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngles[0] * fov.GetRadius());
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngles[1] * fov.GetRadius());

        if (fov.CanSeePlayer())
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.Player().transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
