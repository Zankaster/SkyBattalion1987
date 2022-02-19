using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Linq;

public class LevelSpawner : MonoBehaviour {
    [SerializeField]
    public TextAsset levelDefinition;
    List<LevelObject> levelObjectsList;
    float totalTime = 0f;

    void Awake() {
        ReadLevel();
    }

    void Update() {
        totalTime += Time.deltaTime;
        for(int i = 0; i < levelObjectsList.Count; i++) {
            if( !levelObjectsList[i].spawned && totalTime >= levelObjectsList[i].time) {
                levelObjectsList[i].spawned = true;
                SpawnObject(levelObjectsList[i]);
            }
        }
    }

    void SpawnObject(LevelObject lo) {
        GameObject levelObj = ObjectPool.SharedInstance.GetPooledObject(lo.name);
        if (levelObj != null) {
            levelObj.transform.position = lo.position;
            levelObj.SetActive(true);
        }
    }

    void ReadLevel() {
        levelObjectsList = new List<LevelObject>();
        var splitFile = new string[] { "\r\n", "\r", "\n" };
        bool comment = false;

        var objectsLines = levelDefinition.text.Split(splitFile, StringSplitOptions.None);
        for (uint i = 0; i < objectsLines.Length; i++) {
            LevelObject levelObject = new LevelObject();
            if (objectsLines[i].Contains("*/")) {
                comment = false;
                continue;
            }
            else if (objectsLines[i].Contains("/*")) {
                comment = true;
                continue;
            }
            else if (comment) {
                continue;
            }
            else if (objectsLines[i] == "") {
                continue;
            }
            var line = objectsLines[i].Split(' ');
            for(uint j = 0; j < line.Length; j++) {
                if (line[j].Contains("name")) {
                    levelObject.name = line[j].Split(':')[1];
                }
                else if (line[j].Contains("position")) {
                    levelObject.position = new Vector2(float.Parse(line[j].Split(':')[1].Split(',')[0]), float.Parse(line[j].Split(':')[1].Split(',')[1]));
                }
                else if (line[j].Contains("time")) {
                    levelObject.time = float.Parse(line[j].Split(':')[1]);
                }
                else if (line[j].Contains("speed")) {
                    levelObject.speed = float.Parse(line[j].Split(':')[1]);
                }
            }
            if(levelObject.name != null && levelObject.name != "")
                levelObjectsList.Add(levelObject);
        }
    }
}
