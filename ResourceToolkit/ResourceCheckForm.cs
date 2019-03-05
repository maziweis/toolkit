using System;
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

namespace ResourceToolkit
{
    public partial class ResourceCheckForm : Form
    {
        public string filePath = "";
        public string strConn = "";
        public ResourceCheckForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel文件|*.xls|Excel文件|*.xlsx";
            List<string> combobox_list = new List<string>();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = dialog.FileName;
                filePath = richTextBox1.Text;
            }

            if (filePath != "")
            {
                string postfix = richTextBox1.Text.Substring(richTextBox1.Text.LastIndexOf("."));
                if (postfix == ".xls")
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + richTextBox1.Text + ";" + "Extended Properties=Excel 8.0;";
                else
                    strConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'", richTextBox1.Text);
                jilu(strConn);
                using (System.Data.OleDb.OleDbConnection connection = new System.Data.OleDb.OleDbConnection(strConn))
                {
                    jilu("47-");
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception ex)
                    {
                        jilu("54-"+ex.Message);
                        throw;
                    }
                    //connection.Open();
                    jilu("48-");
                    DataTable table = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    jilu("49-");
                    string[] strTableNames = new string[table.Rows.Count];
                    string excel_useless = "";
                    for (int k = 0; k < table.Rows.Count; k++)
                    {
                        strTableNames[k] = table.Rows[k]["TABLE_NAME"].ToString();

                        if (strTableNames[k].ToString().Trim().Substring(strTableNames[k].Length - 1, 1) == "$")
                        {
                            excel_useless = strTableNames[k].Substring(0, strTableNames[k].Length - 1);
                            if (excel_useless != "基础数据" && excel_useless != "修订记录表" && excel_useless != "填写说明表" && excel_useless != "教材目录" && excel_useless != "通用设置")
                            {
                                combobox_list.Add(strTableNames[k].ToString().Trim().Substring(0, strTableNames[k].Length - 1).ToString().Trim());
                            }
                            combobox_list.Sort();
                        }
                        else if (strTableNames[k].Substring(strTableNames[k].Length - 2, 2) == "$'")
                        {
                            excel_useless = strTableNames[k].Substring(1, strTableNames[k].Length - 3);
                            if (excel_useless != "基础数据" && excel_useless != "修订记录表" && excel_useless != "填写说明表" && excel_useless != "教材目录" && excel_useless != "通用设置")
                            {
                                combobox_list.Add(strTableNames[k].Substring(1, strTableNames[k].Length - 3).ToString().Trim());
                            }
                        }
                    }
                    this.checkedListBox1.Items.AddRange(combobox_list.ToArray());
                }
            }
        }
        public string sheetName;
        public void jilu(string s)
        {
            string filename = "错误日志.txt";
            StreamWriter sw = File.AppendText(filename);
            sw.WriteLine(s + DateTime.Now.ToString());
            sw.Flush();
            sw.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> count_table = new List<string>();
            DataSet ds = new DataSet();
            bool IsError = true;
            //List<TempModel.ErrorClass> lsError = new List<TempModel.ErrorClass>();//错误列表
            #region 检验是否选中
            bool selectStatus = false;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    selectStatus = true;
                    break;
                }
            }
            if (selectStatus == false)
            {
                MessageBox.Show("请选择检测单元！");
                return;
            }
            #endregion

            for (int j = 0; j < checkedListBox1.Items.Count; j++)
            {
                if (checkedListBox1.GetItemChecked(j))
                {
                    //List<TempModel.ErrorClass> lsError = new List<TempModel.ErrorClass>();//错误列表
                    List<TempModel.ErrorClass> lsError1 = new List<TempModel.ErrorClass>();
                    List<TempModel.ErrorClass> lsError2 = new List<TempModel.ErrorClass>();//错误列表
                    List<TempModel.ErrorClass> lsError3 = new List<TempModel.ErrorClass>();//错误列表
                    //string unit = checkedListBox1.SelectedItem.ToString();  //选中的单元
                    sheetName = checkedListBox1.GetItemText(checkedListBox1.Items[j]).ToString().Trim();
                    ds.Tables.Clear();
                    try
                    {
                        OleDbDataAdapter myCommand = null;
                        string strExcel = "select * from [" + sheetName + "$" + "] where '显示名称' is not null and '文件路径' is not null";
                        myCommand = new OleDbDataAdapter(strExcel, strConn);
                        myCommand.Fill(ds, "Bicycle");
                    }
                    catch (System.Exception ex)
                    {
                        TempModel.ErrorClass test = new TempModel.ErrorClass();
                        test.No = -1;
                        test.NameError = sheetName;
                        test.ErrorMsg = "单元异常。可能原因: sheet命名" + sheetName + "后有空格，请修改后再检测本单元！";
                        lsError1.Add(test);
                        break;
                    }

                    if (ds.Tables.Count > 0)
                    {
                        bool flag = false;//有没有进行过字段检测  
                        int lineNum = 0;
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            if (!flag)
                            {
                                #region 检测一：检测excel字段是否匹配
                                try
                                {
                                    string title1 = ds.Tables[0].Rows[0]["序号"].ToString().Replace("\'", "\'\'");
                                    string title2 = ds.Tables[0].Rows[0]["显示名称"].ToString().Replace("\'", "\'\'");
                                    string title3 = ds.Tables[0].Rows[0]["内容说明"].ToString().Replace("\'", "\'\'");
                                    string title4 = ds.Tables[0].Rows[0]["资源类型"].ToString().Replace("\'", "\'\'");
                                    string title5 = ds.Tables[0].Rows[0]["教材名称"].ToString().Replace("\'", "\'\'");
                                    string title6 = ds.Tables[0].Rows[0]["教材目录"].ToString().Replace("\'", "\'\'");
                                    string title7 = ds.Tables[0].Rows[0]["教学环节"].ToString().Replace("\'", "\'\'");
                                    string title8 = ds.Tables[0].Rows[0]["教学模块"].ToString().Replace("\'", "\'\'");
                                    string title9 = ds.Tables[0].Rows[0]["知识点"].ToString().Replace("\'", "\'\'");
                                    string title10 = ds.Tables[0].Rows[0]["关键字"].ToString().Replace("\'", "\'\'");
                                    string title11 = ds.Tables[0].Rows[0]["资源描述"].ToString().Replace("\'", "\'\'");
                                    string title12 = ds.Tables[0].Rows[0]["制作者"].ToString().Replace("\'", "\'\'");
                                    string title13 = ds.Tables[0].Rows[0]["检测者"].ToString().Replace("\'", "\'\'");
                                    string title14 = ds.Tables[0].Rows[0]["上传者"].ToString().Replace("\'", "\'\'");
                                    string title15 = ds.Tables[0].Rows[0]["文件路径"].ToString().Replace("\'", "\'\'");
                                    string title16 = ds.Tables[0].Rows[0]["缩略图路径"].ToString().Replace("\'", "\'\'");
                                    string title17 = ds.Tables[0].Rows[0]["版权信息"].ToString().Replace("\'", "\'\'");
                                }
                                catch (System.Exception ex)
                                {
                                    TempModel.ErrorClass test = new TempModel.ErrorClass();
                                    test.No = Convert.ToInt32(ds.Tables[0].Rows[0]["序号"].ToString().Replace("\'", "\'\'"));
                                    test.NameError = sheetName + "---" + ds.Tables[0].Rows[0]["显示名称"].ToString().Replace("\'", "\'\'");
                                    test.ErrorMsg = "检测首行字段名称是否正确。提醒:修改完毕后请再次检测" + sheetName + "单元！";
                                    lsError1.Add(test);
                                    break;
                                }
                                finally
                                {
                                    flag = true;
                                }
                                #endregion
                            }

                            string num_test = r["序号"].ToString();
                            if (num_test == "" || num_test == null)
                            {
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = lineNum;
                                test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                test.ErrorMsg = "请删除多余行后，再检测本单元！有效行数" + lineNum + "，有填写信息的；总行数" + ds.Tables[0].Rows.Count + "，包括填写了信息和未填写的行。有效行之外的，称多余行。";
                                lsError1.Add(test);
                                break;
                            }
                            int num = Convert.ToInt32(num_test);
                            string fileUrl = r["文件路径"].ToString();
                            string imgUrl = r["缩略图路径"].ToString();

                            #region 检测二：检测序号是否连续
                            int dist = num - lineNum;
                            if (dist == 1)
                            {
                                lineNum = num;
                            }
                            else
                            {
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = num;
                                test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                test.ErrorMsg = "检测记录“序号”是否连续。";
                                lsError1.Add(test);
                                lineNum = num;
                            }

                            #endregion

                            #region 检测三：检测必填字段
                            string field0 = "", field1 = "", field2 = "", field3 = "", field4 = "", field5 = "", field6 = "", field7 = "";
                            string field8 = "", field9 = "", field10 = "", field11 = "", field12 = "", field13 = "", field14 = "", field15 = "";
                            try
                            {
                                field0 = r["序号"].ToString().Replace("\'", "\'\'");
                                field1 = r["显示名称"].ToString().Replace("\'", "\'\'");
                                field2 = r["资源类型"].ToString().Replace("\'", "\'\'");
                                field3 = r["教材名称"].ToString().Replace("\'", "\'\'");
                                field4 = r["教学环节"].ToString().Replace("\'", "\'\'");
                                field5 = r["关键字"].ToString().Replace("\'", "\'\'");
                                field6 = r["资源描述"].ToString().Replace("\'", "\'\'");
                                field7 = r["制作者"].ToString().Replace("\'", "\'\'");
                                field8 = r["检测者"].ToString().Replace("\'", "\'\'");
                                field9 = r["上传者"].ToString().Replace("\'", "\'\'");
                                field10 = r["文件路径"].ToString().Replace("\'", "\'\'");
                                field12 = r["版权信息"].ToString().Replace("\'", "\'\'");
                                field13 = r["教材目录"].ToString().Replace("\'", "\'\'");
                            }
                            catch (System.Exception ex)
                            {
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = num;
                                test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                test.ErrorMsg = ex.ToString();
                                lsError1.Add(test);
                            }

                            string[] fields = new string[14] { "序号", "显示名称", "资源类型", "教材名称", "教学环节", "关键字", "资源描述", "制作者", "检测者", "上传者", "文件路径", "缩略图路径", "版权信息", "教材目录" };
                            List<string> fieldList = new List<string>();
                            fieldList.Add(field0); fieldList.Add(field1); fieldList.Add(field2);
                            fieldList.Add(field3); fieldList.Add(field4); fieldList.Add(field5);
                            fieldList.Add(field6); fieldList.Add(field7); fieldList.Add(field8);
                            fieldList.Add(field9);
                            fieldList.Add(field10);
                            fieldList.Add(field12); fieldList.Add(field13);

                            string record = "";
                            for (int i = 0; i < fieldList.Count; i++)
                            {
                                if (fieldList[i] == "" || fieldList[i] == null)
                                {
                                    record = record + "“" + fields[i] + "”";
                                }
                            }

                            if (record != "")
                            {
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = num;
                                test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                test.ErrorMsg = "检测字段" + record + "是否填写。";
                                lsError1.Add(test);
                            }
                            #endregion

                            #region 检测四：文件路径是否存在
                            if (fileUrl != "")
                            {
                               string s= richTextBox1.Text.ToString().Substring(0, richTextBox1.Text.ToString().LastIndexOf("\\"));
                                if (!File.Exists(s+"\\"+fileUrl.Substring(1)))
                                {
                                    TempModel.ErrorClass test = new TempModel.ErrorClass();
                                    test.No = num;
                                    test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                    test.ErrorMsg = "找不到Excel表中对应的文件！。";
                                    lsError2.Add(test);
                                }
                                ////---------------音频的缩略图路径可以直接过滤--------
                                if (Path.GetExtension(fileUrl) != ".mp3")
                                {
                                    if (imgUrl != "")
                                    {
                                        if (!File.Exists(s + "\\" + imgUrl.Substring(1)))
                                        {
                                            TempModel.ErrorClass test = new TempModel.ErrorClass();
                                            test.No = num;
                                            test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                            test.ErrorMsg = "找不到Excel表中对应的缩略图文件！";
                                            lsError3.Add(test);
                                        }
                                    }
                                    //else
                                    //{
                                    //    TempModel.ErrorClass test = new TempModel.ErrorClass();
                                    //    test.No = num;
                                    //    test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                    //    test.ErrorMsg = "------检测到Excel表中缩略图路径没有填写！";
                                    //    lsError3.Add(test);
                                    //}
                                }
                            }
                            else
                            {
                                TempModel.ErrorClass test = new TempModel.ErrorClass();
                                test.No = num;
                                test.NameError = sheetName + "---" + r["显示名称"].ToString();
                                test.ErrorMsg = "-----检测到Excel表中文件路径没有填写！";
                                lsError2.Add(test);
                            }
                            #endregion
                        }

                    }
                    if ((lsError1.Count + lsError2.Count + lsError3.Count) > 0)
                    {
                        IsError = false;
                        string filename = "";
                        filename = sheetName + "资源集模板_出问题的资源.txt";
                        if (File.Exists(filename))
                        {
                            File.Delete(filename);
                        }
                        StreamWriter sw = File.AppendText(filename);
                        sw.WriteLine("********** 文件路径***********");
                        foreach (TempModel.ErrorClass t in lsError2)
                        {
                            sw.WriteLine("序号：" + t.No.ToString() + "；" + "显示名称：" + t.NameError.ToString() + "；" + "错误原因：" + t.ErrorMsg);
                        }
                        sw.WriteLine("**********缩略图路径*********");
                        foreach (TempModel.ErrorClass t in lsError3)
                        {
                            sw.WriteLine("序号：" + t.No.ToString() + "；" + "显示名称：" + t.NameError.ToString() + "；" + "错误原因：" + t.ErrorMsg);
                        }
                        sw.WriteLine("***********其     他**********");
                        foreach (TempModel.ErrorClass t in lsError1)
                        {
                            sw.WriteLine("序号：" + t.No.ToString() + "；" + "显示名称：" + t.NameError.ToString() + "；" + "错误原因：" + t.ErrorMsg);
                        }
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            if (IsError == true)
            {
                label3.Text = "检测正确！";
            }
            else
            {
                label3.Text = "检测有误。\n生成本地txt，请查看！";
            }
        }
    }
}
