# 🎥 Media Equipment Management System

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![OOP](https://img.shields.io/badge/Architecture-OOP-blue?style=for-the-badge)

A robust, object-oriented C# console application designed to manage the loan, reservation, and maintenance of university media equipment (Cameras, Microphones, and Tripods).

---

## 👤 Project Identity Card
- **Student Name:** Mohammad Mahdi Hajimobini
- **Student ID:** 4042140039
- **Project Code:** 1-3-4
- **Theme (A4):** Media Equipment (Camera, Microphone, Tripod)

### ⚙️ System Parameters & Constraints
- **Seed:** `1187`
- **Max Student Loans:** 5
- **Max Professor Loans:** 6
- **Loan Duration:** 12 Days
- **Daily Overdue Fine:** 30 Units
- **Maintenance Threshold (Rule B3):** Equipment goes into maintenance after **6 loans**.
- **User ID Prefix:** `USR-1187`
- **Equipment ID Prefix:** `EQ-1187`

---

## 🏗️ Architecture & Class Diagram

<!-- 🖼️ IMAGE PLACEHOLDER: Add your UML or Class Diagram image in the path below -->
<div align="center">
  <img src="assets/class_diagram.png" alt="Project Architecture and Class Diagram" width="800"/>
  <br/>
  <em>Figure 1: System Architecture and Class Diagram</em>
</div>

### 🧩 Design Patterns & Extensibility
The system is built with high extensibility and maintainability in mind:
- **Object-Oriented Principles:** Strong encapsulation, inheritance, and polymorphism are utilized in equipment modeling, allowing developers to extend logic without disrupting existing components.
- **Repository Pattern:** A dedicated `Repository` class handles data persistence and management. This decoupling ensures that switching out the underlying database or storage mechanism in the future will be seamless.
- **Modular Structure:** Distinct separation of concerns across service layers, resulting in clean, readable, and highly scalable code.

---

## ⚙️ Core Logic & Features

### 🆔 ID Validation
Before any operation, system verifies IDs. If a user inputs an ID without the project's specific prefix (`1187`), the system automatically appends `USR-1187-` for users and `EQ-1187-` for equipment.

### 📦 Borrow Equipment
- Checks user status (Inactive users are blocked).
- Verifies equipment is in an `Available` state.
- Enforces maximum active loan limits based on User Type (Student/Professor).
- Checks for reservation conflicts; if reserved by the requester, status changes to `Fulfilled`.

### 🔄 Return Equipment
- Locates the `Active` loan record for the specific user and equipment.
- Calculates and displays any applicable **Fines** based on delays.
- Restores equipment status to `Available`.

### 📅 Reserve Equipment
- Validates user and equipment existence.
- Checks for time conflicts (ensures no prior `Pending` reservations for the requested dates).
- Creates a new reservation if the timeline is clear.

### 🛠️ Finish Maintenance (Rule B3)
- Validates if the equipment exists and is currently `Under Maintenance`.
- Restores the equipment to `Available` upon completion.

### 🔍 Search Equipment
- Advanced search by **ID**, **Name** (Case-Insensitive), **Status**, and **Type**.
- Automatically handles the prefix logic for ID searches.

### 📊 Reports
- **Report C1:** Sorts and displays all equipment in descending order based on their total `BorrowCount` (Most to least used).
- **Overdue Report:** Identifies all active loans that have passed their `DueDate`, calculating and listing their current accumulating fines.

---

## 💻 Sample Console Execution

<!-- 🖼️ IMAGE PLACEHOLDER: Add a screenshot of your running console application below -->
<div align="center">
  <img src="assets/console_execution.png" alt="Console Application Execution" width="600"/>
  <br/>
  <em>Figure 2: Console Interface Action</em>
</div>

```text
Media Equipment Management System (A4-B3-C1) Mohammad Mahdi Hajimobini 4042140039 ===
1. Add New User
2. Add New Equipment
3. Borrow Equipment
...
Enter your choice: 1
User ID (e.g., 1): 1
User Name: Mohammad Mahdi
User Type (1 for Student, 2 for Professor): 1
Is The User Active? (1 for Active 2 for Inactive): 1
SUCCESS: USER USR-1187-1 ADDED
```

---

## 🛡️ Challenges & Error Handling

To ensure a robust and crash-free experience, the following defensive programming practices were implemented:

1. **Exception Handling (`try-catch`):** Implemented in the `Program` class (for menu execution) and `BorrowEquipment` to catch unforeseen errors gracefully without crashing the application.
2. **Safe Parsing (`TryParse`):** Used for parsing dates, numeric IDs, and tripod heights. If invalid formats are entered, the system warns the user (`ERROR: INVALID FORMAT`) instead of throwing exceptions.
3. **Null Validation:** Prevents `NullReferenceException` by strictly verifying object existence in the Repository layer (`if (user == null)`) before proceeding with service logic.
4. **Business Logic Control:** Multi-tier validations check for active user status, loan capacity, and reservation conflicts before processing requests.
5. **Duplicate Prevention:** The Repository layer strictly enforces unique IDs via `userExists` and `equipmentExists` validations.
6. **Structured Exceptions:** Throws standard `InvalidOperationException` specifically when a user exceeds their loan limits.

---

## 🤖 AI Tools Usage & Justification

AI tools were utilized as a supplementary asset to speed up development while strictly managing technical debt and potential hallucinations.

### Areas of Usage
- **Architecture Consultation:** Discussing the optimal way to implement the Repository Pattern.
- **Syntax Generation:** Utilizing AI to generate standard LINQ queries for list traversals.

### Prompts & Discussions
- *"How can I implement the Repository Pattern properly to separate the storage layer from the Service layer?"*
- *"What is the difference between using global static variables versus a Singleton pattern for this project's scale?"*
- *"Which version of this code is cleaner and more optimized: classic loops or modern LINQ?"*
- **Code Review & Debugging:** Requested assistance resolving logical bugs (e.g., conflicts between `Reserved` and `Borrow` statuses, resetting `BorrowCount` logic, and `Available` status tracking after returns).

### Accepted AI Suggestions
- **Layered Structure:** Fully adopting the `Repository` class to handle data storage and transactions, drastically reducing code coupling.
- **LINQ Integration:** Replaced nested `foreach` loops in Search and Reporting modules with optimized LINQ methods (`Where`, `FirstOrDefault`, `OrderByDescending`).
- **Preventative Error Handling:** Using `TryParse` stringently over standard exceptions for console-based inputs.

---
*Developed as a Final Project for Advanced Programming Course.*
