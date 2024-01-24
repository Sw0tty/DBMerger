using NotesNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SqlDBManager
{
    static public class DebugVisualizator
    {
        const int VISUAL_LVL_TWO = 8;
        const int VISUAL_LVL_THREE = 12;
        const int VISUAL_LVL_FOUR = 16;

        /// <summary>
        /// Визуализирует входящие данные в резервный словарь в момент вызова визуализации
        /// </summary>
        static public void VisualizateReserv(Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> reloadDict)
        {
            //
            //Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> reloadDict


            /*Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> reloadDict = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();
            reloadDict["SOME_TABLE"] = new Dictionary<string, List<Tuple<string, string>>>();
            reloadDict["SOME_TABLE"] = new Dictionary<string, List<Tuple<string, string>>>() { { "ISN_SOME", new List<Tuple<string, string>>() } };
            reloadDict["SOME_TABLE"] = new Dictionary<string, List<Tuple<string, string>>>() { { "ISN_SOME_2", new List<Tuple<string, string>>() { new Tuple<string, string>("1232132", "dfgdfgf"), new Tuple<string, string>("1232132", "dfgdfgf") } } };
*/

            if (reloadDict.Count == 0)
            {
                MessageBox.Show("словарь пуст");
            }
            else
            {
                //string wrapperKey = "";
                //string insideDictKey = "";
                //string insideDict = $"{insideDictKey}: " + "[\n" + "\n]";


                List<string> insideDicts = new List<string>();
                List<string> insideTuples = new List<string>();

                string dictWrapper = null;

                foreach (string key in reloadDict.Keys)
                {
                    string wrapperKey = key;
                    foreach (string keyColumn in reloadDict[key].Keys)
                    {
                        string insideDictKey = keyColumn;
                        foreach (Tuple<string, string> insideTuple in reloadDict[key][keyColumn])
                        {


                            insideTuples.Add($"{HelpFunction.CreateSpace(VISUAL_LVL_THREE)}({insideTuple.Item1}, {insideTuple.Item2})");

                            string insideDict = $"{HelpFunction.CreateSpace(VISUAL_LVL_TWO)}{insideDictKey}: " + "[\n" + $"{string.Join(",\n", insideTuples)}" + $"\n{HelpFunction.CreateSpace(VISUAL_LVL_TWO)}]";
                            insideDicts.Add(insideDict);
                        }
                        dictWrapper = $"{wrapperKey}: " + "[\n" + $"{string.Join("\n", insideDicts)}" + "\n]";
                    }
                    MessageBox.Show(dictWrapper);
                }
            }
        }

        static public void VisualizateDefaultTables()
        {

        }
    }
}
