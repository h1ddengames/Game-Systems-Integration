# Game-Systems-Integration

Simple 2D RPG that combines various game systems.

## How to use this project

If you want to download this project in order to extend/modify it yourself or if you would like to create a build for an OS other than Windows, follow these instructions:

1. Clone or download this project using the green icon near the top of this page.
2. Unzip the project files if downloaded.
3. Open the project in Unity using the Unity Hub.
4. On Unity Hub, select Projects.
5. Then select Add.
6. On the file explorer, find the project you cloned or downloaded then click Select Folder.
7. On Unity Hub, double click on this project's name in the Projects section.
8. Once the project is open, expand the scenes folder and open the Default scene.

---

## Important code helpers being used in this project

- Editor Toolbox - Used for Attributes in the Unity Inspector found here: [Editor Toolbox](https://github.com/arimger/Unity-Editor-Toolbox)
- NaughtyAttributes - Used for Attributes in the Unity Inspector found here: [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)
- Serialized Dictionary Lite - Used to see dictionaries in the Unity Inspector found here: [Serialized Dictionary Lite](https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-lite-110992)
- JSON Object - Used to convert a GameObject into a JSON object found here: [JSON Object](https://assetstore.unity.com/packages/tools/input-management/json-object-710)
- StandaloneFileBrowser - Used to open a file/folder dialog for the player to select a file/folder during gameplay/saving found here: [Standalone File Browser](https://github.com/gkngkc/UnityStandaloneFileBrowser)

---

## Setting up a Script Template

- In order to create a custom script template go to the following location: C:\Program Files\Unity\Hub\Editor\2019.3.0b11\Editor\Data\Resources\ScriptTemplates
- Open the file 81-C# Script-NewBehaviourScript.cs.txt
- Replace the contents with the script template you'd like to use.
- Here is the script template I use:

```C#
// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
	public class #SCRIPTNAME# : MonoBehaviour {
		#region Exposed Fields
		#endregion

		#region Private Fields
		#endregion
		
		#region Getters/Setters/Constructors
		#endregion

		#region My Methods
		#endregion
		
		#region Unity Methods
		void OnEnable() {
			#NOTRIM#
		}
		
		void Start() {
			#NOTRIM#
		}

		void Update() {
			#NOTRIM#
		}
		
		void OnDisable() {
			#NOTRIM#
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}
```

---

## Local Webserver Setup

## Creating a Docker container

- Creates a Docker container, then maps port 80 in the container to port 80 on the host, then maps port 443 in the container to port 443 on the host, then names the container 'webserver', then mounts the folder as '/data', and uses a Ubuntu image.

    ```docker
    docker run -dit -p 80:80 -p 443:443 --name webserver -v C:/Users/h1ddengames/Desktop/Items/Builds/WebGL:/data ubuntu
    ```

## Logging into the Docker container

- Logs into the container using /bin/bash shell

    ```docker
    docker exec -it webserver /bin/bash
    ```

## Stopping the Docker container

- Stops the docker container but does not delete it. Can be restarted at a later time.

    ```docker
    docker stop webserver
    ```

## Deleting the Docker container

- Deletes the Docker container and all the data stored within.

    ```docker
    docker rm webserver
    ```

## Starts the Docker container

- Starts the docker container

    ```docker
    docker start webserver
    ```

## Setting up the server instance

- Updates the package list, then upgrades all currently installed software, and finally installs apache2

    ```bash
    apt update -y && apt upgrade -y && apt install apache2 -y
    ```

## Starting the apache service

- Starts the apache service.

    ```bash
    service apache2 start
    ```

## Moving files to the hosting directory

- Copies recursively and asks if files should be overwritten if the names are the same.

    ```bash
    cp -ir /data/* /var/www/html/
    ```

---
