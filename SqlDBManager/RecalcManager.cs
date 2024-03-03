using System.Collections.Generic;
using NotesNamespace;
using System;


namespace SqlDBManager
{
    public class RecalcManager
    {
        protected DBCatalog MainCatalog { get; }

        public RecalcManager(DBCatalog catalog)
        {
            MainCatalog = catalog;
        }

        public void RecalcInventory()
        {
            string tableName = "tblINVENTORY";
            string checkTableName = "tblINVENTORY_CHECK";
            string idLikeColumn = "ISN_INVENTORY";

            List<Dictionary<string, string>> tableData = MainCatalog.SelectAllFrom(tableName, MainCatalog.SelectColumnsNames(tableName, null), true);
            List<Tuple<string, string>> pairOfTrad = new List<Tuple<string, string>>() { new Tuple<string, string>("P", "1"), new Tuple<string, string>("P", "2"), new Tuple<string, string>("P", "3"), new Tuple<string, string>("P", "4"), new Tuple<string, string>("A", "5"), new Tuple<string, string>("A", "6"), new Tuple<string, string>("A", "7"), new Tuple<string, string>("A", "8"), new Tuple<string, string>("M", "9") };
            List<Tuple<string, string>> pairOfElect = new List<Tuple<string, string>>() { new Tuple<string, string>("E", "4"), new Tuple<string, string>("E", "5"), new Tuple<string, string>("E", "6"), new Tuple<string, string>("E", "7"), new Tuple<string, string>("E", "8") };

            foreach (Dictionary<string, string> row in tableData)
            {
                int catRegistered = 0, catOcUnits = 0, catUnique = 0, catHasSF = 0, catHasFP = 0, catNotFound = 0, catSecret = 0, catCatalogued = 0;
                int catRegRegistered = 0, catRegOcUnits = 0, catRegUnique = 0, catRegHasSF = 0, catRegHasFP = 0, catRegNotFound = 0, catRegSecret = 0, catRegCatalogued = 0;
                int allRegistered = 0, allOcUnits = 0, allUnique = 0, allHasSF = 0, allHasFP = 0, allNotFound = 0, allSecret = 0, allCatalogued = 0;
                int allRegRegistered = 0, allRegOcUnits = 0, allRegUnique = 0, allRegHasSF = 0, allRegHasFP = 0, allRegNotFound = 0, allRegSecret = 0, allRegCatalogued = 0;

                switch (row["CARRIER_TYPE"])
                {
                    case "'T'":                       
                        foreach (Tuple<string, string> pair in pairOfTrad)
                        {
                            int regRegistered = 0, regOcUnits = 0, regUnique = 0, regHasSF = 0, regHasFP = 0, regNotFound = 0, regSecret = 0, regCatalogued = 0;
                            int unitCount = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2);
                            bool withAccountingUnits = pair.Item1 == "A" || pair.Item1 == "E";
                            
                            int registered = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2);
                            int ocUnits = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.OCD);
                            int unique = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.UNIQUE);
                            int hasSF = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "HAS_SF", "=", "Y");
                            int hasFP = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "HAS_FP", "=", "Y");
                            int notFound = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "IS_IN_SEARCH", "=", "Y");
                            int secret = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "ISN_SECURLEVEL", "!=", "1");
                            int catalogued = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "CATALOGUED", "=", "Y");

                            if (withAccountingUnits)
                            {
                                regRegistered = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2);
                                regOcUnits = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.OCD);
                                regUnique = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.UNIQUE);
                                regHasSF = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "HAS_SF", "=", "Y");
                                regHasFP = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "HAS_FP", "=", "Y");
                                regNotFound = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "IS_IN_SEARCH", "=", "Y");
                                regSecret = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "ISN_SECURLEVEL", "!=", "1");
                                regCatalogued = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "CATALOGUED", "=", "Y");
                            }

                            catRegistered += registered;
                            catOcUnits += ocUnits;
                            catUnique += unique;
                            catHasSF += hasSF;
                            catHasFP += hasFP;
                            catNotFound += notFound;
                            catSecret += secret;
                            catCatalogued += catalogued;
                            catRegRegistered += regRegistered;
                            catRegOcUnits += regOcUnits;
                            catRegUnique += regUnique;
                            catRegHasSF += regHasSF;
                            catRegHasFP += regHasFP;
                            catRegNotFound += regNotFound;
                            catRegSecret += regSecret;
                            catRegCatalogued += regCatalogued;

                            allRegistered += registered;
                            allOcUnits += ocUnits;
                            allUnique += unique;
                            allHasSF += hasSF;
                            allHasFP += hasFP;
                            allNotFound += notFound;
                            allSecret += secret;
                            allCatalogued += catalogued;
                            allRegRegistered += regRegistered;
                            allRegOcUnits += regOcUnits;
                            allRegUnique += regUnique;
                            allRegHasSF += regHasSF;
                            allRegHasFP += regHasFP;
                            allRegNotFound += regNotFound;
                            allRegSecret += regSecret;
                            allRegCatalogued += regCatalogued;

                            MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], pair.Item2, pair.Item1, withAccountingUnits,
                                                                registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                regRegistered, regOcUnits, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);

                            if (pair.Item2 == "4" || pair.Item2 == "8")
                            {
                                MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, pair.Item1, withAccountingUnits,
                                                                    catRegistered, catOcUnits, catUnique, catHasSF, catHasFP, catNotFound, catSecret, catCatalogued,
                                                                    catRegRegistered, catRegOcUnits, catRegUnique, catRegHasSF, catRegHasFP, catRegNotFound, catRegSecret, catRegCatalogued);

                                catRegistered = catOcUnits = catUnique = catHasSF = catHasFP = catNotFound = catSecret = catCatalogued = 0;
                                catRegRegistered = catRegOcUnits = catRegUnique = catRegHasSF = catRegHasFP = catRegNotFound = catRegSecret = catRegCatalogued = 0;
                            }
                        }
                        MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, null, true,
                                                            allRegistered, allOcUnits, allUnique, allHasSF, allHasFP, allNotFound, allSecret, allCatalogued,
                                                            allRegRegistered, allRegOcUnits, allRegUnique, allRegHasSF, catRegHasFP, allRegNotFound, allRegSecret, allRegCatalogued);
                        MainCatalog.UpdateInventoryCheck(checkTableName, row[idLikeColumn]);
                        break;
                    case "'E'":
                        foreach (Tuple<string, string> pair in pairOfElect)
                        {
                            int regRegistered = 0, regOcUnits = 0, regUnique = 0, regHasSF = 0, regHasFP = 0, regNotFound = 0, regSecret = 0, regCatalogued = 0;
                            int unitCount = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2);
                            bool withAccountingUnits = pair.Item1 == "A" || pair.Item1 == "E";

                            int registered = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2);
                            int ocUnits = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.OCD);
                            int unique = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.UNIQUE);
                            int hasSF = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "HAS_SF", "=", "Y");
                            int hasFP = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "HAS_FP", "=", "Y");
                            int notFound = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "IS_IN_SEARCH", "=", "Y");
                            int secret = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "ISN_SECURLEVEL", "!=", "1");
                            int catalogued = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2, "CATALOGUED", "=", "Y");
                            regRegistered = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2);
                            regOcUnits = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.OCD);
                            regUnique = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "UNIT_CATEGORY", "=", Consts.RecalcConsts.UnitCategory.UNIQUE);
                            regHasSF = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "HAS_SF", "=", "Y");
                            regHasFP = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "HAS_FP", "=", "Y");
                            regNotFound = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "IS_IN_SEARCH", "=", "Y");
                            regSecret = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "ISN_SECURLEVEL", "!=", "1");
                            regCatalogued = MainCatalog.Ultra_SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.ACCOUNTING, row[idLikeColumn], pair.Item2, "CATALOGUED", "=", "Y");
                            
                            catRegistered += registered;
                            catOcUnits += ocUnits;
                            catUnique += unique;
                            catHasSF += hasSF;
                            catHasFP += hasFP;
                            catNotFound += notFound;
                            catSecret += secret;
                            catCatalogued += catalogued;
                            catRegRegistered += regRegistered;
                            catRegOcUnits += regOcUnits;
                            catRegUnique += regUnique;
                            catRegHasSF += regHasSF;
                            catRegHasFP += regHasFP;
                            catRegNotFound += regNotFound;
                            catRegSecret += regSecret;
                            catRegCatalogued += regCatalogued;

                            MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], pair.Item2, pair.Item1, withAccountingUnits,
                                                                registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                regRegistered, regOcUnits, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);

                            if (pair.Item2 == "8")
                            {
                                MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, pair.Item1, withAccountingUnits,
                                                                    catRegistered, catOcUnits, catUnique, catHasSF, catHasFP, catNotFound, catSecret, catCatalogued,
                                                                    catRegRegistered, catRegOcUnits, catRegUnique, catRegHasSF, catRegHasFP, catRegNotFound, catRegSecret, catRegCatalogued);

                                catRegistered = catOcUnits = catUnique = catHasSF = catHasFP = catNotFound = catSecret = catCatalogued = 0;
                                catRegRegistered = catRegOcUnits = catRegUnique = catRegHasSF = catRegHasFP = catRegNotFound = catRegSecret = catRegCatalogued = 0;
                            }
                        }
                        MainCatalog.UpdateInventoryCheck(checkTableName, row[idLikeColumn]);
                        break;
                }              
            }
        }

        public void RecalcFund()
        {
            string tableName = "tblFUND";
            string checkTableName = "tblFUND_CHECK";
            string idLikeColumn = "ISN_FUND";

            List<Dictionary<string, string>> tableData = MainCatalog.SelectAllFrom(tableName, MainCatalog.SelectColumnsNames(tableName, null), true);

            foreach (Dictionary<string, string> row in tableData)
            {

                // -- FundCheck recalc --
                List<Dictionary<string, string>>  fundAttachedInventoryCheck = MainCatalog.SelectFundAttachedInventoryCheck(row[idLikeColumn]);
                int cardboarded = 0, needCardboarded = 0, damaged = 0, needRestoration = 0, needBinding = 0, needDisinfection = 0, needDisinsection = 0, fading = 0, needEnciphering = 0, needCoverChange = 0, flamed = 0, needKPO = 0;               
                
                foreach (Dictionary<string, string> checkRow in fundAttachedInventoryCheck)
                {
                    cardboarded += HelpFunction.ConventToInt(checkRow["CARDBOARDED"]);
                    needCardboarded += HelpFunction.ConventToInt(checkRow["UNITS_NEED_CARDBOARDED"]);
                    damaged += HelpFunction.ConventToInt(checkRow["UNITS_DBR"]);
                    needRestoration += HelpFunction.ConventToInt(checkRow["UNITS_NEED_RESTORATION"]);
                    needBinding += HelpFunction.ConventToInt(checkRow["UNITS_NEED_BINDING"]);
                    needDisinfection += HelpFunction.ConventToInt(checkRow["UNITS_NEED_DISINFECTION"]);
                    needDisinsection += HelpFunction.ConventToInt(checkRow["UNITS_NEED_DISINSECTION"]);
                    fading += HelpFunction.ConventToInt(checkRow["FADING_PAGES"]);
                    needEnciphering += HelpFunction.ConventToInt(checkRow["UNITS_NEED_ENCIPHERING"]);
                    needCoverChange += HelpFunction.ConventToInt(checkRow["UNITS_NEED_COVER_CHANGE"]);
                    flamed += HelpFunction.ConventToInt(checkRow["UNITS_INFLAMMABLE"]);
                    needKPO += HelpFunction.ConventToInt(checkRow["UNITS_NEED_KPO"]);
                }
                if (row[idLikeColumn] == "'39700'")
                {
                    MainCatalog.UpdateFundInventoryCount(row[idLikeColumn], fundAttachedInventoryCheck.Count);
                    MainCatalog.UpdateFundCheck(checkTableName, idLikeColumn, row[idLikeColumn], cardboarded, needCardboarded, damaged, needRestoration, needBinding, needDisinfection, needDisinsection, fading, needEnciphering, needCoverChange, flamed, needKPO);
                }
                // -- FundCheck recalc --

                // -- FundDocStats recalc --
                List<Tuple<string, string>> pairOfTypes = new List<Tuple<string, string>>() { new Tuple<string, string>("P", "1"), new Tuple<string, string>("P", "2"), new Tuple<string, string>("P", "3"), new Tuple<string, string>("P", "4"), new Tuple<string, string>("A", "5"), new Tuple<string, string>("A", "6"), new Tuple<string, string>("A", "7"), new Tuple<string, string>("A", "8"), new Tuple<string, string>("M", "9"), new Tuple<string, string>("E", "4"), new Tuple<string, string>("E", "5"), new Tuple<string, string>("E", "6"), new Tuple<string, string>("E", "7"), new Tuple<string, string>("E", "8"), new Tuple<string, string>("null", "null"), new Tuple<string, string>("E", "null"), new Tuple<string, string>("P", "null"), new Tuple<string, string>("A", "null") };
                List<Dictionary<string, string>> fundAttachecInventoryDocStats = MainCatalog.SelectFundAttachedInventoryDocStats(row[idLikeColumn]);
                int unitsCount = 0, unitsOCCount = 0, unitsHasSF = 0, unitsHasFP = 0, unitsNotFoundCount = 0, unitsSecretCount = 0, unitsInSearchCount = 0, unintsUniqueCount = 0, unitsCatalaguedCount = 0;
                int regUnitsCount = 0, regUnitsOCCount = 0, regUnitsHasSF = 0, regUnitHasFP = 0, regUnitNotFoundCount = 0, regUnitSecretCount = 0, regUnitInSearchCount = 0, reqUnintsUniqueCount = 0, regUnitsCatalaguedCount = 0;

                foreach (Tuple<string, string> pairOfType in pairOfTypes)
                {
                    foreach (Dictionary<string, string> invDocStat in fundAttachecInventoryDocStats)
                    {                       
                        if (invDocStat["ISN_DOC_TYPE"].Replace("\'", "") == pairOfType.Item2 && invDocStat["CARRIER_TYPE"].Replace("\'", "") == pairOfType.Item1)
                        {
                            unitsCount += HelpFunction.ConventToInt(invDocStat["UNIT_REGISTERED"]);
                            regUnitsCount += HelpFunction.ConventToInt(invDocStat["REG_UNIT_REGISTERED"]);
                            unitsOCCount += HelpFunction.ConventToInt(invDocStat["UNIT_OC_COUNT"]);
                            regUnitsOCCount += HelpFunction.ConventToInt(invDocStat["REG_UNIT_OC"]);
                            unitsHasSF += HelpFunction.ConventToInt(invDocStat["UNIT_HAS_SF"]);
                            regUnitsHasSF += HelpFunction.ConventToInt(invDocStat["REG_UNIT_HAS_SF"]);
                            unitsHasFP += HelpFunction.ConventToInt(invDocStat["UNIT_HAS_FP"]);
                            regUnitHasFP += HelpFunction.ConventToInt(invDocStat["REG_UNIT_HAS_FP"]);
                            unitsNotFoundCount += HelpFunction.ConventToInt(invDocStat["UNITS_NOT_FOUND"]);
                            regUnitNotFoundCount += HelpFunction.ConventToInt(invDocStat["REG_UNITS_NOT_FOUND"]);
                            unitsSecretCount += HelpFunction.ConventToInt(invDocStat["SECRET_UNITS"]);
                            regUnitSecretCount += HelpFunction.ConventToInt(invDocStat["SECRET_REG_UNITS"]);
                            unitsInSearchCount += HelpFunction.ConventToInt(invDocStat["UNITS_SEARCH"]);
                            regUnitInSearchCount += HelpFunction.ConventToInt(invDocStat["REG_UNITS_SEARCH"]);
                            unintsUniqueCount += HelpFunction.ConventToInt(invDocStat["UNITS_UNIQUE"]);
                            reqUnintsUniqueCount += HelpFunction.ConventToInt(invDocStat["REG_UNITS_UNIQUE"]);
                            unitsCatalaguedCount += HelpFunction.ConventToInt(invDocStat["UNITS_CATALOGUED"]);
                            regUnitsCatalaguedCount += HelpFunction.ConventToInt(invDocStat["REG_UNITS_CTALOGUE"]);
                        }
                    }
                    MainCatalog.UpdateFundDocStats(row[idLikeColumn], pairOfType.Item2, pairOfType.Item1, true, unitsCount, unitsOCCount, unintsUniqueCount, unitsHasSF, unitsHasFP, unitsNotFoundCount, unitsSecretCount, unitsCatalaguedCount,
                                                   regUnitsCount, regUnitsOCCount, reqUnintsUniqueCount, regUnitsHasSF, regUnitHasFP, regUnitNotFoundCount, regUnitSecretCount, regUnitsCatalaguedCount);

                    unitsCount = regUnitsCount = unitsOCCount = regUnitsOCCount = unitsHasSF = regUnitsHasSF = unitsHasFP = regUnitHasFP = unitsNotFoundCount = regUnitNotFoundCount = unitsSecretCount = regUnitSecretCount = unitsInSearchCount = regUnitInSearchCount = unintsUniqueCount = reqUnintsUniqueCount = unitsCatalaguedCount = regUnitsCatalaguedCount = 0;
                }
                // -- FundDocStats recalc --
            }
        }

        public void RecalcPassports()
        {
            List<Tuple<string, string>> pairOfTypes = new List<Tuple<string, string>>() { new Tuple<string, string>("P", "1"), new Tuple<string, string>("P", "2"), new Tuple<string, string>("P", "3"), new Tuple<string, string>("P", "4"), new Tuple<string, string>("A", "5"), new Tuple<string, string>("A", "6"), new Tuple<string, string>("A", "7"), new Tuple<string, string>("A", "8"), new Tuple<string, string>("M", "9"), new Tuple<string, string>("E", "4"), new Tuple<string, string>("E", "5"), new Tuple<string, string>("E", "6"), new Tuple<string, string>("E", "7"), new Tuple<string, string>("E", "8"), new Tuple<string, string>("null", "null"), new Tuple<string, string>("E", "null"), new Tuple<string, string>("P", "null"), new Tuple<string, string>("A", "null") };

            MainCatalog.DeleteArchivePassports();

            string startYear = MainCatalog.SelectFirstYear();
            int passportID = 1;
            int statID = 1;

            for (int year = Convert.ToInt32(startYear); year <= DateTime.Now.Year; year++)
            {
                MainCatalog.CreatePassport($"{passportID}", $"{year}");

                foreach (Tuple<string, string> pair in pairOfTypes)
                {
                    // при создании сразу пересчитанные значения.
                    MainCatalog.CreatePassportStat($"{passportID}", $"{statID}", pair.Item2, pair.Item1);


                    statID++;
                }
                passportID++;
            }
        }
    }
}
