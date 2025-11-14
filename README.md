# Final project for Advanced Database university course Documentation

# DBCLI Project

Project was prepared by:

- Bartłomiej Matuszewski  

- Jakub Fudro

## 1. Choice of Technologies

While working on the project we used the following technologies:

- **Linux `tar` command to unpack .gz files**
- **git and GitHub for version control**
- **ArangoDB - Community Edition**
- Python 3.10 – initial data analysis (CSV files)
- Rust 1.83 – attempted implementation of the program; idea abandoned due to weak familiarity with the ArangoDB client library (arangors) and poor documentation
- **C#/.NET 8.0 – the actual, working implementation of the program**

The final version of the program has no additional dependencies besides C#/.NET and ArangoDB - Community Edition.

## 2. Architecture

![alt text](docs/ztb.drawio.png)

## 3. Requirements and Dependencies (software modules, databases, etc.)

### 3.1 System Requirements

The project was fully developed on Linux Ubuntu 24.04.1 (LTS).

### 3.2 Database Technology

As the database we chose ArangoDB - Community Edition. Arango natively supports graphs and has built-in graph functions such as shortest path.

### 3.3 Programming Technology

As the programming language we chose C#/.NET 8.0, a popular corporate technology for building applications.  
The technology is cross-platform and uses the MIT license.  
In our project we used .NET version 8.0 (the newest standard at the time we started the work).  

For communication with the database we used the ArangoDBNetStandard library.  
It is the official database client library provided by the vendor.  
The library is licensed under Apache 2.0.

### 3.4 Program Modules

The project tree looks as follows:

![alt text](docs/image-1.png)

### 3.4.1 DbcliArangoLoader

A library used to fix the `popularity_iw.csv` file, which is the source of popularity data for graph nodes.  
The second responsibility of this library is a collection of functions for loading data into the ArangoDB database.

### 3.4.2 DbcliCoreUtilities

A library that contains functions executing the appropriate queries on the graph in ArangoDB.

### 3.4.3 DbcliModels

A library that contains data models used in the program. Mainly these are classes representing values returned by queries, as well as a class for deserializing the config file.

### 3.4.4 DbcliProject

A project that wires the whole solution together and exposes a CLI interface to run the appropriate tasks.

## 4. Installation and Configuration Instructions

In each subsection we include links to installation and configuration guides, because these may change over time.

### 4.1 ArangoDB Installation

