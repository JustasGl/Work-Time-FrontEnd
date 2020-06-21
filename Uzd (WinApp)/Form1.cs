using Newtonsoft.Json;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Uzd__WinApp_
{
    public partial class Form1 : Form
    {
        string url = "https://localhost:44353/api/Todo/";
        bool JobBeginning;
        string StartDate = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!JobBeginning)
            {
                StartDate = DateTime.Now.ToString();
                Post(new Dates(StartDate, null));
                button1.Text = "Baigti dirbti";
                JobBeginning = true;
                Print();
            }
            else
            {
                int id = FindId(StartDate);
                Dates d = new Dates(id,StartDate, DateTime.Now.ToString());
                Put(d, id);
                button1.Text = "Pradėti dirbti";
                JobBeginning = false;
                Print();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!Check(textBox1.Text))
            {
                label4.Text = "Įveskite natūralūjį skaičių";
                return;
            }
            Delete(textBox1.Text);
            textBox1.Text = "";
            Print();
        }

        void Print ()
        {
            dataGridView1.DataSource = GetList();
        }
        List<Dates> GetList()
        {
            string data;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadLine();
            }

            return JsonConvert.DeserializeObject<List<Dates>>(data);
        }
        int FindId(string StartDate)
        {

            List<Dates> DateList = GetList();
            foreach(Dates d in DateList)
            {
                if (StartDate == d.wstart)
                    return d.id;
            }
            return -1;
        }
        public string Delete(string id, string method = "DELETE")
        {
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+"/"+id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Method = method;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        public string Put(Dates date, int id, string method = "Put")
        {
            string json = JsonConvert.SerializeObject(date, Newtonsoft.Json.Formatting.Indented);
            byte[] dataBytes = Encoding.UTF8.GetBytes(json);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url+"/"+id);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json; charset=utf-8";
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
        public string Post(Dates date, string method = "POST")
        {
            string json = JsonConvert.SerializeObject(date, Newtonsoft.Json.Formatting.Indented);
            byte[] dataBytes = Encoding.UTF8.GetBytes(json);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = "application/json; charset=utf-8";
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                requestBody.Write(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static bool Check(string s)
        {
            if (s == "" || s == null)
                return false;
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
