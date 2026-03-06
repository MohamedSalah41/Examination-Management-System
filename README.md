# 📝 Examination Management System

A console-based C# application that simulates a full examination management workflow — including question creation, student enrollment, exam execution, event-driven notifications, and result grading.

---

## 📌 Table of Contents

- [Overview](#overview)
- [Project Structure](#project-structure)
- [Class Descriptions](#class-descriptions)
- [Design Decisions](#design-decisions)
- [Relationships & UML](#relationships--uml)
- [How to Run](#how-to-run)
- [Sample Output](#sample-output)
- [Technologies Used](#technologies-used)

---

## Overview

This system allows you to:
- Create a **Subject** and enroll **Students** into it
- Build different types of **Questions** (True/False, Choose One, Choose All)
- Run a **Practice Exam** (with instant feedback and grading) or a **Final Exam** (answers submitted, results announced later)
- Automatically **notify all enrolled students** via a custom event system when an exam starts
- Store and sort exams using a **Generic Repository**
- Log all questions to a file using a custom **QuestionList** collection

---

## Project Structure

```
ExamSystem/
│
├── Enums/
│   └── ExamMode.cs               → Enum: Queued, Starting, Finished
│
├── Models/
│   ├── Answer.cs                 → Represents a single answer (Id + Text)
│   └── Student.cs                → Student with name, ID, and exam notification handler
│
├── Questions/
│   ├── Question.cs               → Abstract base class for all question types
│   ├── TrueFalseQuestion.cs      → True/False question (2 fixed answers)
│   ├── ChooseOneQuestion.cs      → Single-choice question from a list
│   └── ChooseAllQuestion.cs      → Multi-choice question (select all correct)
│
├── Exams/
│   ├── Exam.cs                   → Abstract base class for all exam types
│   ├── PracticeExam.cs           → Shows correct answers + score after finish
│   └── FinalExam.cs              → Submits answers only, results announced later
│
├── Collections/
│   ├── AnswerList.cs             → Custom array-based collection for Answer objects
│   ├── QuestionList.cs           → Extends List<Question>, logs every Add() to file
│   └── Repository.cs             → Generic repository with sort, add, remove, getAll
│
├── Events/
│   └── ExamEvents.cs             → Custom delegate + ExamEventArgs class
│
├── Subject/
│   └── Subject.cs                → Subject with student enrollment and notification
│
└── Program.cs                    → Entry point — wires everything together
```

---

## Class Descriptions

### `Answer`
Represents a single possible answer to a question.
- Properties: `Id` (int), `Text` (string)
- Implements `IComparable<Answer>` to allow sorting by ID
- Used inside `AnswerList` and referenced as `CorrectAnswer` in questions

---

### `AnswerList`
A custom collection that holds `Answer` objects internally using a plain array (not `List<T>`).
- Supports: `Add()`, `GetById(int)`, indexer `this[int]`
- Automatically resizes when capacity is exceeded (doubles array size)

---

### `Question` *(abstract)*
The base class for all question types.
- Properties: `Header`, `Body`, `Marks`, `Answers` (AnswerList), `CorrectAnswer`
- Abstract methods: `Display()`, `CheckAnswer(Answer)`
- Subclasses must implement their own display and answer-checking logic

---

### `TrueFalseQuestion`
Extends `Question`. Automatically provides two answers: `[1] True` and `[2] False`.
- Correct answer is passed in the constructor
- `CheckAnswer()` compares student answer ID with correct answer ID

---

### `ChooseOneQuestion`
Extends `Question`. Accepts a custom `AnswerList` of options.
- Student picks one answer by ID
- `CheckAnswer()` verifies the selected answer matches the correct one

---

### `ChooseAllQuestion`
Extends `Question`. Student must select **all** correct answers.
- Has an extra property: `CorrectAnswers` (AnswerList) — can have multiple correct answers
- Student submits answer IDs as a comma-separated string (e.g. `"1,3"`)
- `CheckAnswer()` parses the string and verifies all correct IDs are selected

---

### `Exam` *(abstract)*
The base class for all exam types. Implements `ICloneable` and `IComparable<Exam>`.
- Holds: `Questions[]`, `QuestionAnswerDictionary`, `Subject`, `ExamMode`, `ExamStarted` event
- When `Mode` is set to `Starting`, it automatically fires the `ExamStarted` event
- Abstract method: `ShowExam()` — each subclass defines how questions are presented
- `Start()` → sets mode to Starting → fires event → notifies students
- `Finish()` → sets mode to Finished
- `CorrectExam()` → calculates and prints the score
- `CompareTo()` compares exams by time then number of questions (used in Repository sort)

---

### `PracticeExam`
Extends `Exam`. After finishing, shows the student's answers, correct answers, and final score.
- Useful for self-study and practice sessions

---

### `FinalExam`
Extends `Exam`. After finishing, shows submitted answers only — no correct answers revealed.
- Simulates a real exam environment where results are announced later

---

### `QuestionList`
Extends `List<Question>`. Overrides the `Add()` method to **log every question to a file** on disk (append mode).
- Log includes: timestamp, question text, and marks
- Useful for tracking which questions were used in an exam session

---

### `Repository<T>`
A generic repository class constrained to types that implement both `ICloneable` and `IComparable<T>`.
- Internally uses a plain array (not `List<T>`)
- Supports: `Add()`, `Remove()`, `Sort()` (bubble sort), `GetAll()`
- In this system it is used to store and sort `Exam` objects

---

### `Student`
Represents an enrolled student.
- Properties: `Name`, `Id`
- Method: `OnExamStarted()` — event handler that prints a notification when an exam starts

---

### `Subject`
Represents an academic subject. Owns an array of enrolled `Student` objects (composition).
- `Enroll(Student)` — adds a student (max 20)
- `NotifyStudents(Exam)` — loops through all students and calls their `OnExamStarted()` handler

---

### `ExamEventArgs`
Custom event arguments class that carries `Subject` and `Exam` information when the exam starts.

---

### `ExamStartedHandler`
Custom delegate: `void ExamStartedHandler(object sender, ExamEventArgs e)`
Used as the event type in `Exam.ExamStarted`.

---

### `ExamMode` *(enum)*
Three states: `Queued`, `Starting`, `Finished`
Tracks the lifecycle of an exam.

---

## Design Decisions

| Decision | Reason |
|----------|--------|
| `Question` and `Exam` are abstract | Enforces polymorphism — each subclass defines its own `ShowExam()` / `Display()` / `CheckAnswer()` behavior |
| `AnswerList` uses internal array instead of `List<T>` | Assignment requirement — demonstrates manual array management with resize logic |
| `QuestionList` extends `List<Question>` | Adds file-logging behavior on top of standard list — open/closed principle |
| `Repository<T>` constrained to `ICloneable + IComparable<T>` | Ensures stored objects can be cloned and sorted safely |
| Custom delegate `ExamStartedHandler` | Demonstrates event-driven programming with a typed delegate instead of generic `EventHandler` |
| `Subject` owns `Student[]` array (composition) | Subject manages its own students — students cannot exist independently of a subject context |
| `ExamMode` enum drives event firing | Changing `Mode` to `Starting` automatically triggers the event — no manual event call needed |

---

## Relationships & UML

```
Exam (abstract)
 ├── implements: ICloneable, IComparable<Exam>
 ├── has-a:      Subject
 ├── has-a:      ExamMode
 ├── has-a[]:    Question[]
 ├── has-a:      Dictionary<Question, Answer>
 ├── event:      ExamStartedHandler
 ├── ◄── PracticeExam
 └── ◄── FinalExam

Question (abstract)
 ├── has-a:      AnswerList  (Answers)
 ├── has-a:      Answer      (CorrectAnswer)
 ├── ◄── TrueFalseQuestion
 ├── ◄── ChooseOneQuestion
 └── ◄── ChooseAllQuestion
          └── has-a: AnswerList (CorrectAnswers)

AnswerList
 └── has-a[]:    Answer[]

QuestionList
 └── extends:    List<Question>

Subject
 └── has-a[]:    Student[]  (composition)

ExamEventArgs
 ├── has-a:      Subject
 └── has-a:      Exam

Repository<T>
 └── constrained to: ICloneable + IComparable<T>
```

---

## How to Run

1. Open the solution in **Visual Studio**
2. Press **F5** or **Ctrl+F5** to build and run
3. The system will automatically:
   - Create a subject (`C# Programming`)
   - Enroll 3 students
   - Build 3 questions (True/False, Choose One, Choose All)
   - Create a Practice and Final exam
   - Log questions to `practice_questions.log`
   - Sort exams using the generic repository
4. You will be prompted to select an exam type:
   - Enter `1` for **Practice Exam**
   - Enter `2` for **Final Exam**
5. Answer each question by entering the answer ID
6. For Choose All questions, enter IDs separated by commas (e.g. `1,3`)
7. Results are shown at the end based on the exam type

---

## Sample Output

```
========================================
    EXAMINATION MANAGEMENT SYSTEM
========================================

Subject: C# Programming (3 students)

Questions logged to 'practice_questions.log'

Exam repository sorted by time:
  PracticeExam | Subject: C# Programming | Time: 30min | Questions: 3
  FinalExam    | Subject: C# Programming | Time: 60min | Questions: 3

Select Exam Type:
  1 - Practice Exam
  2 - Final Exam
Enter choice: 1

=== Exam Starting (PracticeExam) | Subject: C# Programming | Time: 30 min ===

--- Notifying 3 student(s) enrolled in 'C# Programming' ---
  [Notification] Student 'Ali Hassan' notified: Exam for 'C# Programming' is starting!
  [Notification] Student 'Sara Mohamed' notified: Exam for 'C# Programming' is starting!
  [Notification] Student 'Omar Tarek' notified: Exam for 'C# Programming' is starting!

====== PRACTICE EXAM ======
...
=== Final Grade: 10 / 10 ===
```

---

## Technologies Used

- **Language:** C# (.NET)
- **IDE:** Visual Studio
- **Concepts:** OOP, Abstract Classes, Inheritance, Polymorphism, Interfaces, Generics, Events & Delegates, Custom Collections, File I/O
