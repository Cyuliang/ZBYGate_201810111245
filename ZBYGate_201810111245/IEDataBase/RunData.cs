using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace ZBYGate_Data_Collection.IEDataBase
{
    class RunData
    {
        public Action<string> SetMessageAction;//状态回显
        private Log.CLog _Log = new Log.CLog();

        /// <summary>
        /// 读取统计数据库回显到界面和变量
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string[] Statistics_Select(DateTime dt)
        {
            string[] ReturnDatabse = new string[3] {null,null,null};

            string Selecttext = string.Format("SELECT * FROM `hw`.`traffic` WHERE `Date`='{0}'", dt.ToUniversalTime().AddHours(8).ToString("yyyy-MM-dd"));

            using (MySqlDataReader reader = LocalDataBase.MySqlHelper.ExecuteReader(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Selecttext, null))
            {
                if (reader.Read())
                {
                    ReturnDatabse[0] = reader["In"].ToString();
                    ReturnDatabse[1] = reader["Out"].ToString();
                    ReturnDatabse[2] = reader["Balance"].ToString();
                }
            }
            return ReturnDatabse;
        }

        /// <summary>
        /// 更新统计数据库
        /// </summary>
        /// <param name="Sum">结余</param>
        /// <param name="In">进闸数量</param>
        /// <param name="Out">出闸数量</param>
        public int Statistics_Update(int Balance, int In ,int Out,DateTime dt)
        {
            string Updatetext = string.Empty;
            //入闸和出闸数量变化
            if (In!=0)
            {
                Updatetext = string.Format("UPDATE `hw`.`traffic` SET `In`='{0}' ,`Balance`='{1}' WHERE `Date`='{2}'", In, Balance, dt.ToUniversalTime().AddHours(8).ToString("yyyy-MM-dd"));
            }
            if (Out!=0)
            {
                Updatetext = string.Format("UPDATE `hw`.`traffic` SET `Out`='{0}' ,`Balance`='{1}' WHERE `Date`='{2}'", Out, Balance, dt.ToUniversalTime().AddHours(8).ToString("yyyy-MM-dd"));
            }

            SetMessageAction?.Invoke(string.Format("Statistics_Update[函数|Log|{0}]", Updatetext));

            int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);

            if (i != 0)
            {
                SetMessageAction?.Invoke("Statistics_Update[函数|更新|更新统计数据库成功]");
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("Statistics_Update[函数|Log|{0} 失败]", Updatetext));
            }


            SetMessageAction?.Invoke(i.ToString());
            _Log.logInfo.Info(Updatetext);

            return i;//判断更新数据成功没有
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="da"></param>
        public void Statistics_Insert(DateTime dt ,bool now)
        {
            string Inserttext = string.Empty;
            //SetMessageAction?.Invoke("DEBUG");
            if (now)
            {
                Inserttext = string.Format("INSERT INTO `hw`.`traffic` (`Date`) VALUES('{0}')", dt.ToUniversalTime().AddHours(8).ToString("yyyy-MM-dd"));//创建今天                
            }
            else
            {
                Inserttext = string.Format("INSERT INTO `hw`.`traffic` (`Date`) VALUES('{0}')", dt.ToUniversalTime().AddHours(8 + 2).ToString("yyyy-MM-dd"));//23点加2小时创建明天记录
            }

            SetMessageAction?.Invoke(string.Format("Statistics_Insert[函数|插入|{0}]", Inserttext));

            int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);

            if (i != 0)
            {
                SetMessageAction?.Invoke("Statistics_Insert[函数|插入|插入统计数据库成功]");
            }

            _Log.logInfo.Info(Inserttext);
        }

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
            string Inserttext = string.Format("INSERT INTO `hw`.`indata` (`Plate`,`Container`,`Time`,`Auto`) VALUES('{0}','{1}','{2}','{3}')", lpn,container, dt.ToUniversalTime().AddHours(8), auto);

            SetMessageAction?.Invoke(string.Format("In_Insert[函数|插入|{0}]", Inserttext));

            int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);

            if (i != 0)
            {
                SetMessageAction?.Invoke("In_Insert[函数|插入|插入进闸InData数据库成功]");
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("In_Insert[函数|Log|{0} 失败]", Inserttext));
            }

            _Log.logInfo.Info(Inserttext);
        }

        /// <summary>
        /// 出闸写入数据库
        /// </summary>
        /// <param name="lpn"></param>
        /// <param name="dt"></param>
        public void Out_Insert(string lpn,DateTime dt,int auto)
        {
            string Inserttext = string.Format("INSERT INTO `hw`.`outdata` (`Plate`,`Time`,`Auto`) VALUES('{0}','{1}','{2}')", lpn, dt.ToUniversalTime().AddHours(8), auto);

            SetMessageAction?.Invoke(string.Format("Out_Insert[函数|插入|{0}]", Inserttext));

            int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);

            if (i != 0)
            {
                SetMessageAction?.Invoke("Out_Insert[函数|插入|插入出闸OutData数据库成功]");
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("Out_Insert[函数|Log|{0} 失败]", Inserttext));
            }

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
            string Updatetext = string.Format("UPDATE  `hw`.`indata` SET `Cards` = '{0}', `Auto`='{1}' WHERE `Time` = '{2}'", Cards,auto, dt.ToUniversalTime().AddHours(8));

            SetMessageAction?.Invoke(string.Format("In_Update[函数|更新|{0}]", Updatetext));

            int i = LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);

            if (i != 0)
            {
                SetMessageAction?.Invoke("In_Update[函数|更新|更新入闸InData数据库成功]");
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("In_Update[函数|Log|{0} 失败]", Updatetext));
            }

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

            if (!string.IsNullOrEmpty(Plate))
            {
                if(!string.IsNullOrEmpty(Container))
                {
                    Inserttext = string.Format("INSERT INTO `hw`.`rundata` (`Plate`,`Container`,`InDatetime`,`Auto`) VALUES('{0}','{1}','{2}','{3}')", Plate, Container, dt.ToUniversalTime().AddHours(8), auto);
                    SetMessageAction?.Invoke("车牌，箱号");
                }
                else
                {
                    Inserttext = string.Format("INSERT INTO `hw`.`rundata` (`Plate`,`InDatetime`,`Auto`) VALUES('{0}','{1}','{2}')", Plate, dt.ToUniversalTime().AddHours(8), auto);
                    SetMessageAction?.Invoke("车牌");
                }
            }
            else 
            {
                if (!string.IsNullOrEmpty(Container))
                {
                    Inserttext = string.Format("INSERT INTO `hw`.`rundata` (`Container`,`InDatetime`,`Auto`) VALUES('{0}','{1}','{2}')", Container, dt.ToUniversalTime().AddHours(8), auto);
                    SetMessageAction?.Invoke("箱号");
                }
                else
                {
                    SetMessageAction?.Invoke("Rundata_Insert[函数|Log|没有识别到数据，插入数据库不动作]");
                }
            }

            SetMessageAction?.Invoke(string.Format("Rundata_Insert[函数|插入|{0}]", Inserttext));
            //SetMessageAction?.Invoke("------------------------------------------------");
            int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Inserttext, null);



            if (i != 0)
            {
                SetMessageAction?.Invoke("Rundata_Insert[函数|插入|插入入闸RunData数据库成功]");
            }
            else
            {
                SetMessageAction?.Invoke(string.Format("Rundata_Insert[函数|Log|{0} 失败]", Inserttext));
            }
            
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
                string Selecttext = string.Format("SELECT * FROM  `hw`.`rundata` WHERE `Plate`='{0}' order by Id desc limit 1", plate);
                object Id = LocalDataBase.MySqlHelper.ExecuteScalar(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Selecttext, null);
                if(Id==null)
                {
                    SetMessageAction?.Invoke("Rundata_Insert[函数|Log|没有找到入闸数据]");
                }
                else
                {
                    Updatetext = string.Format("UPDATE `hw`.`rundata` SET `OutDatetime`='{0}' WHERE `Plate`='{1}' AND `Id`='{2}' AND OutDatetime is null", dt.ToUniversalTime().AddHours(8), plate,Int32.Parse(Id.ToString()));
                }

                SetMessageAction?.Invoke(string.Format("Rundata_update[函数|更新|{0}]", Updatetext));

                int i= LocalDataBase.MySqlHelper.ExecuteNonQuery(LocalDataBase.MySqlHelper.Conn, CommandType.Text, Updatetext, null);

                if (i != 0)
                {
                    SetMessageAction?.Invoke("Rundata_update[函数|更新|出闸数据更新RunData数据库成功]");
                }
                else
                {
                    SetMessageAction?.Invoke(string.Format("Rundata_update[函数|Log|{0} 失败]", Updatetext));
                }

                _Log.logInfo.Info(Updatetext);
            }
        }
    }
}