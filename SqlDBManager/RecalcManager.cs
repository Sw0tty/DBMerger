using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            // Trad
            List<Tuple<string, string>> pairOfTrad = new List<Tuple<string, string>>() { new Tuple<string, string>("P", "1"), new Tuple<string, string>("P", "2"), new Tuple<string, string>("P", "3"), new Tuple<string, string>("P", "4"), new Tuple<string, string>("A", "5"), new Tuple<string, string>("A", "6"), new Tuple<string, string>("A", "7"), new Tuple<string, string>("A", "8"), new Tuple<string, string>("M", "9") };

            // Elect
            List<Tuple<string, string>> pairOfElect = new List<Tuple<string, string>>() { new Tuple<string, string>("E", "4"), new Tuple<string, string>("E", "5"), new Tuple<string, string>("E", "6"), new Tuple<string, string>("E", "7"), new Tuple<string, string>("E", "8") };

            foreach (Dictionary<string, string> row in tableData)
            {
                int allInventoryUnitCount = 0;
                int categoryUnitCount = 0;

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
                                // тогда нужно 703 и 704
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


                            // обновление строк не считая строки Всего и блока Всего
                            if (row[idLikeColumn] == "'39776'")
                                MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], pair.Item2, pair.Item1, withAccountingUnits,
                                                                    registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                    regRegistered, regOcUnits, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);


                            allInventoryUnitCount += unitCount;
                            categoryUnitCount += unitCount;


                            // Обновление строки всего в блоке
                            if (pair.Item2 == "4" || pair.Item2 == "8")
                            {
                                //MessageBox.Show(categoryUnitCount.ToString());
                                // делаем запрос на обновление количества в категории
                                if (row[idLikeColumn] == "'39776'")
                                    MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, pair.Item1, withAccountingUnits,
                                                                        catRegistered, catOcUnits, catUnique, catHasSF, catHasFP, catNotFound, catSecret, catCatalogued,
                                                                        catRegRegistered, catRegOcUnits, catRegUnique, catRegHasSF, catRegHasFP, catRegNotFound, catRegSecret, catRegCatalogued);
                                    //MainCatalog.UpdateInventoryCount(row[idLikeColumn], null, pair.Item1, categoryUnitCount);

                                categoryUnitCount = 0;

                                catRegistered = catOcUnits = catUnique = catHasSF = catHasFP = catNotFound = catSecret = catCatalogued = 0;
                                catRegRegistered = catRegOcUnits = catRegUnique = catRegHasSF = catRegHasFP = catRegNotFound = catRegSecret = catRegCatalogued = 0;
                            }

                        }
                        if (row[idLikeColumn] == "'39776'")
                        {
                            MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, null, true,
                                                                allRegistered, allOcUnits, allUnique, allHasSF, allHasFP, allNotFound, allSecret, allCatalogued,
                                                                allRegRegistered, allRegOcUnits, allRegUnique, allRegHasSF, catRegHasFP, allRegNotFound, allRegSecret, allRegCatalogued);

                            //MainCatalog.UpdateInventoryCount(row[idLikeColumn], null, null, allInventoryUnitCount);

                            MainCatalog.UpdateInventoryCheck(checkTableName, row[idLikeColumn]);
                        }
                            
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


                            // обновление строк не считая строки Всего и блока Всего
                            MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], pair.Item2, pair.Item1, withAccountingUnits,
                                                                registered, ocUnits, unique, hasSF, hasFP, notFound, secret, catalogued,
                                                                regRegistered, regOcUnits, regUnique, regHasSF, regHasFP, regNotFound, regSecret, regCatalogued);


                            // Обновление строки всего в блоке
                            if (pair.Item2 == "8")
                            {
                                MainCatalog.UpdateInventoryDocStats(row[idLikeColumn], null, pair.Item1, withAccountingUnits,
                                                                    catRegistered, catOcUnits, catUnique, catHasSF, catHasFP, catNotFound, catSecret, catCatalogued,
                                                                    catRegRegistered, catRegOcUnits, catRegUnique, catRegHasSF, catRegHasFP, catRegNotFound, catRegSecret, catRegCatalogued);

                                catRegistered = catOcUnits = catUnique = catHasSF = catHasFP = catNotFound = catSecret = catCatalogued = 0;
                                catRegRegistered = catRegOcUnits = catRegUnique = catRegHasSF = catRegHasFP = catRegNotFound = catRegSecret = catRegCatalogued = 0;
                            }







                            /*int unitCount = MainCatalog.SelectInventoryUnitCount(Consts.RecalcConsts.UnitKind.KEEPING, row[idLikeColumn], pair.Item2);
                            categoryUnitCount += unitCount;

                            if (pair.Item2 == "8")
                            {
                                if (row[idLikeColumn] == "'39776'")
                                    MainCatalog.UpdateInventoryCount(row[idLikeColumn], null, pair.Item1, categoryUnitCount);

                                categoryUnitCount = 0;
                            }*/
                        }
                        
                            //MainCatalog.UpdateInventoryCount(row[idLikeColumn], null, null, allInventoryUnitCount);
                            //MainCatalog.UpdateCheck(checkTableName, idLikeColumn, row[idLikeColumn], MainCatalog.SelectCarboarderedUnit(idLikeColumn, row[idLikeColumn]));

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

                // -- FundDocStats recalc --
            }
        }
    }
}
