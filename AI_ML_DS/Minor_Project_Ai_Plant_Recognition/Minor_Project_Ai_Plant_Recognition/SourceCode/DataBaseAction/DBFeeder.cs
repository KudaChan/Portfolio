using Minor_Project_Ai_Plant_Recognition.SourceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SourceCode.DataBaseAction
{
    internal class DBFeeder
    {
        public static void DataFeederToOrignalPathtable(List<OrignalImgPath.ImgPathOrignal> imgData)
        {
            DBConnector _dbConnector = new();
            string baseSQLQuery = "INSERT INTO pathtableorignal (catagory, species, imgpath) VALUES ";
            foreach (OrignalImgPath.ImgPathOrignal img in imgData)
            {
                string SQLQuery = baseSQLQuery + $"({img.catagoryId}, {img.speciesId}, '{img.imgPath}');";

                _dbConnector.Create(SQLQuery);
            }

            _dbConnector.CloseConnection();
        }

        public static void DataFeederToPathtable(List<PreprocessedPath.ImgPathPreprocessed> imgData)
        {
            DBConnector _dbConnector = new();
            string baseSQLQuery = "INSERT INTO pathtable (catagory, species, preprocess, modifier, imgpath) VALUES ";
            foreach (PreprocessedPath.ImgPathPreprocessed img in imgData)
            {
                string SQLQuery = baseSQLQuery + $"({img.catagoryId}, {img.speciesId}, {img.preprocessedId}, {img.modifierId}, '{img.imgPath}');";

                _dbConnector.Create(SQLQuery);
            }

            _dbConnector.CloseConnection();
        }
    }
}