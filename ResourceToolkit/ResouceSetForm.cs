using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace ResourceToolkit
{
    public partial class ResouceSetForm : Form
    {
        public class Info
        {
            public string value { get; set; }
            public string index { get; set; }

        }
        public static bool checkTableName;
        public static bool check_connect_status;
        public static string strConn;
        Dictionary<string, string> Testdic = new Dictionary<string, string>();
        List<string> testlist = new List<string>();
        KSDataSource.ServiceSoapClient InitService = new KSDataSource.ServiceSoapClient();
        public static string sqlstr;
        public static string fileUrl;
        public static string ThumbnailUrl;
        int clearnumber_count;
        string ExcelPath;
        public delegate void delegateTest();
        public ResouceSetForm()
        {
            InitializeComponent();
            btn_OpenFile.Enabled = false;
            Control.CheckForIllegalCrossThreadCalls = false;
            IList<Info> infoList = new List<Info>();
            Info info3 = new Info() { value = "素 材 库", index = "0" };
            infoList.Add(info3);
            comboBox1.DataSource = infoList;
            comboBox1.ValueMember = "index";
            comboBox1.DisplayMember = "value";
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ModContent();
        }

        /// <summary>
        /// mod数据初始化内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ModContent()
        {
            //btn_content.Enabled = false;
            string ED_data = InitService.GetMetadata("ED");
            string SUB_data = InitService.GetMetadata("SUB");
            string GRADE_data = InitService.GetMetadata("GRADE");
            string BREEL_data = InitService.GetMetadata("BREEL");
            string BX_data= InitService.GetMetadata("BX");
            string RSORT_data = InitService.GetMetadata("RSORT");
            JavaScriptSerializer js = new JavaScriptSerializer();
            var ED_obj = js.Deserialize<dynamic>(ED_data);
            foreach (var item in ED_obj["ED"])
            {
                this.ED_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
            }

            var SUB_obj = js.Deserialize<dynamic>(SUB_data);
            foreach (var item in SUB_obj["SUB"])
            {
                this.SUB_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
            }

            var GRADE_obj = js.Deserialize<dynamic>(GRADE_data);
            foreach (var item in GRADE_obj["GRADE"])
            {
                this.GRADE_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
            }

            var BREEL_obj = js.Deserialize<dynamic>(BREEL_data);
            foreach (var item in BREEL_obj["BREEL"])
            {
                this.BREEL_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
            }

        }

        /// <summary>
        /// 检测是否连接数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_check_con_Click(object sender, EventArgs e)
        {
            string sqlstr = string.Empty;
            string fileUrl = string.Empty;

            System.Configuration.ConfigurationManager.RefreshSection(sqlstr);
            System.Configuration.ConfigurationManager.RefreshSection(fileUrl);
            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];
            //"Data Source=192.168.3.187;Initial Catalog=Wisdom_Resource";
            fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"];
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                {
                    sqlConn.Open();
                    if (sqlConn.State == ConnectionState.Open)
                    {
                        this.Lb_sql.Text = sqlstr.Substring(0,sqlstr.IndexOf(";Initial"));
                        MessageBox.Show("成功连接上数据库");
                        btn_OpenFile.Enabled = true;
                        sqlConn.Close();
                    }
                    else
                    {
                        MessageBox.Show("请检查数据库连接配置或网络！");
                        return;
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请检查数据库连接配置或网络！");

            }
        }

        /// <summary>
        /// 选择并连接excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OpenFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel文件|*.xls|Excel文件|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFileUrl.Text = dialog.FileName;
            }
            strConn = string.Empty;
            checkTableName = false;
            List<string> combobox_list = new List<string>();
            string excel_useless = string.Empty;
            this.checkedListBox1.Items.Clear();
            if (txtFileUrl.Text == "")
            {
            }
            else
            {
                string TestFile = txtFileUrl.Text.ToString().Substring(txtFileUrl.Text.ToString().LastIndexOf("."));
                ExcelPath = txtFileUrl.Text.ToString().Substring(0, txtFileUrl.Text.ToString().LastIndexOf("\\"));
                if (TestFile == ".xls")
                {
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + txtFileUrl.Text + ";" + "Extended Properties=Excel 8.0;";
                }
                else
                {
                    strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", txtFileUrl.Text);
                }

                using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(strConn))
                {
                    connection.Open();
                    DataTable table = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    string[] strTableNames = new string[table.Rows.Count];
                    for (int k = 0; k < table.Rows.Count; k++)
                    {//我是倒序插入到strTableNames中，因为dtSheetName中行是从后往前读sheet页的
                        strTableNames[k] = table.Rows[table.Rows.Count - k - 1]["TABLE_NAME"].ToString();

                        if (strTableNames[k].ToString().Trim().Substring(strTableNames[k].Length - 1, 1) == "$")
                        {
                            excel_useless = strTableNames[k].Substring(0, strTableNames[k].Length - 1);
                            if (excel_useless != "基础数据" && excel_useless != "修订记录表" && excel_useless != "填写说明表" && excel_useless != "教材目录" && excel_useless != "通用设置")
                            {
                                combobox_list.Add(strTableNames[k].ToString().Trim().Substring(0, strTableNames[k].Length - 1).ToString().Trim());
                            }
                        }
                        else if (strTableNames[k].Substring(strTableNames[k].Length - 2, 2) == "$'")
                        {
                            excel_useless = strTableNames[k].Substring(1, strTableNames[k].Length - 3);
                            if (excel_useless != "基础数据" && excel_useless != "修订记录表" && excel_useless != "填写说明表" && excel_useless != "教材目录" && excel_useless != "通用设置")
                            {
                                combobox_list.Add(strTableNames[k].Substring(1, strTableNames[k].Length - 3).ToString().Trim());
                            }
                        }
                        combobox_list.Sort();
                    }
                    this.checkedListBox1.Items.AddRange(combobox_list.ToArray());
                    checkTableName = true;
                }
            }
        }

        /// <summary>
        /// 读取远程数据库教材目录，解析字段，返回bool值
        /// </summary>
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
        /// 多个单元一起操作
        /// </summary>
        public void OpenExcel_Muti(string tableName)
        {
            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];
            fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"];// +"UploadHandler.ashx";
            ThumbnailUrl = System.Configuration.ConfigurationManager.AppSettings["R_ThumbnailfileUrl"]; //+"SetResImg.ashx";
                                                                                                        //14资源类型
            string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
            string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];
            string urlRSORT = String.Format(head + tail, "RSORT");//14资源类型            
            string jsonRSORT = Common.HttpGet(urlRSORT); //y({....})
            string jsonRSORT2 = jsonRSORT.Substring(2, jsonRSORT.Length - 3);//{}
            string tep = jsonRSORT2.Substring(7, jsonRSORT2.Length - 8);

            progressBar1.Value = 0;
            List<TempModel.ErrorClass> lsError = new List<TempModel.ErrorClass>();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();

            string[] banquanxinxi = new string[4] { "方直自主版权_001", "方直争议版权_002", "用户上传非原创_003", "用户上传原创_004" };//版权信息
            Dictionary<string, int> banquanMirror = new Dictionary<string, int>();
            banquanMirror.Add("A", 0);
            banquanMirror.Add("B", 1);
            banquanMirror.Add("C", 2);
            banquanMirror.Add("D", 3);
            Dictionary<string, int> ZYDL = new Dictionary<string, int>();
            ZYDL.Add("同步资源", 1);
            ZYDL.Add("拓展资源", 2);
            string shiyongduixiang = "";//适用对象
            string ziyuanpingji = "";//资源评级
            string ziyuanquanxian = "";//资源权限
            string ziyuanlaiyuan = "";//资源来源
            string ziyuantuijian = "";//资源推荐
            string jiaoxuexingshi = "";//教学形式

            if (checkTableName)
            {
                #region 读取“通用设置”表
                try
                {
                    OleDbDataAdapter myCommand2 = null;
                    //从指定的表明查询数据，可先把所有表名列出来
                    string strExcel2 = "select * from [" + "通用设置$" + "]";
                    myCommand2 = new OleDbDataAdapter(strExcel2, strConn);
                    myCommand2.Fill(ds2, "table");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("通用设置单元异常，请检测！");
                }

                if (ds2.Tables.Count > 0)
                {
                    foreach (DataRow r in ds2.Tables[0].Rows)
                    {
                        shiyongduixiang = r["适用对象"].ToString();
                        ziyuanpingji = r["资源评级"].ToString();
                        ziyuanquanxian = r["资源权限"].ToString();
                        ziyuanlaiyuan = r["资源来源"].ToString();
                        ziyuantuijian = r["资源推荐"].ToString();
                        jiaoxuexingshi = r["教学形式"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("缺少“通用设置”表");
                }
                #endregion

                #region 读取所选sheet表
                try
                {
                    OleDbDataAdapter myCommand = null;
                    //从指定的表明查询数据，可先把所有表名列出来
                    string strExcel = "select * from [" + tableName + "$" + "] where '显示名称' is not null and '文件路径' is not null";
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, "Bicycle");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("单元异常，请检测！");
                }
                #endregion
            }
            else
            {
                MessageBox.Show("请检查文件命名是否正确！");
                return;
            }

            if (ds.Tables.Count > 0)
            {
                int pb = 1;
                int NameLessNum = 0;
                int FileLessNum = 0;
                int alreadyNum = 0;
                string checkfile = string.Empty;
                string errorNum = string.Empty;
                bool checkOut = false;
                progressBar1.Maximum = ds.Tables[0].Rows.Count;

                #region 循环处理DataSet
                foreach (DataRow r in ds.Tables[0].Rows)
                {

                    progressBar1.Value = pb++;

                    #region 检测Excel字段是否匹配数据库
                    //try
                    //{
                    //    TempModel.Resource ExcelTest = new TempModel.Resource();
                    //    ExcelTest.Title = r["显示名称"].ToString().Replace("\'", "\'\'");
                    //    ExcelTest.StandardBook =  DealData.RetSubstringUnderline(r["教材名称"].ToString());
                    //    ExcelTest.Catalog =  DealData.RetSubstringBracket(r["教材目录"].ToString());
                    //    ExcelTest.TeachingStep =  DealData.RetSubstringUnderline(r["教学环节"].ToString());
                    //    ExcelTest.TeachingModules =  DealData.RetSubstringUnderline(r["教学模块"].ToString());
                    //    ExcelTest.BreviaryImgUrl = r["缩略图路径"].ToString();
                    //    //ExcelTest.Copyright =  DealData.RetSubstringUnderline(r["版权信息"].ToString());
                    //    ExcelTest.Copyright =  DealData.RetSubstringUnderline(banquanxinxi[banquanMirror[r["版权信息"].ToString()]]);
                    //}
                    //catch (System.Exception ex)
                    //{
                    //    MessageBox.Show("无法读取，请检查模板内容是否匹配！" + ex.ToString());
                    //    checkOut = true;
                    //    break;
                    //}
                    #endregion
                    #region 判断文件名和文件路径是否存在

                    checkfile = r["文件路径"].ToString();
                    errorNum = r["序号"].ToString();
                    if (r["显示名称"].ToString() == "")
                    {
                        NameLessNum++;
                        continue;
                    }
                    else if (checkfile != "")
                    {
                        if (!File.Exists(ExcelPath+"\\"+checkfile.Substring(1)))
                        {
                            FileLessNum++;
                            TempModel.ErrorClass test = new TempModel.ErrorClass();
                            test.No = int.Parse(errorNum);
                            test.NameError = r["显示名称"].ToString();
                            test.ErrorMsg = "检测文件路径问题";
                            test.Sheet = tableName;
                            lsError.Add(test);
                            continue;
                        }
                    }

                    #endregion
                    TempModel.Resource m = new TempModel.Resource();
                    string testValue = string.Empty;

                    #region 取excel值
                    m.Sort = Convert.ToInt32(r["序号"]);
                    m.ID = Guid.NewGuid();
                    m.Title = r["显示名称"].ToString().Replace("\'", "\'\'");

                    #region 解析‘教材名称’字段，扩展为‘学科，版本，学段，年级，册别’
                    m.StandardBook = DealData.RetSubstringUnderline(r["教材名称"].ToString());
                    foreach (KeyValuePair<string, string> kvp in Testdic)
                    {
                        if (kvp.Key == m.StandardBook.ToString())
                        {
                            testValue = kvp.Value;
                        }
                    }
                    if (testValue != "")
                    {
                        string[] sArray = testValue.Split(',');
                        if (sArray == null || sArray.Length == 0)
                        {
                            m.SchoolStage = null;
                            m.Subject = null;
                            m.Grade = null;
                            m.BookReel = null;
                            m.Edition = null;
                        }
                        else
                        {
                            m.SchoolStage = int.Parse(sArray[0]);
                            m.Subject = int.Parse(sArray[1]);
                            m.Grade = int.Parse(sArray[2]);
                            m.BookReel = int.Parse(sArray[3]);
                            m.Edition = int.Parse(sArray[4]);
                        }
                    }
                    else
                    {
                        m.SchoolStage = null;
                        m.Subject = null;
                        m.Grade = null;
                        m.BookReel = null;
                        m.Edition = null;
                    }
                    #endregion

                    m.Catalog = DealData.RetSubstringBracket(r["教材目录"].ToString());
                    m.TeachingStep = DealData.RetSubstringUnderline(r["教学环节"].ToString());
                    m.TeachingModules = DealData.RetSubstringUnderline(r["教学模块"].ToString());
                    m.Copyright = DealData.RetSubstringUnderline(banquanxinxi[banquanMirror[r["版权信息"].ToString()]]);
                    m.CopyrightName = banquanxinxi[banquanMirror[r["版权信息"].ToString()]].Split('_')[0];
                    m.ResourceClass = ZYDL[r["资源大类"].ToString()];
                    //资源类型
                    //m.ResourceStyle = DealData.RetSubstringBracket(r["资源类型"].ToString());
                    int eId = DealData.RetSubstringBracket(r["资源类型"].ToString());
                    string ResourceType = string.Empty;
                    string ResourceStyle = string.Empty;
                    GetResType(tep, eId.ToString(), out ResourceType, out ResourceStyle);
                    if (ResourceType != string.Empty)
                    {
                        m.ResourceType = int.Parse(ResourceType);
                    }
                    if (ResourceStyle != string.Empty)
                    {
                        m.ResourceStyle = int.Parse(ResourceStyle);
                    }

                    m.ResourceLevel = DealData.RetSubstringUnderline(ziyuanpingji);
                    m.Purview = DealData.RetSubstringUnderline(ziyuanquanxian);

                    if (comboBox1.SelectedIndex == 1)
                    {
                        m.ComeFrom = ziyuanlaiyuan.Substring(0, ziyuanlaiyuan.IndexOf('_'));
                        m.IsRecommend = DealData.RetSubstringUnderline(ziyuantuijian);
                    }

                    m.TeachingStyle = DealData.RetSubstringUnderline(jiaoxuexingshi);
                    m.KeyWords = r["关键字"].ToString().Replace("\'", "\'\'");
                    m.Description = r["资源描述"].ToString().Replace("\'", "\'\'");
                    m.UploadUser = r["上传者"].ToString();

                    //适用对象
                    List<int> lsApplicable = new List<int>();
                    lsApplicable = DealData.RetApplicable(shiyongduixiang);

                    //教学模块多选
                    List<int> TeachingModules = new List<int>();
                    TeachingModules = DealData.RetApplicable(r["教学模块"].ToString());

                    //知识点
                    List<int> lsKnowledge = new List<int>();
                    lsKnowledge = DealData.RetKnowledge(r["知识点"].ToString());
                    #endregion

                    #region 上传文件
                    try
                    {
                        //string path1 = System.Windows.Forms.Application.StartupPath; 
                        string path1 = ExcelPath+ r["文件路径"].ToString();
                        string temp = "";
                        newWebClient web = new newWebClient();
                        if (File.Exists(path1))
                        {

                            byte[] bUp = web.UploadFile(fileUrl + "UploadHandler.ashx", path1);
                            temp = System.Text.Encoding.UTF8.GetString(bUp);
                        }



                        //////////////////////////////////////////////////////////////////////////
                        m = Get_Data_From_Server(m, temp);
                        if (m.IsSuccess)
                        {
                            #region 上传文件 commit
                            System.Collections.Specialized.NameValueCollection myCol = new System.Collections.Specialized.NameValueCollection();
                            myCol.Add("test", "['']");

                            byte[] b1 = web.UploadValues(fileUrl + "ConfirmHandler.ashx?t=%5B%22" + m.FileID + "%22%5D", myCol);
                            string s = System.Text.Encoding.UTF8.GetString(b1);
                            #endregion
                        }
                        else
                        {
                            FileLessNum++;
                            TempModel.ErrorClass t = new TempModel.ErrorClass();
                            if (r["序号"].ToString() != "")
                            {
                                t.No = int.Parse(r["序号"].ToString());
                            }
                            t.Sheet = tableName;
                            t.NameError = m.Title.ToString();
                            t.ErrorMsg = "文件上传失败！";
                            lsError.Add(t);

                        }
                        #region 获取fileID ，上传缩略图
                        if (r["缩略图路径"].ToString() != "")
                        {
                            if (m.FileID != Guid.Empty || m.FileID != null)
                            {

                                //string path2 = System.Windows.Forms.Application.StartupPath;
                                string path2 = ExcelPath + r["缩略图路径"].ToString();
                                WebClient Cweb = new WebClient();
                                // byte[] ThumbByte = Cweb.UploadFile(ThumbnailUrl + "SetResImg.ashx?fileID=" + m.FileID, path2);
                                byte[] ThumbByte = Cweb.UploadFile(fileUrl + "SetResImg.ashx?fileID=" + m.FileID, path2);
                                string ThumbData = System.Text.Encoding.UTF8.GetString(ThumbByte);

                            }
                            else
                            {
                                FileLessNum++;
                                TempModel.ErrorClass error = new TempModel.ErrorClass();
                                if (r["序号"].ToString() != "")
                                {
                                    error.No = int.Parse(r["序号"].ToString());
                                }
                                error.Sheet = tableName;
                                error.NameError = r["显示名称"].ToString().Replace("\'", "\'\'");
                                error.ErrorMsg = "缩略图问题".ToString();
                                lsError.Add(error);
                            }
                        }


                        #endregion


                        //////////////////////////////////////////////////////////////////////////

                    }
                    catch (System.Exception ex)
                    {
                        FileLessNum++;
                        TempModel.ErrorClass error = new TempModel.ErrorClass();
                        if (r["序号"].ToString() != "")
                        {
                            error.No = int.Parse(r["序号"].ToString());
                        }
                        error.NameError = r["显示名称"].ToString().Replace("\'", "\'\'");
                        error.ErrorMsg = ex.ToString();
                        error.Sheet = tableName;
                        lsError.Add(error);
                        continue;
                    }
                    #endregion

                    List<int> lsCreateID = new List<int>();
                    List<int> lsDetectorID = new List<int>();
                    if (comboBox1.SelectedIndex == 0)
                    {
                        #region 读数据库制作者 审核者 ID=lsCreateID,lsDetectorID
                        using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                        {
                            #region 读取数据
                            //制作者
                            string sName = "";
                            if (r["制作者"].ToString() != "")
                            {
                                List<string> lsName = r["制作者"].ToString().Replace('，', ',').Split(',').ToList();
                                foreach (string name in lsName)
                                {
                                    if (name != string.Empty)
                                    {
                                        sName += "'" + name + "'";
                                        if (name != lsName[lsName.Count - 1])
                                            sName = sName + ",";
                                    }
                                }
                            }
                            else
                            {
                                sName = "''";
                            }

                            //检测者
                            string DetectorName = "";
                            if (r["检测者"].ToString() != "")
                            {
                                List<string> lsDetectorName = r["检测者"].ToString().Replace('，', ',').Split(',').ToList();
                                foreach (string name in lsDetectorName)
                                {
                                    if (name != string.Empty)
                                    {
                                        DetectorName += "'" + name + "'";
                                        if (name != lsDetectorName[lsDetectorName.Count - 1])
                                            DetectorName = DetectorName + ",";
                                    }
                                }
                            }
                            else
                            {
                                DetectorName = "''";
                            }

                            sqlConn.Open();
                            SqlCommand com = sqlConn.CreateCommand();

                            com.CommandText = "select UserID from tb_InnerUser where UserState=1 and UserName in (" + sName + ")";
                            SqlDataAdapter daCreate = new SqlDataAdapter(com);
                            DataSet dtCreate = new DataSet();
                            daCreate.Fill(dtCreate);


                            com.CommandText = "select UserID from tb_InnerUser where UserState=1 and UserName in (" + DetectorName + ")";
                            SqlDataAdapter daDetector = new SqlDataAdapter(com);
                            DataSet dtDetector = new DataSet();
                            daDetector.Fill(dtDetector);

                            sqlConn.Close();
                            #endregion

                            if (dtCreate != null && dtCreate.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dtCreate.Tables[0].Rows)
                                {
                                    lsCreateID.Add(int.Parse(dr["UserID"].ToString()));
                                }
                            }
                            if (dtDetector != null && dtDetector.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dtDetector.Tables[0].Rows)
                                {
                                    lsDetectorID.Add(int.Parse(dr["UserID"].ToString()));
                                }
                            }
                        }
                        #endregion
                    }

                    #region 写数据库，如果有重复数据，不写入。
                    if (m.FileID == null)
                    {
                    }
                    else
                    {
                        try
                        {
                            using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                            {
                                //资源表
                                sqlConn.Open();
                                SqlCommand mycmd = new SqlCommand("select ID from tb_Resource where Title = ('" + m.Title + "' )"
                                + "and SchoolStage = ('" + m.SchoolStage + "' ) and Grade = ('" + m.Grade + "' ) and Subject = ('" + m.Subject + "' )"
                                + "and Edition = ('" + m.Edition + "' ) and BookReel = ('" + m.BookReel + "' ) and Catalog = ('" + m.Catalog + "' )and ResourceType = ('" + m.ResourceType + "') and ResourceStyle = ('" + m.ResourceStyle + "' )"
                                + "and ResourceLevel = ('" + m.ResourceLevel + "' ) and KeyWords = ('" + m.KeyWords + "' ) and TeachingStep = ('" + m.TeachingStep + "' ) and TeachingStyle = ('" + m.TeachingStyle + "' ) and Description = ('" + m.Description + "' )"
                                + "and Purview = ('" + m.Purview + "' ) and UploadUser = ('" + m.UploadUser + "' ) and FileType = ('" + m.FileType + "' ) and ResourceSize = ('" + m.ResourceSize + "' )and Copyright = ('" + m.Copyright + "' )and CopyrightName = ('" + m.CopyrightName + "' ) and Sort = ('" + m.Sort + "') and ResourceClass = ('" + m.ResourceClass + "') and IsDelete =0", sqlConn);
                                SqlDataReader myExpandsdr = mycmd.ExecuteReader();
                                if (myExpandsdr.HasRows)
                                {
                                    alreadyNum++;
                                    myExpandsdr.Close();
                                }
                                else
                                {
                                    myExpandsdr.Close();
                                    string resInsert = " insert into tb_Resource(ID,Title,FileID,FileType,Subject,Edition,SchoolStage,Grade,BookReel,Catalog,"
                                                 + "TeachingStep,ResourceType,ResourceStyle,ResourceLevel,Purview,ResourceSize,"
                                                 + "" + (comboBox1.SelectedIndex == 1 ? "ComeFrom,IsRecommend," : "") + "TeachingStyle,"
                                                 + "KeyWords,Description,UploadUser,UploadDate,ModifyDate,Copyright,CopyrightName,Sort,ResourceClass) values('" + m.ID + "','" + m.Title + "','" + m.FileID + "','" + m.FileType + "'," + m.Subject + "," + m.Edition + ","
                                                 + m.SchoolStage + "," + m.Grade + "," + m.BookReel + "," + m.Catalog + "," + m.TeachingStep + ","
                                                 + m.ResourceType + "," + m.ResourceStyle + ","
                                                 + m.ResourceLevel + "," + m.Purview + ",'" + m.ResourceSize + "'," + (comboBox1.SelectedIndex == 1 ? "'" + m.ComeFrom + "'," + m.IsRecommend + "," : "")
                                                 + m.TeachingStyle + ",'" + m.KeyWords + "','" + m.Description + "','" + m.UploadUser + "','" + DateTime.Now + "','" + DateTime.Now + "','" + m.Copyright + "','" + m.CopyrightName + "','" + m.Sort + "','" + m.ResourceClass + "') ";
                                    //com.CommandText = resInsert;
                                    SqlCommand cmd = new SqlCommand(resInsert, sqlConn);
                                    int execNum = cmd.ExecuteNonQuery();

                                    //知识点
                                    string knowledgeInsert = "";
                                    if (lsKnowledge != null && lsKnowledge.Count > 0)
                                    {
                                        foreach (int t in lsKnowledge)
                                        {
                                            knowledgeInsert += "insert into tb_ResourceKnowledge(ResourceID,KnowledgeID) values('" + m.ID + "'," + t + ");";
                                        }
                                        SqlCommand cmdKnowledge = new SqlCommand(knowledgeInsert, sqlConn);
                                        int execKnowledge = cmdKnowledge.ExecuteNonQuery();
                                    }

                                    if (comboBox1.SelectedIndex == 0)
                                    {
                                        //制作者
                                        if (lsCreateID != null && lsCreateID.Count > 0)
                                        {
                                            string userCreateInsert = "";
                                            foreach (int t in lsCreateID)
                                            {
                                                userCreateInsert += "insert into tb_ResourceCreator(ResourceID,InnerUserID) values('" + m.ID + "'," + t + ");";
                                            }
                                            SqlCommand cmdUserCreate = new SqlCommand(userCreateInsert, sqlConn);
                                            int execUserCreate = cmdUserCreate.ExecuteNonQuery();
                                        }

                                        //检测者
                                        if (lsDetectorID != null && lsDetectorID.Count > 0)
                                        {
                                            string userDetectorInsert = "";
                                            foreach (int t in lsDetectorID)
                                            {
                                                userDetectorInsert += "insert into tb_ResourceDetector(ResourceID,InnerUserID) values('" + m.ID + "'," + t + ");";
                                            }
                                            SqlCommand cmdUserDetector = new SqlCommand(userDetectorInsert, sqlConn);
                                            int execUserDetector = cmdUserDetector.ExecuteNonQuery();
                                        }
                                    }

                                    //教学模块更改为多选
                                    if (TeachingModules != null && TeachingModules.Count > 0)
                                    {
                                        string userTeachingModules = "";
                                        foreach (int t in TeachingModules)
                                        {
                                            userTeachingModules += "insert into tb_ResourceTeachingModule(ResourceID,TeachingModuleID) values('" + m.ID + "'," + t + ");";
                                        }
                                        SqlCommand cmdApplicable = new SqlCommand(userTeachingModules, sqlConn);
                                        int execApplicable = cmdApplicable.ExecuteNonQuery();
                                    }

                                    //适用对象
                                    if (lsApplicable != null && lsApplicable.Count > 0)
                                    {
                                        string userApplicable = "";
                                        foreach (int t in lsApplicable)
                                        {
                                            userApplicable += "insert into tb_ResourceApplicable(ResourceID,ApplicableID) values('" + m.ID + "'," + t + ");";
                                        }
                                        SqlCommand cmdApplicable = new SqlCommand(userApplicable, sqlConn);
                                        int execApplicable = cmdApplicable.ExecuteNonQuery();
                                    }
                                }
                                sqlConn.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            FileLessNum++;
                            TempModel.ErrorClass error = new TempModel.ErrorClass();
                            if (r["序号"].ToString() != "")
                            {
                                error.No = int.Parse(r["序号"].ToString());
                            }
                            error.NameError = r["显示名称"].ToString().Replace("\'", "\'\'");
                            error.ErrorMsg = e.ToString();
                            error.Sheet = tableName;
                            lsError.Add(error);
                        }
                    }
                    #endregion

                    Application.DoEvents();
                }
                #endregion

                if (checkOut)
                {
                    progressBar1.Value = 0;
                }
                else
                {
                    progressBar1.Value = ds.Tables[0].Rows.Count;

                    //     MessageBox.Show(string.Format("上传结束！\r\n 此次共上传{0}个资源;\r\n其中有问题资源{1}个;\r\n 重复上传资源{2}个", (ds.Tables[0].Rows.Count - NameLessNum - FileLessNum - alreadyNum).ToString(), FileLessNum.ToString(), alreadyNum.ToString()));
                    #region 显示错误信息
                    LoadErrorMsg(lsError);
                    #endregion
                }
            }
        }
        /// <summary>
        /// 点击上传
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Muti_upload_Click(object sender, EventArgs e)
        {
            List<string> count_table = new List<string>();
            string check_table_name;
            for (int j = 0; j < checkedListBox1.Items.Count; j++)
                if (checkedListBox1.GetItemChecked(j))
                {
                    //MessageBox.Show(checkedListBox1.GetItemText(checkedListBox1.Items[j]));

                    listBoxError.Items.Clear();
                    if (txtFileUrl.Text != "")
                    {
                        //if (this.checkedListBox1.Text.ToString() == string.Empty || this.checkedListBox1.Text.ToString() == null)
                        //{
                        //    MessageBox.Show("请选择单元！");
                        //    return;
                        //}
                        string sqlstr = string.Empty;
                        string fileUrl = string.Empty;
                        string ThumbnailUrl = string.Empty;
                        check_table_name = checkedListBox1.GetItemText(checkedListBox1.Items[j]).ToString();
                        if (comboBox1.SelectedIndex == 0)
                        {
                            //素材库
                            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];
                            fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"];// +"UploadHandler.ashx";
                            ThumbnailUrl = System.Configuration.ConfigurationManager.AppSettings["R_ThumbnailfileUrl"]; //+"SetResImg.ashx";

                            if (NewAnalyzeWords())
                            {
                                OpenExcel_Muti(check_table_name);
                                count_table.Add(check_table_name);

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择模板文件！");
                    }

                }

            MessageBox.Show("完成单元个数为：" + count_table.Count.ToString());
        }


        /// <summary>
        /// 清空数据库操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //int? SchoolStage; //學段
        int Grade;
        int Subject;
        int Edition;
        int BookReel;
        string SelectIntStr;
        string selectStr;
        string s1;
        private void ClearData_Click_1(object sender, EventArgs e)
        {
            if (this.ED_cb.Text.ToString() == string.Empty || this.ED_cb.Text.ToString() == null)
            {
                MessageBox.Show("请选择版本！");
                return;
            }
            if (this.SUB_cb.Text.ToString() == string.Empty || this.SUB_cb.Text.ToString() == null)
            {
                MessageBox.Show("请选择学科！");
                return;
            }
            if (this.GRADE_cb.Text.ToString() == string.Empty || this.GRADE_cb.Text.ToString() == null)
            {
                MessageBox.Show("请选择年级！");
                return;
            }
            if (this.BREEL_cb.Text.ToString() == string.Empty || this.BREEL_cb.Text.ToString() == null)
            {
                MessageBox.Show("请选择册别！");
                return;
            }

            string sqlstr = string.Empty;

            Grade = DealData.RetSubstringUnderline(this.GRADE_cb.Text.ToString());
            Subject = DealData.RetSubstringUnderline(this.SUB_cb.Text.ToString());
            Edition = DealData.RetSubstringUnderline(this.ED_cb.Text.ToString());
            BookReel = DealData.RetSubstringUnderline(this.BREEL_cb.Text.ToString());

            System.Configuration.ConfigurationManager.RefreshSection(sqlstr);
            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];

            SelectIntStr = "select count(*) from tb_Resource where  Grade = ('" + Grade + "' ) and Subject = ('" + Subject + "' )"
                            + "and Edition = ('" + Edition + "' ) and BookReel = ('" + BookReel + "' )";

            selectStr = "select * from tb_Resource where  Grade = ('" + Grade + "' ) and Subject = ('" + Subject + "' )"
                                     + "and Edition = ('" + Edition + "' ) and BookReel = ('" + BookReel + "' )";

            s1 = "delete tb_VerifyResource where  resourceid in (select id from  [tb_Resource] where Grade = ('" + Grade + "' ) and  Subject = ('" + Subject + "' ) and Edition = ('" + Edition + "' ) and BookReel = ('" + BookReel + "' ));" +
                            "delete tb_ResourceKnowledge where  resourceid in (select id from  [tb_Resource] where Grade = ('" + Grade + "' ) and  Subject = ('" + Subject + "' ) and Edition = ('" + Edition + "' ) and BookReel = ('" + BookReel + "' ));" +
                            "delete [tb_Resource] where Grade = ('" + Grade + "' ) and  Subject = ('" + Subject + "' ) and Edition = ('" + Edition + "' ) and BookReel = ('" + BookReel + "' );";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                {
                    sqlConn.Open();
                    SqlCommand mycmd = new SqlCommand(SelectIntStr, sqlConn);
                    clearnumber_count = (int)mycmd.ExecuteScalar();
                    sqlConn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请检查数据库连接配置或网络(exc)！");
                return;

            }
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;

            if (clearnumber_count > 0)
            {
                string ed = DealData.stringUnderline(ED_cb.Text);
                string sub = DealData.stringUnderline(SUB_cb.Text);
                string grade = DealData.stringUnderline(GRADE_cb.Text);
                string breel = DealData.stringUnderline(BREEL_cb.Text);

                DialogResult dr = MessageBox.Show(string.Format("即将清空{0}{1}{2}{3}目录下的{4}条记录", ed, sub, grade, breel, clearnumber_count), "清空数据库", messButton);
                if (dr == DialogResult.OK)
                {
                    int returnID;
                    using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                    {
                        sqlConn.Open();
                        SqlCommand delcmd1 = new SqlCommand(s1, sqlConn);
                        returnID = delcmd1.ExecuteNonQuery();
                        sqlConn.Close();
                        if (returnID > 0)
                        {
                            MessageBox.Show(string.Format("{0}条记录清除完毕", clearnumber_count), "完成清空");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("取消清空数据库");
                    return;
                }
            }
            else
            {
                MessageBox.Show("此版本内容为空，可以再次导入");
                return;
            }
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
        /// <summary>
        /// 显示错误信息，将错误信息显示到窗体
        /// </summary>
        public void LoadErrorMsg(List<TempModel.ErrorClass> ls)
        {

            if (ls != null && ls.Count > 0)
            {
                foreach (TempModel.ErrorClass t in ls)
                {
                    listBoxError.Items.Add("页签："+t.Sheet+"；序号：" + t.No.ToString() + ":" + "显示名称：" + t.NameError + ":" + "错误原因：" + t.ErrorMsg + " " + DateTime.Now.ToString());
                    string filename = "资源集模板_出问题的资源.txt";
                    filename = filename.Replace(":", "-");
                    StreamWriter sw = File.AppendText(filename);
                    sw.WriteLine("页签：" + t.Sheet + "；序号：" + t.No.ToString() + "；" + "显示名称：" + t.NameError + "；" + "错误原因：" + t.ErrorMsg + " " + DateTime.Now.ToString());
                    sw.Flush();
                    sw.Close();
                }
            }
        }
        /// <summary>
        /// 将json 反序列化为Dic
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        private Dictionary<string, object> JsonToDic(string jsonData)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public TempModel.Resource Get_Data_From_Server(TempModel.Resource MatchModel, string temp)
        {
            Dictionary<string, object> dic = JsonToDic(temp); // 将JSON 数据转成dic
            foreach (KeyValuePair<string, object> item in dic)
            {
                if (item.Key.ToString() == "Success")
                {
                    MatchModel.IsSuccess = Convert.ToBoolean(item.Value);
                }

            }
            if (MatchModel.IsSuccess)
            {
                foreach (KeyValuePair<string, object> item1 in dic)
                {
                    if (item1.Key.ToString() == "Data")
                    {
                        var subItem = (Dictionary<string, object>)item1.Value;
                        foreach (var str in subItem)
                        {
                            if (str.Key.ToString() == "ID")
                            {
                                MatchModel.FileID = new Guid(str.Value.ToString());
                            }
                            if (str.Key.ToString() == "FileExtension")
                            {
                                MatchModel.FileType = str.Value.ToString();
                            }
                            if (str.Key.ToString() == "FileSize")
                            {
                                MatchModel.ResourceSize = str.Value.ToString();
                            }

                        }
                    }
                }
            }
            return MatchModel;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_upt_Click(object sender, EventArgs e)
        {
            List<string> count_table = new List<string>();
            string check_table_name;
            for (int j = 0; j < checkedListBox1.Items.Count; j++)
                if (checkedListBox1.GetItemChecked(j))
                {
                    listBoxError.Items.Clear();
                    if (txtFileUrl.Text != "")
                    {
                    //    if (this.checkedListBox1.Text.ToString() == string.Empty || this.checkedListBox1.Text.ToString() == null)
                    //    {
                    //        MessageBox.Show("请选择单元！");
                    //        return;
                    //    }
                        check_table_name = checkedListBox1.GetItemText(checkedListBox1.Items[j]).ToString();
                        if (comboBox1.SelectedIndex == 0)
                        {
                            if (NewAnalyzeWords())
                            {
                                OpenExcel(check_table_name);
                                count_table.Add(check_table_name);

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选择模板文件！");
                    }

                }

            MessageBox.Show("完成单元个数为：" + count_table.Count.ToString());
        }

        private void OpenExcel(string tableName)
        {
            string FileID = "";
            sqlstr = System.Configuration.ConfigurationManager.AppSettings["R_materialConnString"];
            fileUrl = System.Configuration.ConfigurationManager.AppSettings["R_materialfileUrl"];
            ThumbnailUrl = System.Configuration.ConfigurationManager.AppSettings["R_ThumbnailfileUrl"];
            progressBar1.Value = 0;
            List<TempModel.ErrorClass> lsError = new List<TempModel.ErrorClass>();
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            if (checkTableName)
            {
                #region 读取“通用设置”表
                try
                {
                    OleDbDataAdapter myCommand2 = null;
                    //从指定的表明查询数据，可先把所有表名列出来
                    string strExcel2 = "select * from [" + "通用设置$" + "]";
                    myCommand2 = new OleDbDataAdapter(strExcel2, strConn);
                    myCommand2.Fill(ds2, "table");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("通用设置单元异常，请检测！");
                }
                #endregion

                #region 读取所选sheet表
                try
                {
                    OleDbDataAdapter myCommand = null;
                    //从指定的表明查询数据，可先把所有表名列出来
                    string strExcel = "select * from [" + tableName + "$" + "] where '显示名称' is not null and '文件路径' is not null";
                    myCommand = new OleDbDataAdapter(strExcel, strConn);
                    myCommand.Fill(ds, "Bicycle");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("单元异常，请检测！");
                }
                #endregion
            }
            else
            {
                MessageBox.Show("请检查文件命名是否正确！");
                return;
            }
            if (ds.Tables.Count > 0)
            {
                int pb = 1;
                int NameLessNum = 0;
                int FileLessNum = 0;
                string checkfile = string.Empty;
                string errorNum = string.Empty;
                bool checkOut = false;
                progressBar1.Maximum = ds.Tables[0].Rows.Count;
                #region 循环处理DataSet
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    progressBar1.Value = pb++;
                    #region 判断文件名和文件路径是否存在
                    checkfile = r["文件路径"].ToString();
                    errorNum = r["序号"].ToString();
                    if (r["显示名称"].ToString() == "")
                    {
                        NameLessNum++;
                        continue;
                    }
                    else if (checkfile != "")
                    {
                        if (!File.Exists(ExcelPath + "\\" + checkfile.Substring(1)))
                        {
                            FileLessNum++;
                            TempModel.ErrorClass test = new TempModel.ErrorClass();
                            test.No = int.Parse(errorNum);
                            test.NameError = r["显示名称"].ToString();
                            test.ErrorMsg = "检测文件路径问题";
                            test.Sheet = tableName;
                            lsError.Add(test);
                            continue;
                        }
                    }
                    #endregion
                    TempModel.Resource m = new TempModel.Resource();
                    string testValue = string.Empty;
                    m.Sort = Convert.ToInt32(r["序号"]);
                    m.Title = r["显示名称"].ToString().Replace("\'", "\'\'");
                    m.Catalog = DealData.RetSubstringBracket(r["教材目录"].ToString());
                    string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
                    string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];
                    string urlRSORT = String.Format(head + tail, "RSORT");//14资源类型            
                    string jsonRSORT = Common.HttpGet(urlRSORT); //y({....})
                    string jsonRSORT2 = jsonRSORT.Substring(2, jsonRSORT.Length - 3);//{}
                    string tep = jsonRSORT2.Substring(7, jsonRSORT2.Length - 8);
                    int eId = DealData.RetSubstringBracket(r["资源类型"].ToString());
                    string ResourceType = string.Empty;
                    string ResourceStyle = string.Empty;
                    GetResType(tep, eId.ToString(), out ResourceType, out ResourceStyle);
                    if (ResourceType != string.Empty)
                    {
                        m.ResourceType = int.Parse(ResourceType);
                    }
                    if (ResourceStyle != string.Empty)
                    {
                        m.ResourceStyle = int.Parse(ResourceStyle);
                    }
                    #endregion
                    using (SqlConnection con = new SqlConnection(sqlstr))
                    {
                        con.Open();
                        SqlCommand com = new SqlCommand("select * from tb_Resource where Catalog='" + m.Catalog + "' and Title ='" + m.Title + "' and Description ='" + m.Description + "' and ResourceStyle='" + m.ResourceStyle + "'", con);
                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            FileID = reader["FileID"].ToString();
                        }
                        con.Close();
                    }
                    string path1 = ExcelPath;
                    #region 上传文件
                    try
                    {
                        path1 = path1 + r["文件路径"].ToString();
                        string temp = "";
                        newWebClient web = new newWebClient();
                        if (File.Exists(path1))
                        {

                            byte[] bUp = web.UploadFile(fileUrl + "UpdateFile.ashx?FileID=" + FileID, path1);
                            temp = System.Text.Encoding.UTF8.GetString(bUp);
                        }
                        //////////////////////////////////////////////////////////////////////////
                        else
                        {
                            FileLessNum++;
                            TempModel.ErrorClass t = new TempModel.ErrorClass();
                            if (r["序号"].ToString() != "")
                            {
                                t.No = int.Parse(r["序号"].ToString());
                            }
                            t.NameError = m.Title.ToString();
                            t.ErrorMsg = "文件上传失败！";
                            t.Sheet = tableName;
                            lsError.Add(t);
                        }
                        #region 获取fileID ，上传缩略图
                        if (r["缩略图路径"].ToString() != "")
                        {
                            if (FileID != null)
                            {
                                string path2 = ExcelPath + r["缩略图路径"].ToString();
                                WebClient Cweb = new WebClient();
                                byte[] ThumbByte = Cweb.UploadFile(fileUrl + "SetResImg.ashx?fileID=" + FileID, path2);
                                string ThumbData = System.Text.Encoding.UTF8.GetString(ThumbByte);

                            }
                            else
                            {
                                FileLessNum++;
                                TempModel.ErrorClass error = new TempModel.ErrorClass();
                                if (r["序号"].ToString() != "")
                                {
                                    error.No = int.Parse(r["序号"].ToString());
                                }
                                error.NameError = r["显示名称"].ToString().Replace("\'", "\'\'");
                                error.ErrorMsg = "缩略图问题".ToString();
                                error.Sheet = tableName;
                                lsError.Add(error);
                            }
                        }
                        #endregion
                        //////////////////////////////////////////////////////////////////////////
                    }
                    catch (System.Exception ex)
                    {
                        FileLessNum++;
                        TempModel.ErrorClass error = new TempModel.ErrorClass();
                        if (r["序号"].ToString() != "")
                        {
                            error.No = int.Parse(r["序号"].ToString());
                        }
                        error.NameError = r["显示名称"].ToString().Replace("\'", "\'\'");
                        error.ErrorMsg = ex.ToString();
                        error.Sheet = tableName;
                        lsError.Add(error);
                        continue;
                    }
                    #endregion
                    //using (SqlConnection sqlConn = new SqlConnection(sqlstr))
                    //{
                    //    //资源表
                    //    sqlConn.Open();
                    //    if (m.Catalog != catalog)
                    //    {
                    //        if (FileID != null && FileID != "")
                    //        {
                    //            string updatestr = "update tb_Resource set Title ='" + m.Title + "', Catalog = '" + m.Catalog + "', KeyWords ='" + m.KeyWords + "',Description='" + m.Description + "',Sort='" + m.Sort + "',ResourceClass =" + m.ResourceClass + " where FileID='" + FileID + "'";
                    //            SqlCommand cmd = new SqlCommand(updatestr, sqlConn);
                    //            int execNum = cmd.ExecuteNonQuery();
                    //        }
                    //    }
                    //    Application.DoEvents();
                    //}

                    if (checkOut)
                    {
                        progressBar1.Value = 0;
                    }
                    else
                    {
                        progressBar1.Value = ds.Tables[0].Rows.Count;
                        LoadErrorMsg(lsError);
                    }
                }
            }
        }

        private void OnChange(object sender, EventArgs e)
        {
            if (this.GRADE_cb.SelectedItem != null && this.GRADE_cb.SelectedItem.ToString() == "高中全年级_18")
            {
                string BX_data = InitService.GetMetadata("BX");
                JavaScriptSerializer js = new JavaScriptSerializer();
                var BREEL_obj = js.Deserialize<dynamic>(BX_data);
                this.BREEL_cb.Items.Clear();
                foreach (var item in BREEL_obj["BX"])
                {
                    this.BREEL_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
                }
            }
            else
            {
                string BX_data = InitService.GetMetadata("BREEL");
                JavaScriptSerializer js = new JavaScriptSerializer();
                var BREEL_obj = js.Deserialize<dynamic>(BX_data);
                this.BREEL_cb.Items.Clear();
                foreach (var item in BREEL_obj["BREEL"])
                {
                    this.BREEL_cb.Items.Add(item["CodeName"] + "_" + item["ID"]);
                }
            }
        }

        private void SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (this.SelectAll.Checked)
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                {
                    checkedListBox1.SetItemChecked(j, true);
                }
            }
            else
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                {
                    checkedListBox1.SetItemChecked(j, false);
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
}
