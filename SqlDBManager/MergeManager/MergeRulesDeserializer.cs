using System.Collections.Generic;


namespace SqlDBManager
{
    public class MergeRulesDeserializer
    {
        public Dictionary<string, TableReferenceRuleDeserializer> tablesReferences { get; set; }
        public Dictionary<string, OnlyIdsTableRulesDeserializer> onlyIdsTables { get; set; }
        public List<string> tablesForCleaning { get; set; }
        public Dictionary<string, SimpleTableRulesDeserializer> simpleTables { get; set; }
        public Dictionary<string, LinkTableRulesDeserializer> linksTables { get; set; }
    }

    public class OnlyIdsTableRulesDeserializer
    {
        public string idColumnName { get; set; }
        public string uniqueValueColumnName { get; set; }
    }

    public class SimpleTableRulesDeserializer
    {
        public string secondIdColumnName { get; set; }
        public string parentIdColumnName { get; set; }
        public string uniqueValueColumnName { get; set; }
        public List<string> excludeSpecialsColumnsNames { get; set; }
        public bool allowsNullValues { get; set; }
        public bool saveIdsForLinks { get; set; }
    }

    public class LinkTableRulesDeserializer
    {
        public string secondIdColumnName { get; set; }
        public string parentIdColumnName { get; set; }
        public string uniqueValueColumnName { get; set; }
        public string numerateColumn { get; set; }
        public List<string> excludeSpecialsColumnsNames { get; set; }
        public List<string> foreignKeyColumns { get; set; } // Принудительные связи - сейчас не актуально
        public bool allowsNullValues { get; set; }
        public List<CheckColumnValueDeserializer> checkOnValue { get; set; }
        public LinkTableRuleExistingDaughterValuesForParent existingDaughterValuesForParent { get; set; }
    }

    public class LinkTableRuleExistingDaughterValuesForParent
    {
        public string toTableName { get; set; }
        public string fromColumnName { get; set; }
        public bool onlyNew { get; set; }
    }

    public class TableReferenceRuleDeserializer
    {
        public string columnForAddRef { get; set; }
        public string refOnTable { get; set; }
        public string refOnColumn { get; set; }
    }

    public class CheckColumnValueDeserializer
    {
        public string columnName { get; set; }
        public string equalTo { get; set; }
        public string notEqualTo { get; set; }
        public List<string> equalAnyTo { get; set; }
    }
}
