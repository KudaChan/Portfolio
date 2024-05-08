using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction
{
    internal class DBReader
    {
        public void IndexReader(string SQLQuery, Dictionary<int, string> dict, int c1 = 0, int c2 = 0)
        {
            DBConnector _dbConnector = new DBConnector();

            using (var reader = _dbConnector.Read(SQLQuery))
            {
                while (reader.Read())
                {
                    dict.Add(reader.GetInt32(0), reader.GetString(1));
                }

                reader.Close();
            }

            _dbConnector.CloseConnection();
        }

        public void orignalImgPathReader(string SQLQuery, List<OrignalImgPath.ImgPathOrignal> imgPathList)
        {
            DBConnector _dbConnector = new DBConnector();

            using (var reader = _dbConnector.Read(SQLQuery))
            {
                while (reader.Read())
                {
                    OrignalImgPath.ImgPathOrignal imgPathOrignal = new OrignalImgPath.ImgPathOrignal
                    {
                        catagoryId = reader.GetInt16(0),
                        speciesId = reader.GetInt32(1),
                        imgPath = reader.GetString(2)
                    };

                    imgPathList.Add(imgPathOrignal);
                }
                reader.Close();
            }

            _dbConnector.CloseConnection();
        }
    }
}