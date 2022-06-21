using LitJson;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public interface ISaveable
{
    public string SaveID { get; }
    public JsonData SaveData { get; }

    public void LoadFromData(JsonData data);

}

public static class SavingService
{
    private const string ACTIVE_SCENE_KEY = "ActiveScene";
    private const string SCENES_KEY       = "Scenes";
    private const string OBJECTS_KEY      = "Objects";
    private const string SAVEID_KEY       = "$SaveID";

    static UnityEngine.Events.UnityAction<Scene, LoadSceneMode> _loadObjectsAfterSceneLoad;

    public static void SaveGame(string file)
    {
        var res = new JsonData();

        var allSaveableObjects = Object.FindObjectsOfType<MonoBehaviour>()
            .OfType<ISaveable>();

        if (allSaveableObjects.Count() > 0)
        {
            var savedObjects = new JsonData();

            foreach (var saveableObject in allSaveableObjects)
            {
                var data = saveableObject.SaveData;

                if (data.IsObject)
                {
                    data[SAVEID_KEY] = saveableObject.SaveID;
                    savedObjects.Add(data);
                }
                else
                {
                    var behaviour = saveableObject as MonoBehaviour;
                    Debug.LogWarningFormat(
                        behaviour,
                        "{0}'s save data is not a dictionary. The " +
                        "object was not saved.",
                        behaviour.name
                        );
                }
            }

            res[OBJECTS_KEY] = savedObjects;
        }
        else
        {
            Debug.LogWarningFormat("The scene did not include any saveable objs.");
        }

        var openScene = new JsonData();

        var sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; ++i)
        {
            var scene = SceneManager.GetSceneAt(i);
            openScene.Add(scene.name);
        }

        res[SCENES_KEY] = openScene;
        res[ACTIVE_SCENE_KEY] = SceneManager.GetActiveScene().name;

        var outputPath = Path.Combine(Application.persistentDataPath, file);

        var writer = new JsonWriter();
        writer.PrettyPrint = true;

        res.ToJson(writer);

        File.WriteAllText(outputPath, writer.ToString());

        Debug.LogFormat("Wrote saved game to {0}", outputPath);

        res = null;
        System.GC.Collect();
    }
    public static bool LoadGame(string file)
    {
        var dataPath = Path.Combine(Application.persistentDataPath, file);

        if (!File.Exists(dataPath))
        {
            Debug.LogErrorFormat("No file exists at {0}", dataPath);
            return false;
        }

        var text = File.ReadAllText(dataPath);

        var data = JsonMapper.ToObject(text);

        if (data == null || !data.IsObject)
        {
            Debug.LogErrorFormat("Data at {0} is not a JSON object", dataPath);
            return false;
        }

        if (!data.ContainsKey(SCENES_KEY))
        {
            Debug.LogWarningFormat(
                "Data at {0} does not contain any scenes; not " 
                + "loading any!", 
                dataPath
                );
            return false;
        }

        var scenes = data[SCENES_KEY];

        int sceneCount = scenes.Count;

        if (sceneCount == 0)
        {
            Debug.LogWarningFormat(
                "Data at {0} doesn't specify any scenes to load.",
                dataPath
                );
            return false;
        }

        for (int i = 0; i < sceneCount; ++i)
        {
            var scene = (string)scenes[i];

            if (i == 0)
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }
        }

        if (data.ContainsKey(ACTIVE_SCENE_KEY))
        {
            var activeSceneName = (string)data[ACTIVE_SCENE_KEY];

            var activeScene = SceneManager.GetSceneByName(activeSceneName);

            if (!activeScene.IsValid())
            {
                Debug.LogErrorFormat(
                        "Data at {0} specifies an active scene that " +
                        "doesn't exist. Stopping loading here.",
                        dataPath
                    );
                return false;
            }

            SceneManager.SetActiveScene(activeScene);
        }
        else
        {
            Debug.LogWarningFormat(
                "Data at {0} does not specify an " + 
                "active scene.", 
                dataPath
                );
        }

        if (data.ContainsKey(OBJECTS_KEY))
        {
            var objects = data[OBJECTS_KEY];

            _loadObjectsAfterSceneLoad = (Scene, LoadSceneMode) =>
            {
                var allLoadableObjects = Object.FindObjectsOfType<MonoBehaviour>()
                    .OfType<ISaveable>()
                    .ToDictionary(o => o.SaveID, o => o);

                var objectsCount = objects.Count;

                for (int i = 0; i < objectsCount; ++i)
                {
                    var objectData = objects[i];

                    var saveID = (string)objectData[SAVEID_KEY];

                    if (allLoadableObjects.ContainsKey(saveID))
                    {
                        var loadableObject = allLoadableObjects[saveID];
                        loadableObject.LoadFromData(objectData);
                    }
                }

                SceneManager.sceneLoaded -= _loadObjectsAfterSceneLoad;

                _loadObjectsAfterSceneLoad = null;

                System.GC.Collect();
            };
            SceneManager.sceneLoaded += _loadObjectsAfterSceneLoad;
        }

        return true;
    }
}
