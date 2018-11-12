using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_Data_Collection.IEDataBase
{
    class RunData
    {
        public Action<string> SetMessage;//状态回显
        private Log.CLog _Log = new Log.CLog();

        /// <summary>
        /// 入闸写入数据库
        /// </summary>
        /// <param name="lpn"></param>
        /// <param name="contaoner"></param>
        /// <param name="dt"></param>
        public void In_Insert(string lpn,string container,DateTime dt,int auto)
        {
            string Inserttext = string.Format("INSERT INTO `hw`.`indata` (Plate,Container,Time,Auto) VALUES('{0}','{1}','{2}','{3}')", lpn,container,dt, auto);
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);
            SetMessage?.Invoke(Inserttext);
            _Log.logInfo.Info(Inserttext);
        }

        /// <summary>
        /// 出闸写入数据库
        /// </summary>
        /// <param name="lpn"></param>
        /// <param name="dt"></param>
        public void Out_Insert(string lpn,DateTime dt,int auto)
        {
            string Inserttext = string.Format("INSERT INTO `hw`.`outdata` (Plate,Time,Auto) VALUES('{0}','{1}','{2}')", lpn, dt,auto);
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);
            SetMessage?.Invoke(Inserttext);
            _Log.logInfo.Info(Inserttext);
        }
    }
}
