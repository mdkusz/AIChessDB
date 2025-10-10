# AIChessDB (Educational & Collaborative Project)

This repository is intended as a **non-commercial, collaborative project for training purposes**.  
Its goal is to help developers practice integrating AI assistants into desktop applications.

- Open-source projects are provided under the MIT License for learning and contribution.  
- Proprietary binaries are included to allow running the full system, but remain closed and protected.  
- You are welcome to extend the system (e.g., add support for other AI providers) as long as it runs inside AIChessDB.  

---

## Overview

- **Open source projects (source code included)**  
  - `AIChessDatabase`  
  - `GlobalCommonEntities`  
  - `DesktopControls`  
  - `Resources`  

- **Application binaries (distributed only)**  
  - `AIChessDB.exe` (protected executable, acts as entry point for the system)  
  - Third-party/NuGet dependencies (see [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md))  
  - Proprietary libraries (see below)  

---

## Database setup

AIChessDB requires a working database backend.  
Supported backends: **MySQL**, **Oracle**, **SQL Server**.

- All scripts needed to create the schema and objects are included under `AIChessDB/Scripts/`.  
- Each developer is expected to create and configure the database on their own environment.  
- The application can connect to any of the supported backends, provided the schema is created from the scripts.  

> **Note:** These are not trivial databases. If you choose Oracle, MySQL, or SQL Server you are expected to know how to create users, schemas, and run the provided scripts.  
> This project is intended for developers with prior experience in database administration.

### Optional dataset

For convenience, a **MySQL dump with ~200,000 chess games** is available for download (not included in this repository).  
It can be imported into a fresh MySQL instance to save time searching and loading PGN files manually.  
👉 Download link available on the project blog.

## Proprietary Libraries

The following libraries are proprietary and distributed only in binary form.  
They may only be used together with **AIChessDB.exe**. They cannot be linked from external programs, redistributed separately, or modified.  
You are free to develop extensions (for example, support for additional AI providers) **as long as they run inside AIChessDB**.

### AI Package
- `AIAssistants.dll`  
- `DesktopAIAssistants.dll`  
- `OpenAIAPI.dll`  

### Database Package
- `BaseClassesAndInterfaces.dll`  
- `QueryDesktopControls.dll`  
- `MySQLLibrary.dll`  
- `OracleLibrary.dll`  
- `SqlServerLibrary.dll`  

See [EULA-Proprietary.md](EULA-Proprietary.md) for full terms.

---

## Building

1. Clone the repository.  
2. Open `AIChessDB.sln` in Visual Studio 2022.  
3. Restore NuGet packages.  
4. Build the solution.  
   - The open-source projects compile normally.  
   - Proprietary libraries are already included in `/AIChessDB/` and do not need to be built.  

---

## Debugging

To debug the open-source projects against the protected executable:

- Configure **Start external program** in project properties to point to:  

