using Minor_Project_Ai_Plant_Recognition.SourceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SourceCode.DataBaseAction
{
    internal class DBReader
    {
        public static void IndexReader(string SQLQuery, Dictionary<int, string> dict)
        {
            DBConnector _dbConnector = new();

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

        public static void OrignalImgPathReader(string SQLQuery, List<OrignalImgPath.ImgPathOrignal> imgPathList)
        {
            DBConnector _dbConnector = new();

            using (var reader = _dbConnector.Read(SQLQuery))
            {
                while (reader.Read())
                {
                    OrignalImgPath.ImgPathOrignal imgPathOrignal = new()
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

        public static void PreprocessedImgPathReader(string SQLQuery, List<PreprocessedPath.ImgPathPreprocessed> imgPathList)
        {
            DBConnector _dbConnector = new();

            using (var reader = _dbConnector.Read(SQLQuery))
            {
                while (reader.Read())
                {
                    PreprocessedPath.ImgPathPreprocessed imgPathPreprocessed = new()
                    {
                        catagoryId = reader.GetInt16(0),
                        speciesId = reader.GetInt32(1),
                        preprocessedId = reader.GetInt32(2),
                        modifierId = reader.GetInt32(3),
                        imgPath = reader.GetString(4)
                    };

                    imgPathList.Add(imgPathPreprocessed);
                }
                reader.Close();
            }

            _dbConnector.CloseConnection();
        }
    }
}