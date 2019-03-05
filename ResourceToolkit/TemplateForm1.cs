using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Net;

namespace ResourceToolkit
{
    public partial class TemplateForm1 : Form
    {
        public static List<object> metadataLists = new List<object>();//基础数据列表集合
        public static String bookName = "";//具体那一册教材
        public static List<String> catologList = new List<string>();//教材目录列表

        public TemplateForm1()
        {
            InitializeComponent();
            bookName = "";
            metadataLists.Clear();
            catologList.Clear();
        }

        /*
        /// <summary>
        /// 二级关联示范
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {      
            string fileName, strConn;
            ArrayList sheetsList = new ArrayList();
            ArrayList contentList = new ArrayList();     //新建动态数组
            DataSet ds = new DataSet();
            fileName = "test.xlsx";
            
            if (File.Exists(fileName))
            {
                strConn = "";
                string fileTail = Path.GetExtension(fileName);
                if (fileTail == ".xlsx")
                    strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", fileName);
                else if(fileTail == ".xls")
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";" + "Extended Properties=Excel 8.0;";
                using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(strConn))
                {
                    connection.Open();
                    DataTable table = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    string[] strTableNames = new string[table.Rows.Count];
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        strTableNames[k] = table.Rows[k]["TABLE_NAME"].ToString();
                        if (strTableNames[k].ToString().Trim().Substring(strTableNames[k].Length - 1, 1) == "$")
                        {
                            sheetsList.Add(strTableNames[k].Substring(0, strTableNames[k].Length - 1));
                        }
                    }
                    if (sheetsList.Count > 1)
                    {
                        try
                        {
                            OleDbDataAdapter myCommand = null;
                            //从指定的表明查询数据，可先把所有表名列出来
                            string strExcel = "select * from [" + sheetsList[1] + "$" + "]";
                            myCommand = new OleDbDataAdapter(strExcel, strConn);
                            myCommand.Fill(ds, "Bicycle");
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show("单元异常，请检测！");
                        }

                        #region 处理DataSet
                        if (ds.Tables.Count > 0)
                        {
                            foreach (DataRow r in ds.Tables[0].Rows)
                            {
                                contentList.Add(r["科目"].ToString());
                            }
                        }
                        #endregion                      
                    }
                    connection.Close();
                }

                if (contentList.Count > 0)
                {
                    try
                    {
                        string fileTail2 = Path.GetExtension(fileName);
                        if (fileTail2 == ".xlsx")
                        {
                            XSSFWorkbook xssfworkbook = new XSSFWorkbook(new FileStream(fileName, FileMode.Open));

                            XSSFSheet sheet = (XSSFSheet)xssfworkbook.GetSheet(sheetsList[0].ToString());
                            CellRangeAddressList regions = new CellRangeAddressList(0, 65535, 0, 0);
                            XSSFDataValidationHelper dvHelper = new XSSFDataValidationHelper(sheet);
                            XSSFDataValidationConstraint dvConstraint = (XSSFDataValidationConstraint)dvHelper.CreateExplicitListConstraint((String[])contentList.ToArray(typeof(String)));
                            XSSFDataValidation dataValidate = (XSSFDataValidation)dvHelper.CreateValidation(dvConstraint, regions);
                            sheet.AddValidationData(dataValidate);

                            using (FileStream fs = new FileStream("test1.xlsx", FileMode.OpenOrCreate, FileAccess.ReadWrite))
                            {
                                xssfworkbook.Write(fs);
                                fs.Close();
                            }
                        }
                        else if (fileTail2 == ".xls")
                        {
                            HSSFWorkbook hssfworkbook = new HSSFWorkbook(new FileStream(fileName, FileMode.Open));
                            
                            NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheet(sheetsList[0].ToString());
                            CellRangeAddressList regions = new CellRangeAddressList(0, 65535, 0, 0);
                            DVConstraint constraint = DVConstraint.CreateExplicitListConstraint((String[])contentList.ToArray(typeof(String)));
                            HSSFDataValidation dataValidate = new HSSFDataValidation(regions, constraint);
                            sheet.AddValidationData(dataValidate);

                            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                            {
                                hssfworkbook.Write(fs);
                                fs.Close();
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                
            }
        }
 */

