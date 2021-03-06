# Census

Quick Docker image build: `docker build . -t vedph2020/census_api:1.0.4 -t vedph2020/census_api:latest` (replace with the current version).

Tools for importing and remodeling Census data.

## Import Command

Import data from a set of Excel files into a MySql database.

Syntax:

```ps1
.\CensusTool.exe import InputDir FilesMask DBName
```

Sample:

```ps1
.\CensusTool.exe import C:\Users\Dfusi\Desktop\Census\ *.tsv census
```

Tip: to truncate tables without deleting the DB:

```sql
SET FOREIGN_KEY_CHECKS = 0;
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
SET FOREIGN_KEY_CHECKS = 1;
```

Some handy counts:

```sql
SELECT COUNT(*) FROM archive AS archive_count;
SELECT COUNT(*) FROM book AS book_count;
SELECT COUNT(*) FROM person AS person_count;
SELECT COUNT(*) FROM family AS family_count;
SELECT COUNT(*) FROM place AS place_count;
SELECT COUNT(*) FROM profession AS profession_count;
SELECT COUNT(*) FROM category AS category_count;
```
