using MySql.Data.MySqlClient;
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
            string Inserttext = string.Format("INSERT INTO `hw`.`indata` (Plate,Container,Time,Auto) VALUES('{0}','{1}','{2}','{3}')", lpn,container, dt.ToUniversalTime().AddHours(8), auto);
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
            string Inserttext = string.Format("INSERT INTO `hw`.`outdata` (Plate,Time,Auto) VALUES('{0}','{1}','{2}')", lpn, dt.ToUniversalTime().AddHours(8), auto);
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);
            SetMessage?.Invoke(Inserttext);
            _Log.logInfo.Info(Inserttext);
        }

        /// <summary>
        /// 更新身份证
        /// </summary>
        /// <param name="Cards"></param>
        /// <param name="dt"></param>
        /// <param name="auto"></param>
        public void In_Update(string Cards,DateTime dt,int auto)
        {
            string Updatetext = string.Format("UPDATE  `hw`.`indata` SET Cards = '{0}', Auto='{1}' WHERE Time = '{2}'", Cards,auto, dt.ToUniversalTime().AddHours(8));
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
            _Log.logInfo.Info(Updatetext);
        }

        /// <summary>
        /// 插入闸口进数据
        /// </summary>
        /// <param name="Plate"></param>
        /// <param name="Container"></param>
        /// <param name="auto"></param>
        /// <param name="dt"></param>
        public void Rundata_Insert(string Plate,string Container,int auto,DateTime dt)
        {
            string Inserttext = string.Empty;
            if (string.IsNullOrEmpty(Plate))
            {
                Inserttext = string.Format("INSERT INTO `hw`.`rundata` (Container,InDatetime,Auto) VALUES('{0}','{1}','{2}')", Container, dt.ToUniversalTime().AddHours(8), auto);                
            }
            else if (string.IsNullOrEmpty(Container))
            {
                Inserttext = string.Format("INSERT INTO `hw`.`rundata` (Plate,InDatetime,Auto) VALUES('{0}','{1}','{2}')", Plate, dt.ToUniversalTime().AddHours(8), auto);
            }
            else
            {
                Inserttext = string.Format("INSERT INTO `hw`.`rundata` (Plate,Container,InDatetime,Auto) VALUES('{0}','{1}','{2}'.'{3}')", Plate, Container, dt.ToUniversalTime().AddHours(8), auto);
            }
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);
            SetMessage?.Invoke(Inserttext);
            _Log.logInfo.Info(Inserttext);
        }

        /// <summary>
        /// 插入闸口出数据
        /// </summary>
        /// <param name="Plate"></param>
        /// <param name="dt"></param>
        public void Rundata_update(string plate,DateTime dt)
        {
            if(plate!=string.Empty)
            {
                int ID = 0;
                string Selecttext = string.Format("SELECT * FROM  `hw`.`rundata` WHERE Plate='{0}' order by Id desc limit 1", plate);
                MySqlDataReader reader = LocalDataBase.MySqlHelper.ExecuteReader(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Selecttext, null);
                if(reader.Read())
                {
                    ID = int.Parse(reader[0].ToString());
                }
                //var result = LocalDataBase.MySqlHelper.ExecuteScalar(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Selecttext, null);
                //int ID = int.Parse(result.GetType().ToString());
                //string Updatetext = string.Format("UPDATE  `hw`.`indata` SET Cards = '{0}', Auto='{1}' WHERE Time = '{2}'", Cards, auto, dt.ToUniversalTime().AddHours(8));
                string Updatetext = string.Format("UPDATE `hw`.`rundata` SET OutDatetime='{0}' WHERE Plate='{1}' AND Id=ID AND OutDatetime is null order by Id desc limit 1", dt.ToUniversalTime().AddHours(8), plate);
                LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
                _Log.logInfo.Info(Updatetext);
            }
        }
    }
}