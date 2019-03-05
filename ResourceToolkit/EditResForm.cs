using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceToolkit
{
    public partial class EditResForm : Form
    {
        List<String> specificBooksList = new List<string>();//E
        List<String> resultEDList = new List<string>();
        List<String> contentsList = new List<string>();
        public static List<String> catologList = new List<string>();
        string head = System.Configuration.ConfigurationManager.AppSettings["MetadataService"];
        string tail = System.Configuration.ConfigurationManager.AppSettings["mField"];
        string bctail = System.Configuration.ConfigurationManager.AppSettings["bookContents"];
        string bntail = System.Configuration.ConfigurationManager.AppSettings["bookName"];
        public EditResForm()
        {
            InitializeComponent();
            //15版本
            string urlED = String.Format(head + tail, "ED");//15版本    
            string jsonED = Common.HttpGet(urlED); //y({....})
            string jsonED2 = jsonED.Substring(2, jsonED.Length - 3);//{}
            resultEDList = Common.DealJson(jsonED2, "ED");
            if (resultEDList.Count > 0)
            {
                editVersion.Items.AddRange(resultEDList.ToArray());
                editVersion.Text = "请选择版本";
            }
        }
        /// <summary>
        /// 根据版本选择册别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editVersion_SelectionChangeCommitted(object sender, EventArgs e)
        {
            editCe.Items.Clear();
            //String edition = cxbVersion.Text;
            String edition = editVersion.SelectedItem.ToString();
            if (edition == "请选择版本")
            {
                MessageBox.Show("请选择版本");
            }
            else 
            {
                String editionNum = edition.Substring(edition.IndexOf("_") + 1);
                int edNum = int.Parse(editionNum);
                //需要查询的字段
                string urlBooks = head + bntail;

                string jsonStr = Common.HttpGet(urlBooks);
                Dictionary<String, object> jsonDic = Common.JsonToDictionary(jsonStr);
                ArrayList BookList = new ArrayList();
                specificBooksList = new List<string>();
                String success = "";
                foreach (KeyValuePair<String, object> item in jsonDic)
                {
                    if (item.Key.ToString() == "Success")
                        success = item.Value.ToString();
                    if (item.Key.ToString() == "Data")
                        BookList = (ArrayList)item.Value;
                }
                if (success == "True" && BookList.Count > 0)
                {
                    specificBooksList = Common.GetBooksOfSpecificEdition(urlBooks, edNum, BookList);
                }
                if (specificBooksList.Count > 0)
                {
                    editCe.Items.AddRange(specificBooksList.ToArray());
                    editCe.Text = "请选择册别";
                }
            }
            
        }
        /// <summary>
        /// 生成教材目录
        /// </summary>
        private void editCe_SelectionChangeCommitted(object sender, EventArgs e)
        {
            editList.Items.Clear();
            string bookName = editCe.SelectedItem.ToString();
            String ceNum = bookName.Substring(bookName.LastIndexOf("_") + 1);
            String[] ceArray = ceNum.Split('-');
            string urlBookContents = String.Format(head + bctail, ceArray[0], ceArray[1], ceArray[2], ceArray[3], ceArray[4]);
            string jsonStr = Common.HttpGet(urlBookContents);
            string jsonStr2 = jsonStr.Substring(2, jsonStr.Length - 3);
            contentsList = Common.GetContentsOfSpecificBook(jsonStr2);
            editList.Items.AddRange(contentsList.ToArray());
        }
        public delegate void DelegateData(string str1, string str2 ,string str3);
        public event DelegateData DelegateDataEvent;
        private void button1_Click(object sender, EventArgs e)
        {

            DelegateDataEvent(this.editVersion.Text, this.editCe.Text, this.editList.Text);
            if (editVersion.Text != string.Empty && editCe.Text != string.Empty && editList.Text != string.Empty)
            {
                Close();
            }
        }

        private void btn_Cancel(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
