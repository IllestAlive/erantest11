using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions
{
    public static class Extensions
    {
        public static T PickRandom<T>(this List<T> list)
        {
            if (list == null || list.Count <= 0) return default;
            int index = Random.Range(0, list.Count);
            return list[index];
        }

        public static T PickRandom<T>(this T[] array)
        {
            return array.ToList().PickRandom();
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Capacity < 1 || list.Count < 1 || list.TrueForAll(x => x == null);
        }

        public static Vector3 ModifyXValue(this Vector3 original, float newValue)
        {
            return new Vector3(newValue, original.y, original.z);
        }
        public static Vector3 ModifyYValue(this Vector3 original, float newValue)
        {
            return new Vector3(original.x, newValue, original.z);
        }
        public static Vector3 ModifyZValue(this Vector3 original, float newValue)
        {
            return new Vector3(original.x, original.y, newValue);
        }

        public static Vector2 ModifyXValue(this Vector2 original, float newValue)
        {
            return new Vector2(newValue, original.y);
        }
        public static Vector2 ModifyYValue(this Vector2 original, float newValue)
        {
            return new Vector2(original.x, newValue);
        }
    }
}

