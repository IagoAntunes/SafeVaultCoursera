# Capstone Project: SafeVault (ASP.NET Security)

This project is a secure ASP.NET Core API, "SafeVault," developed for the Security Capstone. The project implements input validation, SQL Injection prevention, authentication, and Role-Based Access Control (RBAC).

## Summary of Vulnerabilities and Fixes

### 1. (Activity 1) SQL Injection (SQLi)
* **Vulnerability:** Risk of an attacker inserting malicious SQL commands (e.g., `admin'--`) into input fields to bypass login or extract data.
* **Applied Fix:** The project uses **Entity Framework Core (EF Core)**. EF Core uses **parameterized queries** by default. Any user input is treated as a literal value, not as an executable SQL command. This effectively neutralizes the SQLi threat across the entire data access layer.

### 2. (Activity 1) Cross-Site Scripting (XSS)
* **Vulnerability:** Risk of an attacker saving malicious scripts (e.g., `<script>alert('xss')</script>`) to the database via forms, which would then execute in other users' browsers (such as administrators).
* **Applied Fix:** Mitigation was applied in two layers:
    1.  **Input Validation:** The `RegisterDto` and other DTOs (Data Transfer Objects) use **Data Annotations** (e.g., `[RegularExpression]`) to reject inputs containing dangerous characters (like `<` or `>`) before they are even processed.
    2.  **Output Encoding:** Although the API returns JSON (which is inherently safe from XSS if consumed correctly by a modern front-end), ASP.NET Core has native output encoding protections that prevent the rendering of untrusted HTML.

### 3. (Activity 2) Insecure Authentication
* **Vulnerability:** Storing passwords in plaintext or using weak hashing algorithms (like MD5).
* **Applied Fix:** The project uses **ASP.NET Core Identity**. Identity manages the entire user lifecycle and, crucially, stores passwords using **secure, salted hashing** (PBKDF2 by default). This makes it impossible to reverse the hash to obtain the original password.

### 4. (Activity 2) Broken Access Control
* **Vulnerability:** Allowing authenticated but unauthorized users (standard users) to access administrative endpoints.
* **Applied Fix:** **Role-Based Access Control (RBAC)** was implemented.
    * Users are assigned to roles ("User" or "Admin") during registration.
    * Sensitive endpoints (e.g., `/api/admin/dashboard`) are protected with the `.RequireAuthorization(policy => policy.RequireRole("Admin"))` attribute.
    * Authentication is managed by **JWT (Bearer Tokens)**, which contain the user's roles and are validated on every request.

## How Copilot (AI Assistant) Helped in the Process

The AI assistant was fundamental in accelerating development and ensuring best practices:

* **(Activity 1):** Generated the boilerplate code for the **DTOs with Data Annotations** and suggested the correct **Regular Expression (Regex)** for username validation.
* **SQLi:** Explained why EF Core already prevents SQLi, saving the need to write manual sanitization code.
* **(Activity 2):** Provided the boilerplate code (standard configuration) to set up **ASP.NET Core Identity** and **JWT (Bearer Token) authentication** in `Program.cs`.
* **(Activity 2):** Generated the `TokenService` implementation to create JWTs, including adding user roles to the token's claims.
* **(Activity 2):** Demonstrated how to apply **RBAC** in Minimal APIs using `.RequireAuthorization()` and `.RequireRole()`.
* **(Activity 3):** Helped structure the **Unit/Integration Tests** (xUnit) to simulate XSS attacks (via invalid input) and authorization failures.
