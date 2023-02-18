# AiRPlane

### Win/lose condition:
Plane player has to get 5 planes through the hoop within a limited amount of time. Otherwise the pellet shooting player wins the game. 
### 2 goals
  - Main: Plane flying player shoots paper planes into the hoop
  - Secondary: Pellet player blocks the opponent by shooting down their planes
### 2 resources
(presented in a limited quantity at the start of the game; players need to collect them by picking them up after shooting out all the resources)
  - Paper planes: Can pick up fallen planes to shoot again
  - Ammo Box: Whenever the pellet person runs out of pellets, ammunition/supply box appears on the image target that need to be picked up for refill.
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
This project is based on https://github.com/Unity-Technologies/arfoundation-samples#interaction