Link to the ArangoDB installation instructions on Ubuntu: [ArangoDB - Community Edition](https://arangodb.com/download-major/ubuntu/)

Add the keys to the repository:

```bash
curl -OL <https://download.arangodb.com/arangodb312/DEBIAN/Release.key>
sudo apt-key add - < Release.key

```

Install via apt-get:

```bash
echo 'deb <https://download.arangodb.com/arangodb312/DEBIAN/> /' | sudo tee /etc/apt/sources.list.d/arangodb.list
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install arangodb3=3.12.3-1

```

### 4.2 .NET Installation

Link to the .NET installation instructions on Ubuntu: [.NET](http://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8&pivots=os-linux-ubuntu-2404)

```bash
sudo apt-get update && \\
  sudo apt-get install -y dotnet-sdk-8.0
```

## 5. User instructions (on how to run the program)

To build the container in the main project folder you need to use the command `docker compose up -d`. After the project build is finished it will be possible to use the command docker exec -it adv-db-systems.app /bin/bash; this command will start a shell inside the container. From the Docker shell it will be possible to use the commands defined in the application project.

## 6. Step-by-step design and deployment process

Thanks to the wide range of functions implemented in ArangoDB, the system design process was limited to getting familiar with the capabilities of the database system.

The first step in the process of implementing the solution was testing using the built-in ArangoDB interface; the purpose of the tests was to check which functionalities we can support using only the built-in database system functions. ArangoDB is able to support all required functionalities using the built-in functions.

The next step was preparing the functionality of connecting to the database using C# code; the library used was easy to use and allowed sending queries in string form.

Next we prepared 18 methods (each of them corresponds to a task that the application is expected to perform). Each of these methods has a single goal; some of them required parameterization.

The next step was preparing a parser which, based on the command provided by the user, will run the appropriate application method and, if necessary, pass parameters to it.

The last step was preparing a Docker environment which allows quick deployment of the application regardless of the environment in which it is run.

## 7. Roles of all people in the project and description of who did what

Bartłomiej Matuszewski: tests in the ArangoDB environment, preparation of database queries, implementation of C# code

Jakub Fudro: tests in the ArangoDB environment, preparation of database queries

## 8. Results

Unfortunately our deployment in the Docker environment did not allow us to perform tests; during the population of the database the application returns many errors. The number of edges does not match the number that should be in the database. Attempting to carry out further tests (tasks 10 and 16–18) would be unreliable or impossible (the paths the program was supposed to return may not exist).

## 9. Step-by-step instructions on how to reproduce the results

The benchmarking script should produce 6 files (1 file for each of the measured functions) which should then be summarized using a Jupyter notebook. The notebook summarizes and calculates the average for 4 selected functions and generates plots illustrating the behavior of the application over time.

## 10. Self-assessment: discussion of effectiveness

Functions from 1 to 18 execute very quickly, the database system itself is well optimized, and the C# wrapper prepared by our team does not significantly affect the efficiency of query execution. The problem is the population of the database; writing data into the database is slow, the browser interface and the CLI interface perform data write operations significantly faster.

## 11. Strategies for future mitigation of identified shortcomings

The biggest problem of the project is the population of the database; we suspect that using the CLI tools prepared by the developers of the ArangoDB system may improve the data loading process.

# Technical Documentation (more detailes, corrections)

During development and testing of the application we used the Ubuntu 20.04.1 (LTS) operating system, which we both have installed on our personal computers.
ArangoDB – Community Edition version 3.12.3 for Linux (specifically Ubuntu 24.04) was installed locally on the system without containers.

![](docs/image-2.png)

The programming environment/technology we used was .NET 8.0 and C# version 12.

## Developer environment setup

To install the .NET and ArangoDB packages we used the apt-get package manager.
Below we present detailed instructions on how to install these packages on a Linux/Ubuntu system.

### Installing ArangoDB

To install ArangoDB Community Edition on our system we followed the instructions on the vendor’s website: [Arango DB - Community Edition](https://arangodb.com/download-major/ubuntu/).

The initial requirement is registering the keys for the ArangoDB repository:

```bash
curl -OL https://download.arangodb.com/arangodb312/DEBIAN/Release.key
sudo apt-key add - < Release.key
```

In an obvious way we need the curl tool to download the appropriate files with the keys for the repository.
Then the actual installation looks as follows:

```bash
echo 'deb https://download.arangodb.com/arangodb312/DEBIAN/ /' | sudo tee /etc/apt/sources.list.d/arangodb.list
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install arangodb3=3.12.3-1
```

During the installation we had to provide the password for the root user in the database.
To simplify the software development process we did not create an additional user in the database and used the root user.
Installing .NET

The best solution for installing .NET is to use the [Microsoft docs](http://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8&pivots=os-linux-ubuntu-2404).
Following the instructions from this page we installed .NET 8.0 on our system. It was then the newest available language standard.
Installing .NET on Linux/Ubuntu is easier in that you do not need to add additional repositories; it is enough to use apt-get:

```bash
sudo apt-get update && \
sudo apt-get install -y aspnetcore-runtime-8.0
```

## .NET libraries used

### ArangoDBNetStandard

This is the official library for communication with the ArangoDB database provided by the vendor; we mainly used the functionality for executing queries against the database’s REST endpoint.

### Newtonsoft.Json

This is the most popular library for JSON serialization and deserialization in .NET, used for parsing the dbcli_config.json file and parsing query responses from the database.

### CsvHelper

A library for handling .csv files, used for parsing the taxonomy_iw.csv and popularity_iw.csv files.

### Additional notes

All libraries are downloaded using the NuGet package manager, which is the official and very smooth-working tool for package management in .NET.
To make the libraries available in our project we needed to add appropriate references in the **.csproj** files and then, at the **.sln** level, run the commands:

```bash
dotnet restore
dotnet build
```

In this way **dotnet** will download all required packages and build our project.

## Project/software structure

Our project consists of a solution folder which holds our git repository and all projects which make up our application.
This structure is standard for .NET projects.

![alt text](docs/image-1.png)

### DbcliArangoLoader

This is a .NET class library project that contains the functionality for repairing and loading data into the ArangoDB database.
The **CsvFixer** class contains the definition of a static function (in C# there are no global functions; such functionality can be simulated by creating classes containing static methods) that repairs the **popularity_iw.csv** file.
The **TaxonomyLoader** and **PopularityLoader** classes load data from the **taxonomy.csv** and **popularity_iw.csv** files into program memory.
This solution is quite memory-heavy, but because of the structure of our development process, it was the solution we already had prepared.
**TaxonomyLoader** loads data from the file into a dictionary as follows:

- appropriately defining how the .csv uses separators, whether the file contains headers, and the escape character
- after opening the file, the source node and target node are read
- checking whether the nodes exist in the dictionary (if not, they are added and a GUID is generated for them)

This procedure is necessary because the `_key` in ArangoDB must follow specific character rules, meaning that it cannot contain characters outside the Latin alphabet or certain special characters.
You can check the exact key requirements [here](https://docs.arangodb.com/3.11/concepts/data-structure/documents/#user-specified-keys).
Nodes from the repaired **popularity_iw.csv** file are loaded into memory in a similar way.
The value of a node is its popularity score.

**PopulateDatabase** is the class that contains functions for communicating with the database and performing specific operations.
The methods of this class are used to:

- drop the database if it exists
- create the database
- prepare the **Graph** instance
- add nodes to the database (insert node collections)
- add edges to the database (insert edge collections)
- additionally, there is a method that calls the other methods in the correct order to properly load data into the database

### DbcliCoreUtilities

This is a .NET class library project that contains functionality for executing specific queries on the database, as specified in the project PDF.
The **DbTasks** class contains 18 methods, each of which corresponds to one of the tasks we had to complete.
The **DbConnector** class contains a static method for connecting to the database.

We are not describing each method from **DbTasks**, because each of them is explained in the project specification file, and their code is clear enough that additional explanation would be redundant.

### DbcliModels

This is a .NET class library project that contains data models used by the program.
The **ConfigParameters** class is used to deserialize the **dbcli_config.json** file, which contains connection data for the database used across the entire program.
The **TaskModels** folder contains the classes used to deserialize the responses from database queries.

### DbcliProject

This is a .NET console project that ties the entire solution together and provides the CLI interface for running the appropriate tasks.
The **Program.cs** file is the equivalent of `main` in other programming languages.
This file contains the code that reads the task number entered by the user and calls the appropriate method from the **CommandManager** class. It also parses the **dbcli_config.json** file.
The **CommandManager** class is the main class linking the entire project.
Each method in this class corresponds to one of the tasks we had to complete or to database preparation.
This class also handles parsing commands from user input arguments.

The general structure is that **Program.cs** creates an instance of **CommandManager**, then checks the task number entered by the user.
A switch-case then runs the appropriate **CommandManager** method.

To keep a unified CLI interface we added the following:

- `./dbcli 0 fix` – repairs the .csv  
- `./dbcli 0 load` – loads data into the database

Additional example uses of **dbcli**:

- `./dbcli 1 "1880s_films"`
- `./dbcli 2 "1880s_films"`
- `./dbcli 3 "1880s_films"`
- `./dbcli 4 "1889_films"`
- `./dbcli 5 "1889_films"`
- `./dbcli 6 "1889_films"`
- `./dbcli 7`
- `./dbcli 8`
- `./dbcli 9`
- `./dbcli 10 10`
- `./dbcli 11`
- `./dbcli 12 "1889_films" "1890s_films"`
- `./dbcli 13 "1889_films" 50`
- `./dbcli 14 "1880s_films" "1920s_films"`
- `./dbcli 15 "1880s_films" "1920s_films"`
- `./dbcli 16 "Tourism_in_Uttarakhand" 1`
- `./dbcli 17 "19th-century_works" "1887_directorial_debut_films"`
- `./dbcli 18 "19th-century_works" "1887_directorial_debut_films" 15`

## Compilation, execution, and additional requirements

To compile the application, use the command:

```bash
mkdir publish

dotnet build

dotnet publish -c Release -r linux-x64 --self-contained /p:PublishSingleFile=true -o ./publish/
```

Now the **publish** folder contains the executable **dbcli**.
For the application to run correctly, the same directory must contain a **Resources/** folder with the **taxonomy_iw.csv** and **popularity_iw.csv** files, as well as the **dbcli_config.json** file.
To run the binary, use the commands shown in the previous section.

## Problems encountered during implementation

### Implementing in Rust and issues with the arangors library

The first programming language we attempted to use for our solution was Rust.
We chose Rust because it is extremely fast and provides excellent multithreading support.
Unfortunately, the **arangors** library, which is the official library for communicating with the ArangoDB database, was very poorly documented; much of its functionality had no explanation beyond the function signatures.
Additionally, there were no usage examples, so it was difficult to understand how to use the library.
After several hours of trying, we decided to switch to another programming language.

### Running Docker with ArangoDB and our project

The main issue was that we had no real experience with Docker, so trying to resolve container-related issues was nearly impossible.

### Populating the database

At first, we thought that documents representing nodes in the database could have arbitrary keys.  
However, it turned out that keys must meet certain character requirements, as described earlier.

We initially expected documents like this:

```json
{
  "_key": "People_from_Al-Qassim_Province",
  "popularity_score": 31739
}
```

Unfortunately, they had to look like this:

```json
{
  "_key": "0000052a-168e-41c0-8fc9-0058b0bd1199",
  "name": "People_from_Al-Qassim_Province",
  "popularity_score": 31739
}
```

## Internal architecture of the database in ArangoDB

To work with graphs in ArangoDB, we must define the appropriate collections representing nodes and edges, and we must declare the graph that includes these collections.

## Methodology for fixing the popularity_iw.csv file

During the performance testing sessions for our solutions, we agreed that the **taxonomy_iw.csv** file is correct, and that records in **popularity_iw.csv** can be ignored if they do not match the first file.
To load data from **popularity_iw.csv** correctly, we applied the following rules:

```c#
string newLine = line.Replace("\"", "");
newLine = "\"" + newLine.Replace(",", "\",");
string outputString = newLine + "\n";
```

## Performance and areas for improvement

### Initialization

Database initialization is quite slow; each time it takes around 10 minutes.
The long time is probably partially caused by inserting nodes first and then edges afterwards.
However, compared to a colleague using the browser interface, the initialization time is similar (about 7 minutes).

### Command parsing

Parsing commands from user-provided arguments is implemented quite primitively.  
This could be improved by using a dedicated command-line parsing library.

### Database population

As mentioned earlier, database population is implemented rather inefficiently.
Instead of loading data from **taxonomy_iw.csv** into memory, it could be written directly into the database, keeping only the nodes and popularity values from **popularity_iw.csv** in memory.

### Database queries

If a database query returns no results, a null exception is thrown in our program.  
This does not affect further execution, but it is something that could be improved.

### Exception handling

Exception handling in our program is done rather amateurishly and could be meaningfully improved.

## Summary

The project was a very interesting challenge for us, both in designing the correct program architecture and in figuring out how to properly populate the database.
