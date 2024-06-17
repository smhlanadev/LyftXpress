# Elevator Challenge
## Setup
### Prerequisites
- .NET8.0 SDK must be installed.
- Visual Studio 2022 must be installed.
- Git must be installed if you want to clone the repository, otherwise you can download it as a zip.

Use a command line interface (cmd, PowerShell etc.), follow the steps below:
### Step 1. Clone or download this repository
git clone [https://github.com/smhlanadev/LyftXpress.git](https://github.com/smhlanadev/LyftXpress.git)
### Step 2. Install .NET dependencies
```
cd LyftXpress
dotnet restore
```
### Step 4. Run the application
```
Open the LyftXpress.sln in Visual Studio 2022 
(LyftXpress\LyftXpress.sln).
Build and run the solution.

This will open up a console screen with instructions and prompts. Enter the required data to simulate the movement of elevators.
```
## The Solution
In the console you will be asked to enter some data to initialise the environment. These are the number of elevators and the the number of floors. 
You will then be asked to enter a command. Commands are used to request the elevators. Depending on the position and availability of the elevators, the program will allocate the request to the nearest elevator.

### The Command
The format is a string in the form: [x y].
x -> Current floor - the floor where the passenger is requesting from
y -> Destination flooe - the floor the passenger wants to go to

### The Request
The command is converted to a request that can be assigned to an elevator.

### Elevator Movement
The elevator is assigned request that are in the same direction.

### The Scheduler
The scheduler is responsible for assigning the requests to the elevators based on an algorithm.

The scheduler first checks if there are idle elevators (not moving). If there are it assigns the incoming requests to them based on which elevator is the closest.
If an elevator is moving up it will pick up the requests that are going up that are from the same floor (`CurrentFloor`) and above. These are called `EligibleRequests`.
If an elevator is moving down it will pick up the requests that are going down that are from the same floor (`CurrentFloor`) and below. These are called `EligibleRequests`.

### The Elevator Service
This service receives inputs and commands from the UI and acts on them by employing the `DataService` or the `Scheduler`.

### The Data Service
This is the persistence layer that handles the data related operations. It creates new elevator instances, receives and stores requests from the commands.

### The Elevator Class
This class contains data related to the elevator instance and the logic to move it to the different floors based on the requests assigned to it.

It uses a `BackgroundWorker` to run the move logic in a separate thread so that interaction with the UI is still possible while the elevators are moving.

## Limitations
The console is used to display information about the statuses of the elevators and also to interact with the elevators. Although the heavy processing of moving the elevator is running in a separate thread, updating the consoling and accepting commands is still tricky.
Statuses tend to be written in the console in between the commands, this makes it difficult to follow.
### Potential Solution
One way to solve this is to create a second console application that will receive the statuses from the main application and display them. This way the commands can be entered on one console without any interruptions, and the statuses can be viewed in a separate console without any interruptions.
I can share the messages between the two applications using name pipes. The server will be the main application that will send the messages to the new application, the client, and the client displays the messages.
