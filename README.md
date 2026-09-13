# PC Health Hub

PC Health Hub is a Windows desktop monitoring dashboard built with **C#**, **WPF**, and **.NET**.

I created this project as a learning and portfolio project to strengthen my understanding of desktop application development, WPF data binding, Windows process monitoring, filesystem access, and live UI updates.

The application provides a simple system-health dashboard with machine information, drive statistics, running process data, and file information.

## Screenshot

<img width="970" height="732" alt="image" src="https://github.com/user-attachments/assets/eefde9b2-ef55-422d-855a-bc3a330f679c" />

<img width="966" height="733" alt="image" src="https://github.com/user-attachments/assets/e95ff8bd-9d04-4fb4-9368-47f3f8ff3328" />


## Features

* Displays machine and operating system information
* Displays logical processor count
* Shows drive capacity, available space, and percent used
* Shows file and directory counts for the `C:\` drive
* Displays sample files from the system drive
* Converts raw file sizes into readable units such as KB, MB, and GB
* Displays the current top 10 processes by memory usage
* Dynamically reevaluates which processes belong in the top 10
* Updates CPU and memory usage on a timer
* Uses `ObservableCollection` for live UI collections
* Uses `INotifyPropertyChanged` for automatic UI updates
* Separates process and filesystem logic into service classes
* Uses a dark dashboard-style interface

## Technologies

* C#
* WPF
* XAML
* .NET
* LINQ
* `System.Diagnostics`
* `System.IO`
* `System.Management`
* `ObservableCollection`
* `INotifyPropertyChanged`
* `DispatcherTimer`

## Project Structure

```text
PcHealthHub/
├── Model/
│   ├── ComputerInfo.cs
│   ├── FileItem.cs
│   └── ProcessItem.cs
├── Service/
│   ├── FileService.cs
│   └── ProcessService.cs
├── Utility/
│   └── FriendlyFileSize.cs
├── App.xaml
├── MainWindow.xaml
├── MainWindow.xaml.cs
└── PcHealthHub.csproj
```

## Architecture

The project separates responsibilities between models, services, utilities, and the WPF user interface.

### Models

**ComputerInfo**
Stores machine and selected-drive information used by the dashboard.

**FileItem**
Represents a file displayed in the file list.

**ProcessItem**
Represents a running Windows process and stores information such as:

* Process ID
* Process name
* CPU usage
* Memory usage
* Previous processor time
* Previous sample time

`ProcessItem` implements `INotifyPropertyChanged`, which allows changes in CPU and memory usage to automatically update in the WPF interface.

### Services

**FileService**
Reads files from a requested directory and creates `FileItem` objects for display.

**ProcessService**
Retrieves running Windows processes, finds the highest-memory processes, updates process statistics, and calculates CPU usage between samples.

### Utilities

**FriendlyFileSize**
Converts raw byte values into readable file sizes such as B, KB, MB, GB, and larger units.

## Live Process Monitoring

The process section uses a WPF `DispatcherTimer` to refresh process information periodically.

During each refresh, the application:

1. Retrieves the current highest-memory processes.
2. Matches them against existing process items using process IDs.
3. Updates CPU and memory values for processes that are still present.
4. Adds new processes that enter the top 10.
5. Removes processes that are no longer in the top 10.

Existing `ProcessItem` objects are preserved when possible so previous CPU samples remain available for CPU-usage calculations.

## CPU Usage Calculation

CPU usage is calculated by comparing processor-time samples over a period of time.

The application stores the previous processor time and sample time for each process.

On the next update, it compares those values with the current processor time and current sample time to estimate the percentage of CPU used during the interval.

The calculation also accounts for the number of logical processors in the system.

## Drive and File Information

The current version focuses on the system `C:\` drive.

The dashboard displays:

* Total drive space
* Available drive space
* Percent of drive space used
* Number of files
* Number of directories
* Sample files
* File sizes
* File modification dates

Filesystem logic is handled by `FileService` instead of directly inside the WPF window.

## Requirements

* Windows 10 or Windows 11
* Visual Studio with the **.NET Desktop Development** workload
* A compatible .NET SDK

This application is Windows-specific because it uses WPF and Windows system APIs.

## Running the Project

Clone the repository:

```bash
git clone YOUR_REPOSITORY_URL
```

Then:

1. Open the solution or project in Visual Studio.
2. Restore dependencies if needed.
3. Build the project.
4. Run the application.

## Current Limitations

PC Health Hub is still an educational and portfolio project.

Current limitations include:

* The file browser currently focuses on `C:\`
* The dashboard is primarily read-only
* Some Windows system processes may deny access or terminate while being inspected
* Error handling can still be expanded
* Automated test coverage is currently limited
* Additional system metrics could be added

## Planned Improvements

Possible future improvements include:

* Add a dedicated ViewModel layer
* Add support for selecting different drives
* Add total system CPU usage
* Add total RAM usage
* Add disk activity monitoring
* Add process sorting options
* Add additional process details
* Add logging for process-access failures
* Add automated tests for services and utilities
* Continue improving the dashboard UI

## What I Learned

This project helped me practice:

* Designing classes with separate responsibilities
* Keeping application logic out of UI code where practical
* WPF data binding
* `ObservableCollection`
* `INotifyPropertyChanged`
* Periodic UI updates with `DispatcherTimer`
* Windows process monitoring
* CPU usage sampling over time
* Working with drives, directories, and files
* LINQ sorting and filtering
* Formatting raw system data for display
* Building a desktop application from multiple cooperating classes

## Disclaimer

PC Health Hub is an educational project and is not intended to replace professional system-monitoring, security, or diagnostic software.
