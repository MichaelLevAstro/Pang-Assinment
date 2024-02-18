# Pang-Assingment
 
Extra credit:

- Made with the MVC Paradigm
- 3 cosecutive levels with increasing difficulty

Some known bugs:
- Using physics meant every once in a while an enemy might fly out of the board, but rarely happens.
would've loved to utilize the enemy motions without physics.
- Also related to the way i implemented the enemies, after resuming the game from the pause popup, they sometimes get extra velocity to the up direction (if their low enough).
- No game completion screen, finishing the last level you can say continue to next level but nothing will happen.
Ideally i would've made another popup stating the end of the game with only a quit and retry buttons.
- Score is always saved, even if you lose the game.
I would've made a Session state service that holds the score without saving it as the players data, and only updates the ui.
If game lost the controller would ask for the real state of the score and revert to that.
