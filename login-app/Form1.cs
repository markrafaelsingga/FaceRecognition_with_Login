using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FaceRecognition;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Face;

namespace login_app
{
    public partial class Form1 : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        string full;
        string uname;
        string pword;
        
        public Form1()
        {
            InitializeComponent();
            
           
        }
        FaceRec face = new FaceRec();           

        
        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Hide();
            face.isTrained = true;
            face.openCamera(pbCaptured, pbCaptured);
            face.getPersonName(label1);
            
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int role = (int)comboBox1.SelectedIndex;
            db.register(fname.Text, lname.Text, user.Text, pass.Text, role);
            face.Save_IMAGE(fname.Text +" "+lname.Text);
            MessageBox.Show("Save successfully!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var name = db.getAcc(label1.Text).ToList();
            if(name != null && name.Any())
            {
                foreach(var item in name)
                {
                    full = item.Fullname;
                }
            }

            var up = db.sp_login(user.Text, pass.Text).ToList();
            if(up!=null && up.Any())
            {
                foreach(var item in up)
                {
                    uname = item.username;
                    pword = item.pass;
                }
            }

            try
            {
                if (full == label1.Text)
                {
                    
                    if(db.sp_type(uname,pword) == 0)
                    {
                        MessageBox.Show($"Hi: {full} Welcome Admin!");
                    }
                    else
                    {
                        MessageBox.Show($"Hi: {full} Welcome Staff!");
                    }
                }
                else if (uname == user.Text && pword == pass.Text)
                {
                    if (db.sp_type(uname, pword) == 0)
                    {
                        MessageBox.Show($"Hi: {full} Welcome Admin!");
                    }
                    else
                    {
                        MessageBox.Show($"Hi: {full} Welcome Staff!");
                    }
                }
                else
                {
                    MessageBox.Show("Username,Password or Your face is not recorded");
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Didn't detect your face!");
            }
        }
    }
}
