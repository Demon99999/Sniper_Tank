using UnityEngine;

namespace Assets.Scripts.Services.SaveLoadProgressServices
{
    public static class DataExtansions
    {
        public static T ToDeserialized<T>(this string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static string ToJson(this object obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