        /// <summary>
        /// 生成基础数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            metadataLists.Clear();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
            string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];
            //需要查询的字段21
            string urlSUB = String.Format(head + tail, "SUB");//0学科
            string urlGRADE = String.Format(head + tail, "GRADE");//1年级
            string urlSSTAGE = String.Format(head + tail, "SSTAGE");//2学段
            string urlBREEL = String.Format(head + tail, "BREEL");//3册别
            //string urlSTANDARDBOOK = String.Format(head+tail,"STANDARDBOOK");//3????????教材目录
            string urlJXHJ = String.Format(head + tail, "JXHJ");//4教学环节 
            string urlSYDX = String.Format(head + tail, "SYDX");//5适用对象 
            string urlNEWJXMK = String.Format(head + tail, "NEWJXMK");//6新教学模块 
            string urlJXMK = String.Format(head + tail, "JXMK");//7英语教学模块
            string urlSXJXMK = String.Format(head + tail, "SXJXMK");//8数学教学模块
            string urlYWJXMK = String.Format(head + tail, "YWJXMK");//9语文教学模块
            string urlRIGHTS = String.Format(head + tail, "RIGHTS");//10资源权限       
            string urlPJ = String.Format(head + tail, "PJ");//11资源评级       
            string urlRESOURCEORIGIN = String.Format(head + tail, "RESOURCEORIGIN");//12资源来源        
            string urlRESOURCERECOMMEND = String.Format(head + tail, "RESOURCERECOMMEND");//13资源推荐
            string urlRSORT = String.Format(head + tail, "RSORT");//14资源类型                   
            string urlED = String.Format(head + tail, "ED");//15版本        
            string urlJXXS = String.Format(head + tail, "JXXS");//16教学形式 
            string urlQUESTIONTYPES = String.Format(head + tail, "QUESTIONTYPES");//17题型       
            string urlWORDDATABASE = String.Format(head + tail, "WORDDATABASE");//18词库      
            string urlCRINFO = String.Format(head + tail, "CRINFO");//19版权信息 
            string urlKLLX = String.Format(head + tail, "KLLX");//21课例类型   
            string urlKNOWLEDGE = String.Format(head + tail, "KNOWLEDGE");//22知识点  20,21,22              
            //0学科
            string jsonSUB = Common.HttpGet(urlSUB); //y({....})
            string jsonSUB2 = jsonSUB.Substring(2, jsonSUB.Length - 3);//{}
            List<String> resultSUBList = Common.DealJson(jsonSUB2, "SUB");
            //1年级
            string jsonGRADE = Common.HttpGet(urlGRADE); //y({....})
            string jsonGRADE2 = jsonGRADE.Substring(2, jsonGRADE.Length - 3);//{}
            List<String> resultGRADEList = Common.DealJson(jsonGRADE2, "GRADE");
            //2学段 
            string jsonSSTAGE = Common.HttpGet(urlSSTAGE); //y({....})
            string jsonSSTAGE2 = jsonSSTAGE.Substring(2, jsonSSTAGE.Length - 3);//{}
            List<String> resultSSTAGEList = Common.DealJson(jsonSSTAGE2, "SSTAGE");
            //3册别
            string jsonBREEL = Common.HttpGet(urlBREEL); //y({....})
            string jsonBREEL2 = jsonBREEL.Substring(2, jsonBREEL.Length - 3);//{}
            List<String> resultBREELList = Common.DealJson(jsonBREEL2, "BREEL");
            //4教学环节
            string jsonJXHJ = Common.HttpGet(urlJXHJ); //y({....})
            string jsonJXHJ2 = jsonJXHJ.Substring(2, jsonJXHJ.Length - 3);//{}
            List<String> resultJXHJList = Common.DealJson(jsonJXHJ2, "JXHJ");
            //5适用对象
            string jsonSYDX = Common.HttpGet(urlSYDX); //y({....})
            string jsonSYDX2 = jsonSYDX.Substring(2, jsonSYDX.Length - 3);//{}
            List<String> resultSYDXList = Common.DealJson(jsonSYDX2, "SYDX");
            //6新教学模块
            string jsonNEWJXMK = Common.HttpGet(urlNEWJXMK); //y({....})
            string jsonNEWJXMK2 = jsonNEWJXMK.Substring(2, jsonNEWJXMK.Length - 3);//{}
            List<String> resultNEWJXMKList = Common.DealJson(jsonNEWJXMK2, "NEWJXMK");

