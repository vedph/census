# Census

Tools for importing and remodeling Census data.

## Import Command

Import data from a set of Excel files into a MySql database.

Syntax:

```ps1
.\CensusTool.exe import InputDir FilesMask DBName [-d]
```

Where `-d`=dry, i.e. do not import but just test the procedure. Remove the option to effectively import data.

Sample:

```ps1
.\CensusTool.exe import C:\Users\Dfusi\Desktop\Census\ *.tsv census -d
```

Tip: to truncate tables without deleting the DB:

```sql
TRUNCATE TABLE actCategory;
TRUNCATE TABLE actPartner;
TRUNCATE TABLE actProfession;
TRUNCATE TABLE act;
TRUNCATE TABLE actSubtype;
TRUNCATE TABLE actType;
TRUNCATE TABLE book;
TRUNCATE TABLE bookSubtype;
TRUNCATE TABLE bookType;
TRUNCATE TABLE archive;
TRUNCATE TABLE category;
TRUNCATE TABLE company;
TRUNCATE TABLE family;
TRUNCATE TABLE person;
TRUNCATE TABLE place;
TRUNCATE TABLE profession;
```
