# AiRPlane

Play Demo
--------
https://drive.google.com/file/d/11I-4QKY7rCSQm9ujKxON3esVoIMdymjD/view?usp=sharing


Overview
--------
### Win/lose condition:
Plane player wins the game by getting 20 plane flies through the hoop with only 5 planes that can be picked up only if they are not shot by a pellet. The shooter player wins the game by hitting 5 planes using an initial load of 10 pellets, which can also be refilled. 
### 2 goals
  - Main: Plane flying player shoots paper planes into the hoop
  - Secondary: Pellet player blocks the opponent by shooting down their planes
### 2 resources
(presented in a limited quantity at the start of the game; both players need to collect them after shooting out all the resources)
  - Paper planes: Plane player can pick up fallen planes to shoot again.
  - Ammo Box: Whenever the pellet person runs out of pellets, ammunition/supply box randomly appears within a range around the image target that needs to be picked up for refill. 
### More than 3 prebabs:
  - Paper plane
  - Pellet
  - Slingshot
  - Hoop
  - Ammunition box
 
Development notes:

AR Base Code:
- Collision detection: add colliders on paper plane, pellet, and hoop
- Clipping planes: change camera far clipping plane to 1000??
- Add to Prefabs to Resources folder for Photon to recognize them
- NetworkManager (prefab) has a Photon View component. Must add this component to anything you want to instantiate over the network.







### References
This project is based on https://github.com/Unity-Technologies/arfoundation-samples#interaction and https://github.com/googlecreativelab/balloon-pop.

