using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;
using Npgsql;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction
{
    internal class DBFeeder
    {
        public void DataFeederToOrignalPathtable(List<OrignalImgPath.ImgPathOrignal> imgData)
        {
            DBConnector _dbConnector = new DBConnector();
            string baseSQLQuery = "INSERT INTO pathtableorignal (catagory, species, imgpath) VALUES ";
            foreach (OrignalImgPath.ImgPathOrignal img in imgData)
            {
                string SQLQuery = baseSQLQuery + $"({img.catagoryId}, {img.speciesId}, '{img.imgPath}');";

                _dbConnector.Create(SQLQuery);
            }

            _dbConnector.CloseConnection();
        }
    }
}