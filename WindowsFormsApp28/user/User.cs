
/**
* 命名空间: WindowsFormsApp28.user
*
* 功 能： N/A
* 类 名： User
*
* Ver 变更日期  负责人 变更内容
* ───────────────────────────────────
* V0.01 2020/6/29 星期一 下午 2:59:10 董文武 初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：河北鹰眼智能科技有限公司 　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WindowsFormsApp28.user
{
    public interface Person
    {
        string Speack();
        string w();
        string s();
        string a();
        string d();
        Point getPosition();
        string getName();
    }



   

    public class User : Person
    {
        public User(string name,Point point,int speed)
        {
            this.name = name;
            this.speed = speed;
            this.position = position;


        }

        


        Point position;
        int speed;
        
        string name;
        public string Speack()
        {

            return this.name;
        }

        public string w()
        {

            this.position.Y+=speed;
            return "OK";
        }

        public string s()
        {
            this.position.Y -= speed;
            return "OK";
        }

        public string a()
        {
            this.position.X += speed;
            return "OK";
        }

        public string d()
        {
            this.position.X-= speed;
            return "OK";
        }

        public Point getPosition()
        {
            return this.position;
        }

        public string getName()
        {
            return this.name;
        }
    }
    

    public class Personfactory
    {
        public Person getPerson(string personType,string name,Point point,int speed)
        {
           
            if(personType== "User")
            {
                return new User(name,point,speed);

            }
            else if(personType==null)
            {
                return null;
            }
            return null;
        }
    }

}
