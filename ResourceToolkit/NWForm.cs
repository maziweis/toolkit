using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using ResourceToolkitr;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace ResourceToolkit
{
    public partial class NWForm : Form
    {
        public string filePath = "";
        public string strConn = "";
        NWJson nWJson = new NWJson();
        string grade = "";

        public NWForm()
        {
            InitializeComponent();
        }

        private void OpenExcel_Click(object sender, EventArgs e)
        {
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

            }
        }

        private void BuildJson_Click(object sender, EventArgs e)
        {
            string fileDirectory = System.IO.Path.GetDirectoryName(filePath);
            HSSFWorkbook templateWb;//模板
                                    //源数据
            using (FileStream fs =File.OpenRead(filePath))//new FileStream(filePath, FileMode.Open)
            {
                templateWb = new HSSFWorkbook(fs);
            }
            var desSheet = templateWb.GetSheetAt(0);
            
            grade = System.IO.Path.GetFileName(fileDirectory);
            nWJson.cover = grade + "/Cover.gif";
            nWJson.id = Convert.ToInt32(grade.Replace("A", "").Replace("B",""));

            nWJson.name = Regex.Split(Regex.Split(filePath, "英语南外电影课系列", RegexOptions.IgnoreCase)[1], "趣配音", RegexOptions.IgnoreCase
)[0];
            string[] units = System.IO.Directory.GetDirectories(fileDirectory);
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            string sheetName1 = "总视频信息";
            string sheetName2 = "分视频信息";
            OleDbDataAdapter myCommand = null;
            string strExcel = "select * from [" + sheetName1 + "$" + "]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            myCommand.Fill(ds, "Bicycle");
            OleDbDataAdapter myCommand1 = null;
            string strExcel1 = "select * from [" + sheetName2 + "$" + "]";
            myCommand1 = new OleDbDataAdapter(strExcel1, strConn);
            myCommand1.Fill(ds1, "Bicycle1");
            for (int m = 0; m < units.Length; m++)
            {
                VideoList videoList = null;
                int unitID = m + 1;
                videoList = new VideoList();
                videoList.id = unitID;
                string jianjie = null;//视频简介
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    if (ds.Tables[0].Rows[j]["序号"].ToString() == unitID.ToString())
                    {
                        videoList.name = ds.Tables[0].Rows[j]["视频标题"].ToString();
                        var cells = desSheet.GetRow(j + 1).Cells;
                        foreach (var item in cells)
                        {
                            
                        }
                        jianjie = desSheet.GetRow(j+1).GetCell(23).ToString();
                        //jianjie = ds.Tables[0].Rows[j]["视频简介"].ToString();
                    }
                }
                videoList.cover = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/01.jpg";
                nWJson.videoList.Add(videoList);
                #region 模块1
                ModList mod3 = new ModList() { id = 1, name = "Let's watch" };
                mod3.data.url = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/02.mp4";
                videoList.modList.Add(mod3);
                #endregion
                #region 模块2
                ModList mod4 = new ModList() { id = 2, name = "Let's talk" };
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["视频标题"].ToString() == videoList.name&&!string.IsNullOrEmpty(r["角色名称音频"].ToString()))
                    {
                        Card card = new Card();
                        card.dir = r["英文性格描述"].ToString();
                        card.dir2 = r["中文性格描述"].ToString();
                        card.imgUrl = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["角色名称"].ToString();
                        card.audioUrl1 = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["角色名称音频"].ToString();
                        card.audioUrl2 = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["性格音频"].ToString();
                        mod4.data.cards.Add(card);
                    }
                }
                nWJson.videoList[unitID - 1].modList.Add(mod4);
                #endregion
                #region 模块3
                ModList mod1 = new ModList() { id = 3, name = "Let's learn" };
                Data data1 = new Data();
                data1.url = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/02.mp4";
                data1.intro = jianjie;
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    if (ds1.Tables[0].Rows[i]["序号"].ToString() == unitID.ToString())
                    {
                        Section section = new Section();
                        section.title = ds1.Tables[0].Rows[i]["分视频文本"].ToString();
                        section.chinese = ds1.Tables[0].Rows[i]["分视频文本2"].ToString();
                        long start = ConvertDateI(Convert.ToDateTime("1970-01-01 " + ds1.Tables[0].Rows[i]["起始时间"].ToString()));
                        long end = ConvertDateI(Convert.ToDateTime("1970-01-01 " + ds1.Tables[0].Rows[i]["结束时间"].ToString()));
                        section.timeNode.Add((int)start);
                        section.timeNode.Add((int)end);
                        data1.section.Add(section);
                    }
                }
                mod1.data = data1;
                videoList.modList.Add(mod1);
                #endregion
                #region 模块4
                ModList mod2 = new ModList() { id = 4, name = "Let's sing" };
                if (File.Exists(fileDirectory + "/" + unitID.ToString().PadLeft(2, '0') + "/song.txt"))
                {
                    string song = Read(fileDirectory + "/" + unitID.ToString().PadLeft(2, '0') + "/song.txt");
                    mod2.data.name = Regex.Split(song, "@@")[0];
                    mod2.data.content = Regex.Split(song, "@@")[1];
                    mod2.data.url = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/song.mp3";
                    string mppath = string.Format(fileDirectory + "/" + unitID.ToString().PadLeft(2, '0') + "/song.mp3");
                    if (!File.Exists(mppath))
                    {
                        mod2.data.url = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/song.mp4";
                    }
                }
                else
                    mod2.data = null;
               
                nWJson.videoList[unitID - 1].modList.Add(mod2);
                #endregion    
                #region 模块5
                ModList mod5 = new ModList() { id = 5, name = "Let's read"};
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["视频标题"].ToString() == videoList.name)
                    {
                        if (mod5.data.wordList.Any(w => w.word == r["英文词汇"].ToString()))
                        {
                            mod5.data.wordList.Where(w => w.word == r["英文词汇"].ToString()).FirstOrDefault().translate += string.IsNullOrEmpty(r["词性"].ToString()) ? r["词汇中文"].ToString() : ";[" + r["词性"].ToString() + "]" + r["词汇中文"].ToString();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(r["英文词汇"].ToString()))
                            {
                                WordList word = new WordList();
                                word.word = r["英文词汇"].ToString();
                                word.translate = string.IsNullOrEmpty(r["词性"].ToString()) ? r["词汇中文"].ToString() : ";[" + r["词性"].ToString() + "]" + r["词汇中文"].ToString();
                                if(word.translate.IndexOf(";")==0)
                                    word.translate=word.translate.Remove(0,1);
                                word.audioUrl = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["词汇音频"].ToString();
                                if (!string.IsNullOrEmpty(r["词汇配图"].ToString()))
                                {
                                    word.imgUrl = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["词汇配图"].ToString();
                                }
                                else
                                {
                                    word.imgUrl = "";
                                }
                                mod5.data.wordList.Add(word);
                            }
                        }
                    }
                }
                nWJson.videoList[unitID - 1].modList.Add(mod5);
                #endregion
                #region 模块6
                ModList mod6 = new ModList() { id = 6, name = "Let's practice"};
                int idnum = 0;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (r["视频标题"].ToString() == videoList.name&&!string.IsNullOrEmpty(r["英文句子"].ToString()))
                    {
                        idnum++;
                        SentenceList sentence = new SentenceList();
                        sentence.sentence = idnum+"."+ r["英文句子"].ToString();
                        sentence.translate = r["句子中文"].ToString();
                        sentence.audioUrl = grade + "/" + unitID.ToString().PadLeft(2, '0') + "/" + r["句子音频"].ToString();
                        mod6.data.sentenceList.Add(sentence);
                    }
                }
                nWJson.videoList[unitID - 1].modList.Add(mod6);
                #endregion
            }
            string result = JsonHelper.EncodeJson(nWJson);
            write(fileDirectory + "/" + grade + ".json", result);
            MessageBox.Show("生成成功");
        }

        /// <summary>
        /// Datetime转时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long ConvertDateI(DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddHours(-8); // 当地时区
            long timeStamp = (long)(dt - startTime).TotalMilliseconds; // 相差毫秒数
            return timeStamp;
        }

        public string Read(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            StringBuilder sb = new StringBuilder();
            String line; bool first = true;
            while ((line = sr.ReadLine()) != null)
            {
                if (first)
                {
                    line += "@@";
                    first = false;
                    sb.Append(line);
                }
                else
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        sb.Append(line + "\n");
                    }
                }
            }
            return sb.ToString();
        }
        public void write(string path, string json)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            StreamWriter sw = File.AppendText(path);
            sw.WriteLine(json);
            sw.Flush();
            sw.Close();
        }
    }

    public class NWJson
    {
        public string cover { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<VideoList> videoList { get; set; } = new List<VideoList>();

    }
    public class VideoList
    {
        public string cover { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public List<ModList> modList { get; set; } = new List<ModList>();

    }
    public class ModList
    {
        public int id { get; set; }
        public string name { get; set; }
        public Data data { get; set; } = new Data();

    }
    public class Data
    {
        public string url { get; set; }
        public string intro { get; set; }
        public string content { get; set; }
        public string name { get; set; }
        public List<Section> section { get; set; } = new List<Section>();
        public List<Card> cards { get; set; } = new List<Card>();
        public List<SentenceList> sentenceList { get; set; } = new List<SentenceList>();
        public List<WordList> wordList { get; set; } = new List<WordList>();

    }
    public class Card
    {
        public string dir { get; set; }
        public string dir2 { get; set; }
        public string imgUrl { get; set; }
        public string audioUrl1 { get; set; }
        public string audioUrl2 { get; set; }

    }
    public class Section
    {
        public string title { get; set; }
        public string chinese { get; set; }
        public List<int> timeNode { get; set; } = new List<int>();

    }
    public class WordList
    {
        public string word { get; set; }
        public string translate { get; set; }
        public string audioUrl { get; set; }
        public string imgUrl { get; set; }
    }

    public class SentenceList
    {
        public string sentence { get; set; }
        public string translate { get; set; }
        public string audioUrl { get; set; }
    }
   
}
