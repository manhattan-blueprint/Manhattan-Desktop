using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapObject
{
    // This class holds info on a specific object, such as string ID, whether
    // it has any things inside etc

    public bool instantiated;

    public MapResource mapResource;

    public GameObject gameObject;

    public Quaternion rotation;

    public Vector3 position;

    // Whenever the holder is freshly created
    public MapObject(MapResource inpResourceType, Vector3 inpPosition, Quaternion inpRotation)
    {
        this.instantiated = true;

        this.mapResource = inpResourceType;

        this.position = inpPosition;

        this.rotation = inpRotation;

        this.gameObject = MonoBehaviour.Instantiate(GenerateHex.resourceDict[inpResourceType],
                                                    inpPosition,
                                                    inpRotation);
    }

    // TODO: needs some way of deleting the instance of this class too
    public void Remove()
    {
        MonoBehaviour.Destroy(this.gameObject);
    }
}
