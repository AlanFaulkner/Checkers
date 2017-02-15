# Checkers

15/2/17 Checkers version 1.0 

Overview
This project is intended to showcase the ability of neural networks to learn to play a game of checkers. No serious attempts have been made to optimise either the code itself or the overall performance of resulting network. This project represents my own work and relies only on standard libraries included with C# .Net framework version 4.5.2.

Background
A few months I was looking for some project with which I could expand my rather basic knowledge of C# (I’d really only completed a few introductory tutorials online at this point), when I decided to try and code a simple game which employed a windows form interface to play.  To that end I decided to focus on checkers (https://en.wikipedia.org/wiki/Draughts). While the interface I created has gone through several incarnations prior to the one now present in the code my original attempt was limited to only human vs human gameplay. Realising the need for an AI I did some browsing online looking for some suggestions on how to implement one. My first attempt was to include a MinMax player (https://en.wikipedia.org/wiki/Minimax|), which while successful I still found rather simplistic. Wishing to further improve the complexity of my AI player and after some more research online I came across the idea of implementing a neural network as the scoring function within the MinMax algorithm. In particular this method, first proposed by Fogel in his work on the checkers playing bot Blondie24 (https://en.wikipedia.org/wiki/Blondie24), showed that such an implantation led to an expert player capable of beating most humans. Being new to both coding and machine learning I decided that rather than just finding a library that implements neural networks behind the scenes I would implement my own from scratch, with the help of both Wikipedia and stackexchange. The result as shown here is capable of not only playing checkers but also constant improvement though either interaction with a human opponent or playing against itself/a second neural network.

Description of the function of each class
Program:
runs program, allows user to either evolve a network from scratch, improve an existing network or call and instance of the windows for to actually play against the network.
User_Interface: 
all functions related to windows form. Gets input from human player/s updates game board, keeps track of scores, initialises an AI player which runs as a background worker.
UIGamePlay :
 Contains functions related to playing the game. Differs slightly form CheckersGamePlay as due to the addition of functions relating to AI and no looped gameplay as interface is input driven. Possibly merge with CheckersGamePlay at later date.
CheckerGamePlay:
Contains all the required info to play a single game of checkers.
PieceMovment:
Details how a checker piece moves.  Returns all possible moves for a given player on a given game board.
RandomPlayer:
Largely useless class creates a player that just moves pieces at random was used previously when testing UI.
MinMaxPlayer:
Returns a move for an AI player that invokes the MinMax algorithm. This move is scored using a simple piece difference function at a pre-selected depth. --- There is a possibility of a bug somewhere in this algorithm as depth seems to have little effect on the chosen move.
NN_MinMaxPlayer:
Similar to above but designed to use a Neural network as the scoring function. In contrast to Fogels work where he employs a MinMax search depth of 4 I’ve opted to forgo this setting the max depth to 1. This means that the rather than determine the best move some distance in the future and then back propagate the relevant score through the search tree, I’d hope that network can be trained to just calculate this back propagated score directly from the game board configuration.  I have yet to determine what effect this has on how well the network learns.
NN_Evaluator:
This details the neural network itself. The network is a List<List<objects>> where each sublist represents a layer of N neurons. This class allows networks to be created from scratch using a random seed or and existing network to be loaded. This class also includes the back propagation algorithm.
Neuron:
Details the associated weights and bias for a given neuron in the network.  Error and output values for the neuron are also stored here.
NN_ReinformentLearning:
This class allows two s given network to learn from playing a game of checkers against any type of opponent.  This is achieved as follows.
Current game state is save as an input. 
If training the move made is chosen using the distribution of scores for each move. i.e. the best move at a given time is chosen statistically more often than any other. This improves the likelihood that over 1000’s of games if the initial best move for a given state is in fact suboptimal the system will eventually find the optimal move. The score for the chosen move is save into outputs.
Else return the optimal move.
Once game is over if neural network player has won gets a reward of 1, else it receives a reward of -2. 
The output scores are then updated (see the score training outputs function).  Not that a decay rate of 0.99 is included. This can be thought of as the network ‘forgetting’ over time and prevents saturation of neuron outputs.
The inputs and updated scores are then used in the back propagation algorithm to update the weights within the network.
NOTE:- for some reason if an activation function of anything other than sigmoid is used the back propagation algorithm fails.
Evolve_NN_Evaluator:
This generates n networks with random biases and weights. Each network plays 2 games against each other network playing one game as red and one as white. If a network wins it scores 1 point. If it loses, draws or the game exceeds 100 moves it receives -2 point. After all games are complete the top 5 genes are kept for the next generation (elitism). The remainder of the gene pool is populated by new networks obtained from cross breeding two other networks chosen based on the distribution of their scores. The new child inherits the weight or bias of one parent or the other chosen at random. There is also a small chance (mutation rate) that the child inherits from neither parent instead it is assigned a rate at random. This prevents stagnation of the gene pool. This process is then repeated for a desired number of cycles. The resulting network after approximant 50-100 generations is capable of playing checkers.

Known bugs.
Only sigmoid function seems to work with the backpropagation algorithm. Attempts to use TanH, sigmoidal, and ReLu result in NaN error for the weights.
Changing the depth in minmax player doesn’t seem to drastically improve AI.
MinMax player is somewhat defeatist.
Evolve_NN_Evaluator is slow. Paralysation of main algorithm may help to speed this up.
Neural network architecture is static and fully feed forward in nature. Evolving both connectivity and number of neurons/layers may improve overall performance of network. This will cause a massive increase in overall speed however.
Background worker complete function is unable to update the title bar text of windows form. While this originally worked fine it suddenly reports not initialised error despite no changes to code. Current offending lines are commented out.
General input checking particularly with file names has not been included. In fact input to most functions is never actually checked. This should probably be addressed.
