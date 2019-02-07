# Manhattan-Desktop

## In order to add new game objects:
 * Add prefab to Resources folder
 * Add new enum to MapResource.cs
 * Add info on location to HexMapController.cs
 * Attach MapObject script
    * Set type of object to correct type
    * Set object height as total height of the prefab

## Fixing the Async Error
Note, in the case of the following error:

```
error CS1644: Feature `asynchronous functions' cannot be used because it is not part of the C# 4.0 language specification
```

These changes must be made:

1. Add Async Await Support from the asset store
<img width="500" alt="screenshot 2019-01-05 10 29 26" src="https://user-images.githubusercontent.com/25430089/50724528-ac416480-10e6-11e9-863f-e6ef41e1f18d.png">

2. Open Player settings
<img width="300" alt="screenshot 2019-01-05 10 28 37" src="https://user-images.githubusercontent.com/25430089/50724534-c5e2ac00-10e6-11e9-957d-fcda590fcb54.png">

3. Change Scripting Runtime Version to 4.x
<img width="500" alt="screenshot 2019-01-05 12 37 11" src="https://user-images.githubusercontent.com/25430089/50724536-dbf06c80-10e6-11e9-971b-00b2f9f93fb4.png">
