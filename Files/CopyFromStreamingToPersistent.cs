 void CopyFromStreamingAssetsToPersistentDataPath(string fileName)
    {  //copies and unpacks file from apk to persistentDataPath where it can be accessed
        string destinationPath = System.IO.Path.Combine(Application.persistentDataPath, fileName);
        string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        //if DB does not exist in persistent data folder (folder "Documents" on iOS) or source DB is newer then copy it
        if (!System.IO.File.Exists(destinationPath) || (System.IO.File.GetLastWriteTimeUtc(sourcePath) > System.IO.File.GetLastWriteTimeUtc(destinationPath)))
        {
            Debug.Log("Start copy");
            if (sourcePath.Contains("://"))
            {// Android  
                WWW www = new WWW(sourcePath);
                while (!www.isDone) {; }                // Wait for download to complete - not pretty at all but easy hack for now 
                if (String.IsNullOrEmpty(www.error))
                {
                    System.IO.File.WriteAllBytes(destinationPath, www.bytes);
                    Debug.Log("Finish copy");
                }
                else
                {
                    Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                }
            }
            else
            {                // Mac, Windows, Iphone                
                             //validate the existens of the DB in the original folder (folder "streamingAssets")
                if (System.IO.File.Exists(sourcePath))
                {
                    //copy file - alle systems except Android
                    System.IO.File.Copy(sourcePath, destinationPath, true);
                    Debug.Log("Finish copy");
                }
                else
                {
                    Debug.Log("ERROR: the file DB named " + fileName + " doesn't exist in the StreamingAssets Folder, please copy it there.");
                }
            }
        }
    }