            int a = 0, b = 0, c = 0;
            for (int i = 0; i < resultNEWJXMKList.Count; i++)
            {
                String item = resultNEWJXMKList[i];
                if (item.Substring(item.IndexOf("_") + 1) == "011") //英语教学模块
                    a = i;
                if (item.Substring(item.IndexOf("_") + 1) == "012") //语文教学模块
                    b = i;
                if (item.Substring(item.IndexOf("_") + 1) == "013") //数学教学模块
                    c = i;
            }
            List<String> resultJXMKList = new List<string>();
            List<String> resultSXJXMKList = new List<string>();
            List<String> resultYWJXMKList = new List<string>();
            for (int i = a + 1; i < b; i++)
            {
                resultJXMKList.Add(resultNEWJXMKList[i]);
            }
            for (int i = b + 1; i < c; i++)
            {
                resultYWJXMKList.Add(resultNEWJXMKList[i]);
            }
            for (int i = c + 1; i < resultNEWJXMKList.Count; i++)
            {
                resultSXJXMKList.Add(resultNEWJXMKList[i]);
            }


            //7英语教学模块
            string jsonJXMK = Common.HttpGet(urlJXMK); //y({....})
            string jsonJXMK2 = jsonJXMK.Substring(2, jsonJXMK.Length - 3);//{}
            List<String> resultJXMKList2 = Common.DealJson(jsonJXMK2, "JXMK");
            //8数学教学模块
            string jsonSXJXMK = Common.HttpGet(urlSXJXMK); //y({....})
            string jsonSXJXMK2 = jsonSXJXMK.Substring(2, jsonSXJXMK.Length - 3);//{}
            List<String> resultSXJXMKList2 = Common.DealJson(jsonSXJXMK2, "SXJXMK");
            //9语文教学模块
            string jsonYWJXMK = Common.HttpGet(urlYWJXMK); //y({....})
            string jsonYWJXMK2 = jsonYWJXMK.Substring(2, jsonYWJXMK.Length - 3);//{}
            List<String> resultYWJXMKList2 = Common.DealJson(jsonYWJXMK2, "YWJXMK");
            //10资源权限
            string jsonRIGHTS = Common.HttpGet(urlRIGHTS); //y({....})
            string jsonRIGHTS2 = jsonRIGHTS.Substring(2, jsonRIGHTS.Length - 3);//{}
            List<String> resultRIGHTSList = Common.DealJson(jsonRIGHTS2, "RIGHTS");
            //11资源评级
            string jsonPJ = Common.HttpGet(urlPJ); //y({....})
            string jsonPJ2 = jsonPJ.Substring(2, jsonPJ.Length - 3);//{}
            List<String> resultPJList = Common.DealJson(jsonPJ2, "PJ");
            //12资源来源
            string jsonRESOURCEORIGIN = Common.HttpGet(urlRESOURCEORIGIN); //y({....})
            string jsonRESOURCEORIGIN2 = jsonRESOURCEORIGIN.Substring(2, jsonRESOURCEORIGIN.Length - 3);//{}
            List<String> resultRESOURCEORIGINList = Common.DealJson(jsonRESOURCEORIGIN2, "RESOURCEORIGIN");
            //13资源推荐
            string jsonRESOURCERECOMMEND = Common.HttpGet(urlRESOURCERECOMMEND); //y({....})
            string jsonRESOURCERECOMMEND2 = jsonRESOURCERECOMMEND.Substring(2, jsonRESOURCERECOMMEND.Length - 3);//{}
            List<String> resultRESOURCERECOMMENDList = Common.DealJson(jsonRESOURCERECOMMEND2, "RESOURCERECOMMEND");
            //14资源类型
            string jsonRSORT = Common.HttpGet(urlRSORT); //y({....})
            string jsonRSORT2 = jsonRSORT.Substring(2, jsonRSORT.Length - 3);//{}
            List<String> resultRSORTList = Common.DealJson(jsonRSORT2, "RSORT");
            //15版本
            string jsonED = Common.HttpGet(urlED); //y({....})
            string jsonED2 = jsonED.Substring(2, jsonED.Length - 3);//{}
            List<String> resultEDList = Common.DealJson(jsonED2, "ED");
            if (resultEDList.Count > 0)
            {
                comboBox1.Items.AddRange(resultEDList.ToArray());
                comboBox1.Text = "请选择版本";
            }
            //16教学形式
            string jsonJXXS = Common.HttpGet(urlJXXS); //y({....})
            string jsonJXXS2 = jsonJXXS.Substring(2, jsonJXXS.Length - 3);//{}
            List<String> resultJXXSList = Common.DealJson(jsonJXXS2, "JXXS");
            //17题型
            string jsonQUESTIONTYPES = Common.HttpGet(urlQUESTIONTYPES); //y({....})
            string jsonQUESTIONTYPES2 = jsonQUESTIONTYPES.Substring(2, jsonQUESTIONTYPES.Length - 3);//{}
            List<String> resultQUESTIONTYPESList = Common.DealJson(jsonQUESTIONTYPES2, "QUESTIONTYPES");
            //18词库
            string jsonWORDDATABASE = Common.HttpGet(urlWORDDATABASE); //y({....})
            string jsonWORDDATABASE2 = jsonWORDDATABASE.Substring(2, jsonWORDDATABASE.Length - 3);//{}
            List<String> resultWORDDATABASEList = Common.DealJson(jsonWORDDATABASE2, "WORDDATABASE");
            //19版权信息
            string jsonCRINFO = Common.HttpGet(urlCRINFO); //y({....})
            string jsonCRINFO2 = jsonCRINFO.Substring(2, jsonCRINFO.Length - 3);//{}
            List<String> resultCRINFOList = Common.DealJson(jsonCRINFO2, "CRINFO");
            //20课时
            List<String> resultKSList = new List<string>() { "第一课时", "第二课时", "第三课时" };
            //21课例类型
            string jsonKLLX = Common.HttpGet(urlKLLX); //y({....})
            string jsonKLLX2 = jsonKLLX.Substring(2, jsonKLLX.Length - 3);//{}
            List<String> resultKLLXList = Common.DealJson(jsonKLLX2, "KLLX");
            //------- 新增“资源大类”20170213
            List<String> resultZYDLList = new List<string>() { "同步资源", "拓展资源" };
            //22知识点 22,23,24
            string jsonKNOWLEDGE = Common.HttpGet(urlKNOWLEDGE); //y({....})
            string jsonKNOWLEDGE2 = jsonKNOWLEDGE.Substring(2, jsonKNOWLEDGE.Length - 3);//{}
            List<List<String>> resultKNOWLEDGEList = Common.DealJsonOfKNOWLEDGE(jsonKNOWLEDGE2, "KNOWLEDGE");

