using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains a bunch of helper methods for all sorts of tasks.
/// </summary>
public static class Helpers
{
    public static readonly Vector2Int[] directions2D = new Vector2Int[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

    public static readonly Vector3Int[] directions3DFlat = new Vector3Int[] { new Vector3Int(1, 0, 0), new Vector3Int(0, 0, 1), new Vector3Int(-1, 0, 0), new Vector3Int(0, 0, -1)};

    public static readonly Vector3Int[] directions3DVertical = new Vector3Int[] { new Vector3Int(0, 1, 0), new Vector3Int(0, -1, 0)};

    /// <summary>
    /// Random int from 0 to max (ex).
    /// </summary>
    public static int Rand(int max)
    {
        return Random.Range(0, max);
    }

    /// <summary>
    /// Random int from min to max (ex).
    /// </summary>
    public static int Rand(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static float AngleRad(Vector3 from, Vector3 to)
    {
        var dir = from.ToNorm(to);
        return Mathf.Atan2(dir.y, dir.x);
    }
    
    public static float Angle(Vector3 dir)
    {
        dir = dir.normalized;
        return Mathf.Atan2(dir.y, dir.x);
    }

    /// <summary>
    /// Random float from 0 to max (inc).
    /// </summary>
    public static float Rand(float max)
    {
        return Random.Range(0f, max);
    }

    /// <summary>
    /// Random float from min to max (inc).
    /// </summary>
    public static float Rand(float min, float max)
    {
        return Random.Range(min, max);
    }
    
    /// <summary>
    /// Random float from range.x to range.y (inc).
    /// </summary>
    public static float Rand(Vector2 range)
    {
        return Rand(range.x, range.y);
    }

    /// <summary>
    /// Random float from 0f to 1f(ex).
    /// </summary>
    public static float RandFloat()
    {
        return Random.Range(0f, 1f);
    }
    /// <summary>
    /// Random int from 0 to 100(ex)
    /// </summary>
    public static int RandPercent()
    {
        return Random.Range(0, 100);
    }

    /// <summary>
    /// A percent check
    /// </summary>
    public static bool RandPercent(int chance)
    {
        return RandPercent()< chance;
    }

    public static bool RandBool()
    {
        return RandPercent()< 50;
    }
    /// <summary>
    /// Random vector3. All values from -1f to 1f.
    /// </summary>
    public static Vector3 RandVector3()
    {
        return new Vector3(Random.Range(-1f, 1), Random.Range(-1f, 1), Random.Range(-1f, 1));
    }

    public static Vector3 RandVector2(Vector2 min, Vector2 max)
    {
        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }

    public static Color RandColor()
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
    }

    public static Color RandColor(float alpha)
    {
        return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f), alpha);
    }

    public static int ZeroOutside(int value, int min, int max)
    {
        if (value < min || value > max)
            return 0;
            
        return value;
    }
    
    public static int ZeroInside(int value, int min, int max)
    {
        if (value > min && value < max)
            return 0;
            
        return value;
    }

    public static Color RandColor(Color c1, Color c2)
    {
        return Color.Lerp(c1, c2, RandFloat());
    }

    public static T RandEnum<T>()
    {
        return Rand(EnumValues<T>());
    }

    public static T RandParam<T>(params T[] ts)
    {
#if UNITY_EDITOR
        if (ts.Count()== 0)
        {
            Debug.LogError("Rand error!");
            return default(T);
        }
#endif

        return ts.ElementAt(Rand(ts.Count()));
    }

    public static T Rand<T>(IEnumerable<T> enumerable)
    {
#if UNITY_EDITOR
        if (enumerable == null || enumerable.Count()== 0)
        {
        Debug.LogError("Rand error!");
        return default(T);
        }
#endif

        return enumerable.ElementAt(Rand(enumerable.Count()));
    }

    public static T RandRemove<T>(List<T> list)
    {
        T v = Rand(list);
        list.Remove(v);
        return v;
    }

    public static int RandWeighted(int[] weights, int totalWeights)
    {
        int rand = Rand(totalWeights);
        for (int i = 0; i < weights.Length; i++)
        {
            rand -= weights[i];
            if (rand < 0)
                return i;
        }

        throw new System.Exception("Weighted random issue!");
    }

    public static T RandWeighted<T>(T[] values, int[] weights, int totalWeights)
    {
        int rand = Rand(totalWeights + 1);
        for (int i = 0; i < weights.Length; i++)
        {
            rand -= weights[i];
            if (rand <= 0)
                return values[i];
        }

        throw new System.Exception("Weighted random issue!");
    }

    public static T RandWeightedLength<T>(T[] values, int[] weights, int length)
    {
        int totalWeights = 0;
        for (int i = 0; i < length; i++)
        {
            totalWeights += weights[i];
        }

        int rand = Rand(totalWeights + 1);
        for (int i = 0; i < length; i++)
        {
            rand -= weights[i];
            if (rand <= 0)
                return values[i];
        }

        throw new System.Exception("Weighted random issue!");
    }

    //area
    public static bool InsideArea(Vector2 Position, Rect area)
    {
        return (Position.x >= area.x && Position.x < area.x + area.width && Position.y >= area.y && Position.y < area.y + area.height);
    }

    public static bool InsideArea(int x, int y, int ax, int ay, int aw, int ah)
    {
        return (x >= ax && x < ax + aw && y >= ay && y < ay + ah);
    }

    public static bool InsideArea(float x, float y, int ax, int ay, int aw, int ah)
    {
        return (x >= ax && x < ax + aw && y >= ay && y < ay + ah);
    }

    public static bool OutsideArea(Vector2 Position, Rect area)
    {
        return !InsideArea(Position, area);
    }

    //strings
    public static string[] Split(string str, string separator)
    {
        return str.Split(new string[] { separator }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    //range
    public static bool OutsideRange(Vector3 v, float min, float max)
    {
        return (v.x < min || v.x >= max || v.y < min || v.y >= max || v.z < min || v.z >= max);
    }
    public static bool OutsideRange(Vector3 v, Vector3 min, Vector3 max)
    {
        return (v.x < min.x || v.x >= max.x || v.y < min.y || v.y >= max.y || v.z < min.z || v.z >= max.z);
    }

    //distance
    public static float DistanceSquared(Vector2 pos1, Vector2 pos2)
    {
        float dx = pos1.x - pos2.x;
        float dy = pos1.y - pos2.y;
        return (int)((dx * dx)+ (dy * dy));
    }

    public static float Distance(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position1, position2);
    }

    public static float Distance(Transform transform, Vector3 position)
    {
        return Vector3.Distance(transform.position, position);
    }

    public static float Distance(Vector3 position, Transform transform)
    {
        return Vector3.Distance(transform.position, position);
    }

    public static float Distance(Transform transform1, Transform transform2)
    {
        return Vector3.Distance(transform1.position, transform2.transform.position);
    }

    /// <summary>
    /// Changes the enable state of the component if it's not in the same state already.
    /// </summary>
    public static void EnableComponent(Behaviour mb, bool enable)
    {
        if (mb.enabled != enable)mb.enabled = enable;
    }
    /// <summary>
    /// Changes the active state of the object if it's not in the same state already.
    /// </summary>
    public static void SetActive(GameObject obj, bool active)
    {
        if (obj.activeSelf != active)obj.SetActive(active);
    }
    /// <summary>
    /// Gets angle around local y axis of the parent from a world space direction
    /// </summary>
    public static float GetAngleTowardsDirection(Transform parent, Vector3 worldDirection)
    {
        Vector3 local = parent.InverseTransformDirection(worldDirection);
        return Mathf.Atan2(local.x, local.z)* Mathf.Rad2Deg;
    }
    /// <summary>
    /// Returns a signed angle between two direction vectors around an arbitrary axis
    /// </summary>
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 axis)
    {
        return Mathf.Atan2(Vector3.Dot(axis, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2))* Mathf.Rad2Deg;
    }

    public static float DistanceOnHorizontalPlane(Vector3 pos1, Vector3 pos2)
    {
        pos1.y = pos2.y;
        return Vector3.Distance(pos1, pos2);
    }

    /// <summary>
    /// Returns all values of an enum in an iterable collection. Usable in a foreach loop.
    /// </summary>
    public static IEnumerable<T> EnumValues<T>()
    {
        return System.Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static List<T> FindRecursive<T>(Transform transform)
    {
        var result = new List<T>();
        FindRecursiveR(transform, result);
        return result;
    }


    private static void FindRecursiveR<T>(Transform transform, List<T> result)
    {
        result.Add(transform.GetComponent<T>());

        foreach (Transform t in transform)
        {
            FindRecursiveR(t, result);
        }
    }
    
    public static List<int> GetIndexList(int count)
    {
        var list = new List<int>();
        for (int i = 0; i < count; i++)
        {
            list.Add(i);
        }
        return list;
    }

    //Unity specifics
    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static bool ScreenPointToObject3D<T>(Vector3 point, float distance, int layerMask, out T entity)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit info;
        if (Physics.Raycast(ray, out info, distance, layerMask))
        {
            entity = info.transform.GetComponentInParent<T>();
            return true;
        }
        entity = default(T);
        return false;
    }

    public static bool ScreenPointToObject2D<T>(Vector3 point, int layerMask, out T entity)
    {
        var info = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(point), Vector2.zero, 0.1f, layerMask);

        if (info)
        {
            entity = info.transform.GetComponentInParent<T>();
            return true;
        }
        entity = default(T);
        return false;
    }

    //Vector3 extensions
    /// <summary>
    /// Returns the direction vector from from to to.
    /// </summary>
    public static Vector3 To(this Vector3 from, Vector3 to)
    {
        return (to - from);
    }

    /// <summary>
    /// Returns the normalized direction vector from from to to.
    /// </summary>
    public static Vector3 ToNorm(this Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }

    /// <summary>
    /// Returns the distance from from to to.
    /// </summary>
    public static float ToDistance(this Vector3 from, Vector3 to)
    {
        return Vector3.Distance(from, to);
    }

    //Transform extensions
    /// <summary>
    /// Returns the direction vector from from to to.
    /// </summary>
    public static Vector3 To(this Transform from, Transform to)
    {
        return (to.position - from.position);
    }

    /// <summary>
    /// Returns the normalized direction vector from from to to.
    /// </summary>
    public static Vector3 ToNorm(this Transform from, Transform to)
    {
        return (to.position - from.position).normalized;
    }

    /// <summary>
    /// Returns the distance from from to to.
    /// </summary>
    public static float ToDistance(this Transform from, Transform to)
    {
        return Vector3.Distance(from.position, to.position);
    }

    //Ray extensions

    /// <summary>
    /// Draws the ray with a color and distance.
    /// Debug use only.
    /// </summary>
    public static void Draw(this Ray ray, float distance, Color color)
    {
        Debug.DrawRay(ray.origin, ray.direction * distance, color);
    }

    public static Vector3 LerpOvershoot(Vector3 start, Vector3 end, float percent)
    {
        return start + start.To(end)* percent;
    }

    /// <summary>
    /// Draws the ray with a color.
    /// Debug use only.
    /// </summary>
    public static void Draw(this Ray ray, Color color)
    {
        Debug.DrawRay(ray.origin, ray.direction, color);
    }

    public static bool IsDemo()
    {
        return UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings < 3;
    }

    public static bool PointerOverGUI()
    {
#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current == null)return false;
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 0)
            return true;
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#else
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
#endif
    }

    public static int Wrap(int index, int min, int max)
    {
        if (index >= max)
            return min + (index - max);
        if (index < min)
            return max - (min - index);

        return index;
    }

    public static float Wrap(float value, float min, float max)
    {
        if (value >= max)
            return min + (value - max);
        if (value < min)
            return max - (min - value);

        return value;
    }

}