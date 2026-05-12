# Wordle Game Documentation

## 1. Problem Statement, Aim, and Requirements

### Problem Statement
Build a console-based Wordle-style word guessing game in C# that uses PostgreSQL and ADO.NET for persistence. The game should support user registration/login, difficulty modes, word storage in database, score storage, input validation, and feedback generation for each guess.

### Aim
The aim is to enhance the existing Wordle game by adding user authentication, database persistence for users/words/scores, proper input validation using custom exceptions, and a clean, maintainable project structure.

### Requirements
- Implement user registration and login.
- Store users, hidden words, and scores in PostgreSQL.
- Use Npgsql with ADO.NET classes: `NpgsqlConnection`, `NpgsqlCommand`, `NpgsqlDataReader`.
- Randomly pick a hidden 5-letter word from the database.
- Support two difficulty levels:
  - Easy: 6 attempts
  - Hard: 4 attempts
- Validate guess input for:
  - Empty input
  - Less than 5 letters
  - More than 5 letters
  - Numbers
  - Special characters
  - Duplicate guesses
- Use a custom exception for invalid guesses.
- Generate feedback with `G`, `Y`, `X`.
- Save user score in database after each game.
- Allow replay after game completion.
- Use `Console.ForegroundColor` for colored output.

## 2. Pseudocode for Project Flow

1. Click on Start Game
   a. Create or open database connection.
   b. Initialize database tables if they do not exist.
   c. Show login/register menu.

2. User registration / login
   a. If new user: collect username and password and store in database.
   b. If existing user: verify username and password from database.

3. Difficulty selection
   a. Show options: Easy or Hard.
   b. Set `maxAttempts` to 6 for Easy or 4 for Hard.

4. Game start
   a. Fetch a random hidden word from the database.
   b. Initialize `attemptCount = 0`.
   c. Initialize `previousGuesses` list.

5. User starts guessing the 5-letter word
   a. Read `userInput` from console.
   b. Validate `userInput`:
      i. empty input
      ii. less than 5 characters
      iii. greater than 5 characters
      iv. numbers present
      v. special characters present
      vi. duplicate guess
   c. If invalid:
      i. throw custom exception
      ii. display proper message
      iii. do not reduce `attemptCount`

6. Feedback generation
   a. For each char in `userInput`, validate against `hiddenWord`:
      i. Same letter + same position → `G`
      ii. Letter exists elsewhere → `Y`
      iii. Letter not found → `X`
   b. Display color-coded feedback using `Console.ForegroundColor`.

7. Win condition
   a. If feedback is `GGGGG`:
      i. game ends immediately
      ii. display win message
      iii. calculate score based on attempt number
      iv. save score to database

8. Lose condition
   a. If `attemptCount` reaches maximum attempts:
      i. reveal hidden word
      ii. display "Better luck next time"
      iii. save score of 0 to database

9. Replay option
   a. Prompt the user: "Would you like to play again? (Y/N)"
   b. If `Y`, restart game from difficulty selection.
   c. If `N`, exit.

## 3. Folder Structure

```
WordleGame/
│
├── Core/
│   ├── Game.cs
│   ├── WordProvider.cs
│   ├── GuessValidator.cs
│   └── FeedbackGenerator.cs
│
├── Database/
│   ├── DbConnectionFactory.cs
│   └── GameRepository.cs
│
├── Models/
│   ├── User.cs
│   ├── Attempt.cs
│   ├── Score.cs
│   └── Word.cs
│
├── Exceptions/
│   └── InvalidGuessException.cs
│
├── Program.cs
└── WordleGame_Documentation.md
```

## 4. File Responsibilities

- `Program.cs`
  - Application entry point.
  - Handles user authentication menu.
  - Starts the game loop and replay flow.

- `Core/Game.cs`
  - Contains game execution logic.
  - Manages difficulty selection, guess loop, win/lose conditions, score saving, and replay prompts.

- `Core/WordProvider.cs`
  - Fetches a random hidden word from the database.
  - Encapsulates word selection logic.

- `Core/GuessValidator.cs`
  - Validates each guess.
  - Throws `InvalidGuessException` for invalid input.

- `Core/FeedbackGenerator.cs`
  - Generates guess feedback as `G/Y/X`.
  - Displays colored output using `Console.ForegroundColor`.

- `Database/DbConnectionFactory.cs`
  - Builds and returns PostgreSQL connection objects.
  - Reads connection string from environment or default configuration.

- `Database/GameRepository.cs`
  - Initializes database tables.
  - Handles user authentication and registration.
  - Fetches random words.
  - Saves scores to database.

- `Models/User.cs`
  - Represents a user record.

- `Models/Attempt.cs`
  - Represents a single guess attempt and its feedback.

- `Models/Score.cs`
  - Represents the score data that is saved to the database.

- `Models/Word.cs`
  - Represents the hidden word data structure.

- `Exceptions/InvalidGuessException.cs`
  - Custom exception for invalid guess inputs.

---

### Notes
- The documentation is written to match the current project design and behavior.
- The current implementation uses PostgreSQL through `Npgsql` and ADO.NET.
