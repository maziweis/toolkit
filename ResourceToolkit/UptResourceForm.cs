
using ResourceToolkitr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using unvell.ReoGrid;
using unvell.ReoGrid.CellTypes;
using unvell.ReoGrid.Events;

namespace ResourceToolkit
{
    public partial class UptResourceForm : Form
    {
        public Worksheet worksheet;
        public UptResourceForm()
        {
            InitializeComponent();
            winLoad();
            SelectData();
            worksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);
        }
        #region
        public void Open(string file)
        {
            this.Open(file, null);
        }

        public void Open(string file, Action<Worksheet> postHandler)
        {
            grid.Load(file);
            this.PostHandler = postHandler;
        }

        public void Init(Action<Worksheet> postHandler)
        {
            this.PostHandler = postHandler;
        }

        public Action<Worksheet> PostHandler { get; set; }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            if (this.PostHandler != null) PostHandler(grid.CurrentWorksheet);
        }
        #endregion
        #region
        public static string sqlstr;
        public static string fileUrl;
        public static string ThumbnailUrl;
        List<string> testlist = new List<string>();
        Dictionary<string, string> Testdic = new Dictionary<string, string>();

        TempModel.Resource m = new TempModel.Resource();

        List<String> resultRSORTList = new List<string>(); //D
        public static List<String> catologList = new List<string>();
        List<String> resultZYDLList = new List<string>(); //D
        Dictionary<int, string> banquanMirror = new Dictionary<int, string>();//应需求 将版本信息用A、B、C、D代替
        string[] banquanxinxi = new string[4] { "方直自主版权_001", "方直争议版权_002", "用户上传非原创_003", "用户上传原创_004" };//版权信息
        Dictionary<string, int> Mirror = new Dictionary<string, int>();
        #endregion
        string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
        string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];


        /// <summary>
        /// 加载资源集数据
        /// </summary>
        public void SelectData()
        {
            worksheet = grid.CurrentWorksheet;
            var checkBoxHeader = this.worksheet.ColumnHeaders[0];
            checkBoxHeader.Text = string.Empty;
            checkBoxHeader.DefaultCellBody = typeof(CheckBoxCell);
            checkBoxHeader.Width = 50;
            checkBoxHeader.Style.HorizontalAlign = ReoGridHorAlign.Center;
            checkBoxHeader.Style.VerticalAlign = ReoGridVerAlign.Middle;
            checkBoxHeader.Style.Padding = new PaddingValue(3);

            //允许点击cell
            //worksheet.SelectionMode = WorksheetSelectionMode.Cell;
            worksheet.ColumnHeaders[1].Text = "序号";
            worksheet.ColumnHeaders[2].Text = "显示名称";
            worksheet.ColumnHeaders[3].Text = "内容说明";
            worksheet.ColumnHeaders[4].Text = "资源类型";
            worksheet.ColumnHeaders[5].Text = "教材名称";
            worksheet.ColumnHeaders[6].Text = "教材目录";
            worksheet.ColumnHeaders[7].Text = "关键字";
            worksheet.ColumnHeaders[8].Text = "资源描述";
            worksheet.ColumnHeaders[9].Text = "文件路径";
            worksheet.ColumnHeaders[10].Text = "缩略图路径";
            worksheet.ColumnHeaders[11].Text = "版权信息";
            worksheet.ColumnHeaders[12].Text = "资源大类";
            grid.AllowDrop = false;
        }
        private void GetData(string str1, string str2, string str3)
        {
            this.txbVersion.Text = str1;
            this.txbCe.Text = str2;
            this.txbList.Text = str3;
            contentsList();

        }
        private void winLoad()
        {
            //14D资源类型    
            string urlRSORT = String.Format(head + tail, "RSORT");
            string jsonRSORT = Common.HttpGet(urlRSORT); //y({....})
            string jsonRSORT2 = jsonRSORT.Substring(2, jsonRSORT.Length - 3);//{}
            resultRSORTList = Common.DealJson(jsonRSORT2, "RSORT");
            //资源大类
            string urlZYDL = String.Format(head + tail, "RCLASS");
            string jsonZYDL = Common.HttpGet(urlZYDL); //y({....})
            string jsonZYDL2 = jsonZYDL.Substring(2, jsonZYDL.Length - 3);//{}
            resultZYDLList = Common.DealJson(jsonZYDL2, "RCLASS");

            if (banquanMirror.Count == 0)
            {
                banquanMirror.Add(0, "A");
                banquanMirror.Add(1, "B");
                banquanMirror.Add(2, "C");
                banquanMirror.Add(3, "D");
            }

            if (Mirror.Count == 0)
            {
                Mirror.Add("A", 0);
                Mirror.Add("B", 1);
                Mirror.Add("C", 2);
                Mirror.Add("D", 3);
            }
        }
        string cell = string.Empty;
        int cellnum = 1;
        /// <summary>
        /// worksheet_CellMouseDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worksheet_CellMouseDown(object sender, CellMouseEventArgs e)
        {
            if (row == 0)
            {
                worksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);
            }
            else
            {
                if (e.CellPosition.Row < row)
                {
                    worksheet.SetSettings(WorksheetSettings.Edit_Readonly, false);
                    //contentsList();
                    string bookname = txbCe.Text.Substring(0, txbCe.Text.LastIndexOf("_"));
                    if (cell != string.Empty)
                    {
                        if (cellnum >= 1)
                        {
                            worksheet[cell] = new CellBody();
                        }
                        //worksheet.SelectionMode = WorksheetSelectionMode.Cell;
                    }
                    cellnum = e.CellPosition.Col;
                    cell = e.CellPosition.ToString();
                    #region
                    if (e.CellPosition.Col == 4) //D
                        worksheet[e.CellPosition] = new DropdownListCell(resultRSORTList.ToArray());
                    if (e.CellPosition.Col == 6) //F
                        worksheet[e.CellPosition] = new DropdownListCell(catologList.ToArray());
                    if (e.CellPosition.Col == 11)//K
                        worksheet[e.CellPosition] = new DropdownListCell(banquanMirror.Values.ToArray());
                    if (e.CellPosition.Col == 12)//L
                        worksheet[e.CellPosition] = new DropdownListCell(resultZYDLList.ToArray());
                    #endregion
                }
                else
                {
                    worksheet.SetSettings(WorksheetSettings.Edit_Readonly, true);
                }

            }

        }
        //}

        /// <summary>
        /// 生成教材目录
        /// </summary>
        private void contentsList()
        {
            if (txbCe.Text != null && txbCe.Text != "请选择册别")
            {

                string bookName = txbCe.Text;
                String ceNum = bookName.Substring(bookName.LastIndexOf("_") + 1);
                String[] ceArray = ceNum.Split('-');
                string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
                string tail = System.Configuration.ConfigurationManager.AppSettings["bookContents"];
                string urlBookContents = String.Format(head + tail, ceArray[0], ceArray[1], ceArray[2], ceArray[3], ceArray[4]);
                string jsonStr = Common.HttpGet(urlBookContents);
                string jsonStr2 = jsonStr.Substring(2, jsonStr.Length - 3);
                List<String> contentsList = Common.GetContentsOfSpecificBook(jsonStr2);
                catologList = contentsList;
            }
        }
        private static readonly Random rand = new Random();

        private static readonly WorksheetRangeStyle grayBackgroundStyle = new WorksheetRangeStyle
        {
            Flag = PlainStyleFlag.BackColor,
        };

        private WorksheetRangeStyle GetRandomBackColorStyle()
        {
            grayBackgroundStyle.BackColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            return grayBackgroundStyle;
        }




        bool val = true;
        public string get(string eid, IList<RSort> rsort)
        {
            string resType = "";
            string resStyle = "";
            foreach (RSort item in rsort)
            {
                if (val)
                {
                    resType = item.id.ToString();
                    if (item.id.ToString() == eid)
                    {
                        resStyle = eid;
                        val = false;
                        break;
                    }
                    if (item.Children != null)
                    {
                        get(eid, item.Children);
                    }
                }

            }
            return resType;
        }
        /// <summary>
        /// 根据Excel中的ID 得到type/style
        /// </summary>
        /// <param name="eId"></param>
        public void GetResType(string temp, string eId, out string ResourceType, out string ResourceStyle)
        {
            ResourceType = eId;
            ResourceStyle = string.Empty;
            val = true;
            IList<RSort> list = ResourceToolkitr.JsonHelper.DecodeJson<List<RSort>>(temp);
            if (list != null)
            {
                foreach (RSort RSort in list)
                {
                    if (eId == "10" || eId == "22")
                    {
                        ResourceType = eId;
                        ResourceStyle = "0";
                        break;
                    }

                    else
                    {
                        ResourceStyle = eId;
                        ResourceType = get(eId, list);
                        if (ResourceType != "" || ResourceType != null)
                        {
                            break;
                        }
                    }
                }
            }
        }

        public class RSort
        {
            public int id { get; set; }
            public string title { get; set; }
            public IList<RSort> Children { get; set; }

            public bool isFolder { get; set; }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        int row;
        private void btn_SelectData_Click(object sender, EventArgs e)
        {
            //---------点击查询前，先清理表格上的数据
            grid.Reset();
            SelectData();

            progressBar1.Value = 0;

            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];


            m.Catalog = DealData.RetSubstringBracket(txbList.Text);
            SqlConnection sqlConn = new SqlConnection(sqlstr);
            sqlConn.Open();
            string selectStr = "select * from tb_Resource where Catalog = ('" + m.Catalog + "')";
            SqlCommand sc = new SqlCommand(selectStr, sqlConn);
            SqlDataReader reader = sc.ExecuteReader();

            row = 0;
            int k = 1;
            dataload();


            progressBar1.Value = 5;
            while (reader.Read())
            {
                worksheet[row, 0] = false;
                worksheet[row, 1] = k;//序号
                worksheet[row, 2] = reader["Title"]; //显示名称2
                worksheet[row, 4] = DealData.SelectString(resultRSORTList, Convert.ToInt32(reader["ResourceStyle"]));//资源类型10
                worksheet[row, 5] = txbCe.Text.Split('_')[0] + "_" + txbCe.Text.Split('_')[1]; //教材名称
                worksheet[row, 6] = txbList.Text; //教材目录
                worksheet[row, 7] = reader["KeyWords"];//关键字12
                worksheet[row, 8] = reader["Description"];//描述16
                int copyright = Convert.ToInt32(reader["Copyright"]);
                worksheet[row, 11] = banquanMirror[copyright];
                worksheet[row, 12] = reader["ResourceClass"];
                worksheet[row, 13] = reader["FileID"];
                k++;
                row++;
                if (progressBar1.Value < 95)
                {
                    progressBar1.Value += 5;
                }

            }
            for (int i = 1; i < reader.FieldCount; i++)
            {
                worksheet.SetColumnsWidth(i, reader.FieldCount, 150);
                //worksheet.AutoFitColumnWidth(i, true);
                worksheet.HideColumns(13, 1);
            }
            progressBar1.Value = 100;


        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void dataload()
        {
            if (txbVersion.Text != string.Empty && txbCe.Text != string.Empty)
            {
                worksheet.CellMouseDown += worksheet_CellMouseDown;
            }
        }
        /// <summary>
        /// 读取远程数据库教材目录，解析字段，返回bool值
        /// </summary>
        public static bool check_connect_status;
        public bool NewAnalyzeWords()
        {
            check_connect_status = true;
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["metaConnString"];
                string strSql = "select * from tb_StandardBook ";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = strSql;
                cmd.Connection = conn;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        testlist.Clear();
                        testlist.Add((dr.GetValue(1).ToString()));
                        testlist.Add((dr.GetValue(2).ToString()));
                        testlist.Add((dr.GetValue(3).ToString()));
                        testlist.Add((dr.GetValue(4).ToString()));
                        testlist.Add((dr.GetValue(5).ToString()));
                        if (Testdic.ContainsKey(dr.GetValue(0).ToString()) == false)
                        {
                            Testdic.Add(dr.GetValue(0).ToString(), string.Join(",", testlist.ToArray()));
                        }
                    }
                }
                conn.Close();
            }
            catch (System.Exception ex)
            {
                check_connect_status = false;
                MessageBox.Show("请检测mod数据库配置！");
            }
            return check_connect_status;
        }
        /// <summary>
        /// 更新資源數據
        /// </summary>
        /// <param name="sender"></param>
        /// <param nae="e"></param>

        private void btn_UpdateData_Click(object sender, EventArgs e)
        {
            int k = 0;
            int j = 0;
            if (NewAnalyzeWords())
            {

                for (int i = 0; i < row; i++)
                {
                    if (worksheet[i, 0].ToString() == "True")
                    {
                        //只用更新文件
                        //只用更新数据库数据
                        //既要更新文件也要更新数据
                        //判断用户是更新文件还是更新数据
                        ResourceData(i);
                        k++;
                    }
                    else
                    {
                        j++;
                    }

                }
                if (j == row)
                {
                    MessageBox.Show("还没有勾选任何数据噢！\r\n确认不需要勾选吗？");
                    return;
                }
                MessageBox.Show("符合条件的数据总共有：" + row + "条"
                              + "\r\n 已更新" + k + "条!");
            }
        }
        /// <summary>
        /// 上傳資源
        /// </summary>
        /// <param name="i"></param>
        public int UploadResource(int i, Guid id)
        {
            int result = 0;
            fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"];
            #region 上传文件
            try
            {
                string path1 = System.Windows.Forms.Application.StartupPath;
                path1 = path1 + worksheet[i, 9].ToString();
                string temp = "";
                WebClient web = new WebClient();
                if (File.Exists(path1))
                {
                    byte[] bUp = web.UploadFile(fileUrl + "UpdateFile.ashx?FileID=" + id, path1);
                    temp = System.Text.Encoding.UTF8.GetString(bUp);
                    Dictionary<string, string> res = JsonHelper.DecodeJson<Dictionary<string, string>>(temp);
                    if (res["result"] == "false")
                    {
                        result = 1;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {

            }
            //#endregion
            #region 获取fileID ，上传缩略图
            if (worksheet[i, 10] != null)
            {
                string path2 = System.Windows.Forms.Application.StartupPath;
                path2 = path2 + worksheet[i, 9].ToString();
                if (File.Exists(path2))
                {
                    if (id != Guid.Empty || id != null)
                    {
                        WebClient Cweb = new WebClient();
                        byte[] ThumbByte = Cweb.UploadFile(fileUrl + "SetResImg.ashx?fileID=" + id, path2);
                        string ThumbData = System.Text.Encoding.UTF8.GetString(ThumbByte);
                        Dictionary<string, string> res = JsonHelper.DecodeJson<Dictionary<string, string>>(ThumbData);
                        if (res["result"] == "false")
                        {
                            if (result == 1)
                            {
                                result = 3;
                            }
                            result = 2;
                        }
                    }
                }
            }
            #endregion
            return result;
        }
        /// <summary>
        /// 更新资源集数据库数据
        /// 目前改功能已通过测试
        /// </summary>
        /// <param name="i"></param>
        string testValue = string.Empty;
        string updateStr = "";
        public void ResourceData(int i)
        {
            string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
            string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];
            string urlRSORT = String.Format(head + tail, "RSORT");//14资源类型            
            string jsonRSORT = Common.HttpGet(urlRSORT); //y({....})
            string jsonRSORT2 = jsonRSORT.Substring(2, jsonRSORT.Length - 3);//{}
            string tep = jsonRSORT2.Substring(7, jsonRSORT2.Length - 8);

            m.Title = worksheet[i, 2].ToString();
            m.StandardBook = DealData.RetSubstringUnderline(worksheet[i, 5].ToString());
            #region  版本 册别 学科 年级
            //foreach (KeyValuePair<string, string> kvp in Testdic)
            //{
            //    if (kvp.Key == m.StandardBook.ToString())
            //    {
            //        testValue = kvp.Value;
            //    }
            //}
            //if (testValue != "")
            //{
            //    string[] sArray = testValue.Split(',');
            //    if (sArray == null || sArray.Length == 0)
            //    {
            //        m.SchoolStage = null;
            //        m.Subject = null;
            //        m.Grade = null;
            //        m.BookReel = null;
            //        m.Edition = null;
            //    }
            //    else
            //    {
            //        m.SchoolStage = int.Parse(sArray[0]);
            //        m.Subject = int.Parse(sArray[1]);
            //        m.Grade = int.Parse(sArray[2]);
            //        m.BookReel = int.Parse(sArray[3]);
            //        m.Edition = int.Parse(sArray[4]);

            //    }
            //}
            //else
            //{
            //    m.SchoolStage = null;

            //    m.Subject = null;

            //    m.Grade = null;

            //    m.BookReel = null;

            //    m.Edition = null;
            //}
            #endregion
            m.Catalog = DealData.RetSubstringBracket(worksheet[i, 6].ToString());
            //m.Copyright = Mirror[worksheet[i, 23].ToString()];
            //string CopyrightName = banquanxinxi[m.Copyright].Split('_')[0];

            //资源类型
            int eId = DealData.RetSubstringBracket(worksheet[i, 4].ToString());
            string ResourceType = string.Empty;
            string ResourceStyle = string.Empty;
            GetResType(tep, eId.ToString(), out ResourceType, out ResourceStyle);
            if (ResourceType != string.Empty)
            {
                ResourceType = int.Parse(ResourceType).ToString();
            }
            if (ResourceStyle != string.Empty)
            {
                ResourceStyle = int.Parse(ResourceStyle).ToString();
            }
            m.KeyWords = worksheet[i, 7].ToString().Replace("\'", "\'\'");
            m.Description = worksheet[i, 8].ToString().Replace("\'", "\'\'");

            updateStr = "update dbo.[tb_Resource] set Title = '" + m.Title + "',Catalog ='" + m.Catalog + "', ResourceType ='" + ResourceType
              + "',ResourceStyle = '" + ResourceStyle + "',KeyWords ='" + m.KeyWords + "',Description = '" + m.Description + "' where " + "FileID ='" + worksheet[i, 13] + "'";
            SqlConnection sqlconn = new SqlConnection(sqlstr);
            sqlconn.Open();
            SqlCommand cmd = new SqlCommand(updateStr, sqlconn);
            int uptnum = cmd.ExecuteNonQuery();
            if (false)
            {
                string url = System.Configuration.ConfigurationManager.AppSettings["ResourceImplement"] + "Handler.ashx";
                string str = "'{'Form':'{\'RID\':\'BEC2C674-AADE-283D-A506-35BFE61B1387\',\'SKEY\':\'ResourceImplement\',\'Pack\':\'{\\\'ID\\\':\\\'BEC2C674-AADE-283D-A506-35BFE61B1387\\\',\\\'Function\\\':\\\'PushResource\\\',\\\'Data\\\':\\\'{}\\\'}\',\'Ticket\':\'\'}'}'";
                var response = PostData.RequestData(url, str);
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_edit_Click(object sender, EventArgs e)
        {
            EditResForm ef = new EditResForm();
            ef.DelegateDataEvent += new EditResForm.DelegateData(GetData);
            ef.ShowDialog();
        }
        //更新出错文件功能暂未完全实现。
        List<TempModel.ErrorClass> lsError = new List<TempModel.ErrorClass>();
        private void btn_updatePath_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < row; i++)
            {
                if (worksheet[i, 0].ToString() == "True")
                {
                    if (worksheet[i, 9] == null || worksheet[i, 9].ToString().Trim() == string.Empty)
                    {
                        MessageBox.Show("请填写文件路径！");
                        return;
                    }
                    else
                    {
                        string checkfile = string.Empty;
                        string errorNum = string.Empty;
                        int NameLessNum = 0;
                        int FileLessNum = 0;
                        #region 判断文件名和文件路径是否存在
                        checkfile = worksheet[i, 9].ToString();
                        errorNum = i.ToString();
                        if (worksheet[i, 2].ToString() == "")
                        {
                            NameLessNum++;
                            continue;
                        }
                        else if (checkfile != "")
                        {
                            if (!File.Exists(checkfile.Substring(1)))
                            {
                                FileLessNum++;
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = int.Parse(errorNum);
                                test.NameError = worksheet[i, 2].ToString();
                                test.ErrorMsg = "检测文件路径问题";
                                lsError.Add(test);
                                continue;
                            }
                        }
                        #endregion
                        string id = worksheet[i, 13].ToString();
                        //string fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"] + "DelFileHandler.ashx?FileID=" + id;
                        //HttpWebRequest wr = WebRequest.Create(fileUrl) as HttpWebRequest;
                        //var response = wr.GetResponse();//发送请求

                        Guid fileid = new Guid();
                        fileid = new Guid(id);
                        int result = UploadResource(i, fileid); // 0成功，1文件失败， 2附属文件失败，3都失败
                        if (result == 0)
                        {
                            MessageBox.Show("success");
                        }
                        else
                        {
                            MessageBox.Show("falied");
                        }
                    }
                }
            }
            if (lsError.Count > 0)
            {
                foreach (TempModel.ErrorClass t in lsError)
                {
                    string filename = "资源集模板_出问题的资源_" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".txt";
                    filename = filename.Replace(":", "-");//		filename	"出问题的资源_2016-9-6 11:24:11.txt"	string
                    StreamWriter sw = File.AppendText(filename);
                    sw.WriteLine("序号：" + t.No.ToString() + "；" + "显示名称：" + t.NameError.ToString() + "；" + "错误原因：" + t.ErrorMsg);
                    sw.Flush();
                    sw.Close();
                }
                label3.Text = "检测有误。\n生成本地txt，请查看！";
            }
        }
    }
}
