# OrgChart
This is a program to manage the Organizational Structure in a company.

## Goals
The program should allow us to perform the following.
1) Add employees
2) Generate the Org Chart in a specific format.
3) Remove employees
4) Move an employee from on manager to another
5) Count the number of employees that ultimately report up to the given manager.

## Input
The input for the program is a text file comprising of a series of commands and parameters that may look like this:

```
6 <== Number of lines in the input data to process.
add, 10, Sharilyn Gruber, -1  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
add, 7, Denice Mattice, 10  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
add, 3, Lawana Futrell, -1  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
add, 34, Lissette Gorney, 7  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
add, 5, Lan Puls, 3  <== Add Sharilyn Gruber as an employee with ID 10, with no manager.
Print  <== Print the Org Chart as specified below.
```

## Output
The output of the program, when fed the above as input, will be
```
Sharlyn Gruber [10]
 Denice Mattice [7]
  Lissette Gorney [34]
Lawana Futrell [3]
 Lan Puls [5]
```

Each employee is printed as a Name [employee id] - There is a space between the name and ID.
At each reporting level, add 2 spaces precedding the name.

Employees are always displayed in the order they are processed.


## Adding and Employee
The command is of the format
```add, <employee id>, <name>, <manager id>```

If an employee with an id has been added, then subsequent additions of employees with the same id are ignored.
If an employee's manager has not already been added, then the employee is considered to have no manager.

## Moving an Employee
The command is of the format
```move, <employee id>, <new manager id>```

If an employee of that id does not exist, then do nothing.
If an employee with the new manager id does not exist, then do nothing.
The newly moved employee will be appended to the manager's list of reports, when the org chart is printed.
Subsequent adds will continue to be added to the end of the list of reports.

## Removing an Employee
The command is of the format
```remove, <employee id>```

If an employee of that id does not exist, then do nothing.
After removal, if the employee had any reports, they should now report to the removed employee's manager.
These employees will show up in the same order as they did under thier former manager, but after any current reports of the new manager.
If the removed employee had no manager, then the reports will have no manager either.

## Counting Employees
The command is of the format
```count, <employee id>```

This should return the total number of reports, including any indirect reports (all descendant reports), that the specified employee has.

