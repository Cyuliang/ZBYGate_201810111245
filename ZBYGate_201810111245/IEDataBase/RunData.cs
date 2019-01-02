using System;
using System.Data;

namespace ZBYGate_Data_Collection.IEDataBase
{
    class RunData
    {
        public Action<string> SetMessageAction;//状态回显
        private Log.CLog _Log = new Log.CLog();

        /// <summary>
        /// 入闸写入数据库
        /// </summary>
        /// <param name="lpn"></param>
        /// <param name="contaoner"></param>
        /// <param name="dt"></param>
        public void In_Insert(string lpn,string container,DateTime dt,int auto)
        {
            if(string.IsNullOrEmpty(lpn))
            {
                lpn = "*";
            }
            if(string.IsNullOrEmpty(container))
            {
                container = "*";
            }
            string Inserttext = string.Format("INSERT INTO `hw`.`indata` (Plate,Container,Time,Auto) VALUES('{0}','{1}','{2}','{3}')", lpn,container, dt.ToUniversalTime().AddHours(8), auto);
            LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);
            SetMessageAction?.Invoke(Inserttext);
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
            SetMessageAction?.Invoke(Inserttext);
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
            SetMessageAction?.Invoke(Updatetext);
            _Log.logInfo.Info(Updatetext);
        }

        /// <summary>
        /// 写入rundata闸口数据库进数据
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
            SetMessageAction?.Invoke(Inserttext);
            _Log.logInfo.Info(Inserttext);
        }

        /// <summary>
        /// 写入RUNDATA闸口数据库出数据
        /// </summary>
        /// <param name="Plate"></param>
        /// <param name="dt"></param>
        public void Rundata_update(string plate,DateTime dt)
        {
            string Updatetext = string.Empty;
            if (!string.IsNullOrWhiteSpace(plate))
            {
                string Selecttext = string.Format("SELECT * FROM  `hw`.`rundata` WHERE Plate='{0}' order by Id desc limit 1", plate);
                object Id = LocalDataBase.MySqlHelper.ExecuteScalar(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Selecttext, null);
                if(Id==null)
                {
                    SetMessageAction?.Invoke("没有找到入闸数据");
                }
                else
                {
                    Updatetext = string.Format("UPDATE `hw`.`rundata` SET OutDatetime='{0}' WHERE Plate='{1}' AND Id='{2}' AND OutDatetime is null", dt.ToUniversalTime().AddHours(8), plate,Int32.Parse(Id.ToString()));
                }
                LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);
                SetMessageAction?.Invoke(Updatetext);
                _Log.logInfo.Info(Updatetext);
            }
        }
    }
}