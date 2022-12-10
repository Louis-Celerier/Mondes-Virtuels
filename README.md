# Mondes-Virtuels

## Scripts

### PlanetGravity

Contains the weight of a planet.
Weight is randomly generated.

### PlayerMove

Script that allows the player to move while being affected by the gravity from all planets.

The gravity affecting the player is computed for each planet and added.
That allows the player to be attracted by the nearest and heaviest planet.

The player has a speed and a weight that is ajusted by the user/developer.

The rigidbody permits the application of multiple forces and the use of colliders.

The boolean "isGrounded" keeps track of if the player is on a planet ground or not.

The gravity is computed with this formula : currentForce = (6.67f * masse * masseCurrentPlanet / (distance * distance)) * direction.normalized;
with "distance" standing for the distance between the player and the current planet used in the formula.
We then add the result of the application of this formula on all planets to compute the final gravity affecting the player.
The force is a vector and its direction will be the most attracting planet.
Source for the formula : https://fr.wikipedia.org/wiki/Gravitation

To keep the player standing, we use interpolation between the direction of the player going to the selected planet
and the "up" vector of the player gameObject and use the result as the new "up" vector of the player.

We also use the collider functions of unity when entering or exiting a collider :

When entering we cancel the previous acceleration, update the "isGrounded" boolean and keep track of the current collision.
When exiting we just update the "isGrounded" boolean and remove the tracking of the previous collision.
