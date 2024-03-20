using System.Collections.Generic;
using UnityEngine;

namespace _game.Extentions
{
    public static class TransformExtensions
    {
        #region Position

        public static void SetPositionX(this Transform transform, float value)
        {
            var pos = transform.position;
            pos.x = value;
            transform.position = pos;
        }

        public static void SetPositionY(this Transform transform, float value)
        {
            var pos = transform.position;
            pos.y = value;
            transform.position = pos;
        }

        public static void SetPositionZ(this Transform transform, float value)
        {
            var pos = transform.position;
            pos.z = value;
            transform.position = pos;
        }

        public static void SetLocalPositionX(this Transform transform, float value)
        {
            var pos = transform.localPosition;
            pos.x = value;
            transform.localPosition = pos;
        }

        public static void SetLocalPositionY(this Transform transform, float value)
        {
            var pos = transform.localPosition;
            pos.y = value;
            transform.localPosition = pos;
        }

        public static void SetLocalPositionZ(this Transform transform, float value)
        {
            var pos = transform.localPosition;
            pos.z = value;
            transform.localPosition = pos;
        }

        public static float GetPositionX(this Transform transform)
        {
            return transform.position.x;
        }

        public static float GetPositionY(this Transform transform)
        {
            return transform.position.y;
        }

        public static float GetPositionZ(this Transform transform)
        {
            return transform.position.z;
        }

        public static float GetLocalPositionX(this Transform transform)
        {
            return transform.localPosition.x;
        }
        
        public static float GetLocalPositionY(this Transform transform)
        {
            return transform.localPosition.y;
        }
        
        public static float GetLocalPositionZ(this Transform transform)
        {
            return transform.localPosition.z;
        }

        public static float Magnitude(this Transform transform)
        {
            return transform.position.magnitude;
        }

        #endregion

        #region Rotation

        public static void SetEulerX(this Transform transform, float value)
        {
            var rot = transform.eulerAngles;
            rot.x = value;
            transform.eulerAngles = rot;
        }

        public static void SetEulerY(this Transform transform, float value)
        {
            var rot = transform.eulerAngles;
            rot.y = value;
            transform.eulerAngles = rot;
        }

        public static void SetEulerZ(this Transform transform, float value)
        {
            var rot = transform.eulerAngles;
            rot.z = value;
            transform.eulerAngles = rot;
        }

        public static void SetLocalEulerX(this Transform transform, float value)
        {
            var rot = transform.localEulerAngles;
            rot.x = value;
            transform.localEulerAngles = rot;
        }

        public static void SetLocalEulerY(this Transform transform, float value)
        {
            var rot = transform.localEulerAngles;
            rot.y = value;
            transform.localEulerAngles = rot;
        }

        public static void SetLocalEulerZ(this Transform transform, float value)
        {
            var rot = transform.localEulerAngles;
            rot.z = value;
            transform.localEulerAngles = rot;
        }

        #endregion
        

        public static IEnumerable<Transform> GetChildren(this Transform transform)
        {
            var childCount = transform.childCount;
            var children = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            return children;
        }
    }
}