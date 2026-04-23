# How to build Workshop CLI

1. Open JetBrains Rider.
2. Click on the hammer, in the top right corner, to `build` the project.
   ![alt text](image3.png)
3. If it didn't work, go to the `workshop-cli` folder, look for a file called `Install.bat`, and run that file as administrator.
4. Check that the workshop-cli folder is in the right location, as shown in the image.
   ![alt text](image2.png)

5. If `python` gives an error when running the project, check if the python installation is on the environment variables.

<img src="image4.png" alt="alt text" width="300">

6. If `Install.bat` looks stuck while pulling the `qwen2.5:3b` Ollama model (the *"Pulling the model (~2 GB)"* step), give it a few minutes — on a slow connection this can take 2–8 minutes and the progress only refreshes occasionally. Only close the window and re-run `Install.bat` if the download is clearly frozen (no network activity for several minutes).
![alt text](image5.png)

