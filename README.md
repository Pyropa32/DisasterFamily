# Disaster Family
## _Point and click Disaster Prevention_

Disaster family is a 2D Unity game about a dilligent father who is caught up in an earthquake disaster! It's time to double down: Only 60 seconds remain to pack up all your belongings and brave the impending earthquake!

## Installation (via Wizard):

If you like, you can obtain and run the installer for Disaster Family repo by doing the following:
```
git clone https://github.com/Pyropa32/disaster-family-innounp
cd disaster-family-innounp
./disaster-family-installer-0.1.exe
```

## Build From Source:

For Disaster Family itself, obtain the source as following:
```
git clone https://github.com/Pyropa32/DisasterFamily.git
cd DisasterFamily
...
```
### Build using Unity Editor GUI
If you want to modify source and build, you can open the project using the `Unity` editor installed on your local machine *(requires 2022.3.32f1 and up)*, navigate to the menu bar, and then click `File/Build and Run`.

### Build via Command Line Interface (CLI)
Building and running Disaster Family through the CLI for Windows can be handled with the following:
```
cd DisasterFamily
C:\path\to\Unity.exe -quit -batchmode -nographics -buildTarget StandaloneWindows64 -executeMethod Builder.Build -projectPath .
cd ..\StandaloneWindows
./DisasterFamily.exe
```
