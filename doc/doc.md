# Running the App
The app has to two UIs: one in the console and one in Blazor (web app). To run the app, simply navigate to either the ConsoleUI or the BlazorUI folder (not the BlazorUI.Client folder) and enter the **dotnet run** command.

The Visual Studio solution for the whole project should be located in the ConsoleUI folder.

# Data saving
The current version only uses JSON files.  These files are saved in the **data** folder, relative to the executable file. The data folder contains three subfolders:
* characters - a folder where all the created characters should be located
* classes - a folder containing all the possible classes (no editor has been created yet).
* races - a folder containing all the possible races (no editor has been created yet).
JSON files were chosen primarily because they can be easily edited without the need for an editor.

# Structure
The whole Project20 is separated into 4 parts:
* Core
* ConsoleUI
* BlazorUI
* Project20Tests

Further, it is explained what these parts do and how they work.

## Core
Core basically includes all the parts that are common to all possible user interfaces. Most of these functions and classes are related to the DnD 5e 2014 ruleset.

Not all rules are implemented. The main reason for this is that the app should allow the player to change the rules on the one hand, and not just be a glorified piece of paper on the other, and finding a meaningful middle ground is a difficult task and requires feedback from users, something this project does not have at this point.

Later on, when different games and their rulesets are implemented, it makes sense to split this part even further and generalise it more.

Parts of Core are:
* Character
* GameClass
* GameRace
* JSONManager
* Die

### Character
Class that is designed to hold all needed information about character and also contains methods based on ruleset, for example GetAbilityModifier that counts and returns characters ability modifier.

All information about the character's class and race is held as class and race string IDs. Multiple classes are not supported yet.

### GameClass
Contains information about the class. Subclasses are not yet included.

### GameRace
Contains information about the race.

### JSONManager
Manages all the necessary manipulations with JSONs, i.e. loading characters, classes and races, and saving characters.
The manipulation is done using a dictionary, where for characters the key, also called the ID, is the same as the filename.

### Die
Method that implements dice. Used to convert shorthand like '1d20' into actual dice rolls.

## ConsoleUI
User interface built as a command line interface.

The main part is the ConsoleManager, which manages data for the who application and works with Menus. The architecture of this interface is similar to a finite state machine, where Menus would be the states and the React methods can be used as state transition functions. In this architecture, the ConsoleManager runs an infinite loop in which the Show and React methods of the active Menu are called.
The Show method is basically a series of console writes that create the graphical structure of a Menu. The React function handles what should happen based on the user's input.

Most Menus are made up of a series of options, each of which is given a number and the user enters the number to select that option. This style is fairly easy to understand and follow, but is impractical or impossible to use for more complex input. For this reason, Menus like the CharacterMenu use a set of commands to which they respond.

## BlazorUI
Using the .NET Blazor technology, BlazorUI is a user interface created as a web application. It uses both client and server parts of Blazor. The client part is used to maximise the speed of the application and the server part exists mainly for the future possibility of using this manager to manage a whole campaign with multiple users on the same database.

## Project20Tests
Unit tests for the application. At this point, the tests are for the Core classes Character and Die.