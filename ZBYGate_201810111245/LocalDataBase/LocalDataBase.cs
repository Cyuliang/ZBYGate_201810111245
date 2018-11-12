using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBYGate_Data_Collection.LocalDataBase
{
    class LocalDataBase
    {
        public Action<string> SetMessage;//状态回显
        private Log.CLog _Log = new Log.CLog();

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Plate">车牌</param>
        /// <param name="Container">集装箱号码</param>
        /// <param name="Cards">身份证号码</param>
        /// <returns></returns>
        public string[] SelectData(string Plate,string Container,string Cards)
        {
            string cmdText = "SELECT *  FROM `hw`.`gate` WHERE Plate=@Plate or Container=@Container or Cards=@Cards";
            MySqlParameter[] parameters = { new MySqlParameter("@Plate", MySqlDbType.VarChar),
                                        new MySqlParameter("@Container", MySqlDbType.VarChar),
                                        new MySqlParameter("@Cards",MySqlDbType.VarChar)};

            string[] ResultTrup = new string[5];//返回数据

            #region
            string plate = Plate;
            if (Plate == string.Empty||Plate==null)
            {
                plate = "NONE";
            }
            string container = Container;
            if (Container == string.Empty||Container==null)
            {
                container = "NONE";
            }
            string cards = Cards;
            if (Cards == string.Empty||Cards==null)
            {
                cards = "NONE";
            }
            #endregion

            parameters[0].Value = plate;
            parameters[1].Value = container;
            parameters[2].Value = cards;

            _Log.logInfo.Info(cmdText);

            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(MySqlHelper.Conn, CommandType.Text, cmdText, parameters))
            {
                while(reader.Read())
                {
                    string Result = string.Format("Return Data {0} {1} {2} {3} {4} {5} {6} {7} {8}",
                        reader["Id"].ToString(),
                        reader["Plate"].ToString(),
                        reader["Container"].ToString(),
                        reader["Supplier"].ToString(),
                        reader["Appointment"].ToString(),
                        reader["Parked"].ToString(),
                        reader["Ontime"].ToString(),
                        reader["Cards"].ToString(),
                        reader["Truetime"].ToString());

                    ResultTrup[0] = reader["Plate"].ToString()+"/"+reader["Container"].ToString();                   
                    ResultTrup[1] = reader["Supplier"].ToString();
                    ResultTrup[2] = reader["Appointment"].ToString();
                    ResultTrup[3] = reader["Parked"].ToString();
                    ResultTrup[4] = reader["Ontime"].ToString();

                    SetMessage?.Invoke(Result);
                    break;                    
                }
            }
            return ResultTrup;
        }
    }
}
