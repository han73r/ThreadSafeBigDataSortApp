# File Sorting and Creation Application

## Overview

This project is a high-performance system for generating large text files with random content and efficiently sorting them, even when they exceed available memory. The solution leverages dependency injection, asynchronous processing, and modular design principles for scalability and maintainability.

## Features

- **Dependency Injection Entry Point:** Managed through a dependency injection container for easy configuration and scalability.
- **Factories:**
  - `IFileCreatorFactory` and `IFileSorterFactory` are responsible for creating instances of `FileCreator` and `FileSorter`, respectively.
- **Asynchronous and Thread-Safe Logger:** Supports both file and console logging with message-level filtering for flexible debugging and monitoring.
- **Timer Service:** Measures task execution times without requiring dedicated benchmarking tools.
- **FileCreator:** Generates a file with random lines of specific structure while respecting a file size limit.
  - Utilizes a dedicated line generation service.
  - Employs a blocking collection for thread-safe data handling.
  - Integrates a separate file writing service with size control.
- **FileSorter:** Reads, sorts, and merges large files using memory-efficient chunking and multi-threading techniques.

---

## Components

### **FileCreator**

**Purpose:**  
Generates a large text file with random lines while ensuring the file size does not exceed specified limits.

**Key Elements:**

1. **BlockingCollection:**  
   A thread-safe queue used for managing chunks of data. Enables a producer-consumer pattern where one thread generates data while another writes it to disk.
2. **LineGenerator:**
   - Generates random lines until the defined chunk size limit is reached.
   - Limits memory usage using `_chunkSizeBytes`.
3. **FileWriter:**
   - Writes data chunks to the output file.
   - Ensures the file does not exceed the specified size limit.

---

### **FileSorter**

**Purpose:**  
Efficiently reads, sorts, and merges large files that cannot fit into memory, leveraging multi-threading and memory optimization.

**Key Elements:**

1. **Chunking Logic:**  
   Reads file chunks that fit into available memory (calculated based on 80% of system memory and available threads).
2. **In-Memory Sorting:**  
   Sorts each chunk in memory to minimize disk I/O during the merging phase.
3. **Temporary File Storage:**  
   Saves sorted chunks to temporary files, reducing memory usage and enabling the handling of large files.
4. **MergeChunks:**  
   Uses a priority queue for an efficient k-way merge of sorted temporary files into a final output file.

---

## Project Architecture

### **High-Level Design Principles**

1. **Separation of Concerns:**  
   Each class or component has a distinct responsibility (`Logger`, `TimerService`, `FileCreator`, `FileSorter`, etc.).
2. **Scalability:**

   - The system dynamically adapts to available memory and CPU resources.
   - Designed to handle files larger than the system’s memory capacity.

3. **SOLID Principles:**

   - The Factory Pattern ensures adherence to the Single Responsibility and Dependency Inversion principles.
   - Interface-based design promotes testability and extensibility.

4. **Asynchronous Processing:**
   - Asynchronous programming is used in both file creation and sorting to optimize resource utilization.

---

## Directory Structure

```plaintext
SorterApp/
├── Interfaces/                # Interfaces for dependency injection and component abstraction
│   ├── IFileCreator.cs
│   ├── IFileSorter.cs
│   └── ILogger.cs
├── Services/                  # Core services implementing business logic
│   ├── FileCreator.cs
│   ├── FileSorter.cs
│   ├── Logger.cs
│   └── TimerService.cs
├── Utilities/                 # Utility classes for system info and global settings
│   ├── GlobalSettings.cs
│   ├── LineGenerator.cs
│   ├── FileWriter.cs
│   └── SystemInfo.cs
├── Program.cs                 # Application entry point
├── Factory/
│   ├── FileCreatorFactory.cs
│   └── FileSorterFactory.cs
└── README.md                  # Project documentation (this file)
```
