# 🔧 Unity Tools and Helpers
Useful tools and handy functions to use for Unity projects.

### :hammer: Get Started

- `git clone https://github.com/BrandonBartram98/Unity-ToolsAndHelpers` - Clone project
- Import scripts into Unity project

## Helpers/Extensions
### General Helpers
Useful functions for optimization and some that are just reguarly used.
#### Example
```c#
// Non-Allocating WaitForSeconds
// Reduce garbage collection by reusing WaitForSeconds if same wait time exists
private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

public static WaitForSeconds GetWait(float time)
{
    if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

    WaitDictionary[time] = new WaitForSeconds(time);
    return WaitDictionary[time];
}
```
### LatLongs
Scripts for converting latitude + longitudes into Unity meters and getting bearings between them.

#### Get Bearing
```c#
public float GetBearing(float lat1, float lon1, float lat2, float lon2)
{
    double φ1 = lat1 * Math.PI / 180; // φ, λ in radians
    double φ2 = lat2 * Math.PI / 180;
    double λ1 = lon1 * Math.PI / 180;
    double λ2 = lon2 * Math.PI / 180;

    // where φ1,λ1 is the start point, φ2,λ2 the end point (Δλ is the difference in longitude)
    double y = Math.Sin(λ2 - λ1) * Math.Cos(φ2);
    double x = Math.Cos(φ1) * Math.Sin(φ2) -
               Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(λ2 - λ1);
    double θ = Math.Atan2(y, x);
    double brng = (θ * 180 / Math.PI + 360) % 360; // in degrees

    return (float)brng;
}
```
#### Get Distance
```c#
public float GetDistance(float lat1, float lon1, float lat2, float lon2)
{
    // φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,371km)
    double R = 6371e3; // metres
    double φ1 = lat1 * Math.PI / 180; // φ, λ in radians
    double φ2 = lat2 * Math.PI / 180;
    double Δφ = (lat2 - lat1) * Math.PI / 180;
    double Δλ = (lon2 - lon1) * Math.PI / 180;

    double a = Math.Sin(Δφ / 2) * Math.Sin(Δφ / 2) +
               Math.Cos(φ1) * Math.Cos(φ2) *
               Math.Sin(Δλ / 2) * Math.Sin(Δλ / 2);
    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

    double d = R * c; // in metres

    return (float)d;
}
```
### VR
Useful Virtual Reality scripts.

#### Force Player Rig Rotation
```c#
[Header("XR Rig")]
[SerializeField] private Transform _xrRig;
[SerializeField] private Transform _rigOffset;
[SerializeField] private Transform _rigCamera;

[Header("Transforms")]
[SerializeField] private Transform _desiredTransform;

// Set the player position and rotation, useful to force rotation after loading scene
public void SetPlayerPositionAndRotation()
{
    Vector3 newPos = new Vector3(_desiredTransform.position.x, _xrRig.position.y, _desiredTransform.position.z);

    _xrRig.SetPositionAndRotation(newPos, _desiredTransform.rotation);

    _rigOffset.localRotation = Quaternion.identity;
    _rigOffset.localRotation = Quaternion.Inverse(Quaternion.Euler(0, _rigCamera.eulerAngles.y, 0)) * _xrRig.rotation;
}
```

## Prefabs/Plugins

### Custom Keyboard
A prefab + scripts for a Onscreen or VR/AR keyboard.

## :ghost: Contribute

```bash
git clone https://github.com/BrandonBartram98/Unity-ToolsAndHelpers
```
