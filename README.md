# SQLDBManager
## MergeRulesFile
### tablesReferences
Dictionary of tables with logical references, but without configured connectivity in the database. This step is necessary so that the utility can detect existing connections.
Each table must have keys:
columnForAddRef - the column in table with foreign key;
refOnTable - the table where the key refers to;
refOnColumn - the column in table where the key refers to.

Example:
"tblACT": {
			"columnForAddRef": "doc_id",
			"refOnTable": "tblDOCUMENT",
			"refOnColumn": "id"
		},

### onlyIdsTables

Example: "tblARCHIVE": {
			"idColumnName": "ISN_ARCHIVE"
		},

### tablesForCleaning
List of tables for cleaning. If necessary, you can transfer the tables that need to be cleaned. For example, from logs.

### simpleTables
Dictionary with tables rules without foreign keys.

Example: "tableName": {
            "secondIdColumnName": "",
            "parentIdColumnName": "",
            "uniqueValueColumnName": "",
            "excludeSpecialsColumnsNames": [],
            "allowsNullValues": false,
            "saveIdsForLinks": false
        }

### linksTables
Dictionary with tables rules with foreign keys.

Example: "tableName": {
            "uniqueValueColumnName": "",
			"secondIdColumnName": "",
			"parentIdColumnName": "",
			"numerateColumn": "",
			"excludeSpecialsColumnsNames": [],
			"allowsNullValues": true,
            "existingDaughterValuesForParent": {
				"toTableName": "",
				"fromColumnName": "",
                "onlyNew": false
			}
		}
