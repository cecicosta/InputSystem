This direcotry contains the following files for the Circle Gesture Detection project:

- GestureInputCore/DataSample.cs - A samples of mouse coordinates representing closely a circle. The data is mainly used by unit tests to validate the algorithms and in a small demo with a brief explanation and demonstration of the algorithm working.
- GestureInputCore/InputStateMachine.cs - Implementation of a nearly generic state machine used to process he detection states. The states transition conditionals explicitly take a list of states representing the state machine stack and a list of Vector2 which is used on the specific Gesture Detection implementation to validate the inputs.
- GestureInputCore/CircleGesture.cs - Implements the detection algorithms. This class uses the state machine to process the regions of the circle, implementing the conceptualized graph as follows:
