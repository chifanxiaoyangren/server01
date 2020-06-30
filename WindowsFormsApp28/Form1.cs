using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApp28.log;
using WindowsFormsApp28.user;

namespace WindowsFormsApp28
{
    public partial class Form1 : Form
    {

        private delegate void SetButCallback(Button button2,Person person);
        private delegate void Addkongjian(Button button2);
        private void add(Button button2)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Addkongjian Addkongjian1 = add;
                    this.Invoke(Addkongjian1, new object[] { button2 });
                }
                else
                {
                    this.Controls.Add(button2);
                }
            }
            catch (Exception)
            {
            }
        }


        private void buttonset(Button button2,Person person)
        {
            try
            {
                if (button2.InvokeRequired)
                {
                    SetButCallback setTextDele = buttonset;
                    button2.Invoke(setTextDele, new object[] { button2, person });
                }
                else
                {
                    button2.Location = person.getPosition();
                    button2.Text = person.getName();
                }
            }
            catch (Exception e)
            {
            }
        }

        private delegate void SetTextCallback(string text);

        /// <summary>
        /// 控件状态更新
        /// </summary>
        /// <param name="text"></param>
        private void TextBox1SetText(string text)
        {
            try
            {
                if (textBox1.InvokeRequired)
                {
                    SetTextCallback setTextDele = TextBox1SetText;
                    textBox1.Invoke(setTextDele, new object[] { text });
                }
                else
                {
                    textBox1.AppendText(text);

                }
            }
            catch (Exception)
            {
            }
        }




        public Form1()
        {
            InitializeComponent();
        }

        Personfactory Personfactory = new Personfactory();
        private void button1_Click(object sender, EventArgs e)
        {
            LogHelper.WriteLog("sdf");
            
        }


        public void MyAsyncCallback(IAsyncResult ar)
        {

            //初始化一个SOCKET，用于其它客户端的连接
            Socket server1 = (Socket)ar.AsyncState;
            Socket Client = server1.EndAccept(ar);
            
            //获取客户端的IP和端口
            string ip = Client.RemoteEndPoint.ToString();
            //当客户端终止连接时
            TextBox1SetText(ip + "登录");
            //将要发送给连接上来的客户端的提示字符串

            User  user=(User)Personfactory.getPerson("User", ip,new Point(0,0),10);
            Button userbutton=new Button();

            userbutton.Enabled = true;
            userbutton.Visible = true;
            add(userbutton);
            string strDateLine = "Login";
            Byte[] byteDateLine = System.Text.Encoding.ASCII.GetBytes(strDateLine);
            //将提示信息发送给客户端
            Client.Send(byteDateLine, byteDateLine.Length, 0);
            //等待新的客户端连接
            server1.BeginAccept(new AsyncCallback(MyAsyncCallback), server1);
            while (true)
            {
                int recv = 0;
                try
                {
                    recv = Client.Receive(byteDateLine);
                }
                catch
                {
                    break;
                }
               
                string stringdata = Encoding.ASCII.GetString(byteDateLine, 0, recv);
                DateTimeOffset now = DateTimeOffset.Now;
                //获取客户端的IP和端口
                if (stringdata == "STOP" || stringdata == "")
                {
                    //当客户端终止连接时
                    TextBox1SetText(ip + "已从服务器断开");
                    break;
                }
                else if(stringdata.Trim() == "GET")
                {
                    byte[] re = Encoding.Default.GetBytes(user.Speack());
                    Client.Send(re, re.Length, 0);
                }
                else if(stringdata.Trim() == "w")
                {
                    Client.Send(Encoding.Default.GetBytes(user.w()));
                }
                else if (stringdata.Trim() == "a")
                {

                    Client.Send(Encoding.Default.GetBytes(user.a()));
                }
                else if (stringdata.Trim() == "s")
                {

                    Client.Send(Encoding.Default.GetBytes(user.s()));
                }
                else if (stringdata.Trim() == "d")
                {

                    Client.Send(Encoding.Default.GetBytes(user.d()));
                }

                buttonset(userbutton,user);
                ////显示客户端发送过来的信息
                //TextBox1SetText(ip + "    " + now.ToString("G") + "     " + stringdata + "\r\n");
            }




        }


      






        public void Init()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          
            try
            {
                socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10086));
                
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(ex.Message.ToString(), ex);
            }
           socket.Listen(5);
           socket.BeginAccept(MyAsyncCallback, socket);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Init();
        }
    }
}
