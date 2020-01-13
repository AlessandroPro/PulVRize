# PulVRize

Semi-finalist in Best Gameplay at Level Up 2019 <br>

Trailer: https://www.youtube.com/watch?v=PvLaZvu0beI <br>
Gameplay video: https://www.youtube.com/watch?v=3j0KoOLg3ck <br>

The name is multi-pun, since the game involves pulling, rising, VR, and pulverizing monsters.

![logoFINAL3](https://user-images.githubusercontent.com/15040875/72230574-6a15f300-3584-11ea-9139-647e67d2ad88.png)

### Description 

This is PulVRize, a fast-paced, single-player survival game in virtual reality. You, as the player, are alone in an abyss of ancient ruins, filled with floating monsters that will actively approach you. When they get too close to you, they'll suck your soul out until your health is completely drained. Then it's GAME OVER. There is only one way to destroy the monsters: by crushing them between the pillars! Do this by first pointing and grabbing at a pillar below or above you using your extendable hand device, then pull the pillar up or down to pulverize any creatures in its way. Push away the creatures by sending out a white halo force ring, which is a resource replenished only by the souls of each creature you destroy. The goal is to achieve a high score, based on your survival time and how many creatures you crushed! 

Playable with the HTC Vive on PC.

![HowToPlayFinal](https://user-images.githubusercontent.com/15040875/72230805-dfce8e80-3585-11ea-90fd-c982e3ba40ab.jpg)

### Requirements

Standing-only setup with the HTC Vive VR headset and two controllers.
PC with SteamVR.

The most recent version of the game was built using Unity 2017.3.1, which is also the version used to develop the game. To create a new build, open Unity, go to File > Build and then make sure the scene ‘MainScene.unity’ is added. This program only has one scene. Then build.

Running the newly created executable should work if you have the VR headset plugged in and SteamVR installed.

### Implementation


### Scene:

This game was developed as one scene in Unity. To interface with the VR hardware, the SteamVR 2.2.0 API was downloaded and used. The scene was constructed with an array of manually placed column (aka pillar) pairs. The camera rig was placed on one of these columns at the edge of the array, where the user would stand in place through the entirety of the game. The skybox is a blackish-grey colour throughout, and built-in fog is used to create an eerie, dark, abyss atmosphere.

### Game Flow:

Game loads and fades in, player sees array of columns in a dark abyss with the controllers in their hand. The logo floats in space in front of them, with the options of pressing the trigger to see instructions or side grip to start the game. When the game is started, the logo, which is on a canvas, fades away, and monsters start spawning far away from the player. They approach the player, while the player pushes them with force rings and crushes them between pillars to survive. As time goes on, the creatures get faster and spawn more frequently. When a creature gets within a meter or so of the player, they stop and start sucking out the player’s soul. This depletes the player’s preset health value. When this gets to zero, the screen fades, the game fades to black and then fades back to the scene, where they player can now only see a GAME OVER text on a canvas with their final score. The final score is a sum of the number of the number of monsters defeated and the number of seconds the player survived to the power of 1.1.
The game usually lasts between two and four minutes. 

![11](https://user-images.githubusercontent.com/15040875/72230576-6f733d80-3584-11ea-9506-fc4c6d02f09a.PNG)
![12](https://user-images.githubusercontent.com/15040875/72230577-700bd400-3584-11ea-941d-7cdc1956026d.PNG)
![13](https://user-images.githubusercontent.com/15040875/72230578-700bd400-3584-11ea-8bb7-daafe9aa422a.PNG)

### List of used Prefabs:

All other objects are pre-existing, single-use objects manually placed within the scene before the game loads.

•	AbyssCreature* <br>
•	ColumnPair* <br>
•	Column* <br>
•	DamageArrow <br>
•	DeathExplosion <br>
•	DustPuff <br>
•	ForceField <br>
•	GrabLight <br>
•	Hand <br>
•	LifeOrb <br>
•	LineSegment <br>
•	Soul_EnergyOrb <br>


### Sounds:

The game uses background music and sound effects. There are sound effects for the columns moving and colliding, force field launching, idle monster noises, and monster screams for when they get hit or die.


### Third-Party Assets:

Assets used within the game that aren't mine are the following:
 
•	SteamVR <br>
•	the 3D models for the columns/pillars and monsters <br>
•	monster sounds and background music <br>
•	some particle effects <br>

The downloaded third-party folders in the Assets folder are:

•	Fainward <br>
•	Flames of the Phoenix <br>
•	Particlecollection_Free samples <br>
•	SteamVR <br>
•	Stone columns <br>
•	True_Horror <br>