            metadataLists.Add(resultSUBList);//1
            metadataLists.Add(resultGRADEList);
            metadataLists.Add(resultSSTAGEList);
            metadataLists.Add(resultBREELList);
            metadataLists.Add(resultJXHJList);
            metadataLists.Add(resultSYDXList);
            metadataLists.Add(resultNEWJXMKList);
            metadataLists.Add(resultJXMKList);
            metadataLists.Add(resultSXJXMKList);
            metadataLists.Add(resultYWJXMKList);//10
            metadataLists.Add(resultRIGHTSList);
            metadataLists.Add(resultPJList);
            metadataLists.Add(resultRESOURCEORIGINList);
            metadataLists.Add(resultRESOURCERECOMMENDList);
            metadataLists.Add(resultRSORTList);
            metadataLists.Add(resultEDList);
            metadataLists.Add(resultJXXSList);
            metadataLists.Add(resultQUESTIONTYPESList);
            metadataLists.Add(resultWORDDATABASEList);
            metadataLists.Add(resultCRINFOList);
            metadataLists.Add(resultKSList);
            metadataLists.Add(resultKLLXList);
            //-------------新增“资源大类” 20170213
            metadataLists.Add(resultZYDLList);

            metadataLists.Add(resultKNOWLEDGEList);
        }

        /// <summary>
        /// 根据版本生成册别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            String edition = comboBox1.Text;
            if (edition == "请选择版本")
            {
                MessageBox.Show("请选择版本！");
            }
            else
            {
                String editionNum = edition.Substring(edition.IndexOf("_") + 1);
                int edNum = int.Parse(editionNum);
                string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
                string tail = System.Configuration.ConfigurationManager.AppSettings["bookName"];
                //需要查询的字段
                string urlBooks = head + tail;//url

                string jsonStr = Common.HttpGet(urlBooks); //{....}
                Dictionary<String, object> jsonDic = Common.JsonToDictionary(jsonStr);
                ArrayList BooksList = new ArrayList();
                List<String> specificBooksList = new List<string>();
                String success = "";
                foreach (KeyValuePair<String, object> item in jsonDic)
                {
                    if (item.Key.ToString() == "Success")
                        success = item.Value.ToString();
                    if (item.Key.ToString() == "Data")
                        BooksList = (ArrayList)item.Value;
                }
                if (success == "True" && BooksList.Count > 0)
                {
                    specificBooksList = Common.GetBooksOfSpecificEdition(urlBooks, edNum, BooksList);
                }
                if (specificBooksList.Count > 0)
                {
                    comboBox2.Items.AddRange(specificBooksList.ToArray());
                    comboBox2.Text = "请选择册别";
                }
            }
        }

        /// <summary>
        /// 根据册别生成教材目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            String cebie = comboBox2.Text;
            if (cebie == "请选择册别")
            {
                MessageBox.Show("请选择册别！");
            }
            else
            {
                bookName = cebie;
                String cebieNum = cebie.Substring(cebie.LastIndexOf("_") + 1);
                String[] cebieArray = cebieNum.Split('-');
                string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
                string tail = System.Configuration.ConfigurationManager.AppSettings["bookContents"];
                //需要查询的字段
                string urlBookContents = String.Format(head + tail, cebieArray[0], cebieArray[1], cebieArray[2], cebieArray[3], cebieArray[4]);//url
                string jsonStr = Common.HttpGet(urlBookContents); //y({....})
                string jsonStr2 = jsonStr.Substring(2, jsonStr.Length - 3);//{}

                List<String> contentsList = Common.GetContentsOfSpecificBook(jsonStr2);
                catologList = contentsList;
                MessageBox.Show("教材目录生成完毕！");
                button5.Enabled = true;
            }
        }

        /// <summary>
        /// 生成备课资源表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            IWorkbook wb = new HSSFWorkbook();

            #region 创建基础数据表
            ISheet sh = wb.CreateSheet("基础数据");
            List<string> headtitles = new List<string>();
            headtitles.Add("学科"); headtitles.Add("年级");
            headtitles.Add("学段"); headtitles.Add("册别");
            headtitles.Add("教学环节"); headtitles.Add("适用对象");
            headtitles.Add("新教学模块"); headtitles.Add("英语教学模块");
            headtitles.Add("数学教学模块"); headtitles.Add("语文教学模块");
            headtitles.Add("资源权限"); headtitles.Add("资源评级");
            headtitles.Add("资源来源"); headtitles.Add("资源推荐");
            headtitles.Add("资源类型"); headtitles.Add("版本");
            headtitles.Add("教学形式"); headtitles.Add("题型");
            headtitles.Add("词库"); headtitles.Add("版权信息");
            headtitles.Add("课时"); headtitles.Add("课例类型");//
            //-----新增资源大类-------
            headtitles.Add("资源大类");

            headtitles.Add("语文_001"); headtitles.Add("数学_002"); headtitles.Add("英语_003");



            for (int i = 0; i < 500; i++)
            {
                sh.CreateRow(i);
            }

            for (int i = 0; i < metadataLists.Count - 1; i++)//前0—21列表
            {
                List<String> subList = (List<String>)metadataLists[i];
                sh.GetRow(0).CreateCell(i).SetCellValue(headtitles[i]);
                for (int j = 1; j <= subList.Count; j++)
                {
                    sh.GetRow(j).CreateCell(i).SetCellValue(subList[j - 1]);
                }
            }

            sh.GetRow(0).GetCell(19).SetCellValue("");
            sh.GetRow(1).GetCell(19).SetCellValue("");
            sh.GetRow(2).GetCell(19).SetCellValue("");
            sh.GetRow(3).GetCell(19).SetCellValue("");
            sh.GetRow(4).GetCell(19).SetCellValue("");

            //List<List<String>> knowledgeLists = (List<List<String>>)metadataLists[22];
            List<List<String>> knowledgeLists = (List<List<String>>)metadataLists[23];
            for (int i = 0; i < knowledgeLists.Count; i++)
            {
                List<String> subknowledgeList = (List<String>)knowledgeLists[i];
                //sh.GetRow(0).CreateCell(22 + i).SetCellValue(headtitles[22 + i]);
                sh.GetRow(0).CreateCell(23 + i).SetCellValue(headtitles[23 + i]);
                for (int j = 1; j <= subknowledgeList.Count; j++)
                {
                    //sh.GetRow(j).CreateCell(22 + i).SetCellValue(subknowledgeList[j - 1]);
                    sh.GetRow(j).CreateCell(23 + i).SetCellValue(subknowledgeList[j - 1]);
                }
            }
            #endregion

            #region 创建教材目录表
            ISheet sh1 = wb.CreateSheet("教材目录");
            for (int i = 0; i < 500; i++)
            {
                sh1.CreateRow(i);
            }
            for (int i = 0; i < catologList.Count; i++)
            {
                sh1.GetRow(i).CreateCell(0).SetCellValue(catologList[i]);
            }

            //创建通用设置表（适用对象，资源评级，资源评级，资源来源，资源推荐，教学形式）
            ISheet sh2 = wb.CreateSheet("通用设置");
            IRow r0 = sh2.CreateRow(0);//标题
            r0.CreateCell(0).SetCellValue("适用对象"); r0.CreateCell(1).SetCellValue("资源评级");
            r0.CreateCell(2).SetCellValue("资源权限"); r0.CreateCell(3).SetCellValue("资源来源");
            r0.CreateCell(4).SetCellValue("资源推荐"); r0.CreateCell(5).SetCellValue("教学形式");
            IRow r1 = sh2.CreateRow(1);//内容
            r1.CreateCell(0).SetCellValue(((List<String>)metadataLists[5])[0]);//适用对象固定内容
            r1.CreateCell(1).SetCellValue(((List<String>)metadataLists[11])[0]);//资源评级固定内容
            r1.CreateCell(2).SetCellValue(((List<String>)metadataLists[10])[0]);//资源评级固定内容
            r1.CreateCell(3).SetCellValue(((List<String>)metadataLists[12])[1]);//资源来源固定内容
            r1.CreateCell(4).SetCellValue(((List<String>)metadataLists[13])[0]);//资源推荐固定内容
            r1.CreateCell(5).SetCellValue(((List<String>)metadataLists[16])[1]);//教学形式固定内容

            CellRangeAddressList region00 = new CellRangeAddressList(1, 1, 0, 0);
            CellRangeAddressList region11 = new CellRangeAddressList(1, 1, 1, 1);
            CellRangeAddressList region22 = new CellRangeAddressList(1, 1, 2, 2);
            CellRangeAddressList region33 = new CellRangeAddressList(1, 1, 3, 3);
            CellRangeAddressList region44 = new CellRangeAddressList(1, 1, 4, 4);
            CellRangeAddressList region55 = new CellRangeAddressList(1, 1, 5, 5);
            DVConstraint constraint00 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[5]).ToArray());
            DVConstraint constraint11 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[11]).ToArray());
            DVConstraint constraint22 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[10]).ToArray());
            DVConstraint constraint33 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[12]).ToArray());
            DVConstraint constraint44 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[13]).ToArray());
            DVConstraint constraint55 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[16]).ToArray());
            HSSFDataValidation dataValidate00 = new HSSFDataValidation(region00, constraint00);
            HSSFDataValidation dataValidate11 = new HSSFDataValidation(region11, constraint11);
            HSSFDataValidation dataValidate22 = new HSSFDataValidation(region22, constraint22);
            HSSFDataValidation dataValidate33 = new HSSFDataValidation(region33, constraint33);
            HSSFDataValidation dataValidate44 = new HSSFDataValidation(region44, constraint44);
            HSSFDataValidation dataValidate55 = new HSSFDataValidation(region55, constraint55);
            sh2.AddValidationData(dataValidate00);
            sh2.AddValidationData(dataValidate11);
            sh2.AddValidationData(dataValidate22);
            sh2.AddValidationData(dataValidate33);
            sh2.AddValidationData(dataValidate44);
            sh2.AddValidationData(dataValidate55);
            #endregion

            #region 创建U1表
            ISheet sheet = wb.CreateSheet("U1");
            IRow row0 = sheet.CreateRow(0);
            row0.CreateCell(0).SetCellValue("序号"); row0.CreateCell(1).SetCellValue("显示名称");
            row0.CreateCell(2).SetCellValue("内容说明");
            row0.CreateCell(3).SetCellValue("资源类型");//二级关联 14
            row0.CreateCell(4).SetCellValue("教材名称");//bookName             
            sheet.CreateRow(1).CreateCell(4).SetCellValue(bookName.Substring(0, bookName.LastIndexOf("_")));//固化教材名称，一行
            row0.CreateCell(5).SetCellValue("教材目录");//cataloglist
            row0.CreateCell(6).SetCellValue("教学环节");//4 

            row0.CreateCell(7).SetCellValue("教学模块");//语数英 
            row0.CreateCell(8).SetCellValue("知识点");//语数英

            row0.CreateCell(9).SetCellValue("关键字"); row0.CreateCell(10).SetCellValue("资源描述");
            row0.CreateCell(11).SetCellValue("制作者"); row0.CreateCell(12).SetCellValue("检测者");
            row0.CreateCell(13).SetCellValue("上传者"); row0.CreateCell(14).SetCellValue("文件路径");
            row0.CreateCell(15).SetCellValue("缩略图路径");
            row0.CreateCell(16).SetCellValue("版权信息");//19
            //-----------新增“资源大类”20170213
            row0.CreateCell(17).SetCellValue("资源大类");
            CellRangeAddressList region3 = new CellRangeAddressList(0, 65535, 3, 3);
            CellRangeAddressList region5 = new CellRangeAddressList(0, 65535, 5, 5);
            CellRangeAddressList region6 = new CellRangeAddressList(0, 65535, 6, 6);

            CellRangeAddressList region7 = new CellRangeAddressList(0, 65535, 7, 7);
            CellRangeAddressList region8 = new CellRangeAddressList(0, 65535, 8, 8);

            CellRangeAddressList region16 = new CellRangeAddressList(0, 65535, 16, 16);
            //-----------新增“资源大类”20170213
            CellRangeAddressList region17 = new CellRangeAddressList(0, 65535, 17, 17);
            IName range3 = wb.CreateName();
            int num3 = ((List<string>)metadataLists[14]).Count + 1;
            range3.RefersToFormula = "基础数据!$O$2:$O$" + num3.ToString();
            range3.NameName = "dicRange3";
            DVConstraint constraint3 = DVConstraint.CreateFormulaListConstraint("dicRange3");

            IName range5 = wb.CreateName();
            if (catologList.Count == 0)
            {
                MessageBox.Show("请导入教材目录！");
                return;
            }
            else
            {
                int num5 = catologList.Count;
                range5.RefersToFormula = "教材目录!$A$1:$A$" + num5;
                range5.NameName = "dicRange5";
                DVConstraint constraint5 = DVConstraint.CreateFormulaListConstraint("dicRange5");
                DVConstraint constraint6 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[4]).ToArray());
                DVConstraint constraint7;
                DVConstraint constraint8;
                String[] test = bookName.Substring(bookName.LastIndexOf("_")).Split('-');
                if (test[1] == "1")
                {
                    IName range8 = wb.CreateName();
                    int num8 = ((string[])((List<List<String>>)metadataLists[23])[0].ToArray()).Length + 1;
                    range8.RefersToFormula = "基础数据!$X$2:$X$" + num8.ToString();
                    range8.NameName = "dicRange8";
                    constraint7 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[9]).ToArray());
                    constraint8 = DVConstraint.CreateFormulaListConstraint("dicRange8");
                }
                else if (test[1] == "2")
                {
                    IName range8 = wb.CreateName();
                    int num8 = ((string[])((List<List<String>>)metadataLists[23])[1].ToArray()).Length + 1;
                    range8.RefersToFormula = "基础数据!$Y$2:$Y$" + num8.ToString();
                    range8.NameName = "dicRange8";
                    constraint7 = DVConstraint.CreateExplicitListConstraint(((List<string>)metadataLists[8]).ToArray());
                    constraint8 = DVConstraint.CreateFormulaListConstraint("dicRange8");
                }
                else
                {
                    IName range8 = wb.CreateName();
                    //int num8 = ((string[])((List<List<String>>)metadataLists[22])[2].ToArray()).Length + 1;
                    int num8 = ((string[])((List<List<String>>)metadataLists[23])[2].ToArray()).Length + 1;
                    range8.RefersToFormula = "基础数据!$Z$2:$Z$" + num8.ToString();
                    range8.NameName = "dicRange8";
                    constraint7 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[7]).ToArray());
                    constraint8 = DVConstraint.CreateFormulaListConstraint("dicRange8");
                }
                //DVConstraint constraint16 = DVConstraint.CreateExplicitListConstraint((string[])((List<string>)metadataLists[19]).ToArray());
                DVConstraint constraint16 = DVConstraint.CreateExplicitListConstraint(new string[] { "A", "B", "C", "D" });
                DVConstraint constraint17 = DVConstraint.CreateExplicitListConstraint(new string[] { "同步资源", "拓展资源" });
                HSSFDataValidation dataValidate3 = new HSSFDataValidation(region3, constraint3);
                HSSFDataValidation dataValidate5 = new HSSFDataValidation(region5, constraint5);
                HSSFDataValidation dataValidate6 = new HSSFDataValidation(region6, constraint6);
                HSSFDataValidation dataValidate7 = new HSSFDataValidation(region7, constraint7);
                HSSFDataValidation dataValidate8 = new HSSFDataValidation(region8, constraint8);
                HSSFDataValidation dataValidate16 = new HSSFDataValidation(region16, constraint16);
                //----------新增“资源大类”20170213
                HSSFDataValidation dataValidate17 = new HSSFDataValidation(region17, constraint17);
                sheet.AddValidationData(dataValidate3);
                sheet.AddValidationData(dataValidate5);
                sheet.AddValidationData(dataValidate6);
                sheet.AddValidationData(dataValidate7);
                sheet.AddValidationData(dataValidate8);
                sheet.AddValidationData(dataValidate16);
                sheet.AddValidationData(dataValidate17);
                #endregion

                //表格样式
                #region
                sh2.SetColumnWidth(0, 10 * 256);
                sh2.SetColumnWidth(1, 10 * 256);
                sh2.SetColumnWidth(2, 20 * 256);
                sh2.SetColumnWidth(3, 20 * 256);
                sh2.SetColumnWidth(4, 10 * 256);
                sh2.SetColumnWidth(5, 15 * 256);

                sheet.GetRow(0).Height = 30 * 20;
                sheet.SetColumnWidth(0, 10 * 256);
                sheet.SetColumnWidth(1, 20 * 256);
                sheet.SetColumnWidth(2, 40 * 256);
                sheet.SetColumnWidth(3, 20 * 256);
                sheet.SetColumnWidth(4, 40 * 256);
                sheet.SetColumnWidth(5, 40 * 256);
                sheet.SetColumnWidth(6, 10 * 256);
                sheet.SetColumnWidth(7, 15 * 256);
                sheet.SetColumnWidth(8, 40 * 256);
                sheet.SetColumnWidth(9, 40 * 256);
                sheet.SetColumnWidth(10, 20 * 256);
                sheet.SetColumnWidth(11, 20 * 256);
                sheet.SetColumnWidth(12, 10 * 256);
                sheet.SetColumnWidth(13, 10 * 256);
                sheet.SetColumnWidth(14, 40 * 256);
                sheet.SetColumnWidth(15, 40 * 256);
                sheet.SetColumnWidth(16, 10 * 256);
                sheet.SetColumnWidth(17, 10 * 256);
                #endregion

                String fileName = bookName.Substring(0, bookName.IndexOf("_")) + "_资源集模板.xls";
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    wb.Write(fs);
                    fs.Close();
                    MessageBox.Show("创建成功！");
                }
            }
        }

        private void TemplateForm1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
